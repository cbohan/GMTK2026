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
    private int bestPhotoScore = 0;
    private bool levelActive = true;

    private void Awake()
    {
        instance = this;
        levelActive = true;
    }
    
    private void Update()
    {
        var normalizedBattery = _batteryAmount / 100f;
        var batteryImageIndex = Mathf.FloorToInt(Mathf.Lerp(
            _batterySprites.Length,
            0f,
            normalizedBattery));
        batteryImageIndex = Mathf.Clamp(batteryImageIndex, 0, _batterySprites.Length - 1);
        _batteryImage.sprite = _batterySprites[batteryImageIndex];
        _batteryText.text = $"{Mathf.Max(0, Mathf.RoundToInt(_batteryAmount))}%";
        
        _trashText.text = $"{_trashAmount}";
        
        _batteryAmount -= Time.deltaTime;
        
        if (_batteryAmount <= 0 && levelActive)
        {
            levelActive = false;
            EndOfLevel.instance.Show(bestPhotoScore);
            Cursor.lockState = CursorLockMode.None;
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

    public void TakePicture(int highestRayCount)
    {
        bestPhotoScore = highestRayCount;
        _batteryAmount -= 5f;
    }
}
