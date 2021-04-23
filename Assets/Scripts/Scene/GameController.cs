using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int targetFramerate = 60;

    public static bool InputIsEnabled { get; private set; } = false;

    void Awake()
    {
        Application.targetFrameRate = targetFramerate;
        Cursor.visible = true;
        InputIsEnabled = true;
    }

    public void Success()
    {
        Debug.Log("Success");
        InputIsEnabled = false;
    }
}
