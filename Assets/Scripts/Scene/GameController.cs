using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class GameController : MonoBehaviour
{
    [SerializeField]
    private int targetFramerate = 60;

    [SerializeField]
    private TextMeshProUGUI titleText;

    [SerializeField]
    private Button nextButton;

    [SerializeField]
    private Animator GUIAnimator;

    private Animator fadeAnimator;

    public static bool Paused { get; private set; } = true;

    private int levelToLoad;

    public static bool Playing
    {
        get { return !Paused; }
    }

    private enum State
    {
        Intro,
        Playing,
        Paused,
        Outro
    }

    private State state;

    void Awake()
    {
        fadeAnimator = GetComponent<Animator>();
        Application.targetFrameRate = targetFramerate;
        nextButton.interactable = LevelManager.NextLevelExists();
        titleText.SetText(LevelManager.GetCurrentLevelName());
    }

    public void Intro()
    {
        state = State.Intro;
        Cursor.visible = false;
        Paused = true;
    }

    public void Pause()
    {
        state = State.Paused;
        Cursor.visible = true;
        Paused = true;
        titleText.SetText("Paused");
        GUIAnimator.SetTrigger("Pause");
    }

    public void Play()
    {
        state = State.Playing;
        Cursor.visible = false;
        Paused = false;
        GUIAnimator.SetTrigger("Play");
    }

    public void Outro()
    {
        state = State.Outro;
        Cursor.visible = true;
        Paused = true;
        LevelManager.UnlockNextLevel();
        titleText.SetText("Success");
        GUIAnimator.SetTrigger("Success");
    }

    public void Resume()
    {
        Play();
    }

    public void Retry()
    {
        FadeToLevel(LevelManager.GetCurrentLevel());
    }

    public void Menu()
    {
        FadeToLevel(LevelManager.GetMenuLevel());
    }

    public void Next()
    {
        FadeToLevel(LevelManager.GetNextLevel());
    }

    public void FadeToLevel(int level)
    {
        levelToLoad = level;
        fadeAnimator.SetTrigger("Fade Out");
    }

    public void LoadLevel()
    {
        LevelManager.LoadLevel(levelToLoad);
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (state == State.Intro && Input.anyKeyDown)
            Play();
        else if (state == State.Playing && Input.GetKeyDown("escape"))
            Pause();
        else if (state == State.Paused && Input.GetKeyDown("escape"))
            Play();
    }
}
