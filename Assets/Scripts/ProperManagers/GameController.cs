using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public enum MenuState
    {
        Menu,
        Game
    }

    [Header("Menus")]
    [Space]

    public GameObject titleScreen;
    public GameObject levelSelectScreen;
    public GameObject gameOverScreen;
    public GameObject optionsScreen;
    public GameObject instructionsScreen;
    public GameObject creditsScreen;
    public GameObject pauseScreen;
    public GameObject backgroundImage;
    public Transition transition;

    [Header("Audio")]
    [Space]

    public AudioMixer audioMixer;

    public Slider[] masterSliders;
    public Slider[] musicSliders;
    public Slider[] sfxSliders;
    public Slider[] voiceSliders;

    public int highScore = 0;

    bool isPaused = false;
    public float timeScale;
    public MenuState state = MenuState.Menu;

    static GameController instance = null;
    public static GameController Instance { 
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(gameObject);
        OnTitleScreen();
    }

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        state = MenuState.Menu;
        //PlayerPrefs.SetInt("HighScore", highScore);

        OnMasterVolume(PlayerPrefs.GetFloat("MasterVolume", 0));
        OnMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0));
        OnSFXVolume(PlayerPrefs.GetFloat("SFXVolume", 0));
        OnVoiceVolume(PlayerPrefs.GetFloat("VoiceVolume", 0));


        foreach (var slider in masterSliders)
        {
            audioMixer.GetFloat("MasterVolume", out float volume);
            slider.value = volume;
        }
        foreach (var slider in musicSliders)
        {
            audioMixer.GetFloat("MusicVolume", out float volume);
            slider.value = volume;
        }
        foreach (var slider in sfxSliders)
        {
            audioMixer.GetFloat("SFXVolume", out float volume);
            slider.value = volume;
        }
        foreach (var slider in voiceSliders)
        {
            audioMixer.GetFloat("VoiceVolume", out float volume);
            slider.value = volume;
        }
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    #region Game
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && state == MenuState.Game)
        {
            OnPause();
        }
    }

    public void SetHighScore(int score)
    {
        highScore = score;
        PlayerPrefs.SetInt("HighScore", highScore);
    }

    public void OnLoadGameScene(string scene)
    {
        StartCoroutine(LoadGameScene(scene));
    }

    IEnumerator LoadGameScene(string scene)
    {
        if (transition != null)
        {
            transition.StartTransition(Color.black, 1);

            while (!transition.IsDone) { yield return null; }
        }

        if (backgroundImage != null) backgroundImage.SetActive(false);
        levelSelectScreen.SetActive(false);
        SceneManager.LoadScene(scene);
        state = MenuState.Game;
        if (transition != null)
        {
            transition.StartTransition(Color.clear, 1);

            while (!transition.IsDone) { yield return null; }
        }

        yield return null;
    }

    public void OnLoadGameScene(int scene)
    {
        StartCoroutine(LoadGameScene(scene));
    }

    IEnumerator LoadGameScene(int scene)
    {
        if (transition != null)
        {
            transition.StartTransition(Color.black, 1);

            while (!transition.IsDone) { yield return null; }
        }

        if (backgroundImage != null) backgroundImage.SetActive(false);
        levelSelectScreen.SetActive(false);
        SceneManager.LoadScene(scene);
        state = MenuState.Game;
        if (transition != null)
        {
            transition.StartTransition(Color.clear, 1);

            while (!transition.IsDone) { yield return null; }
        }

        yield return null;
    }
    #endregion

    #region Menu Navigation
    public void OnLoadMenuScene(string scene)
    {
        StartCoroutine(LoadMenuScene(scene));
    }

    IEnumerator LoadMenuScene(string scene)
    {
        if (transition != null)
        {
            transition.StartTransition(Color.black, 1);

            while (!transition.IsDone) { yield return null; }
        }

        if (backgroundImage != null) backgroundImage.SetActive(true);
        if (gameOverScreen != null) gameOverScreen.SetActive(false);
        titleScreen.SetActive(true);
        SceneManager.LoadScene(scene);
        state = MenuState.Menu;

        if (transition != null)
        {
            transition.StartTransition(Color.clear, 1);

            while (!transition.IsDone) { yield return null; }
        }

        yield return null;
    }

    public void OnTitleScreen()
    {
        titleScreen.SetActive(true);
        levelSelectScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        optionsScreen.SetActive(false);
        instructionsScreen.SetActive(false);
        creditsScreen.SetActive(false);
    }

    public void OnLevelSelectScreen()
    {
        titleScreen.SetActive(false);
        levelSelectScreen.SetActive(true);
    }

    public void OnOptionsScreen()
    {
        titleScreen.SetActive(false);
        optionsScreen.SetActive(true);
    }

    public void OnInstructions()
    {
        titleScreen.SetActive(false);
        instructionsScreen.SetActive(true);
    }

    public void OnCreditsScreen()
    {
        titleScreen.SetActive(false);
        creditsScreen.SetActive(true);
    }

    public void OnPauseScreen()
    {
        if (isPaused)
        {
            isPaused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = timeScale;
        }
        else
        {
            isPaused = true;
            pauseScreen.SetActive(true);
            timeScale = Time.timeScale;
            Time.timeScale = 0;
        }
    }

    public void OnPause()
    {
        OnPauseScreen();
    }
    #endregion

    #region Audio
    public void OnMasterVolume(float level)
    {
        audioMixer.SetFloat("MasterVolume", level);
        PlayerPrefs.SetFloat("MasterVolume", level);
        foreach (var slider in masterSliders) slider.value = level;
    }

    public void OnMusicVolume(float level)
    {
        audioMixer.SetFloat("MusicVolume", level);
        PlayerPrefs.SetFloat("MusicVolume", level);
        foreach (var slider in musicSliders) slider.value = level;
    }

    public void OnSFXVolume(float level)
    {
        audioMixer.SetFloat("SFXVolume", level);
        PlayerPrefs.SetFloat("SFXVolume", level);
        foreach (var slider in sfxSliders) slider.value = level;
    }

    public void OnVoiceVolume(float level)
    {
        audioMixer.SetFloat("VoiceVolume", level);
        PlayerPrefs.SetFloat("VoiceVolume", level);
        foreach (var slider in voiceSliders) slider.value = level;
    }
    #endregion
}
