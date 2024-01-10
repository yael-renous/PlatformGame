using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class LevelGenerator1 : EditorWindow
{
    string levelName;
    GameObject platformPrefab;
    GameObject coinPrefab;
    GameObject keyPrefab;
    GameObject doorPrefab;
    GameObject trapPrefab;
    
    
    [MenuItem("Tools/Level Generator")]
    public static void ShowWindow()
    {
        GetWindow<LevelGenerator1>("Level Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        levelName = EditorGUILayout.TextField("Level Name", levelName);
        platformPrefab = EditorGUILayout.ObjectField("Platform Prefab", platformPrefab, typeof(GameObject), false) as GameObject;
        coinPrefab = EditorGUILayout.ObjectField("Coin Prefab", coinPrefab, typeof(GameObject), false) as GameObject;
        keyPrefab = EditorGUILayout.ObjectField("Key Prefab", keyPrefab, typeof(GameObject), false) as GameObject;
        doorPrefab = EditorGUILayout.ObjectField("Door Prefab", doorPrefab, typeof(GameObject), false) as GameObject;
        trapPrefab = EditorGUILayout.ObjectField("Trap Prefab", trapPrefab, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Generate Level"))
        {
            GenerateLevel();
        }
    }

    void GenerateLevel()
    {
        // Create a new scene and set its name
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        newScene.name = levelName;

        PopulateScene();
        
        //create folders in project
        string parentDirectory = "Assets/Levels";
        string path = parentDirectory + "/" + levelName+"/";
        AssetDatabase.CreateFolder(parentDirectory, levelName);
        string scenePath = "Assets/Scenes/" + levelName + ".unity";
        EditorSceneManager.SaveScene(newScene, scenePath);
        
        // Generate and save LevelConfig
        CreateLevelConfig("Assets/LevelsConfig/");
    }

    //todo make smarter
    void PopulateScene()
    {
        // Add Main Camera
        GameObject mainCamera = new GameObject("Main Camera");
        mainCamera.AddComponent<Camera>();
        mainCamera.tag = "MainCamera"; // Set the camera tag
        
        // Add Directional Light
        GameObject directionalLight = new GameObject("Directional Light");
        Light light = directionalLight.AddComponent<Light>();
        light.type = LightType.Directional;
        light.transform.eulerAngles = new Vector3(50, -30, 0); // Adjust these angles as needed

        // Define boundaries
        float minX = -10f, maxX = 10f; // X range
        float minY = -5f, maxY = 5f;   // Y range
        float minZ = -1f, maxZ = 1f;   // Z range (tiny depth)
        
        // Create parent GameObjects for organization
        GameObject platformsParent = new GameObject("Platforms");
        GameObject coinsParent = new GameObject("Coins");
        GameObject keysParent = new GameObject("Keys");
        GameObject trapsParent = new GameObject("Traps");

        // Instantiate Platforms
        int platformCount = Random.Range(2, 6); // Random count between 2 and 5
        for (int i = 0; i < platformCount; i++)
        {
            InstantiatePrefab(platformPrefab, minX, maxX, minY, maxY, minZ, maxZ, platformsParent.transform,GameTags.Platform);
        }

        // Instantiate Coins
        int coinCount = Random.Range(10, 16); // Random count between 10 and 15
        for (int i = 0; i < coinCount; i++)
        {
            InstantiatePrefab(coinPrefab, minX, maxX, minY, maxY, minZ, maxZ, coinsParent.transform,GameTags.Coin);
        }

        // Instantiate a Key
        InstantiatePrefab(keyPrefab, minX, maxX, minY, maxY, minZ, maxZ, keysParent.transform,GameTags.Key);

        // Instantiate Traps
        int trapCount = Random.Range(3, 9); // Random count between 3 and 8
        for (int i = 0; i < trapCount; i++)
        {
            InstantiatePrefab(trapPrefab, minX, maxX, minY, maxY, minZ, maxZ, trapsParent.transform,GameTags.Trap);
        }
    }

    void InstantiatePrefab(GameObject prefab, float minX, float maxX, float minY, float maxY, float minZ, float maxZ, Transform parent, string tag)
    {
        Vector3 position = new Vector3(
            Random.Range(minX, maxX),
            Random.Range(minY, maxY),
            Random.Range(minZ, maxZ)
        );

        GameObject instance = Instantiate(prefab, position, Quaternion.identity);
        instance.transform.SetParent(parent, false);
        instance.tag = tag; // Assign the tag
    }


    void CreateLevelConfig(string path)
    {
        LevelConfig config = ScriptableObject.CreateInstance<LevelConfig>();
        config.levelName = levelName;
        config.platformPrefab = platformPrefab;
        config.coinPrefab = coinPrefab;
        config.keyPrefab = keyPrefab;
        config.doorPrefab = doorPrefab;
        config.trapPrefab = trapPrefab;
        config.sceneName = levelName;
        
        string assetPath =  path+ levelName + "Config.asset";
        AssetDatabase.CreateAsset(config, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }
    
}
