using BF1MarneTools.Models;

namespace BF1MarneTools.Data;

public static class MapDB
{
    public static readonly List<MapInfo> GameMapInfoDb = [];

    static MapDB()
    {
        // 1 - 亚眠
        GameMapInfoDb.Add(new()
        {
            Name = "亚眠",
            English = "AMIENS",
            Code = "MP_Amiens",
            DLC = "本体",
            Url = "Levels/MP/MP_Amiens/MP_Amiens",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 2 - 流血宴厅
        GameMapInfoDb.Add(new()
        {
            Name = "流血宴厅",
            English = "BALLROOM BLITZ",
            Code = "MP_Chateau",
            DLC = "本体",
            Url = "Levels/MP/MP_Chateau/MP_Chateau",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 3 - 西奈沙漠
        GameMapInfoDb.Add(new()
        {
            Name = "西奈沙漠",
            English = "SINAI DESERT",
            Code = "MP_Desert",
            DLC = "本体",
            Url = "Levels/MP/MP_Desert/MP_Desert",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 4 - 法欧堡
        GameMapInfoDb.Add(new()
        {
            Name = "法欧堡",
            English = "FAO FORTRESS",
            Code = "MP_FaoFortress",
            DLC = "本体",
            Url = "Levels/MP/MP_FaoFortress/MP_FaoFortress",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 5 - 阿尔贡森林
        GameMapInfoDb.Add(new()
        {
            Name = "阿尔贡森林",
            English = "ARGONNE FOREST",
            Code = "MP_Forest",
            DLC = "本体",
            Url = "Levels/MP/MP_Forest/MP_Forest",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 6 - 帝国边境（无前线模式）
        GameMapInfoDb.Add(new()
        {
            Name = "帝国边境",
            English = "EMPIRE'S EDGE",
            Code = "MP_ItalianCoast",
            DLC = "本体",
            Url = "Levels/MP/MP_ItalianCoast/MP_ItalianCoast",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0"]
        });

        // 7 - 格拉巴山
        GameMapInfoDb.Add(new()
        {
            Name = "格拉巴山",
            English = "MONTE GRAPPA",
            Code = "MP_MountainFort",
            DLC = "本体",
            Url = "Levels/MP/MP_MountainFort/MP_MountainFort",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 8 - 圣康坦的伤痕
        GameMapInfoDb.Add(new()
        {
            Name = "圣康坦的伤痕",
            English = "ST. QUENTIN SCAR",
            Code = "MP_Scar",
            DLC = "本体",
            Url = "Levels/MP/MP_Scar/MP_Scar",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 9 - 苏伊士
        GameMapInfoDb.Add(new()
        {
            Name = "苏伊士",
            English = "SUEZ",
            Code = "MP_Suez",
            DLC = "本体",
            Url = "Levels/MP/MP_Suez/MP_Suez",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        ///////////////////////////////

        // 10 - 庞然暗影
        GameMapInfoDb.Add(new()
        {
            Name = "庞然暗影",
            English = "GIANT'S SHADOW",
            Code = "MP_Giant",
            DLC = "DLC0",
            Url = "Xpack0/Levels/MP/MP_Giant/MP_Giant",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        ///////////////////////////////

        // 11 - 苏瓦松
        GameMapInfoDb.Add(new()
        {
            Name = "苏瓦松",
            English = "SOISSONS",
            Code = "MP_Fields",
            DLC = "DLC1",
            Url = "Xpack1/Levels/MP_Fields/MP_Fields",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 12 - 决裂
        GameMapInfoDb.Add(new()
        {
            Name = "决裂",
            English = "RUPTURE",
            Code = "MP_Graveyard",
            DLC = "DLC1",
            Url = "Xpack1/Levels/MP_Graveyard/MP_Graveyard",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 13 - 法乌克斯要塞
        GameMapInfoDb.Add(new()
        {
            Name = "法乌克斯要塞",
            English = "FORT DE VAUX",
            Code = "MP_Underworld",
            DLC = "DLC1",
            Url = "Xpack1/Levels/MP_Underworld/MP_Underworld",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 14 - 凡尔登高地
        GameMapInfoDb.Add(new()
        {
            Name = "凡尔登高地",
            English = "VERDUN HEIGHTS",
            Code = "MP_Verdun",
            DLC = "DLC1",
            Url = "Xpack1/Levels/MP_Verdun/MP_Verdun",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        ///////////////////////////////

        // 15 - 攻占托尔
        GameMapInfoDb.Add(new()
        {
            Name = "攻占托尔",
            English = "PRISE DE TAHURE",
            Code = "MP_ShovelTown",
            DLC = "DLC1-3",
            Url = "Xpack1-3/Levels/MP_ShovelTown/MP_ShovelTown",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        // 16 - 尼维尔之夜
        GameMapInfoDb.Add(new()
        {
            Name = "尼维尔之夜",
            English = "NIVELLE NIGHTS",
            Code = "MP_Trench",
            DLC = "DLC1-3",
            Url = "Xpack1-3/Levels/MP_Trench/MP_Trench",
            Modes = ["Conquest0", "Rush0", "Possession0", "TugOfWar0", "Domination0", "TeamDeathMatch0"]
        });

        ///////////////////////////////

        // 17 - 勃鲁希洛夫关口
        GameMapInfoDb.Add(new()
        {
            Name = "勃鲁希洛夫关口",
            English = "BRUSILOV KEEP",
            Code = "MP_Bridge",
            DLC = "DLC2",
            Url = "Xpack2/Levels/MP/MP_Bridge/MP_Bridge",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0", "ZoneControl0"]
        });

        // 18 - 阿尔比恩
        GameMapInfoDb.Add(new()
        {
            Name = "阿尔比恩",
            English = "ALBION",
            Code = "MP_Islands",
            DLC = "DLC2",
            Url = "Xpack2/Levels/MP/MP_Islands/MP_Islands",
            Modes = ["Conquest0", "Rush0", "Possession0", "Domination0", "TeamDeathMatch0", "ZoneControl0"]
        });

        // 19 - 武普库夫山口
        GameMapInfoDb.Add(new()
        {
            Name = "武普库夫山口",
            English = "LUPKÓW PASS",
            Code = "MP_Ravines",
            DLC = "DLC2",
            Url = "Xpack2/Levels/MP/MP_Ravines/MP_Ravines",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0", "ZoneControl0"]
        });

        // 20 - 察里津
        GameMapInfoDb.Add(new()
        {
            Name = "察里津",
            English = "TSARITSYN",
            Code = "MP_Tsaritsyn",
            DLC = "DLC2",
            Url = "Xpack2/Levels/MP/MP_Tsaritsyn/MP_Tsaritsyn",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0", "ZoneControl0"]
        });

        // 21 - 加利西亚
        GameMapInfoDb.Add(new()
        {
            Name = "加利西亚",
            English = "GALICIA",
            Code = "MP_Valley",
            DLC = "DLC2",
            Url = "Xpack2/Levels/MP/MP_Valley/MP_Valley",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0", "ZoneControl0"]
        });

        // 22 - 窝瓦河
        GameMapInfoDb.Add(new()
        {
            Name = "窝瓦河",
            English = "VOLGA RIVER",
            Code = "MP_Volga",
            DLC = "DLC2",
            Url = "Xpack2/Levels/MP/MP_Volga/MP_Volga",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0", "ZoneControl0"]
        });

        ///////////////////////////////

        // 23 - 海丽丝岬
        GameMapInfoDb.Add(new()
        {
            Name = "海丽丝岬",
            English = "CAPE HELLES",
            Code = "MP_Beachhead",
            DLC = "DLC3",
            Url = "Xpack3/Levels/MP/MP_Beachhead/MP_Beachhead",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0"]
        });

        // 24 - 泽布吕赫
        GameMapInfoDb.Add(new()
        {
            Name = "泽布吕赫",
            English = "ZEEBRUGGE",
            Code = "MP_Harbor",
            DLC = "DLC3",
            Url = "Xpack3/Levels/MP/MP_Harbor/MP_Harbor",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0"]
        });

        // 25 - 黑尔戈兰湾
        GameMapInfoDb.Add(new()
        {
            Name = "黑尔戈兰湾",
            English = "HELIGOLAND BIGHT",
            Code = "MP_Naval",
            DLC = "DLC3",
            Url = "Xpack3/Levels/MP/MP_Naval/MP_Naval",
            Modes = ["Conquest0"]
        });

        // 26 - 阿奇巴巴
        GameMapInfoDb.Add(new()
        {
            Name = "阿奇巴巴",
            English = "ACHI BABA",
            Code = "MP_Ridge",
            DLC = "DLC3",
            Url = "Xpack3/Levels/MP/MP_Ridge/MP_Ridge",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0"]
        });

        ///////////////////////////////

        // 27 - 剃刀边缘
        GameMapInfoDb.Add(new()
        {
            Name = "剃刀边缘",
            English = "RAZOR'S EDGE",
            Code = "MP_Alps",
            DLC = "DLC4",
            Url = "Xpack4/Levels/MP/MP_Alps/MP_Alps",
            Modes = ["AirAssault0"]
        });

        // 28 - 伦敦的呼唤：夜袭
        GameMapInfoDb.Add(new()
        {
            Name = "伦敦的呼唤：夜袭",
            English = "LONDON CALLING: RAIDERS",
            Code = "MP_Blitz",
            DLC = "DLC4",
            Url = "Xpack4/Levels/MP/MP_Blitz/MP_Blitz",
            Modes = ["AirAssault0"]
        });

        // 29 - 帕斯尚尔
        GameMapInfoDb.Add(new()
        {
            Name = "帕斯尚尔",
            English = "PASSCHENDAELE",
            Code = "MP_Hell",
            DLC = "DLC4",
            Url = "Xpack4/Levels/MP/MP_Hell/MP_Hell",
            Modes = ["Conquest0", "Rush0", "Possession0", "Domination0", "TeamDeathMatch0"]
        });

        // 30 - 伦敦的呼唤：灾祸
        GameMapInfoDb.Add(new()
        {
            Name = "伦敦的呼唤：灾祸",
            English = "LONDON CALLING: SCOURGE",
            Code = "MP_London",
            DLC = "DLC4",
            Url = "Xpack4/Levels/MP/MP_London/MP_London",
            Modes = ["AirAssault0"]
        });

        // 31 - 索姆河
        GameMapInfoDb.Add(new()
        {
            Name = "索姆河",
            English = "RIVER SOMME",
            Code = "MP_Offensive",
            DLC = "DLC4",
            Url = "Xpack4/Levels/MP/MP_Offensive/MP_Offensive",
            Modes = ["Conquest0", "Rush0", "BreakthroughLarge0", "Breakthrough0", "Possession0", "Domination0", "TeamDeathMatch0"]
        });

        // 32 - 卡波雷托
        GameMapInfoDb.Add(new()
        {
            Name = "卡波雷托",
            English = "CAPORETTO",
            Code = "MP_River",
            DLC = "DLC4",
            Url = "Xpack4/Levels/MP/MP_River/MP_River",
            Modes = ["Conquest0", "Rush0", "Possession0", "Domination0", "TeamDeathMatch0"]
        });
    }

    /// <summary>
    /// 通过路径获取地图名称
    /// </summary>
    public static string GetMapNameByPath(string mapPath)
    {
        var result = GameMapInfoDb.Find(x => x.Url == mapPath);
        if (result == null)
            return "Unknown Map";

        return result.Name;
    }

    /// <summary>
    /// 通过名称获取地图图片
    /// </summary>
    public static string GetMapImageByName(string mapName)
    {
        return $"pack://application:,,,/BF1MarneTools;component/Assets/Images/Maps/{mapName}.jpg";
    }

    /// <summary>
    /// 通过路径获取地图图片
    /// </summary>
    public static string GetMapImageByPath(string mapPath)
    {
        var result = GameMapInfoDb.Find(x => x.Url == mapPath);
        if (result == null)
            return "pack://application:,,,/BF1MarneTools;component/Assets/Images/Maps/MP_Alps.jpg";

        return $"pack://application:,,,/BF1MarneTools;component/Assets/Images/Maps/{result.Code}.jpg";
    }

    /// <summary>
    /// 根据地图路径获取地图详情
    /// </summary>
    public static MapInfo GetMapInfoByUrl(string url)
    {
        return GameMapInfoDb.Find(x => x.Url == url);
    }
}