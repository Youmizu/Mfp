using UnityEngine;

public class Shop : MonoBehaviour
{
    [Header("Power-Up Panel")]
    public GameObject powerUpPanel;

    public void BuyKnightLeap()
    {
        Debug.Log("Knight Leap Power-Up bought!");
        PowerUpData.boughtKnightLeap = true;
        ShowPowerUps();
    }

    public void BuyShield()
    {
        Debug.Log("Shield Power-Up bought!");
        PowerUpData.boughtShield = true;
        ShowPowerUps();
    }

    public void BuyTeleport()
    {
        Debug.Log("Teleport Power-Up bought!");
        PowerUpData.boughtTeleport = true;
        ShowPowerUps();
    }

    public void BuySwap()
    {
        Debug.Log("Swap Power-Up bought!");
        PowerUpData.boughtSwap = true;
        ShowPowerUps();
    }

    public void BuyRevive()
    {
        Debug.Log("Revive Power-Up bought!");
        PowerUpData.boughtRevive = true;
        ShowPowerUps();
    }

    public void BuyPawnPromotion()
    {
        Debug.Log("Pawn Promotion (Anytime) Power-Up bought!");
        PowerUpData.boughtPawnPromotion = true;
        ShowPowerUps();
    }

    // ✅ Show power-up panel only if any power-up is bought
    private void ShowPowerUps()
    {
        if (powerUpPanel != null)
        {
            bool anyBought =
                PowerUpData.boughtKnightLeap ||
                PowerUpData.boughtShield ||
                PowerUpData.boughtTeleport ||
                PowerUpData.boughtSwap ||
                PowerUpData.boughtRevive ||
                PowerUpData.boughtPawnPromotion;

            powerUpPanel.SetActive(anyBought);
        }
        else
        {
            Debug.LogWarning("powerUpPanel is not assigned in the Inspector.");
        }
    }
}
