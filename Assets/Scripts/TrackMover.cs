using UnityEngine;
using UnityEngine.Splines;

public class TrackMover : MonoBehaviour
{
    public static Vector3 TrackPosition;
    
    [SerializeField] private SplineContainer _track;
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _heightAboveTrack = 1f;
    [SerializeField] private float _stepHeight = .25f;
    [SerializeField] private float _stepFrequency = 1f;
    [SerializeField] private AudioSource _stepAudio;
    [SerializeField] private AudioClip[] _stepAudioClips;
    
    private float _distance;
    private float _normalizedDistance;
    private float _previousPointInStepCycle;
    
    private void Awake()
    {
        _distance += _speed * Time.deltaTime;
        _normalizedDistance = _distance / _track.Spline.CalculateLength(_track.transform.localToWorldMatrix);
        TrackPosition =
            (Vector3)_track.Spline.EvaluatePosition(_normalizedDistance) +
            _track.transform.position;

        if (Physics.Raycast(TrackPosition + Vector3.up * 10, Vector3.down, out var hit))
        {
            TrackPosition = hit.point;
        }

        var pointInStepCycle = Mathf.Sin(_stepFrequency * Time.time);
        TrackPosition += Vector3.up * (_heightAboveTrack + _stepHeight * Mathf.Abs(pointInStepCycle));
        _player.position = TrackPosition;
    }

    private void Update()
    {
        if (!Hud.instance.levelStarted) return;
        if (EndOfLevel.instance.IsShown) return;
        
        _distance += _speed * Time.deltaTime;
        _normalizedDistance = _distance / _track.Spline.CalculateLength(_track.transform.localToWorldMatrix);
        TrackPosition =
            (Vector3)_track.Spline.EvaluatePosition(_normalizedDistance) +
            _track.transform.position;

        if (Physics.Raycast(TrackPosition + Vector3.up * 10, Vector3.down, out var hit))
        {
            TrackPosition = hit.point;
        }

        var pointInStepCycle = Mathf.Sin(_stepFrequency * Time.time);
        TrackPosition += Vector3.up * (_heightAboveTrack + _stepHeight * Mathf.Abs(pointInStepCycle));
        _player.position = TrackPosition;

        var stepOccurred =
            pointInStepCycle >= 0 && _previousPointInStepCycle < 0 ||
            pointInStepCycle < 0 && _previousPointInStepCycle >= 0;
        if (stepOccurred)
        {
            _stepAudio.volume = Random.Range(.6f, 65f);
            _stepAudio.pitch = Random.Range(.95f, 1.05f);
            _stepAudio.PlayOneShot(_stepAudioClips[Random.Range(0, _stepAudioClips.Length)]);
        }

        _previousPointInStepCycle = pointInStepCycle;
    }
}
