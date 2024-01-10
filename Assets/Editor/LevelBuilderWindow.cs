// using UnityEngine;
// using UnityEditor;
// using System.IO;
// using System.Collections.Generic;
//
// public class LevelBuilderWindow : EditorWindow
// {
//     LevelConfig _currentLevelConfig;
//     string levelName = "";
//     string levelDirectory = "Assets/Resources/Levels";
//
//     [MenuItem("Tools/Level Builder")]
//     public static void ShowWindow()
//     {
//         GetWindow<LevelBuilderWindow>("Level Builder");
//     }
//
//     bool LevelDataExists(string name)
//     {
//         string path = $"Assets/Resources/Levels/{name}.asset";
//         return AssetDatabase.LoadAssetAtPath<LevelConfig>(path) != null;
//     }
//
//     void OnGUI()
//     {
//         GUILayout.Label("Level Configuration", EditorStyles.boldLabel);
//         levelName = EditorGUILayout.TextField("Level Name", levelName);
//
//         if (string.IsNullOrEmpty(levelName))
//         {
//             EditorGUILayout.HelpBox("Please enter a valid level name.", MessageType.Warning);
//             return;
//         }
//
//         if (LevelDataExists(levelName))
//         {
//             EditorGUILayout.HelpBox("This Level Name Exists", MessageType.Warning);
//             return;
//         }
//
//         if (_currentLevelConfig == null)
//         {
//             _currentLevelConfig = ScriptableObject.CreateInstance<LevelConfig>();
//         }
//
//
//         DrawLevelDataEditor();
//
//         GUILayout.Space(10f);
//         GUILayout.Label("Level Design", EditorStyles.boldLabel);
//
//         if (_currentLevelConfig.platformPrefab != null)
//             AddLevelObject("Add Platform", () => AddPlatform());
//         if (_currentLevelConfig.coinPrefab != null)
//             AddLevelObject("Add Coin", () => AddCoin());
//         if (_currentLevelConfig.keyPrefab != null)
//             AddLevelObject("Add Key", () => AddKey());
//         if (_currentLevelConfig.doorPrefab != null)
//             AddLevelObject("Add Door", () => AddDoor());
//         if (_currentLevelConfig.trapPrefab != null)
//             AddLevelObject("Add Trap", () => AddTrap());
//
//         if (GUILayout.Button("Save Level Data"))
//         {
//             SaveLevelData();
//         }
//     }
//
//
//     void DrawLevelDataEditor()
//     {
//         _currentLevelConfig.platformPrefab = (GameObject) EditorGUILayout.ObjectField("Platform Prefab",
//             _currentLevelConfig.platformPrefab, typeof(GameObject), false);
//         _currentLevelConfig.coinPrefab = (GameObject) EditorGUILayout.ObjectField("Coin Prefab",
//             _currentLevelConfig.coinPrefab, typeof(GameObject), false);
//         _currentLevelConfig.keyPrefab =
//             (GameObject) EditorGUILayout.ObjectField("Key Prefab", _currentLevelConfig.keyPrefab, typeof(GameObject),
//                 false);
//         _currentLevelConfig.doorPrefab = (GameObject) EditorGUILayout.ObjectField("Door Prefab",
//             _currentLevelConfig.doorPrefab, typeof(GameObject), false);
//     }
//
//     void AddLevelObject(string buttonText, System.Action addAction)
//     {
//         if (GUILayout.Button(buttonText))
//         {
//             addAction.Invoke();
//         }
//     }
//
//
//     private GameObject InstantiateNewObject(GameObject prefab)
//     {
//         if (prefab == null)
//             return null;
//         GameObject newObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
//         Undo.RegisterCreatedObjectUndo(newObject, $"Create {prefab.name}");
//         return newObject;
//     }
//
//     private void AddPlatform()
//     {
//         GameObject platform = InstantiateNewObject(_currentLevelConfig.platformPrefab);
//         platform.tag = "Platform";
//     }
//
//     void AddCoin()
//     {
//         GameObject newPrefab = InstantiateNewObject(_currentLevelConfig.coinPrefab);
//         // currentLevelData.coinPlacements.Add(new ObjectPlacementData(newPrefab.transform.position,
//         //     newPrefab.transform.rotation));
//     }
//
//     void AddKey()
//     {
//         // if (currentLevelData.keyPlacement != null)
//         // {
//         //     EditorGUILayout.HelpBox("There's already a key in the level", MessageType.Warning);
//         //     return;
//         // }
//
//         GameObject newPrefab = InstantiateNewObject(_currentLevelConfig.keyPrefab);
//         // currentLevelData.keyPlacement =
//         //     new ObjectPlacementData(newPrefab.transform.position, newPrefab.transform.rotation);
//     }
//
//     void AddDoor()
//     {
//         // if (currentLevelData.doorPlacement != null)
//         // {
//         //     EditorGUILayout.HelpBox("There's already a door in the level", MessageType.Warning);
//         //     return;
//         // }
//
//         GameObject newPrefab = InstantiateNewObject(_currentLevelConfig.doorPrefab);
//         // currentLevelData.doorPlacement =
//         //     new ObjectPlacementData(newPrefab.transform.position, newPrefab.transform.rotation);
//     }
//
//     void AddTrap()
//     {
//         GameObject newPrefab = InstantiateNewObject(_currentLevelConfig.trapPrefab);
//         // currentLevelData.trapPlacements.Add(new ObjectPlacementData(newPrefab.transform.position,
//         //     newPrefab.transform.rotation));
//     }
//
//     void SaveLevelData()
//     {
//         if (_currentLevelConfig == null)
//         {
//             Debug.LogWarning("No level data to save. Create or load a level first.");
//             return;
//         }
//         
//         //TODO make sure all requirements are met
//         _currentLevelConfig.levelName = levelName;
//         //
//         // foreach (GameObject obj in FindObjectsOfType<GameObject>())
//         // {
//         //     if (obj.CompareTag("Platform"))
//         //     {
//         //         currentLevelData.platformPlacements.Add(new ObjectPlacementData(obj.transform.position, obj.transform.rotation));
//         //     }
//         // }
//         //
//         
//         string path =
//             AssetDatabase.GenerateUniqueAssetPath($"{levelDirectory}/{_currentLevelConfig.levelName}.asset");
//         AssetDatabase.CreateAsset(_currentLevelConfig, path);
//         AssetDatabase.SaveAssets();
//         AssetDatabase.Refresh();
//         if (File.Exists(path))
//         {
//             // Display success message
//             EditorUtility.DisplayDialog("Level Saved", $"Level '{_currentLevelConfig.levelName}' has been successfully saved.", "OK");
//             Reset();
//
//         }
//         else
//         {
//             // Display error message
//             EditorUtility.DisplayDialog("Error", "Failed to save level data.", "OK");
//         }
//     }
//
//     private void Reset()
//     {
//         _currentLevelConfig = null;
//         levelName = "";
//         EditorGUILayout.HelpBox("Level Saved", MessageType.None);
//     }
// }