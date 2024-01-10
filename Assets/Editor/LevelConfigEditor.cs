using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelConfig config = (LevelConfig) target;

        if (SceneManager.GetActiveScene().name == config.sceneName)
        {
            if (GUILayout.Button("Update"))
            {
                UpdateSceneObjects(config);
            }
        }
        else
        {
            if (GUILayout.Button("Open Scene and Update"))
            {
                if (!string.IsNullOrEmpty(config.sceneName))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(
                            "Assets/Scenes/" + config.levelName + "/" + config.sceneName + ".unity",
                            OpenSceneMode.Single);
                        UpdateSceneObjects(config);
                    }
                }
                else
                {
                    Debug.LogError("Scene name is not set in LevelConfig.");
                }
            }
        }
    }

    private void UpdateSceneObjects(LevelConfig config)
    {
        var scene = SceneManager.GetActiveScene();
        foreach (var rootObject in scene.GetRootGameObjects())
        {
            UpdateObjectWithTag(rootObject, GameTags.Platform, config.platformPrefab);
            UpdateObjectWithTag(rootObject, GameTags.Coin, config.coinPrefab);
            UpdateObjectWithTag(rootObject, GameTags.Trap, config.trapPrefab);
            UpdateObjectWithTag(rootObject, GameTags.Key, config.keyPrefab);
            UpdateObjectWithTag(rootObject, GameTags.Door, config.doorPrefab);
        }
    }

    private void UpdateObjectWithTag(GameObject rootObject, string tag, GameObject prefab)
    {
        if (prefab == null) return;

        foreach (var childObject in rootObject.GetComponentsInChildren<Transform>(true))
        {
            if (childObject!=null && childObject.gameObject.CompareTag(tag))
            {
                GameObject newObject = PrefabUtility.InstantiatePrefab(prefab, childObject.parent) as GameObject;
                newObject.transform.localPosition = childObject.localPosition;
                newObject.transform.localRotation = childObject.localRotation;
                newObject.tag = tag;
                Undo.RegisterCreatedObjectUndo(newObject, "Created new " + tag);

                Undo.DestroyObjectImmediate(childObject.gameObject);
            }
        }
    }
}
