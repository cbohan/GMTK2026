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
    [SerializeField] private Button _creditsButton;

    [Header("Level Select")]
    [SerializeField] private CanvasGroup _levelSelectCanvasGroup;
    
    private void Awake()
    {
        _playButton.onClick.AddListener(Play);
        _creditsButton.onClick.AddListener(Credits);
        
        // _mainMenuCanvasGroup.alpha = 1f;
        // _mainMenuCanvasGroup.blocksRaycasts = true;
        // _mainMenuCanvasGroup.interactable = true;
        
        // _levelSelectCanvasGroup.alpha = 0f;
        // _levelSelectCanvasGroup.blocksRaycasts = false;
        // _levelSelectCanvasGroup.interactable = false;
    }

    private void Play()
    {
        if (_theresOnlyOneLevel)
        {
            SceneManager.LoadScene("Level1_DownsScene");
            return;
        }

        // _mainMenuCanvasGroup.alpha = 0f;
        // _mainMenuCanvasGroup.blocksRaycasts = false;
        // _mainMenuCanvasGroup.interactable = false;
        
        // _levelSelectCanvasGroup.alpha = 1f;
        // _levelSelectCanvasGroup.blocksRaycasts = true;
        // _levelSelectCanvasGroup.interactable = true;
    }
    
    private void Credits()
    {
        
    }
}
