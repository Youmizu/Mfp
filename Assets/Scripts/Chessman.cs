using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessman : MonoBehaviour
{
    public GameObject controller;
    public GameObject movePlate;
    public bool isShielded = false;


    private int xBoard = -1;
    private int yBoard = -1;

    public string player; // public so Game.cs can assign it
    public bool hasShield = false;

    private Game game;

    public void SetGameObject(GameObject g)
    {
        game = g.GetComponent<Game>();
    }

    public string GetColor()
    {
        return name.Contains("white") ? "white" : "black";
    }

    // Sprite references
    public Sprite black_queen, black_knight, black_bishop, black_king, black_rook, black_pawn;
    public Sprite white_queen, white_knight, white_bishop, white_king, white_rook, white_pawn;

    public void Activate()
    {
        controller = GameObject.FindGameObjectWithTag("GameController");

        SetCoords();

        // Set the sprite based on name
        switch (this.name)
        {
            case "black_queen": GetComponent<SpriteRenderer>().sprite = black_queen; player = "black"; break;
            case "black_knight": GetComponent<SpriteRenderer>().sprite = black_knight; player = "black"; break;
            case "black_bishop": GetComponent<SpriteRenderer>().sprite = black_bishop; player = "black"; break;
            case "black_king": GetComponent<SpriteRenderer>().sprite = black_king; player = "black"; break;
            case "black_rook": GetComponent<SpriteRenderer>().sprite = black_rook; player = "black"; break;
            case "black_pawn": GetComponent<SpriteRenderer>().sprite = black_pawn; player = "black"; break;
            case "white_queen": GetComponent<SpriteRenderer>().sprite = white_queen; player = "white"; break;
            case "white_knight": GetComponent<SpriteRenderer>().sprite = white_knight; player = "white"; break;
            case "white_bishop": GetComponent<SpriteRenderer>().sprite = white_bishop; player = "white"; break;
            case "white_king": GetComponent<SpriteRenderer>().sprite = white_king; player = "white"; break;
            case "white_rook": GetComponent<SpriteRenderer>().sprite = white_rook; player = "white"; break;
            case "white_pawn": GetComponent<SpriteRenderer>().sprite = white_pawn; player = "white"; break;
        }
    }

    public void SetCoords()
    {
        float x = xBoard * 0.66f - 2.3f;
        float y = yBoard * 0.66f - 2.3f;
        transform.position = new Vector3(x, y, -1.0f);
    }

    public int GetXBoard() => xBoard;
    public int GetYBoard() => yBoard;
    public void SetXBoard(int x) => xBoard = x;
    public void SetYBoard(int y) => yBoard = y;

    public void ActivateShield() => hasShield = true;
    public bool HasShield() => hasShield;
    public void UseShield() => hasShield = false;

    private void OnMouseUp()
    {
        Game game = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        game = controller.GetComponent<Game>();
        if (game.IsGameOver()) return;

        if (game.GetCurrentPlayer() != player) return;

        Game.PowerUpType selectedPower = game.GetSelectedPowerUp();

        if (selectedPower != Game.PowerUpType.None)
        {
            switch (selectedPower)
            {
                case Game.PowerUpType.KnightLeap:
                    if (name.Contains("knight"))
                    {
                        DestroyMovePlates();
                        ActivateKnightLeap();
                    }
                    break;

                case Game.PowerUpType.PromotePawn:
                    if (name.Contains("pawn"))
                    {
                        game.PromotePawnAnytime(gameObject, "queen"); // Promote to queen by default
                    }
                    break;

                case Game.PowerUpType.Teleport:
                    DestroyMovePlates();
                    game.GenerateTeleportTargets(this.gameObject);
                    break;

                case Game.PowerUpType.Swap:
                    DestroyMovePlates();
                    game.GenerateSwapTargets(this.gameObject);
                    break;

                case Game.PowerUpType.Revive:
                    if (game.CanRevive(player) && game.capturedPieces[player].Count > 0)
                    {
                        string capturedPieceName = game.capturedPieces[player][0];

                        // Try to revive on player's side randomly
                        int maxTries = 50;
                        int x = -1;
                        int y = -1;
                        bool found = false;

                        for (int i = 0; i < maxTries; i++)
                        {
                            x = Random.Range(0, 8);
                            y = (player == "white") ? Random.Range(0, 4) : Random.Range(4, 8);

                            if (game.PositionOnBoard(x, y) && game.GetPosition(x, y) == null)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (found)
                        {
                            game.ReviveChessman(capturedPieceName, player, x, y);
                            game.capturedPieces[player].RemoveAt(0);
                        }
                        else
                        {
                            Debug.LogWarning("No spot to revive found after many tries.");
                        }
                    }
                    break;

            }

            game.UsePowerUp(selectedPower);
            return;
        }

        if (game.CanRevive(player) && game.capturedPieces[player].Count > 0)
        {
            string pieceName = game.capturedPieces[player][0];

            int maxTries = 50;
            int x = -1;
            int y = -1;
            bool found = false;

            for (int i = 0; i < maxTries; i++)
            {
                x = Random.Range(0, 8);
                y = (player == "white") ? Random.Range(0, 4) : Random.Range(4, 8);

                if (game.PositionOnBoard(x, y) && game.GetPosition(x, y) == null)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                game.ReviveChessman(pieceName, player, x, y);
                game.capturedPieces[player].RemoveAt(0);
            }
            else
            {
                Debug.LogWarning("No empty spot found for revival after many tries.");
            }
        }


        // Normal move logic
        DestroyMovePlates();
        InitiateMovePlates();
    }

    public void DestroyMovePlates()
    {
        GameObject[] movePlates = GameObject.FindGameObjectsWithTag("MovePlate");
        foreach (GameObject mp in movePlates) Destroy(mp);
    }

    public void InitiateMovePlates()
    {
        switch (name)
        {
            case "black_queen":
            case "white_queen":
                LineMovePlate(1, 0); LineMovePlate(0, 1); LineMovePlate(1, 1);
                LineMovePlate(-1, 0); LineMovePlate(0, -1); LineMovePlate(-1, -1);
                LineMovePlate(-1, 1); LineMovePlate(1, -1);
                break;
            case "black_knight":
            case "white_knight":
                LMovePlate();
                break;
            case "black_bishop":
            case "white_bishop":
                LineMovePlate(1, 1); LineMovePlate(1, -1);
                LineMovePlate(-1, 1); LineMovePlate(-1, -1);
                break;
            case "black_king":
            case "white_king":
                SurroundMovePlate();
                break;
            case "black_rook":
            case "white_rook":
                LineMovePlate(1, 0); LineMovePlate(0, 1);
                LineMovePlate(-1, 0); LineMovePlate(0, -1);
                break;
            case "black_pawn":
                PawnMovePlate(xBoard, yBoard - 1);
                if (yBoard == 6) PawnMovePlate(xBoard, yBoard - 2); // starting row
                break;
            case "white_pawn":
                PawnMovePlate(xBoard, yBoard + 1);
                if (yBoard == 1) PawnMovePlate(xBoard, yBoard + 2); // starting row
                break;

        }
    }

    public void LineMovePlate(int xInc, int yInc)
    {
        Game sc = controller.GetComponent<Game>();
        int x = xBoard + xInc, y = yBoard + yInc;
        while (sc.PositionOnBoard(x, y) && sc.GetPosition(x, y) == null)
        {
            MovePlateSpawn(x, y);
            x += xInc; y += yInc;
        }
        if (sc.PositionOnBoard(x, y))
        {
            GameObject cp = sc.GetPosition(x, y);
            if (cp != null && cp.GetComponent<Chessman>().player != player)
                MovePlateAttackSpawn(x, y);
        }
    }

    public void LMovePlate()
    {
        PointMovePlate(xBoard + 1, yBoard + 2);
        PointMovePlate(xBoard - 1, yBoard + 2);
        PointMovePlate(xBoard + 2, yBoard + 1);
        PointMovePlate(xBoard + 2, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard - 2);
        PointMovePlate(xBoard - 1, yBoard - 2);
        PointMovePlate(xBoard - 2, yBoard + 1);
        PointMovePlate(xBoard - 2, yBoard - 1);
    }

    public void SurroundMovePlate()
    {
        PointMovePlate(xBoard, yBoard + 1);
        PointMovePlate(xBoard, yBoard - 1);
        PointMovePlate(xBoard - 1, yBoard);
        PointMovePlate(xBoard + 1, yBoard);
        PointMovePlate(xBoard - 1, yBoard + 1);
        PointMovePlate(xBoard - 1, yBoard - 1);
        PointMovePlate(xBoard + 1, yBoard + 1);
        PointMovePlate(xBoard + 1, yBoard - 1);
    }

    public void PointMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (!sc.PositionOnBoard(x, y)) return;

        GameObject cp = sc.GetPosition(x, y);
        if (cp == null)
            MovePlateSpawn(x, y);
        else if (cp.GetComponent<Chessman>().player != player)
            MovePlateAttackSpawn(x, y);
    }

    public void PawnMovePlate(int x, int y)
    {
        Game sc = controller.GetComponent<Game>();
        if (!sc.PositionOnBoard(x, y)) return;

        if (sc.GetPosition(x, y) == null)
            MovePlateSpawn(x, y);

        if (sc.PositionOnBoard(x + 1, y) && sc.GetPosition(x + 1, y) != null &&
            sc.GetPosition(x + 1, y).GetComponent<Chessman>().player != player)
            MovePlateAttackSpawn(x + 1, y);

        if (sc.PositionOnBoard(x - 1, y) && sc.GetPosition(x - 1, y) != null &&
            sc.GetPosition(x - 1, y).GetComponent<Chessman>().player != player)
            MovePlateAttackSpawn(x - 1, y);
    }

    public void MovePlateSpawn(int x, int y)
    {
        float px = x * 0.66f - 2.3f;
        float py = y * 0.66f - 2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(px, py, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(x, y);
    }

    public void MovePlateAttackSpawn(int x, int y)
    {
        float px = x * 0.66f - 2.3f;
        float py = y * 0.66f - 2.3f;

        GameObject mp = Instantiate(movePlate, new Vector3(px, py, -3.0f), Quaternion.identity);
        MovePlate mpScript = mp.GetComponent<MovePlate>();
        mpScript.attack = true;
        mpScript.SetReference(gameObject);
        mpScript.SetCoords(x, y);
    }

    public void ActivateKnightLeap()
    {
        MovePlateSpawn(xBoard + 2, yBoard + 1);
        MovePlateSpawn(xBoard + 2, yBoard - 1);
        MovePlateSpawn(xBoard - 2, yBoard + 1);
        MovePlateSpawn(xBoard - 2, yBoard - 1);
        MovePlateSpawn(xBoard + 1, yBoard + 2);
        MovePlateSpawn(xBoard + 1, yBoard - 2);
        MovePlateSpawn(xBoard - 1, yBoard + 2);
        MovePlateSpawn(xBoard - 1, yBoard - 2);
    }

    public void ApplyShieldToPiece(GameObject piece)
    {
        // Example behavior: mark the piece with a shield
        Chessman cm = piece.GetComponent<Chessman>();
        cm.isShielded = true;
        Debug.Log($"{cm.name} is now shielded!");
    }


}
