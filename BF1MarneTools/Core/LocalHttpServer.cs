using BF1MarneTools.Helper;
using CommunityToolkit.Mvvm.Messaging;

namespace BF1MarneTools.Core;

public static class LocalHttpServer
{
    private static HttpListener _httpListener;

    static LocalHttpServer()
    {
    }

    /// <summary>
    /// 运行服务
    /// </summary>
    public static void Run()
    {
        if (_httpListener is not null)
        {
            LoggerHelper.Warn("Local HTTP 服务正在运行，请勿重复启动");
            return;
        }

        _httpListener = new HttpListener
        {
            AuthenticationSchemes = AuthenticationSchemes.Anonymous
        };
        // 配置服务器地址和端口号
        _httpListener.Prefixes.Add("http://127.0.0.1:59743/");
        // 启动服务器
        _httpListener.Start();
        // 开始监听
        _httpListener.BeginGetContext(Result, null);

        LoggerHelper.Info("启动 Local HTTP 服务成功");
        LoggerHelper.Debug("Local HTTP 服务端口为 59743");
    }

    /// <summary>
    /// 停止服务
    /// </summary>
    public static void Stop()
    {
        _httpListener?.Stop();
        _httpListener?.Close();
        _httpListener = null;

        LoggerHelper.Info("停止 Local HTTP 服务成功");
    }

    /// <summary>
    /// 判断是否为正确的请求路径
    /// </summary>
    private static bool IsTrueRawUrl(HttpListenerRequest request, string rawUrl)
    {
        return request.RawUrl.StartsWith(rawUrl, StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// 监听本地Http服务器线程
    /// </summary>
    private static void Result(IAsyncResult asyncResult)
    {
        try
        {
            if (_httpListener is null)
                return;

            // 继续异步监听
            _httpListener.BeginGetContext(Result, null);

            // 获取当前连接对象
            var context = _httpListener.EndGetContext(asyncResult);
            var request = context.Request;
            var response = context.Response;

            // 告诉客户端返回的ContentType类型为纯文本格式，编码为UTF-8
            response.ContentType = "text/plain;charset=UTF-8";
            response.ContentEncoding = Encoding.UTF8;

            // 添加响应头信息
            response.AddHeader("Content-type", "text/plain");

            if (request.HttpMethod == "GET")
            {
                // 处理 GET 请求
                LoggerHelper.Info($"收到 Local HTTP GET请求 {request.Url}");
                LoggerHelper.Info($"请求的客户端为 {request.RemoteEndPoint}");

                if (IsTrueRawUrl(request, "/RunBf1Game"))
                {
                    // 因为这个启动游戏方法涉及到UI操作，直接调用会出现跨线程问题
                    // 所以使用消息通知的方式调用
                    WeakReferenceMessenger.Default.Send("", "RunBf1Game");

                    // 这个相应内容《启动战地1游戏成功》不能随便修改
                    // 它和Mod安装工具里面对应的
                    response.OutputStream.Write(Encoding.UTF8.GetBytes("启动战地1游戏成功"));
                    response.StatusCode = 200;
                    response.Close();
                }
            }
            else if (request.HttpMethod == "POST")
            {
                // 处理 POST 请求
                LoggerHelper.Info($"收到 Local HTTP POST请求 {request.Url}");
                LoggerHelper.Info($"请求的客户端为 {request.RemoteEndPoint}");
            }
        }
        catch (Exception ex)
        {
            LoggerHelper.Error("处理 Local HTTP 请求出现异常", ex);
        }
    }
}