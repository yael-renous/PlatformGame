using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Config/Game Config", order = 2)]
public class GameConfig : ScriptableObject
{
    public int NumOfLives;
    public LevelConfig[] levels;
}