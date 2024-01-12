using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.SceneTemplate;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class LevelCreator : EditorWindow
{
    string levelName;
    PlatformController platformPrefab;
    CoinController coinPrefab;
    KeyController keyPrefab;
    DoorController doorPrefab;
    TrapController trapPrefab;

    private string scenesPath = "Assets/Scenes/";
    private string configPath = "Assets/Resources/Config/LevelsConfig/";
    private string levelLayoutPartsPath = "Assets/Resources/LevelLayoutParts/";
    private string sceneTemplatePath = "Assets/Resources/SceneTemplates/LevelTemplate.scenetemplate";

    private LevelConfig _levelConfig;


    [MenuItem("Tools/Level Creator")]
    public static void ShowWindow()
    {
        GetWindow<LevelCreator>("Level Creator");
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        levelName = EditorGUILayout.TextField("Level Name", levelName);
        platformPrefab =
            EditorGUILayout.ObjectField("Platform Prefab", platformPrefab, typeof(PlatformController), false) as
                PlatformController;
        coinPrefab =
            EditorGUILayout.ObjectField("Coin Prefab", coinPrefab, typeof(CoinController), false) as CoinController;
        keyPrefab = EditorGUILayout.ObjectField("Key Prefab", keyPrefab, typeof(KeyController), false) as KeyController;
        doorPrefab =
            EditorGUILayout.ObjectField("Door Prefab", doorPrefab, typeof(DoorController), false) as DoorController;
        trapPrefab =
            EditorGUILayout.ObjectField("Trap Prefab", trapPrefab, typeof(TrapController), false) as TrapController;

        if (GUILayout.Button("Generate Level"))
        {
            bool canGenerateLevel = !String.IsNullOrEmpty(levelName) &&
                                    platformPrefab != null &&
                                    coinPrefab != null &&
                                    keyPrefab != null &&
                                    doorPrefab != null &&
                                    trapPrefab != null;
            if (!canGenerateLevel)
            {
                Debug.LogWarning("Please fill in all the required fields.");
                EditorGUILayout.HelpBox("Please fill in all the required fields.", MessageType.Error);
                return;
            }

            if(LevelSceneExists(levelName))
            {
                Debug.LogWarning("Level with the same name already exists.");
            
                EditorGUILayout.HelpBox("Level with same name exists already.", MessageType.Error);
                return;
            }
            
            GenerateLevel();
        }
    }

    bool LevelSceneExists(string name)
    {
        string[] sceneFiles = Directory.GetFiles(scenesPath, "*.unity");
        return sceneFiles.Any(sceneFile => Path.GetFileNameWithoutExtension(sceneFile) == name);
    }

    void GenerateLevel()
    {
        CreateSceneFromTemplate();
        CreateLevelConfig();
        CreateLevelLayout();
        PlaceDoorAndKey();
        Scene currentScene = SceneManager.GetActiveScene();
        LevelUtils.UpdateSceneObjects(_levelConfig, currentScene);
    }

    private void PlaceDoorAndKey()
    {
        // Get all platforms in the scene
        PlatformController[] platforms = FindObjectsOfType<PlatformController>();


        int platformDoorIndex = Random.Range(0, platforms.Length);
        int platformKeyIndex = platformDoorIndex;
        while(platformKeyIndex == platformDoorIndex)
            platformKeyIndex = Random.Range(0, platforms.Length);

        // Select platforms for door and key
        PlatformController doorPlatform = platforms[platformDoorIndex];
        PlatformController keyPlatform = platforms[platformKeyIndex];

        // Instantiate door and key
        Vector3 doorOffset = new Vector3(Random.Range(-0.8f,0.8f), 0.2f, 0);
        Vector3 keyOffset = new Vector3(Random.Range(-0.8f,0.8f), 1f, 0);
        Vector3 doorPos = GetClampedPosition(doorPlatform.transform.position + doorOffset);
        Vector3 keyPos = GetClampedPosition(keyPlatform.transform.position+ keyOffset);
        Instantiate(doorPrefab, doorPos, doorPrefab.transform.rotation);
        Instantiate(keyPrefab, keyPos, keyPrefab.transform.rotation);
    }

    private Vector3 GetClampedPosition(Vector3 transformPos)
    {
        transformPos.x = Mathf.Clamp(transformPos.x, GameManager.MinGameBounds.x, GameManager.MaxGameBounds.x);
        transformPos.z = Mathf.Clamp(transformPos.z, GameManager.MinGameBounds.z, GameManager.MaxGameBounds.z);
        return transformPos;
    }


    public void CreateSceneFromTemplate()
    {
        // Path to your scene template asset
        // string templatePath = "Assets/SceneTemplates/LevelTemplate.scenetemplate";
        string scenePath = scenesPath + levelName + ".unity";
        
        // Load the scene template
        var sceneTemplate = AssetDatabase.LoadAssetAtPath<SceneTemplateAsset>(sceneTemplatePath);
        if (sceneTemplate == null)
        {
            Debug.LogError("Scene Template not found at " + sceneTemplatePath);
            return;
        }
        SceneTemplateService.Instantiate(sceneTemplate, false, scenePath);
    }


    void CreateLevelConfig()
    {
        _levelConfig = ScriptableObject.CreateInstance<LevelConfig>();
        _levelConfig.levelName = levelName;
        _levelConfig.platformPrefab = platformPrefab;
        _levelConfig.coinPrefab = coinPrefab;
        _levelConfig.keyPrefab = keyPrefab;
        _levelConfig.doorPrefab = doorPrefab;
        _levelConfig.trapPrefab = trapPrefab;
        _levelConfig.sceneName = levelName;

        string assetPath = configPath + levelName + "Config.asset";
        AssetDatabase.CreateAsset(_levelConfig, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"Created Level {levelName} Configuration");
    }
    

 
    void CreateLevelLayout()
    {
        List<GameObject> levelLayouts = GetAllPrefabsAtPath(levelLayoutPartsPath);
        int numberOfLayouts = 3;

        float currentTopY = 0f; // Starting Y position for the first layout
        float minDistance = 1.8f; // Minimum distance between platforms
        float maxDistance = 2.8f; // Maximum distance between platforms

        for (int i = 0; i < numberOfLayouts; i++)
        {
            GameObject selectedLayoutPrefab = levelLayouts[Random.Range(0, levelLayouts.Count)];
            GameObject newLayout = Instantiate(selectedLayoutPrefab);

            float bottomY = FindBottommostPlatformY(newLayout);
            float topY = FindTopmostPlatformY(newLayout);

            float newYPosition = currentTopY + Random.Range(minDistance, maxDistance) + (bottomY * -1);
            newLayout.transform.position = new Vector3(newLayout.transform.position.x, newYPosition, 0);

            currentTopY = newYPosition + topY;
        }
    }

    float FindBottommostPlatformY(GameObject layout)
    {
        float minY = float.MaxValue;

        PlatformController[] platformControllers = layout.GetComponentsInChildren<PlatformController>();
        foreach (PlatformController pc in platformControllers)
        {
            if (pc.transform.position.y < minY)
            {
                minY = pc.transform.position.y;
            }
        }

        return minY;
    }

    float FindTopmostPlatformY(GameObject layout)
    {
        float maxY = float.MinValue;

        PlatformController[] platformControllers = layout.GetComponentsInChildren<PlatformController>();
        foreach (PlatformController pc in platformControllers)
        {
            if (pc.transform.position.y > maxY)
            {
                maxY = pc.transform.position.y;
            }
        }

        return maxY;
    }


    public static List<GameObject> GetAllPrefabsAtPath(string folderPath)
    {
        List<GameObject> prefabs = new List<GameObject>();
        string[] guids = AssetDatabase.FindAssets("t:GameObject", new[] { folderPath });

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab != null)
            {
                prefabs.Add(prefab);
            }
        }

        return prefabs;
    }
  
}