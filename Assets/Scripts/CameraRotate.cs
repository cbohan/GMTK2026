using UnityEngine;

public class CameraPivot : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 5f;
    
    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y + _rotationSpeed * Time.deltaTime, 0f);
    }
}
