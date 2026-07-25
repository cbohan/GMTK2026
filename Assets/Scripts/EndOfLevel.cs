using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfLevel : MonoBehaviour
{
    public static EndOfLevel instance { get; private set; }

    public bool IsShown = false;
    
    [SerializeField] private TMP_Text _followersText;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    private void Awake()
    {
        instance = this;
        
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        
        _backToMenuButton.onClick.AddListener(BackToMenu);
    }

    public void Show()
    {
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        IsShown = true;
    }
    
    private void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
