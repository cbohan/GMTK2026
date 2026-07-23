using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSnap : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Camera targetCamera;
    [SerializeField] private int imageWidth = 1920;
    [SerializeField] private int imageHeight = 1080;
    [SerializeField] private string fileName = "captured_image.png";
    public void Update()
    {
        Mouse mouse = Mouse.current;
        if (mouse.leftButton.wasPressedThisFrame)
        {
            // 1. Create a temporary RenderTexture
            //RenderTexture renderTexture = new RenderTexture(imageWidth, imageHeight, 24);
            //targetCamera.targetTexture = renderTexture;

            // 2. Render the camera view manually
            targetCamera.Render();

            // 3. Read pixels from the RenderTexture into a Texture2D
            RenderTexture.active = targetCamera.targetTexture;
            Texture2D image = new Texture2D(imageWidth, imageHeight, TextureFormat.RGB24, false);
            Debug.Log($"Capturing image with width {imageWidth} and height {imageHeight}");
            image.ReadPixels(new Rect(0, 0, imageWidth, imageHeight), 0, 0);
            image.Apply();

            // 4. Reset camera and active render texture settings
            //targetCamera.targetTexture = null;
            RenderTexture.active = null;

            // 5. Encode texture to PNG data
            byte[] bytes = image.EncodeToPNG();

            // 6. Save data to the device path
            string path = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllBytes(path, bytes);

            Debug.Log($"Image successfully saved to: {path}");
        }
    }
}
