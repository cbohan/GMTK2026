using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform _mainCameraTransform;
    [SerializeField] private float _tuneRotation = 180f;

    private void Start()
    {
        if (Camera.main) _mainCameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (!_mainCameraTransform) return;

        var targetPosition = _mainCameraTransform.position;
        targetPosition.y = transform.position.y; 

        transform.LookAt(targetPosition);
        transform.Rotate(Vector3.up, _tuneRotation);
    }
}