using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Tooltip("Current Score.")]
    public int Score = 0;
    [Tooltip("Whether the game has been lost.")]
    public bool isGameOver = false;
    [Tooltip("Whether the game has started yet.")]
    public bool isStarted = false;
    [Tooltip("Whether the game is currently paused.")]
    public bool isPaused;

    [Tooltip("Whether the sound effects are currently muted.")]
    public bool isSoundMuted = false;
    [Tooltip("Whether the music is currently muted.")]
    public bool isMusicMuted = false;

    [Tooltip("Reference to the game score text.")]
    public Text ScoreText;
    [Tooltip("Reference to the death score text.")]
    public Text DeathScoreText;
    [Tooltip("Reference to the Main High Score text.")]
    public Text HighScoreText;
    [Tooltip("Reference to the New High Score text.")]
    public Text NewText;

    [Tooltip("Reference to the Sound Muted Image.")]
    public Image SoundIcon;
    [Tooltip("Reference to the Music Muted Image.")]
    public Image MusicIcon;

    [Tooltip("Reference to the Pause Button Gameobject.")]
    public GameObject PauseButton;
    [Tooltip("Reference to the Death Screen UI Gameobject.")]
    public GameObject DeathScreen;
    [Tooltip("Reference to the Main Music Audio Source.")]
    public AudioSource music;

    [Tooltip("Reference to the Rocket Player Gameobject.")]
    public Player rocket;
    [Tooltip("Reference to the UFO Player Gameobject.")]
    public PlayerUFO saucer;
    [Tooltip("Reference to the UFO unlock Gameobject.")]
    public GameObject UnlockMessage;

    // Game Instance Singleton
    public static GameManager Instance { get; private set; } = null;

    // The current highscore
    private int highScore = 0;

    // Setup correct player object on awake. Setup other initial variables.
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }

        Instance = this;
        isSoundMuted = PlayerPrefs.GetInt("SoundMuted", 0) == 1 ? true : false;
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1 ? true : false;
        SoundIcon.color = isSoundMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
        MusicIcon.color = isMusicMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
        music.volume = isMusicMuted ? 0 : 1;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        if (PlayerPrefs.GetInt("PlayerType", 0) == 1)
        {
            Destroy(rocket.gameObject);
        }
        else
        {
            Destroy(saucer.gameObject);
        }
    }

    // Set the Score Text every frame
    private void Update()
    {
        if(isStarted && !isGameOver && !isPaused)
        {
            ScoreText.text = Score.ToString();
        }
        else
        {
            ScoreText.text = "";
        }
    }

    // Set UI values and other variables on death
    public void OnDeath()
    {
        isGameOver = true;
        DeathScreen.SetActive(true);
        DeathScoreText.text = "Score\n" + Score.ToString();
        PauseButton.SetActive(false);
        if (Score > highScore)
        {
            highScore = Score;
            PlayerPrefs.SetInt("HighScore", highScore);
            NewText.gameObject.SetActive(true);
        }
        HighScoreText.text = "High\nScore\n" + highScore;
    }

    // Add to the current score.
    public void AddScore(int scr)
    {
        Score += scr;
        if (Score > highScore)
        {
            if(Score == 150 && highScore < 150)
            {
                StartCoroutine("ShowUnlockMessage");
            }
            highScore = Score;
            PlayerPrefs.SetInt("HighScore", highScore);
            NewText.gameObject.SetActive(true);
        }
    }

    // Show The UFO unlock message when UFO has been unlocked.
    IEnumerator ShowUnlockMessage()
    {
        UnlockMessage.SetActive(true);
        yield return new WaitForSeconds(7);
        UnlockMessage.SetActive(false);
    }

    // Toggle if sound effects are muted.
    public void toggleSoundMuted()
    {
        isSoundMuted = !isSoundMuted;
        PlayerPrefs.SetInt("SoundMuted", isSoundMuted ? 1 : 0);
        SoundIcon.color = isSoundMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
    }

    // Toggle if music is muted.
    public void toggleMusicMuted()
    {
        isMusicMuted = !isMusicMuted;
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        MusicIcon.color = isMusicMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
        music.volume = isMusicMuted ? 0 : 1;
    }

    // Toggle if the game is paused.
    public void togglePause()
    {
        isPaused = !isPaused;

        DeathScoreText.text = "Score\n" + Score.ToString();
        if (Score > highScore)
        {
            highScore = Score;
            PlayerPrefs.SetInt("HighScore", highScore);
            NewText.gameObject.SetActive(true);
        }
        HighScoreText.text = "High\nScore\n" + highScore;

        Time.timeScale = isPaused ? 0f : 1f;
        DeathScreen.SetActive(isPaused);
        EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
    }
}
