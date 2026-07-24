using UnityEngine;

public class MeshBoundsSetter : MonoBehaviour
{
    private void Start()
    {
        var mesh = GetComponent<MeshFilter>().mesh;
            
        var maxBoundDimension = Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z);
        mesh.bounds = new Bounds(
            mesh.bounds.center,
            new Vector3(maxBoundDimension, maxBoundDimension, maxBoundDimension));
        
        enabled = false;
    }
}
