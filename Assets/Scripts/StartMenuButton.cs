using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Call this method from the Start button
    public void StartGame()
    {
        // Load your game scene here, make sure to add it in Build Settings
        SceneManager.LoadScene("GameScene");
    }

    // Call this method from the Quit button
    public void QuitGame()
    {
        Debug.Log("Quit Game!");
        Application.Quit(); // Works only on built app, not in editor
    }

    // Call this method from Options button if you want to open an options menu
    public void OpenOptions()
    {
        // Implement your options menu logic here
        Debug.Log("Options Menu Opened");
    }
}
