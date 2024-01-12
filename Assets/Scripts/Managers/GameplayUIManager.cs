using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUIManager : MonoBehaviour
{
    //HUD items
    [SerializeField] private TMPro.TMP_Text _numOfCoinsText;
    [SerializeField] private GameObject _lifeParentObject;
    [SerializeField] private Image _keyImage;
    [SerializeField] private Sprite _lifeSprite;
    
    //GameoverScreen
    [SerializeField] private GameObject _gameOverParent;
    [SerializeField] private Image _gameOverBG;
    [SerializeField] private TMPro.TMP_Text _gameOverText;
    [SerializeField] private Button _homeButton;

    private readonly Color _whiteAlpha100 = new Color(1, 1, 1, 1);
    private readonly Color _blackColorAlpha60 = new Color(0, 0, 0, 0.6f);

    private Image[] _lives;
    private int _currentLifeIndex;

    public void Start()
    {
        _gameOverParent.SetActive(false);
        SetupHUDUI();
        RegisterListeners();
    }

    private void OnDestroy()
    {
        UnRegisterListeners();
    }
    private void RegisterListeners()
    {
        GameManager.Instance.GotKeyAction += GotKey;
        GameManager.Instance.LostLifeAction += LostLife;
        GameManager.Instance.CoinCollectedAction += CoinsUpdated;
        GameManager.Instance.GameOverLost += ShowGameLost;
        GameManager.Instance.GameOverWon += ShowGameWon;
        _homeButton.onClick.AddListener(LoadHomeScene);
    }

    private void UnRegisterListeners()
    {
        GameManager.Instance.GotKeyAction -= GotKey;
        GameManager.Instance.LostLifeAction -= LostLife;
        GameManager.Instance.CoinCollectedAction -= CoinsUpdated;
        GameManager.Instance.GameOverLost -= ShowGameLost;
        GameManager.Instance.GameOverWon -= ShowGameWon;
        _homeButton.onClick.RemoveListener(LoadHomeScene);
    }

  
    private void ShowGameWon()
    {
        _gameOverText.text = "YOU WON!";
        Color bgColor = Color.green;
        bgColor.a = 0.6f;
        _gameOverBG.color = bgColor;
        _gameOverParent.SetActive(true);
    }

    private void ShowGameLost()
    {
        _gameOverText.text = "GAME OVER";
        Color bgColor = Color.red;
        bgColor.a = 0.6f;
        _gameOverBG.color = bgColor;
        _gameOverParent.SetActive(true);
    }
    
    
    private void LoadHomeScene()
    {
        GameManager.Instance.ReturnToHomeScreen();
    }

    private void GotKey()
    {
        SetKey(true);
    }

    private void SetKey(bool hasKey)
    {
        _keyImage.color = hasKey ? _whiteAlpha100 : _blackColorAlpha60;
    }

    private void SetupHUDUI()
    {
        _numOfCoinsText.text = GameManager.Instance.CoinsCollected.ToString();
        CreateHUDLives(GameManager.Instance.MaxNumOfLives, GameManager.Instance.CurrentNumOfLives);
        SetKey(GameManager.Instance.HasKey);
    }

    private void CreateHUDLives(int numOfLives, int currentNumOfLives)
    {
        _lives = new Image[numOfLives];
        for (int i = 0; i < numOfLives; i++)
        {
            GameObject life = new GameObject("Heart");
            RectTransform rectTransform = life.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(30, 30);

            life.transform.SetParent(_lifeParentObject.transform, false);
            life.transform.SetAsLastSibling();

            Image heart = life.AddComponent<Image>();
            heart.sprite = _lifeSprite;
            heart.color = _whiteAlpha100;
            _lives[i] = (heart);
        }

        _currentLifeIndex = _lives.Length - 1;
        for (int i = 0; i < numOfLives - currentNumOfLives; i++)
            LostLife();
    }

    private void CoinsUpdated()
    {
        _numOfCoinsText.text = GameManager.Instance.CoinsCollected.ToString();
    }

    private void LostLife()
    {
        if (_currentLifeIndex < 0 && _currentLifeIndex >= _lives.Length)
            return;

        _lives[_currentLifeIndex].color = _blackColorAlpha60;
        _currentLifeIndex--;
    }
}