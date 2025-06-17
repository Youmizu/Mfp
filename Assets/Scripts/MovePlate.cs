using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlate : MonoBehaviour
{
    // Reference to Game controller
    public GameObject controller;

    // The Chessman that created this MovePlate
    GameObject reference = null;

    // Coordinates on the board
    int matrixX;
    int matrixY;

    // False = move, True = attack
    public bool attack = false;

    public void Start()
    {
        if (attack)
        {
            // Red color for attack plates
            gameObject.GetComponent<SpriteRenderer>().color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    void OnMouseUp()
    {
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        Chessman cp = reference.GetComponent<Chessman>();

        // ✅ Check if move is valid before doing anything
        if (!game.PositionOnBoard(matrixX, matrixY))
        {
            Debug.LogWarning($"Invalid move: trying to move to ({matrixX},{matrixY})");
            return;
        }

        // Remove piece from current position
        game.SetPositionEmpty(cp.GetXBoard(), cp.GetYBoard());

        // Set new position
        cp.SetXBoard(matrixX);
        cp.SetYBoard(matrixY);
        cp.SetCoords();

        // Handle attacks
        // Handle attacks
        if (attack)
        {
            GameObject target = game.GetPosition(matrixX, matrixY);
            if (target != null)
            {
                Chessman targetCp = target.GetComponent<Chessman>();
                if (targetCp.HasShield())
                {
                    targetCp.UseShield(); // Shield blocks the hit
                }
                else
                {
                    game.RecordCapture(target); // ✅ Log the capture
                    Destroy(target); // ✅ Remove from board
                }
            }
        }


        // Place the piece at the new position
        Debug.Log($"Trying to SetPosition for piece at ({cp.GetXBoard()}, {cp.GetYBoard()})");
        game.SetPosition(cp.gameObject);

        // Cleanup move plates
        cp.DestroyMovePlates();

        // Switch turn
        game.NextTurn();
    }

    public void SetCoords(int x, int y)
    {
        matrixX = x;
        matrixY = y;
    }

    public void SetReference(GameObject obj)
    {
        reference = obj;
    }

    public GameObject GetReference()
    {
        return reference;
    }

}
