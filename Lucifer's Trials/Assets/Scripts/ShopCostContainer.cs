using Lucifer;
using UnityEngine;

public sealed class ShopCostContainer
{

    public int attackCost;
    public int speedCost;
    public int healthCost;

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

        instance.attackCost = 200;
        instance.speedCost = 50;
        instance.healthCost = 100;
    }

}
