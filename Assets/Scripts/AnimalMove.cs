using System;
using UnityEngine;

public class AnimalMove : MonoBehaviour
{
    public Vector3 Position => transform.position;
    
    // Move to this target position at this speed and wait for wait duration seconds
    public float MoveSpeed = 3f;
    public float WaitDuration = 1f;

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0f, 0f, .5f);
        Gizmos.DrawSphere(Position, .25f);
    }
}
