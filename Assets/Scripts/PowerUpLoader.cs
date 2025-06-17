using UnityEngine;

public class PowerUpLoader : MonoBehaviour
{
    public GameObject powerUpPanel;
    public GameObject knightLeapButton;
    public GameObject shieldButton;
    public GameObject teleportButton;
    public GameObject swapButton;
    public GameObject reviveButton;
    public GameObject pawnPromotionButton;

    void Start()
    {
        bool anyBought = false;

        if (PowerUpData.boughtKnightLeap && knightLeapButton != null)
        {
            knightLeapButton.SetActive(true);
            anyBought = true;
        }

        if (PowerUpData.boughtShield && shieldButton != null)
        {
            shieldButton.SetActive(true);
            anyBought = true;
        }

        if (PowerUpData.boughtTeleport && teleportButton != null)
        {
            teleportButton.SetActive(true);
            anyBought = true;
        }

        if (PowerUpData.boughtSwap && swapButton != null)
        {
            swapButton.SetActive(true);
            anyBought = true;
        }

        if (PowerUpData.boughtRevive && reviveButton != null)
        {
            reviveButton.SetActive(true);
            anyBought = true;
        }

        if (PowerUpData.boughtPawnPromotion && pawnPromotionButton != null)
        {
            pawnPromotionButton.SetActive(true);
            anyBought = true;
        }

        // Set power-up panel active only if something was bought
        if (powerUpPanel != null)
        {
            powerUpPanel.SetActive(anyBought);
        }
    }
}
