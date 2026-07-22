using System;
using UnityEngine;
using UnityEngine.Splines;

public class TrackMover : MonoBehaviour
{
    [SerializeField] private SplineContainer _track;
    [SerializeField] private Transform _player;
    [SerializeField] private float _speed = 2f;
    [SerializeField] private float _heightAboveTrack = 1f;

    private float _distance;
    private float _normalizedDistance;
    
    private void Update()
    {
        _distance += _speed * Time.deltaTime;
        _normalizedDistance = _distance / _track.Spline.CalculateLength(_track.transform.localToWorldMatrix);
        _player.position = 
            (Vector3)_track.Spline.EvaluatePosition(_normalizedDistance) + 
            _track.transform.position + 
            Vector3.up * _heightAboveTrack;
    }
}
