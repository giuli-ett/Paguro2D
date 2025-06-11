using UnityEngine;

public class Hand : MonoBehaviour
{
    public static Hand Instance;
    public bool isSmashing = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("🛑 Hai più di una mano nella scena!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    public void OnSmashStart()
    {
        isSmashing = true;
    }

    public void OnSmashEnd()
    {
        isSmashing = false;
    }
}
