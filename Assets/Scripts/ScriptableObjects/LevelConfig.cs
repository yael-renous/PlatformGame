using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Config/Level Config", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string levelName;
    public PlatformController platformPrefab;
    public CoinController coinPrefab;
    public KeyController keyPrefab;
    public DoorController doorPrefab;
    public TrapController trapPrefab;
    public string sceneName;
}
