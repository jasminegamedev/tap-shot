using UnityEngine;
using UnityEngine.SceneManagement;

// Component to handle actions on death screen.
public class DeathScreen : MonoBehaviour
{
    // Load game on restart
    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    // Go to title screen on quit
    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
