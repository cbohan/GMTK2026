using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private int _gameSceneIndex = 1;

    [Header("Level Select")]
    [SerializeField] private CanvasGroup _levelSelectCanvasGroup;
    
    private void Awake()
    {
        _playButton.onClick.AddListener(Play);
        _creditsButton.onClick.AddListener(Credits);
    }

    private void Play()
    {
        SceneManager.LoadScene(_gameSceneIndex);
    }
    
    private void Credits()
    {
        
    }
}
