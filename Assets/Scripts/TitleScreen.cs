using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Component that controls the UI on the title screen.
public class TitleScreen : MonoBehaviour
{
    [Tooltip("Whether the sound is muted.")]
    public bool isSoundMuted = false;
    [Tooltip("Whether the music is muted.")]
    public bool isMusicMuted = false;

    [Tooltip("Image Used to show sound volume.")]
    public Image SoundIcon;
    [Tooltip("Image used to show music volume.")]
    public Image MusicIcon;

    [Tooltip("Audio Source for the music on the title screen.")]
    public AudioSource music;

    [Tooltip("Title Screen Game Object.")]
    public GameObject TitleObjects;
    [Tooltip("Title Screen UI Object.")]
    public GameObject TitleUI;
    [Tooltip("Credits UI Object.")]
    public GameObject CreditsUI;
    [Tooltip("How To UI Object.")]
    public GameObject HowToUI;

    [Tooltip("Reference to Rocket Ship Button.")]
    public GameObject shipOptions1;
    [Tooltip("Reference to UFO Ship Button.")]
    public GameObject shipOptions2;

    // If the user is in the main Title screen UI or not.
    private bool isOnTitle = true;

    // Set starting variables.
    private void Awake()
    {
        //PlayerPrefs.SetInt("HighScore", 0);
        isSoundMuted = PlayerPrefs.GetInt("SoundMuted") == 1 ? true : false;
        isMusicMuted = PlayerPrefs.GetInt("MusicMuted") == 1 ? true : false;
        SoundIcon.color = isSoundMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
        MusicIcon.color = isMusicMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
        music.volume = isMusicMuted ? 0 : 1;
    }

    // Set flag to mute sound effects.
    public void toggleSoundMuted()
    {
        isSoundMuted = !isSoundMuted;
        PlayerPrefs.SetInt("SoundMuted", isSoundMuted ? 1 : 0);
        SoundIcon.color = isSoundMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
    }

    // Set flag to mute music.
    public void toggleMusicMuted()
    {
        isMusicMuted = !isMusicMuted;
        PlayerPrefs.SetInt("MusicMuted", isMusicMuted ? 1 : 0);
        MusicIcon.color = isMusicMuted ? new Color(0.3f, 0.3f, 0.3f) : new Color(0f, 1f, 1f);
        music.volume = isMusicMuted ? 0 : 1;
    }

    // If player clicks/taps, move to next screen.
    void Update()
    {
        if (isOnTitle && (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())))
        {
            isOnTitle = false;
            TitleUI.SetActive(false);
            TitleObjects.SetActive(false);
            if (PlayerPrefs.GetInt("HighScore", 0) >= 150)
            {
                shipOptions2.SetActive(true);
            }
            else
            {
                shipOptions1.SetActive(true);
            }
        }
    }

    // Set flag to start the game as the rocket.
    public void StartAsRocket()
    {
        PlayerPrefs.SetInt("PlayerType", 0);
        SceneManager.LoadScene(1);
    }

    // Set flag to start the game as the UFO.
    public void StartAsUFO()
    {
        PlayerPrefs.SetInt("PlayerType", 1);
        SceneManager.LoadScene(1);
    }

    // Show Credits UI and hide main UI.
    public void GoToCredits()
    {
        TitleUI.SetActive(false);
        TitleObjects.SetActive(false);
        CreditsUI.SetActive(true);
        isOnTitle = false;
    }

    // Show How TO UI, and hide main UI.
    public void GoToHowTo()
    {
        TitleUI.SetActive(false);
        TitleObjects.SetActive(false);
        HowToUI.SetActive(true);
        isOnTitle = false;
    }

    // Show main UI and hide other UI.
    public void GoToTitle()
    {
        TitleUI.SetActive(true);
        TitleObjects.SetActive(true);
        CreditsUI.SetActive(false);
        HowToUI.SetActive(false);
        isOnTitle = true;
    }

    // Quit Application.
    public void Quit()
    {
        Application.Quit();
    }

    // Determine if the mouse cursor is over any UI element.
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count(r => r.gameObject.GetComponent<Button>() != null) > 0;
    }
}
