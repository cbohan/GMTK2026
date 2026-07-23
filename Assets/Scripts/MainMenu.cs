using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private int _gameSceneIndex = 1;

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
