using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("IF WE GO WITH JUST ONE LEVEL, HERE ARE THE SETTINGS")]
    [SerializeField] private bool _theresOnlyOneLevel = false;
    [SerializeField] private int _gameSceneIndex = 1;
    
    [Header("Main Menu")]
    [SerializeField] private CanvasGroup _mainMenuCanvasGroup;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _controlsButton;
    [SerializeField] private Button _creditsButton;

    [Header("Controls")]
    [SerializeField] private CanvasGroup _controlsCanvasGroup;
    [SerializeField] private Button _controlsBackButton;
    
    [Header("Credits")]
    [SerializeField] private CanvasGroup _creditsCanvasGroup;
    [SerializeField] private Button _creditsBackButton;

    [Header("Other")]
    [SerializeField] private AudioSource _buttonInteractionAudioSource;
    
    private void Awake()
    {
        _playButton.onClick.AddListener(Play);
        _controlsButton.onClick.AddListener(Controls);
        _creditsButton.onClick.AddListener(Credits);
        _controlsBackButton.onClick.AddListener(Back);
        _creditsBackButton.onClick.AddListener(Back);
        
        SetCanvasGroupActive(_mainMenuCanvasGroup, true);
        SetCanvasGroupActive(_controlsCanvasGroup, false);
        SetCanvasGroupActive(_creditsCanvasGroup, false);
    }

    private void SetCanvasGroupActive(CanvasGroup canvasGroup, bool isActive)
    {
        canvasGroup.alpha = isActive ? 1f : 0f;
        canvasGroup.blocksRaycasts = isActive;
        canvasGroup.interactable = isActive;
    }

    private void Play()
    {
        if (_theresOnlyOneLevel)
        {
            SceneManager.LoadScene("Level1_DownsScene");
            _buttonInteractionAudioSource.Play();
        }
    }

    private void Controls()
    {
        SetCanvasGroupActive(_mainMenuCanvasGroup, false);
        SetCanvasGroupActive(_controlsCanvasGroup, true);
        SetCanvasGroupActive(_creditsCanvasGroup, false);
        _buttonInteractionAudioSource.Play();
    }

    private void Credits()
    {
        SetCanvasGroupActive(_mainMenuCanvasGroup, false);
        SetCanvasGroupActive(_controlsCanvasGroup, false);
        SetCanvasGroupActive(_creditsCanvasGroup, true);
        _buttonInteractionAudioSource.Play();
    }

    private void Back()
    {
        SetCanvasGroupActive(_mainMenuCanvasGroup, true);
        SetCanvasGroupActive(_controlsCanvasGroup, false);
        SetCanvasGroupActive(_creditsCanvasGroup, false);
        _buttonInteractionAudioSource.Play();
    }
}
