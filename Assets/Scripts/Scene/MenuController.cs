using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Animator))]
public class MenuController : MonoBehaviour
{
    [SerializeField]
    private int targetFramerate = 60;

    [SerializeField]
    private Button previousButton, nextButton;

    [SerializeField]
    private TextMeshProUGUI levelNameText;

    private Animator fadeAnimator;

    private int shownLevel;

    void Awake()
    {
        fadeAnimator = GetComponent<Animator>();
        Application.targetFrameRate = targetFramerate;
        Cursor.visible = true;
        shownLevel = LevelManager.GetCurrentLevel();
        UpdateUI();
    }

    private void UpdateUI()
    {
        previousButton.interactable = shownLevel > 0;
        nextButton.interactable = LevelManager.LevelExistsAndIsUnlocked(shownLevel + 1);
        levelNameText.SetText(LevelManager.GetLevelName(shownLevel));
    }

    public void Play()
    {
        fadeAnimator.SetTrigger("Fade Out");
    }

    public void LoadLevel()
    {
        LevelManager.LoadLevel(shownLevel);
    }

    public void Next()
    {
        if (LevelManager.LevelExistsAndIsUnlocked(shownLevel + 1))
        {
            shownLevel++;
            UpdateUI();
        }
    }

    public void Previous()
    {
        if (shownLevel > 0)
        {
            shownLevel--;
            UpdateUI();
        }
    }

    public void Quit()
    {
        LevelManager.Save();
        Application.Quit();
    }
}
