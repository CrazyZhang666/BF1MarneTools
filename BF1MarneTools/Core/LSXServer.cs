using BF1MarneTools.Api;
using BF1MarneTools.Helper;

namespace BF1MarneTools.Core;

public static class LSXServer
{
    /// <summary>
    /// TCP服务器
    /// </summary>
    private static TcpListener _tcpServer = null;

    /// <summary>
    /// 静态构造函数
    /// </summary>
    static LSXServer()
    {
    }

    /// <summary>
    /// 启动 TCP 监听服务
    /// </summary>
    public static void Run()
    {
        if (_tcpServer is not null)
        {
            LoggerHelper.Warn("LSX 监听服务已经在运行，请勿重复启动");
            return;
        }

        _tcpServer = new TcpListener(IPAddress.Parse("127.0.0.1"), 3216);
        _tcpServer.Start();

        LoggerHelper.Info("启动 LSX 监听服务成功");
        LoggerHelper.Trace("LSX 服务监听端口为 3216");

        _tcpServer.BeginAcceptTcpClient(Result, null);
    }

    /// <summary>
    /// 停止 TCP 监听服务
    /// </summary>
    public static void Stop()
    {
        _tcpServer?.Stop();
        _tcpServer = null;
        LoggerHelper.Info("停止 LSX 监听服务成功");
    }

    /// <summary>
    /// 处理战地1客户端关闭异常
    /// </summary>
    private static bool MakeGameClientCloseEx(Exception ex)
    {
        if (ex.InnerException is not SocketException)
            return false;

        LoggerHelper.Warn("警告: 战地1游戏客户端已被关闭。");
        return true;
    }

    /// <summary>
    /// 处理TCP客户端连接
    /// </summary>
    private static async void Result(IAsyncResult asyncResult)
    {
        // 避免服务关闭时抛出异常
        if (_tcpServer is null)
            return;

        // 完成检索传入的客户端请求的异步操作
        var client = _tcpServer?.EndAcceptTcpClient(asyncResult);
        // 开始异步检索传入的请求（下一个请求）
        _tcpServer?.BeginAcceptTcpClient(Result, null);

        // 保存客户端连接Ip和地址
        var clientIp = string.Empty;

        try
        {
            // 如果连接断开，则结束
            if (!client.Connected)
                return;

            clientIp = client.Client.RemoteEndPoint.ToString();
            LoggerHelper.Trace($"发现 TCP 客户端连接 {clientIp}");

            /////////////////////////////////////////////////

            // 建立和连接的客户端的数据流（传输数据）
            var networkStream = client.GetStream();
            // 设置读写超时时间为 3600 秒
            networkStream.ReadTimeout = 3600000;
            networkStream.WriteTimeout = 3600000;

            // 最开始向游戏发送的请求文本，未加密
            await WriteTcpString(client, networkStream, LSXResp.Challenge(EaCrypto.StartKey));

            var xmlString = await ReadTcpString(client, networkStream);
            LoggerHelper.Trace($"接收到的TCP字符串为: {xmlString}");

            // 主要是给带EAAC版战地1使用
            // 修复重复的version属性导致XML解析异常
            if (xmlString.Contains("version=\"3\""))
                xmlString = xmlString.Replace("version=\"\"", "");

            // 解析XML字符串
            var xmlDoc = XDocument.Parse(xmlString);
            var xmlRoot = xmlDoc.Root;
            var requestNode = xmlRoot.Element("Request");

            var recipient = requestNode?.Attribute("recipient")?.Value;
            var id = requestNode?.Attribute("id")?.Value;
            LoggerHelper.Trace($"LSX Request 解析结果: recipient={recipient}, id={id}");

            var challengeResponseNode = requestNode.Element("ChallengeResponse");
            var response = challengeResponseNode?.Attribute("response")?.Value;
            var key = challengeResponseNode?.Attribute("key")?.Value;
            var version = challengeResponseNode?.Attribute("version")?.Value;

            var contentId = challengeResponseNode?.Element("ContentId")?.Value;

            LoggerHelper.Trace($"本次启动 Challenge Response 为: {response}");
            LoggerHelper.Info($"本次启动 ContentId 为: {contentId}");
            LoggerHelper.Info("正在处理启动游戏请求...");

            // 检查 Challenge 响应
            if (!EaCrypto.CheckChallengeResponse(response, EaCrypto.StartKey))
            {
                LoggerHelper.Fatal("Challenge Response 致命错误! StartKey不一致");
                return;
            }

            // 处理解密 Challenge 响应
            var newResponse = EaCrypto.HandleChallengeResponse(key);
            LoggerHelper.Trace($"处理解密 Challenge 响应 NewResponse {newResponse}");

            var seed = (ushort)((newResponse[0] << 8) | newResponse[1]);
            LoggerHelper.Trace($"处理解密 Challenge 响应 Seed {seed}");

            // 第二次向游戏发送的请求文本，未加密
            await WriteTcpString(client, networkStream, LSXResp.ChallengeResponse(id, newResponse));

            // 这里死循环要注意
            // 仅客户端已连接时运行
            while (client.Connected)
            {
                try
                {
                    var data = await ReadTcpString(client, networkStream);
                    // 如果读取内容返回为空，则结束
                    if (string.IsNullOrWhiteSpace(data))
                    {
                        LoggerHelper.Warn($"警告: TCP 客户端 {clientIp} 连接已断开");
                        return;
                    }

                    data = EaCrypto.LSXDecryptBF1(data, seed);
                    data = await LSXRequestHandleForBF1(data, contentId);

                    // 过滤空消息
                    if (!string.IsNullOrWhiteSpace(data))
                        LoggerHelper.Trace($"当前 LSX 回复 {data}");

                    data = EaCrypto.LSXEncryptBF1(data, seed);
                    await WriteTcpString(client, networkStream, data);
                }
                catch (TimeoutException ex)
                {
                    LoggerHelper.Error("处理 LSX TCP 客户端连接发生超时异常", ex);
                }
            }
        }
        catch (Exception ex)
        {
            // 处理异常
            if (!MakeGameClientCloseEx(ex))
                LoggerHelper.Error("处理 TCP 客户端连接发生异常", ex);
        }
        finally
        {
            client?.Close();
            LoggerHelper.Trace($"TCP 客户端连接处理结束 {clientIp}");
        }
    }

    /// <summary>
    /// 异步读取 TCP 网络流字符串
    /// </summary>
    private static async Task<string> ReadTcpString(TcpClient client, NetworkStream stream)
    {
        // 如果客户端连接断开，则返回空字符串
        if (!client.Connected)
            return string.Empty;

        var strBuilder = new StringBuilder();
        // 缓冲区设置为4KB
        // 使用 ArrayPool 重用缓冲区
        var buffer = ArrayPool<byte>.Shared.Rent(4096);

        try
        {
            using var memoryStream = new MemoryStream(4096);

            var bytesRead = await stream.ReadAsync(buffer);
            if (bytesRead == 0)
                return string.Empty;

            LoggerHelper.Trace($"读取TCP字节数组长度: {bytesRead}");

            // 查找结束符
            var endIndex = Array.IndexOf(buffer, (byte)0, 0, bytesRead);
            if (endIndex >= 0)
            {
                // 找到了结束符
                memoryStream.Write(buffer, 0, endIndex);
            }
            else
            {
                // 没有结束符，写入全部数据
                memoryStream.Write(buffer, 0, bytesRead);
            }

            // 转换为字符串
            return Encoding.UTF8.GetString(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);
        }
        catch (Exception ex)
        {
            // 处理异常
            if (!MakeGameClientCloseEx(ex))
                LoggerHelper.Error("异步读取 TCP 字符串时发生异常", ex);
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }

        return strBuilder.ToString();
    }

    /// <summary>
    /// 异步写入 TCP 网络流字符串（末尾自带加结束符）
    /// </summary>
    private static async Task WriteTcpString(TcpClient client, NetworkStream stream, string content)
    {
        // 如果客户端连接断开，则结束
        if (!client.Connected)
            return;
        // 过滤空消息
        if (string.IsNullOrWhiteSpace(content))
            return;

        // 计算所需缓冲区大小
        var contentByteCount = Encoding.UTF8.GetByteCount(content);
        var totalLength = contentByteCount + 1; // 内容 + 结束符

        // 从缓冲池租用缓冲区
        var buffer = ArrayPool<byte>.Shared.Rent(totalLength);

        // 这个不要用 try catch 捕获异常
        // 主要是为了避免死循环无限执行（使用异常来中断死循环）
        try
        {
            // 编码字符串到缓冲区
            var encodedBytes = Encoding.UTF8.GetBytes(
                content, 0, content.Length,
                buffer, 0
            );

            // 添加结束符
            buffer[encodedBytes] = 0;

            // 异步写入
            await stream.WriteAsync(buffer.AsMemory(0, encodedBytes + 1));
            await stream.FlushAsync(); // 确保数据发送
        }
        finally
        {
            // 归还缓冲区
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }

    /// <summary>
    /// 处理 BF1 LSX 请求
    /// </summary>
    private static async Task<string> LSXRequestHandleForBF1(string xmlString, string contentId)
    {
        if (string.IsNullOrWhiteSpace(xmlString))
            return string.Empty;

        LoggerHelper.Trace($"BF1 LSX 请求XML字符串为: {xmlString}");

        // 解析XML字符串
        var xmlDoc = XDocument.Parse(xmlString);
        var xmlRoot = xmlDoc.Root;
        var requestNode = xmlRoot.Element("Request");

        var recipient = requestNode?.Attribute("recipient")?.Value;
        var id = requestNode?.Attribute("id")?.Value;
        LoggerHelper.Trace($"LSX Request 解析结果: recipient={recipient}, id={id}");

        var requestTypeNode = requestNode.Elements().First();
        var requestTypeName = requestTypeNode.Name.LocalName;

        LoggerHelper.Debug($"BF1 LSX 请求Id为: {id}");
        LoggerHelper.Debug($"BF1 LSX 请求类型为: {requestTypeName}");

        if (requestTypeName == "InvalidateLicense")
        {
            LoggerHelper.Warn("警告：缓存的D加密许可证已过期");
            LoggerHelper.Info("等待游戏生成新的D加密许可证...");

            // 先重置缓存的D加密许可证
            Globals.ResetGameToken();
            // 直接返回一个空的许可证，这样游戏会立刻请求新的许可证
            return LSXResp.RequestLicense(id, string.Empty);
        }

        return requestTypeName switch
        {
            "GetConfig" => LSXResp.GetConfig(id),
            // 注意：这个UserId不确定是否为这个属性
            "GetAuthCode" => (requestTypeNode.Attribute("ClientId")?.Value ?? requestTypeNode.Attribute("UserId")?.Value) is string settingId
                ? LSXResp.GetAuthCode(id, await EasyEaApi.GetLSXAutuCode(settingId))
                : string.Empty,
            "GetBlockList" => LSXResp.GetBlockList(id),
            "GetGameInfo" => requestTypeNode.Attribute("GameInfoId").Value switch
            {
                "FREETRIAL" => LSXResp.GetGameInfo(id, "false"),
                "UPTODATE" => LSXResp.GetGameInfo(id, "true"),
                "LANGUAGE" => LSXResp.GetGameInfo(id, "zh_TW"),
                "INSTALLED_LANGUAGE" => LSXResp.GetGameInfo(id, "zh_TW"),
                _ => LSXResp.GetGameInfo(id, "ar_SA,en_US,ko_KR,zh_CN,zh_TW,de_DE,es_ES,es_MX,fr_FR,it_IT,ja_JP,pl_PL,pt_BR,ru_RU")
            },
            "GetInternetConnectedState" => LSXResp.GetInternetConnectedState(id),
            "GetPresence" => LSXResp.GetPresence(id),
            "GetProfile" => LSXResp.GetProfile(id, "123456789", "PlayerName", "123456"),
            "RequestLicense" => LSXResp.RequestLicense(id, await EasyEaApi.GetLSXLicense(requestTypeNode.Attribute("RequestTicket").Value)),
            "GetSetting" => requestTypeNode.Attribute("SettingId").Value switch
            {
                "ENVIRONMENT" => LSXResp.GetSetting(id, "production"),
                "IS_IGO_AVAILABLE" => LSXResp.GetSetting(id, "false"),
                "IS_IGO_ENABLED" => LSXResp.GetSetting(id, "false"),
                _ => string.Empty
            },
            "QueryFriends" => LSXResp.QueryFriends(id, "AvatarId"),
            "QueryImage" => LSXResp.QueryImage(id, "ImageId", "Width", "ResourcePath"),
            "QueryPresence" => LSXResp.QueryPresence(id, "123456"),
            "SetPresence" => LSXResp.SetPresence(id),
            "GetAllGameInfo" => LSXResp.GetAllGameInfo(id),
            "IsProgressiveInstallationAvailable" => LSXResp.IsProgressiveInstallationAvailable(id),
            "QueryContent" => LSXResp.QueryContent(id),
            "QueryEntitlements" => LSXResp.QueryEntitlements(id),
            "QueryOffers" => LSXResp.QueryOffers(id),
            "SetDownloaderUtilization" => LSXResp.SetDownloaderUtilization(id),
            "QueryChunkStatus" => LSXResp.QueryChunkStatus(id),
            "GetPresenceVisibility" => LSXResp.GetPresenceVisibility(id),
            _ => string.Empty
        };
    }
}