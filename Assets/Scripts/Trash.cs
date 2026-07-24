using System;
using UnityEngine;

public class Trash : MonoBehaviour
{
    private Vector3 _start;
    private Vector3 _initialVelocity;
    private float _throwTime;
    private bool _hitGround;

    public void Initialize(Vector3 start, Vector3 initialVelocity)
    {
        _start = start;
        _initialVelocity = initialVelocity;
        _throwTime = Time.time;
        
        transform.position = _start;
    }

    private void Update()
    {
        if (_hitGround) return;
        
        var t = Time.time - _throwTime;
        var nextPosition = Ballistics.Evaluate(_start, _initialVelocity, t);
        var toNextPosition = new Ray(transform.position, nextPosition - transform.position);
        var distanceToNextPosition = (nextPosition - transform.position).magnitude;
        if (Physics.Raycast(toNextPosition, out var hit, distanceToNextPosition))
        {
            _hitGround = true;
            transform.position = hit.point;
        }
        else
        {
            transform.position = nextPosition;   
        }
    }
}
