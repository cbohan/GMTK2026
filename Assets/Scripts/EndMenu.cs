using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private Button _replayButton;
    [SerializeField] private TMP_Text _finalScore;
    
    private void Start()
    { 
        var followerCount = (int)FollowerTracker.Instance.followerCount;

        Cursor.lockState = CursorLockMode.None;
        _finalScore.text = followerCount == 0 ? "Unfortunately, you just couldn't hack it as a photographer :(" : $"Congratulations! You ended with {followerCount} followers!";
        
        _replayButton.onClick.AddListener(Replay);
    }

    private void Replay()
    {
        FollowerTracker.Instance.followerCount = 1000;
        SceneManager.LoadScene("Level1_DownsScene");
        return;
    }
}
