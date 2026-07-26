using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trash : MonoBehaviour
{
    public static List<Trash> Trashes = new();
    
    public Vector3 Position => transform.position;

    [SerializeField] private ParticleSystem _onEatParticleSystem;
    
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
            Trashes.Add(this);
        }
        else
        {
            transform.position = nextPosition;   
        }
    }
    
    public IEnumerator Eat()
    {
        var eatPs = Instantiate(_onEatParticleSystem);
        eatPs.transform.position = transform.position;
        eatPs.Play();
        
        yield return new WaitForSeconds(1.5f);
        
        if (!gameObject) yield break; 
        
        Destroy(gameObject);
        Trashes.Remove(this);
    }
}
