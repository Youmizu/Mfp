using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    public static PowerUpManager Instance;

    public bool knightLeapUnlocked;
    public bool teleportUnlocked;
    public bool reviveUnlocked;
    public bool promoteUnlocked;
    public bool shieldUnlocked;
    public bool swapUnlocked;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
