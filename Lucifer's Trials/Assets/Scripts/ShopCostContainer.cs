using Lucifer;
using UnityEngine;

public sealed class ShopCostContainer
{

    public int attackCost;
    public int attackBought;
    public int speedCost;
    public int speedBought;
    public int healthCost;
    public int healthBought;

    private static readonly ShopCostContainer instance = new ShopCostContainer();
    static ShopCostContainer() {}
    private ShopCostContainer() {}

    public static ShopCostContainer Instance
    {
        get
        {
            return instance;
        }
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    private static void init()
    {

        instance.attackCost = 950;
        instance.attackBought = 0;
        instance.speedCost = 1250;
        instance.healthBought = 0;
        instance.healthCost = 450;
        instance.speedBought = 0;
    }

}
