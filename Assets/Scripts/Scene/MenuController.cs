using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private int targetFramerate = 60;

    [SerializeField]
    private Button previousButton, nextButton;

    private int currentLevel, maxLevel;

    void Awake()
    {
        Application.targetFrameRate = targetFramerate;
        Cursor.visible = true;
        maxLevel = PlayerPrefs.GetInt("Max Level", 0);
        currentLevel = PlayerPrefs.GetInt("Current Level", maxLevel);
        UpdateUI();
    }

    private void UpdateUI()
    {
        previousButton.interactable = currentLevel > 0;
        nextButton.interactable = currentLevel < maxLevel;
    }

    public void Play()
    {
        string levelName = GetLevelName();
        SceneManager.LoadScene(levelName);
    }

    public void Next()
    {
        if (currentLevel < maxLevel)
        {
            currentLevel++;
            UpdateUI();
        }
    }

    public void Previous()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            UpdateUI();
        }
    }

    public string GetLevelName()
    {
        return currentLevel == 0 ? "Tutorial" : $"Level {currentLevel}";
    }
}
