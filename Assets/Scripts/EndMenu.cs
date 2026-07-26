using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    [SerializeField] private Button _replayButton;
    [SerializeField] private TMP_Text _finalScore;
    
    private void Awake()
    {
        _finalScore.text = $"Congratulations! You ended with {(int)FollowerTracker.Instance.followerCount} followers!";
        _replayButton.onClick.AddListener(Replay);
    }

    private void Replay()
    {
        FollowerTracker.Instance.followerCount = 1000;
        SceneManager.LoadScene("Level1_DownsScene");
        return;
    }
}
