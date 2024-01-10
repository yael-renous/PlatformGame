using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Level/Level Config", order = 1)]
public class LevelConfig : ScriptableObject
{
    public string levelName;

    public GameObject platformPrefab;
    public GameObject coinPrefab;
    public GameObject keyPrefab;
    public GameObject doorPrefab;
    public GameObject trapPrefab;
    public string sceneName;

    // Placements
    // public List<ObjectPlacementData> platformPlacements = new List<ObjectPlacementData>();
    // public List<ObjectPlacementData> coinPlacements = new List<ObjectPlacementData>();
    // public ObjectPlacementData keyPlacement;
    // public ObjectPlacementData doorPlacement;
    // public List<ObjectPlacementData> trapPlacements = new List<ObjectPlacementData>();
    // public List<MiscItemData> miscItems = new List<MiscItemData>();

}

// [System.Serializable]
// public class ObjectPlacementData
// {
//     public Vector3 position;
//     public Quaternion rotation;
//
//     public ObjectPlacementData(Vector3 pos, Quaternion rot)
//     {
//         position = pos;
//         rotation = rot;
//     }
// }
//
// [System.Serializable]
// public class MiscItemData
// {
//     public GameObject prefab;
//     public Vector3 position;
//     public Quaternion rotation;
//
//     public MiscItemData(GameObject prefab, Vector3 pos, Quaternion rot)
//     {
//         this.prefab = prefab;
//         position = pos;
//         rotation = rot;
//     }
// }