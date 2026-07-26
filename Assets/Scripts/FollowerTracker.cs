using UnityEngine;

public class FollowerTracker : MonoBehaviour
{
    // Start() and Update() methods deleted - we don't need them right now

    public static FollowerTracker Instance;

    public float followerCount;

    private void Awake()
{
    // start of new code
    if (Instance != null)
    {
        Destroy(gameObject);
        return;
    }
    // end of new code

    Instance = this;
    DontDestroyOnLoad(gameObject);
}
}