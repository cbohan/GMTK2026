using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectCard : MonoBehaviour
{
    [SerializeField] private int _requiredFollowers;
    [SerializeField] private TMP_Text _requiredFollowersText;
    [SerializeField] private Button _goButton;
    [SerializeField] private int _sceneIndex;
    [SerializeField] private GameObject _lockedGo;

    private void Start()
    {
        var followers = PlayerPrefs.GetInt("Followers", 0);
        _requiredFollowersText.text = $"Required Followers: {_requiredFollowers}";
        _goButton.interactable = followers >= _requiredFollowers;
        _goButton.onClick.AddListener(Go);
        _lockedGo.SetActive(followers < _requiredFollowers);
    }
    
    private void Go()
    {
        var followers = PlayerPrefs.GetInt("Followers", 0);
        if (followers < _requiredFollowers) return;
        SceneManager.LoadScene(_sceneIndex);
    }
}
