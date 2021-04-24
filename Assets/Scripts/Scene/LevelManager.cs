using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager
{
    public static void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt("Current Level", level);
    }

    public static void UnlockNextLevel()
    {
        int maxUnlockedLevel = GetMaxUnlockedLevel();
        int nextLevel = GetCurrentLevel() + 1;
        if (nextLevel > maxUnlockedLevel)
            PlayerPrefs.SetInt("Max Unlocked Level", nextLevel);
    }

    public static int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("Current Level", 0);
    }

    public static int GetMaxUnlockedLevel()
    {
        return PlayerPrefs.GetInt("Max Unlocked Level", 0);
    }

    public static int GetLastLevel()
    {
        return SceneManager.sceneCountInBuildSettings - 2;
    }

    public static string GetLevelName(int level)
    {
        if (level == -1) return "Menu";
        if (level == 0) return "Tutorial";
        return $"Level {level}";
    }

    public static string GetCurrentLevelName()
    {
        return GetLevelName(GetCurrentLevel());
    }

    public static int GetMenuLevel()
    {
        return -1;
    }

    public static string GetMenuLevelName()
    {
        return "Menu";
    }

    public static int GetNextLevel()
    {
        return GetCurrentLevel() + 1;
    }

    public static string GetNextLevelName()
    {
        return GetLevelName(GetCurrentLevel() + 1);
    }

    public static bool LevelExists(int level)
    {
        return level >= 0 && level <= GetLastLevel();
    }

    public static bool LevelIsUnlocked(int level)
    {
        return level >= 0 && level <= GetMaxUnlockedLevel();
    }

    public static bool LevelExistsAndIsUnlocked(int level)
    {
        return LevelExists(level) && LevelIsUnlocked(level);
    }

    public static bool NextLevelExists()
    {
        return LevelExists(GetCurrentLevel() + 1);
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public static void LoadLevel(int level)
    {
        if (level != GetMenuLevel())
            SetCurrentLevel(level);
        SceneManager.LoadScene(GetLevelName(level));
    }

    public static void ReloadLevel()
    {
        SceneManager.LoadScene(GetLevelName(GetCurrentLevel()));
    }

    public static void LoadNext()
    {
        SceneManager.LoadScene(GetLevelName(GetCurrentLevel() + 1));
    }

    public static void Save()
    {
        PlayerPrefs.Save();
    }
}