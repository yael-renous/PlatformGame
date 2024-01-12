using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutSceneManager : MonoBehaviour
{
    
    [SerializeField] private PlayableDirector director;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForCutscene(director.duration));
    }

    IEnumerator WaitForCutscene(double duration) {
        yield return new WaitForSeconds((float)duration);
        GameManager.Instance.OpenSceneAfterCutScene();
    }
}
