using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStarter : MonoBehaviour
{
    void Awake()
    {
        var spawn = GameObject.Find("PlayerSpawnPoint");
        if (spawn != null && Player.Instance != null)
        {
            Player.Instance.transform.position = spawn.transform.position;
        }
        else
        {
            Debug.LogWarning("Spawn point o Player non trovati.");
        }
    }
}

