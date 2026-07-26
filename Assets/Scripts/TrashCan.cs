using System;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    private static readonly List<TrashCan> TrashCans = new();
    private static readonly int Offset = Shader.PropertyToID("_Offset");

    [SerializeField] private Transform _raycastTarget;
    
    private bool _isOpen = false;
    private Material _trashCanMaterial;
    private Vector3 _raycastTargetPosition => _raycastTarget.position;
    
    private void Awake()
    {
        if (TrashCans.Count > 0)
        {
            TrashCans.Clear();
        }
        _trashCanMaterial = GetComponentInChildren<Renderer>().material;
    }

    private void OpenTrashCan()
    {
        _isOpen = true;
        _trashCanMaterial.SetVector(Offset, new Vector4(1, 0, 0, 0));
        Hud.instance.AddTrash();
    }

    private void Start()
    {
        TrashCans.Add(this);
    }
    
    private void OnDestroy()
    {
        TrashCans.Remove(this);
    }

    public static bool HandleTrashInPicture(Vector3 origin, Camera camera)
    {
        bool hitACan = false;
        TrashCan closestHitTrashCan = null;
        foreach (var trashCan in TrashCans)
        {
            if (trashCan._isOpen) continue;

            var viewportPoint = camera.WorldToViewportPoint(trashCan._raycastTargetPosition);
            var trashCanIsInViewport = viewportPoint.x is >= 0 and <= 1 && viewportPoint.y is >= 0 and <= 1;
            if (!trashCanIsInViewport) continue;
            
            var rayToTrashCan = new Ray(origin, trashCan._raycastTargetPosition - origin);
            var distanceToTrash = Vector3.Distance(origin, trashCan._raycastTargetPosition);
            
            if (distanceToTrash > 35) continue;

            if (Physics.Raycast(rayToTrashCan, out var hit, distanceToTrash)) continue;

            if (!closestHitTrashCan ||
                Vector3.Distance(origin, closestHitTrashCan._raycastTargetPosition) > distanceToTrash)
            {
                closestHitTrashCan = trashCan;
            }
        }

        if (closestHitTrashCan)
        {
            closestHitTrashCan.OpenTrashCan();
            hitACan = true;
        }

        return hitACan;
    }
}
