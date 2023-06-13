using Lucifer;
using UnityEngine;

public sealed class PlayerStatsContainer
{

    public PlayerType playerType;
    public int maxHealthIncrease;
    public int wallet;
    public int attackIncrease;
    public int speedIncrease;
    public int levelsFinished;

    private static readonly PlayerStatsContainer instance = new PlayerStatsContainer();
    static PlayerStatsContainer() {}
    private PlayerStatsContainer() {}

    public static PlayerStatsContainer Instance
    {
        get
        {
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void init()
    {

        instance.playerType = PlayerType.WARRIOR;
        instance.maxHealthIncrease = 0;
        instance.wallet = 0;
        instance.attackIncrease = 0;
        instance.speedIncrease = 0;
        instance.levelsFinished = 0;

    }

}
