
using UnityEngine;

using Lucifer;

[CreateAssetMenu(fileName = "PlayerStatsScriptableObject", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public PlayerType playerType = PlayerType.WARRIOR;
    public int maxHealthIncrease = 12;
    public int attackIncrease = 2;
    public int speedIncrease = 5;
    public int wallet = 0;
}