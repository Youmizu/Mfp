using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public Button knightLeapButton;
    public Button pawnPromotionButton;
    public Button teleportButton;
    public Button swapButton;
    public Button reviveButton;
    public Button shieldButton;

    public GameObject chesspiece;
    private GameObject[,] positions = new GameObject[8, 8];
    private GameObject[] playerBlack = new GameObject[16];
    private GameObject[] playerWhite = new GameObject[16];

    private string currentPlayer = "white";
    private bool gameOver = false;

    public Dictionary<string, List<string>> capturedPieces = new Dictionary<string, List<string>>()
{
    { "white", new List<string>() },
    { "black", new List<string>() }
};

    public void Start()
    {
        playerWhite = new GameObject[] {
            Create("white_rook", 0, 0), Create("white_knight", 1, 0), Create("white_bishop", 2, 0),
            Create("white_queen", 3, 0), Create("white_king", 4, 0), Create("white_bishop", 5, 0),
            Create("white_knight", 6, 0), Create("white_rook", 7, 0), Create("white_pawn", 0, 1),
            Create("white_pawn", 1, 1), Create("white_pawn", 2, 1), Create("white_pawn", 3, 1),
            Create("white_pawn", 4, 1), Create("white_pawn", 5, 1), Create("white_pawn", 6, 1),
            Create("white_pawn", 7, 1)
        };
        playerBlack = new GameObject[] {
            Create("black_rook", 0, 7), Create("black_knight",1,7), Create("black_bishop",2,7),
            Create("black_queen",3,7), Create("black_king",4,7), Create("black_bishop",5,7),
            Create("black_knight",6,7), Create("black_rook",7,7), Create("black_pawn", 0, 6),
            Create("black_pawn", 1, 6), Create("black_pawn", 2, 6), Create("black_pawn", 3, 6),
            Create("black_pawn", 4, 6), Create("black_pawn", 5, 6), Create("black_pawn", 6, 6),
            Create("black_pawn", 7, 6)
        };

        for (int i = 0; i < playerBlack.Length; i++)
        {
            SetPosition(playerBlack[i]);
            SetPosition(playerWhite[i]);
        }

        if (knightLeapButton != null)
            knightLeapButton.onClick.AddListener(() => SelectPowerUp(PowerUpType.KnightLeap));

        if (pawnPromotionButton != null)
            pawnPromotionButton.onClick.AddListener(() => SelectPowerUp(PowerUpType.PromotePawn));

        if (teleportButton != null)
            teleportButton.onClick.AddListener(() => SelectPowerUp(PowerUpType.Teleport));

        if (swapButton != null)
            swapButton.onClick.AddListener(() => SelectPowerUp(PowerUpType.Swap));

        if (reviveButton != null)
            reviveButton.onClick.AddListener(() => SelectPowerUp(PowerUpType.Revive));

        if (shieldButton != null)
            shieldButton.onClick.AddListener(() => SelectPowerUp(PowerUpType.Shield));

    }

    public GameObject Create(string name, int x, int y)
    {
        GameObject obj = Instantiate(chesspiece, new Vector3(0, 0, -1), Quaternion.identity);
        Chessman cm = obj.GetComponent<Chessman>();
        cm.name = name;
        cm.SetXBoard(x);
        cm.SetYBoard(y);
        cm.SetGameObject(this.gameObject); // Let Chessman know the Game object
        cm.Activate();
        return obj;
    }

    public void SetPosition(GameObject obj)
    {
        Chessman cm = obj.GetComponent<Chessman>();
        int x = cm.GetXBoard();
        int y = cm.GetYBoard();

        if (PositionOnBoard(x, y))
        {
            positions[x, y] = obj;
        }
        else
        {
            Debug.LogError($"SetPosition failed: ({x},{y}) is out of bounds.");
        }
    }

    public void SetPositionEmpty(int x, int y)
    {
        positions[x, y] = null;
    }

    public GameObject GetPosition(int x, int y)
    {
        return positions[x, y];
    }

    public bool PositionOnBoard(int x, int y)
    {
        return x >= 0 && y >= 0 && x < 8 && y < 8;
    }

    public string GetCurrentPlayer()
    {
        return currentPlayer;
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void NextTurn()
    {
        currentPlayer = (currentPlayer == "white") ? "black" : "white";
    }

    public void Update()
    {
        if (gameOver && Input.GetMouseButtonDown(0))
        {
            gameOver = false;
            SceneManager.LoadScene("Game");
        }
    }

    public void Winner(string playerWinner)
    {
        gameOver = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().enabled = true;
        GameObject.FindGameObjectWithTag("WinnerText").GetComponent<Text>().text = playerWinner + " is the winner";
        GameObject.FindGameObjectWithTag("RestartText").GetComponent<Text>().enabled = true;
    }

    // ======= POWER-UP FUNCTIONS ========

    public void ActivateKnightLeapForPlayer(string color)
    {
        GameObject[] player = (color == "white") ? playerWhite : playerBlack;
        foreach (GameObject piece in player)
        {
            if (piece != null && piece.name.Contains("knight"))
            {
                piece.GetComponent<Chessman>().ActivateKnightLeap();
            }
        }
    }

    public void PromotePawnAnytime(GameObject pawn, string promoteTo)
    {
        if (pawn == null) return;
        Chessman cm = pawn.GetComponent<Chessman>();
        GameObject newPiece = Create($"{cm.GetColor()}_{promoteTo}", cm.GetXBoard(), cm.GetYBoard());

        SetPosition(newPiece);
        Destroy(pawn);
    }

    public List<Vector2> GenerateTeleportTargets(GameObject piece)
    {
        List<Vector2> targets = new List<Vector2>();
        for (int x = 0; x < 8; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                if (positions[x, y] == null) targets.Add(new Vector2(x, y));
            }
        }
        return targets;
    }

    public List<GameObject> GenerateSwapTargets(GameObject piece)
    {
        Chessman cm = piece.GetComponent<Chessman>();
        GameObject[] team = (cm.GetColor() == "white") ? playerWhite : playerBlack;
        List<GameObject> others = new List<GameObject>();
        foreach (GameObject ally in team)
        {
            if (ally != null && ally != piece) others.Add(ally);
        }
        return others;
    }

    public void ReviveChessman(string color, string pieceName, int x, int y)
    {
        GameObject revived = Create($"{color}_{pieceName}", x, y);
        SetPosition(revived);

        if (color == "white")
        {
            for (int i = 0; i < playerWhite.Length; i++)
            {
                if (playerWhite[i] == null)
                {
                    playerWhite[i] = revived;
                    return;
                }
            }
        }
        else
        {
            for (int i = 0; i < playerBlack.Length; i++)
            {
                if (playerBlack[i] == null)
                {
                    playerBlack[i] = revived;
                    return;
                }
            }
        }
    }
    public enum PowerUpType
    {
        None = 0,
        KnightLeap = 1,
        PromotePawn = 2,
        Teleport = 3,
        Swap = 4,
        Revive = 5,
        Shield = 6          // ✅ New Shield type
    }

    public void SelectPowerUpByIndex(int index)
    {
        if (index >= 0 && index < System.Enum.GetValues(typeof(PowerUpType)).Length)
        {
            SelectPowerUp((PowerUpType)index);
        }
        else
        {
            Debug.LogWarning("Invalid power-up index: " + index);
        }
    }


    private PowerUpType selectedPowerUp = PowerUpType.None;

    public PowerUpType GetSelectedPowerUp()
    {
        return selectedPowerUp;
    }

    public void UsePowerUp(PowerUpType powerUp)
    {
        selectedPowerUp = PowerUpType.None;
    }
    public void SelectPowerUp(PowerUpType powerUp)
    {
        selectedPowerUp = powerUp;
        Debug.Log("Selected Power-Up: " + powerUp);
    }

    public bool CanRevive(string player)
    {
        return capturedPieces.ContainsKey(player) && capturedPieces[player].Count > 0;
    }

    public void RecordCapture(GameObject capturedPiece)
    {
        string player = capturedPiece.GetComponent<Chessman>().player;
        string pieceName = capturedPiece.name;

        // Add to the opponent's captured list
        string opponent = player == "white" ? "black" : "white";

        if (!capturedPieces.ContainsKey(opponent))
            capturedPieces[opponent] = new List<string>();

        capturedPieces[opponent].Add(pieceName);
    }


}