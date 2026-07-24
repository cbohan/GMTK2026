using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class TrashThrower : MonoBehaviour
{
    [SerializeField] private Transform _trashThrowOrigin;
    [SerializeField] private InputActionReference _throwAction;
    [SerializeField] private Camera _camera;
    [SerializeField] private Trash _trashPrefab;

    private void Awake()
    {
        Trash.Trashes.Clear();
    }

    private void OnEnable()
    {
        _throwAction.action.Enable();
    }

    private void Update()
    {
        if (!_throwAction.action.WasPressedThisFrame()) return;
        
        var targetPositon = _camera.transform.position + _camera.transform.forward * 10f;
        var ray = _camera.ViewportPointToRay(new Vector3(.5f, .5f, 0f));
        if (Physics.Raycast(ray, out var hit))
        {
            targetPositon = hit.point;
        }
        
        var angle = Ballistics.Solve(_trashThrowOrigin.position, targetPositon);
        var velocity = Ballistics.GetInitialVelocity(_trashThrowOrigin.position, targetPositon, angle);
        
        var trash = Instantiate(_trashPrefab, _trashThrowOrigin.position, Quaternion.identity);
        trash.Initialize(_trashThrowOrigin.position, velocity);
    }
}
