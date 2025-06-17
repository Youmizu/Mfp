using UnityEngine;
using UnityEngine.UI;

public class PowerUpUIController : MonoBehaviour
{

    public Game game;


    public void OnTeleportPressed()
    {
        Debug.Log("Teleport Power-Up Activated!");
        // Your logic to enable teleport targets
    }

    public void OnPromotePawnPressed()
    {
        Debug.Log("Promote Pawn Power-Up Activated!");
        // Logic to promote pawn anytime
    }

    public void OnShieldPressed()
    {
        Debug.Log("Shield Power-Up Activated!");
        // Logic to shield a chess piece
    }

    public void OnKnightLeapPressed()
    {
        Debug.Log("Knight Leap Power-Up Activated!");
        // Logic to teleport like a knight
    }

    public void OnRevivePressed()
    {
        Debug.Log("Revive Power-Up Activated!");
        // Logic to revive a defeated piece
    }

    public void OnSwapPressed()
    {
        Debug.Log("Swap Power-Up Activated!");
        // Logic to swap positions of two friendly pieces
    }
}
