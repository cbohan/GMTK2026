using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    private const float FollowerDrainPerSecond = 15f;
    
    public static Hud instance { get; private set; }
    
    [Header("Battery")]
    [SerializeField] private Image _batteryImage;
    [SerializeField] private Sprite[] _batterySprites;
    [SerializeField] private float _batteryAmount;
    [SerializeField] private TMP_Text _batteryText;

    [Header("Trash")] 
    public int TrashAmount => _trashAmount;
    [SerializeField] private int _trashAmount;
    [SerializeField] private TMP_Text _trashText;

    [Header("Followers")]
    [SerializeField] private TMP_Text _followerText;
    [SerializeField] private TMP_Text _followerEndText;

    [Header("Other")]
    public bool levelStarted;
    [SerializeField] private RawImage _levelNewspaper;
    [SerializeField] private TMP_Text _snapText;
    private int bestPhotoScore = 0;
    private float _startingFollowerCount;
    private int _snapTextUptime;
    private void Start()
    {
        instance = this;
        _startingFollowerCount = FollowerTracker.Instance.followerCount;
        _snapTextUptime = 0;
        _snapText.text = "";
        _followerText.text = $"{(int)FollowerTracker.Instance.followerCount}";
        levelStarted = false;
    }
    
    private void Update()
    {
        if (!levelStarted)
        {
            Mouse mouse = Mouse.current;
            if (mouse.leftButton.wasPressedThisFrame)
            {
                StartCoroutine(StartMoving(2.5f)); 
                _levelNewspaper.CrossFadeAlpha(0.0f,1,false);
            }
            else
            {
                return;
            }
        }

        // Display the correct battery icon in the hud based on the percentage of the battery remaining
        var normalizedBattery = _batteryAmount / 100f;
        var batteryImageIndex = Mathf.FloorToInt(Mathf.Lerp(
            _batterySprites.Length,
            0f,
            normalizedBattery));
        batteryImageIndex = Mathf.Clamp(batteryImageIndex, 0, _batterySprites.Length - 1);
        _batteryImage.sprite = _batterySprites[batteryImageIndex];
        _batteryText.text = $"{Mathf.Max(0, Mathf.RoundToInt(_batteryAmount))}%";
        
        // Display the amount of trash left to throw
        _trashText.text = $"{_trashAmount}";

        _followerText.text = $"{(int)FollowerTracker.Instance.followerCount}";
        
        // If the level is active, decrement the number of followers and the battery amount
        if (!EndOfLevel.instance.IsShown)
        {
            _batteryAmount -= Time.deltaTime;
            FollowerTracker.Instance.followerCount -= FollowerDrainPerSecond * Time.deltaTime;

            if (_snapTextUptime > 0)
            {
                _snapTextUptime--;
            }
            else
            {
                _snapText.alpha = 0f;
            }
            
            // When the battery hits zero, freeze player controls and show the end-of-level screen
            if (_batteryAmount <= 0)
            {
                _followerEndText.text = $"{(int)FollowerTracker.Instance.followerCount} followers";
                EndOfLevel.instance.Show(bestPhotoScore, _startingFollowerCount, FollowerTracker.Instance.followerCount);
                Cursor.lockState = CursorLockMode.None;
            }
            
            // When follower hits zero, go to the end menu
            if (FollowerTracker.Instance.followerCount <= 0)
            {
                FollowerTracker.Instance.followerCount = 0;
                SceneManager.LoadScene("EndingScene");
            }
        }
    }

    public void ThrowTrash()
    {
        _trashAmount--;
    }

    public void AddTrash()
    {
        _trashAmount += 2;
    }

    public void TakePicture(int hitCount, int highestRayCount, bool trashCan)
    {
        if (trashCan)
        {
            _snapText.text = "<color=\"yellow\">Got some trash!</color>";
        }
        else if (hitCount == 0)
        {
            _snapText.text = "<color=\"red\">No Jimothy in the photo!</color>";
            FollowerTracker.Instance.followerCount -= 100f;
        }
        else
        {
            _snapText.text = "<color=\"green\">Nice Shot!</color>";
            FollowerTracker.Instance.followerCount += (float)hitCount * 100f;
        }
        _snapTextUptime = 100;
        _snapText.alpha = 100f;
        bestPhotoScore = highestRayCount;
        _batteryAmount -= 5f;
    }

    private IEnumerator StartMoving(float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        levelStarted = true;
    }
}
