using BF1MarneTools.Helper;
using BF1MarneTools.Resources;
using RestSharp;

namespace BF1MarneTools.API;

public static class CoreApi
{
    private static readonly RestClient _client;

    static CoreApi()
    {
        var options = new RestClientOptions()
        {
            Timeout = TimeSpan.FromSeconds(20),
            FollowRedirects = false,
            ThrowOnAnyError = false,
            ThrowOnDeserializationError = false
        };

        _client = new RestClient(options);
    }

    /// <summary>
    /// 获取服务器版本信息
    /// </summary>
    public static async Task<Version> GetWebUpdateVersion()
    {
        try
        {
            var request = new RestRequest("https://api.github.com/repos/CrazyZhang666/BF1MarneTools/releases/latest", Method.Get);

            var response = await _client.ExecuteAsync(request);
            LoggerHelper.Info($"GetWebUpdateVersion {Lang.CoreApi_ResponseStatus} {response.ResponseStatus}");
            LoggerHelper.Info($"GetWebUpdateVersion {Lang.CoreApi_StatusCode} {response.StatusCode}");

            if (response.ResponseStatus == ResponseStatus.TimedOut)
            {
                LoggerHelper.Info($"GetWebUpdateVersion {Lang.CoreApi_TimedOut}");
                return null;
            }

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonNode = JsonNode.Parse(response.Content);

                var tagName = jsonNode["tag_name"].GetValue<string>();
                if (Version.TryParse(tagName, out Version version))
                {
                    LoggerHelper.Info($"{Lang.CoreApi_GetUpdateVersionSuccess} v{version}");
                    return version;
                }
            }

            LoggerHelper.Warn($"{Lang.CoreApi_GetUpdateVersionFailed} {response.Content}");
            return null;
        }
        catch (Exception ex)
        {
            LoggerHelper.Error(Lang.CoreApi_GetUpdateVersionException, ex);
            return null;
        }
    }
}