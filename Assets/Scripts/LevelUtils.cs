using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public static class LevelUtils
{
    public static void UpdateSceneObjects(LevelConfig config, Scene targetScene)
    {
        foreach (var rootObject in targetScene.GetRootGameObjects())
        {
            UpdateSceneObject<PlatformController>(config.platformPrefab, rootObject);
            UpdateSceneObject<KeyController>(config.keyPrefab, rootObject);
            UpdateSceneObject<TrapController>(config.trapPrefab, rootObject);
            UpdateSceneObject<CoinController>(config.coinPrefab, rootObject);
            UpdateSceneObject<DoorController>(config.doorPrefab, rootObject);
        }
    }

    private static void UpdateSceneObject<T>(T prefab, GameObject rootObject) where T : Component
    {
        if(rootObject==null)
            return;
        
        T[] objects = rootObject.GetComponentsInChildren<T>();

        foreach (T obj in objects)
        {
            Transform parentTransform = obj.transform.parent;

            T newObject = PrefabUtility.InstantiatePrefab(prefab, parentTransform) as T;

            if (newObject != null)
            {
                newObject.transform.localPosition = obj.transform.localPosition;
                newObject.transform.localRotation = obj.transform.localRotation;

                Undo.RegisterCreatedObjectUndo(newObject, $"Created new {typeof(T).Name}");

                // Properly register the old object for undo before destroying it
                Undo.RecordObject(obj.gameObject, $"Destroying {typeof(T).Name}");
                Object.DestroyImmediate(obj.gameObject);
            }
        }
    }
}
