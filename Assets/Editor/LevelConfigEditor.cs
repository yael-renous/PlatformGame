using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LevelConfig))]
public class LevelConfigEditor : Editor
{
    private LevelConfig _config;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _config = (LevelConfig) target;

        if (SceneManager.GetActiveScene().name == _config.sceneName)
        {
            if (GUILayout.Button("Update"))
            {
                UpdateSceneObjects();
            }
        }
        else
        {
            if (GUILayout.Button("Open Scene and Update"))
            {
                if (!string.IsNullOrEmpty(_config.sceneName))
                {
                    if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        EditorSceneManager.OpenScene(
                            "Assets/Scenes/" + _config.sceneName + ".unity",
                            OpenSceneMode.Single);
                        UpdateSceneObjects();
                    }
                }
                else
                {
                    Debug.LogError("Scene name is not set in LevelConfig.");
                }
            }
        }
    }

    private void UpdateSceneObjects()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        LevelUtils.UpdateSceneObjects(_config, currentScene);
    }
}
