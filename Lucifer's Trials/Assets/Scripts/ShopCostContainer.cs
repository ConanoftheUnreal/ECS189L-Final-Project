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

        instance.attackCost = 250;
        instance.attackBought = 0;
        instance.speedCost = 500;
        instance.healthBought = 0;
        instance.healthCost = 150;
        instance.speedBought = 0;
    }

}
