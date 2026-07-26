using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndOfLevel : MonoBehaviour
{
    public static EndOfLevel instance { get; private set; }

    public bool IsShown = false;
    
    [SerializeField] private TMP_Text _followersText;
    [SerializeField] private TMP_Text _topCommentNameText;
    [SerializeField] private TMP_Text _topCommentText;
    [SerializeField] private Button _backToMenuButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private string _nextScene;
    private string[] usernames = new string[]{"SlipstreamWorks", "DookieDealer", "Medley_Lark", "Aether", "NotJim0thy", "Raccoon<3", "Feetpicsonly"};
    private string[] badComments = new string[]{"Not sure if trolling or just dumb", "No Jimothy trash", "git gud spud"};
    private string[] midComments = new string[]{"Wish the pic was a little clearer", "bruh hold the phone still", "ok whatev"};
    private string[] flyComments = new string[]{"Jimothy!!!! OMG I LOV HIMB", "what a cute little guy!!!", "BABY!!!!!1!!"};
    
    private void Awake()
    {
        instance = this;
        
        _canvasGroup.alpha = 0f;
        _canvasGroup.blocksRaycasts = false;
        _canvasGroup.interactable = false;
        
        _backToMenuButton.onClick.AddListener(BackToMenu);
    }

    public void Show(int bestPhotoScore, float startingFollowerCount, float followerCount)
    {
        if (followerCount > startingFollowerCount)
        {
            _followersText.text = $"You gained {(int)(followerCount - startingFollowerCount)} followers!";
        }
        else
        {
            _followersText.text = $"You lost {(int)(startingFollowerCount - followerCount)} followers!";
        }
        _topCommentNameText.text = usernames[Random.Range(0,usernames.Length)];
        if (bestPhotoScore < 5 )
        {
            _topCommentText.text = badComments[Random.Range(0,badComments.Length)];
        }
        else if (bestPhotoScore < 15)
        {
            _topCommentText.text = midComments[Random.Range(0,midComments.Length)];
        }
        else
        {
            _topCommentText.text = flyComments[Random.Range(0,flyComments.Length)];
        }
        _canvasGroup.alpha = 1f;
        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.interactable = true;
        IsShown = true;
    }
    
    private void BackToMenu()
    {
        SceneManager.LoadScene(_nextScene);
    }
}
