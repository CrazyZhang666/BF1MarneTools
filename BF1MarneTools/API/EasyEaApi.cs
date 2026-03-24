using BF1MarneTools.Helper;

namespace BF1MarneTools.API;

public static class EasyEaApi
{
    private const string ContentId_BF1 = "1026023";

    /// <summary>
    /// 获取 LSX 监听服务所需的许可证 License
    /// </summary>
    public static async Task<string> GetLSXLicense(string requestToken)
    {
        //if (!string.IsNullOrWhiteSpace(Globals.GameToken))
        //{
        //    LoggerHelper.Info("已找到缓存的D加密许可证令牌");
        //    return Globals.GameToken;
        //}

        LoggerHelper.Info("开始申请D加密许可证");
        return await GetLSXLicense(Globals.Email, Globals.Password, requestToken, ContentId_BF1);
    }

    /// <summary>
    /// 获取 LSX 监听服务所需的许可证 License
    /// </summary>
    public static async Task<string> GetLSXLicense(string email, string password, string requestToken, string contentId)
    {
        var result = await EaApi.GetLSXLicense(email, password, requestToken, contentId);
        if (!result.IsSuccess)
            return string.Empty;

        return result.Content;
    }

    /// <summary>
    /// 获取 LSX 监听服务所需的 AutuCode
    /// </summary>
    public static async Task<string> GetLSXAutuCode(string settingId)
    {
        await Task.Delay(1);
        return settingId;
    }
}