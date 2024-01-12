using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    public static Vector3 MinGameBounds = new Vector3(-10, 0, -5);
    public static Vector3 MaxGameBounds = new Vector3(10, 20, 5);

    public int CoinsCollected { get; private set; }
    public int MaxNumOfLives { get; private set; }
    public int CurrentNumOfLives { get; private set; }
    public bool HasKey { get; private set; }

    public Action LostLifeAction;
    public Action GotKeyAction;
    public Action CoinCollectedAction;
    public Action GameOverLost;
    public Action GameOverWon;
    
    private readonly string _openingSceneName = "HomeScene";
    private readonly string _cutSceneName = "CutScene";
    
    [SerializeField] private GameConfig _config;

    private int _currentLevelIndex = -1;
    private string _sceneWaitingForCutToEnd;
   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ResetGameValues();
    }

    private void ResetGameValues()
    {
        MaxNumOfLives = _config.NumOfLives;
        CurrentNumOfLives = MaxNumOfLives;
        CoinsCollected = 0;
        HasKey = false;
    }
    private void ResetLevelValues()
    {
        HasKey = false;
    }

    public GameConfig GetConfig()
    {
        return _config;
    }
    public void GotKey()
    {
        HasKey = true;
        GotKeyAction?.Invoke();
    }

    public void GotHit()
    {
        if(CurrentNumOfLives==0) return;
        
        CurrentNumOfLives--;
        if (CurrentNumOfLives == 0)
            GameOverLost?.Invoke();
        else
            LostLifeAction?.Invoke();
    }

    public void CollectedCoin()
    {
        CoinsCollected++;
        CoinCollectedAction?.Invoke();
    }

    public void LevelFinished()
    {
        NextLevel();
        ResetLevelValues();
    }

    private void NextLevel()
    {
        _currentLevelIndex++;
        if (_currentLevelIndex >= _config.levels.Length)
        {
           GameOverWon?.Invoke();
           return;
        }
        
        ResetLevelValues();
        string sceneName = _config.levels[_currentLevelIndex].sceneName;
        PlayCutsceneAndLoadScene(sceneName);
    }

    public void OpenLevel(string levelName)
    {
        string sceneName="";
        
        //find level index 
        for (int i = 0; i < _config.levels.Length; i++)
        {
            if (_config.levels[i].levelName == levelName)
            {
                _currentLevelIndex = i;
                sceneName = _config.levels[i].sceneName;
                break;
            }
        }

        sceneName = string.IsNullOrEmpty(sceneName) ? _config.levels[0].sceneName : sceneName;
        PlayCutsceneAndLoadScene(sceneName);
        ResetLevelValues();
    }

    public void ReturnToHomeScreen()
    {
        PlayCutsceneAndLoadScene(_openingSceneName);

        ResetGameValues();
    }

    public void OpenSceneAfterCutScene()
    {
        SceneManager.LoadScene(_sceneWaitingForCutToEnd);
    }
    
    public void PlayCutsceneAndLoadScene(string nextSceneName)
    {
        _sceneWaitingForCutToEnd = nextSceneName;
        SceneManager.LoadScene(_cutSceneName);

    }
    
}