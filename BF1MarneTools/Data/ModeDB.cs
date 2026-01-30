using BF1MarneTools.Models;

namespace BF1MarneTools.Data;

public static class ModeDB
{
    public static readonly List<ModeInfo> GameModeInfoDb = [];

    static ModeDB()
    {
        GameModeInfoDb.Add(new()
        {
            Code = "Conquest0",
            Name = "征服",
            English = "CONQUEST"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "Rush0",
            Name = "突袭",
            English = "RUSH"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "BreakthroughLarge0",
            Name = "行动模式",
            English = "OPERATIONS"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "Breakthrough0",
            Name = "闪击行动",
            English = "SHOCK OPERATIONS"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "Possession0",
            Name = "战争信鸽",
            English = "WAR PIGEONS"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "TugOfWar0",
            Name = "前线",
            English = "FRONTLINES"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "Domination0",
            Name = "抢攻",
            English = "DOMINATION"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "TeamDeathMatch0",
            Name = "团队死斗",
            English = "TEAM DEATHMATCH"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "ZoneControl0",
            Name = "空降补给",
            English = "SUPPLY DROP"
        });

        GameModeInfoDb.Add(new()
        {
            Code = "AirAssault0",
            Name = "空中突袭",
            English = "AIR ASSAULT"
        });
    }

    /// <summary>
    /// 通过代码获取模式名称
    /// </summary>
    public static string GetModeNameByCode(string code)
    {
        var result = GameModeInfoDb.Find(x => x.Code == code);
        if (result == null)
            return "Unknown Mode";

        return result.Name;
    }

    /// <summary>
    /// 根据模式代码获取模式详情
    /// </summary>
    public static ModeInfo GetModeInfoByCode(string code)
    {
        return GameModeInfoDb.Find(x => x.Code == code);
    }
}