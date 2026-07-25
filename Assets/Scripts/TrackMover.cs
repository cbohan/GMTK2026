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

    private float _distance;
    private float _normalizedDistance;
    
    private void Update()
    {
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
        TrackPosition += Vector3.up * (_heightAboveTrack + _stepHeight * Mathf.Abs(Mathf.Sin(_stepFrequency * Time.time))) ;
        _player.position = TrackPosition;
    }
}
