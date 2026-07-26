using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
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
    public float followerCount => _followerCount;
    [SerializeField] private float _followerCount;
    [SerializeField] private float _followerDrainPerFrame;
    [SerializeField] private TMP_Text _followerText;
    [SerializeField] private TMP_Text _followerEndText;

    [Header("Other")]
    [SerializeField] private TMP_Text _snapText;
    private int bestPhotoScore = 0;
    private bool levelActive = true;
    private float _startingFollowerCount;
    private int _snapTextUptime;
    private void Awake()
    {
        instance = this;
        levelActive = true;
        _startingFollowerCount = _followerCount;
        _snapTextUptime = 0;
    }
    
    private void Update()
    {
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

        _followerText.text = $"{(int)_followerCount}";
        
        // If the level is active, decrement the number of followers and the battery amount
        if (levelActive)
        {
            _batteryAmount -= Time.deltaTime;
            _followerCount -= _followerDrainPerFrame;

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
                levelActive = false;
                _followerEndText.text = $"{(int)_followerCount} followers";
                EndOfLevel.instance.Show(bestPhotoScore, _startingFollowerCount, _followerCount);
                Cursor.lockState = CursorLockMode.None;
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
            _followerCount -= 100f;
        }
        else
        {
            _snapText.text = "<color=\"green\">Nice Shot!</color>";
            _followerCount += (float)hitCount * 100f;
        }
        _snapTextUptime = 100;
        _snapText.alpha = 100f;
        bestPhotoScore = highestRayCount;
        _batteryAmount -= 5f;
    }
}
