using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraSnap : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private int imageWidth = 600;
    [SerializeField] private int imageHeight = 900;
    [SerializeField] private string fileName = "captured_image.png";
    [SerializeField] private RawImage bestPhoto;
    private LayerMask layerMask;
    private int highestRayCount = 0;
    private bool photoTaken = false;
    void Awake()
    {
        // Grab the integer values for the layer masks of Jimothy and everything else in the game
        layerMask = LayerMask.GetMask("Jimothy", "Obstacle", "Terrain");
    }
    public void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            // Force a render of the phone camera source and set the RenderTexture to the texture used in the phone, which is needed for ReadPixels to know what to sample from
            targetCamera.Render();
            RenderTexture.active = targetCamera.targetTexture;

            // Create an image texture and capture what the phone camera sees
            Texture2D image = new Texture2D(imageWidth, imageHeight, TextureFormat.RGBA32, false);
            image.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
            image.Apply();

            // Set the RenderTexture back to null, otherwise the screen will just show the phone camera view
            RenderTexture.active = null;

            // Create a PNG image of the image texture and save it off to local cache (for debugging only)
            // byte[] bytes = image.EncodeToPNG();
            // string path = Path.Combine(Application.persistentDataPath, fileName);
            // File.WriteAllBytes(path, bytes);
            // Debug.Log($"Image successfully saved to: {path}");

            // Send out an array of Raycasts from the position of the camera and calculate how much of the image was Jimothy by percentage
            RaycastHit hit;
            int hitCount = 0;
            for (float x = -0.5f; x < 0.5f; x+=0.1f)
            {
                for (float y = -0.8f; y < 0.8f; y+=0.1f)
                {
                    Vector3 testDirection = new Vector3(x,y,(2.0f - Mathf.Abs(x)));
                    if (Physics.Raycast(transform.position, transform.TransformDirection(testDirection), out hit, Mathf.Infinity, layerMask))
                    {
                        if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Jimothy"))
                        {
                            hitCount++;
                            // Debug.DrawRay(transform.position, transform.TransformDirection(testDirection) * hit.distance, Color.green, 10.0f);
                        }
                        // else if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Terrain"))
                        // {
                        //     Debug.DrawRay(transform.position, transform.TransformDirection(testDirection) * hit.distance, Color.red, 10.0f);
                        // }
                    }
                    
                }
            }
            Debug.Log($"{hitCount} rays hit Jimothy");

            if ((!photoTaken) || (hitCount > highestRayCount))
            {
                photoTaken = true;
                highestRayCount = hitCount;
                bestPhoto.texture = image;
                Debug.Log($"Best photo updated");
            }

            Hud.instance.TakePicture(highestRayCount);
        }
    }
}
