
using UnityEngine;

using Lucifer;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
    public PlayerType playerType = PlayerType.WARRIOR;
    public int maxHealthIncrease = 0;
    public int attackIncrease = 0;
    public int speedIncrease = 0;
    public int wallet = 0;
}