using UnityEngine;
using UnityEngine.Events;

public class TiggerVolume : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnTriggered;
    
    [SerializeField] private float _radius = 5f;

    private bool _hasBeenTriggered;
    
    private void Update()
    {
        var distance = Vector3.Distance(transform.position, TrackMover.TrackPosition);
        if (distance <= _radius && !_hasBeenTriggered)
        {
            _hasBeenTriggered = true;
            OnTriggered.Invoke();
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0f, 1f,.1f);
        Gizmos.DrawSphere(transform.position, _radius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0f, 0f, 1f,.5f);
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
