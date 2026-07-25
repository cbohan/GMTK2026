using UnityEditor;
using UnityEngine;

// Pads the imported bounds of billboard meshes so Unity's built-in terrain
// tree renderer (which culls straight off the shared mesh asset and never
// runs any per-instance MonoBehaviour) doesn't cull them early. The
// Billboard shader rotates vertices to face the camera, so the mesh's
// natural bounds don't cover the actual on-screen extent once rotated.
public class BillboardMeshBoundsPostprocessor : AssetPostprocessor
{
    private static readonly string[] BillboardMeshPaths =
    {
        "Assets/Art/plane.blend",
        "Assets/Art/planeInGround.blend",
        "Assets/Art/plane_fbx.fbx",
        "Assets/Art/planeInGround_fbx.fbx",
    };

    private void OnPostprocessModel(GameObject root)
    {
        if (System.Array.IndexOf(BillboardMeshPaths, assetPath) < 0)
            return;

        foreach (var meshFilter in root.GetComponentsInChildren<MeshFilter>())
        {
            var mesh = meshFilter.sharedMesh;
            if (mesh == null)
                continue;

            var maxDimension = Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.y, mesh.bounds.size.z);
            mesh.bounds = new Bounds(mesh.bounds.center, Vector3.one * maxDimension);
        }
    }
}
