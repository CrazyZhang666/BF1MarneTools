namespace BF1MarneTools.Core;

public static class LSXResp
{
    /// <summary>
    /// LSX响应字典
    /// </summary>
    private static readonly Dictionary<string, string> LsxRespXmlDic = [];

    static LSXResp()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var allResNames = assembly.GetManifestResourceNames();

        foreach (var resName in allResNames)
        {
            // 过滤无关文件
            if (!resName.Contains("Assets.Files.LSX"))
                continue;

            var lastDotIndex = resName.LastIndexOf('.');
            var nameWithoutExtension = resName[..lastDotIndex];
            var fileName = nameWithoutExtension.Split('.')[^1];

            var stream = assembly.GetManifestResourceStream(resName);
            using var streamReader = new StreamReader(stream);

            // 添加到缓存
            LsxRespXmlDic[fileName] = streamReader.ReadToEnd();
        }
    }

    /// <summary>
    /// 这个只能内部调用，自动带上方法名称
    /// </summary>
    private static string BaseLSX(string id, [CallerMemberName] string methodName = "")
    {
        return LsxRespXmlDic[methodName].Replace("[[Id]]", id);
    }

    /// <summary>
    /// 主动给客户端推送消息，无Id字段
    /// </summary>
    public static string Challenge(string key)
    {
        return BaseLSX(string.Empty)
            .Replace("[[Key]]", key);
    }

    public static string ChallengeResponse(string id, string response)
    {
        return BaseLSX(id)
            .Replace("[[Response]]", response);
    }

    public static string GetConfig(string id)
    {
        return BaseLSX(id);
    }

    public static string GetAuthCode(string id, string authCode)
    {
        return BaseLSX(id)
            .Replace("[[AuthCode]]", authCode);
    }

    public static string GetBlockList(string id)
    {
        return BaseLSX(id);
    }

    public static string GetGameInfo(string id, string gameInfo)
    {
        return BaseLSX(id)
            .Replace("[[GameInfo]]", gameInfo);
    }

    public static string GetInternetConnectedState(string id)
    {
        return BaseLSX(id);
    }

    public static string GetPresence(string id)
    {
        return BaseLSX(id);
    }

    public static string GetProfile(string id, string pid, string persona, string userId)
    {
        return BaseLSX(id)
            .Replace("[[PersonaId]]", pid)
            .Replace("[[Persona]]", persona)
            .Replace("[[UserId]]", userId);
    }

    public static string RequestLicense(string id, string license)
    {
        return BaseLSX(id)
            .Replace("[[License]]", license);
    }

    public static string GetSetting(string id, string setting)
    {
        return BaseLSX(id)
            .Replace("[[Setting]]", setting);
    }

    public static string QueryFriends(string id, string avatarId)
    {
        return BaseLSX(id)
            .Replace("[[AvatarId]]", avatarId);
    }

    public static string QueryImage(string id, string imageId, string width, string resPath)
    {
        return BaseLSX(id)
            .Replace("[[ImageId]]", imageId)
            .Replace("[[Width]]", width)
            .Replace("[[ResourcePath]]", resPath);
    }

    public static string QueryPresence(string id, string userId)
    {
        return BaseLSX(id)
            .Replace("[[UserId]]", userId);
    }

    public static string SetPresence(string id)
    {
        return BaseLSX(id);
    }

    public static string GetAllGameInfo(string id)
    {
        return BaseLSX(id)
            .Replace("[[Version]]", "1.0.57.44284")
            .Replace("[[SystemTime]]", $"{DateTime.Now:s}")
            .Replace("[[Locale]]", "zh_TW");
    }

    public static string IsProgressiveInstallationAvailable(string id)
    {
        return BaseLSX(id)
            .Replace("Origin.OFR.50.0004342", "Origin.OFR.50.0001455");
    }

    public static string QueryContent(string id)
    {
        return BaseLSX(id);
    }

    public static string QueryEntitlements(string id)
    {
        return BaseLSX(id);
    }

    public static string QueryOffers(string id)
    {
        return BaseLSX(id);
    }

    public static string SetDownloaderUtilization(string id)
    {
        return BaseLSX(id);
    }

    public static string QueryChunkStatus(string id)
    {
        return BaseLSX(id);
    }

    public static string GetPresenceVisibility(string id)
    {
        return BaseLSX(id);
    }
}