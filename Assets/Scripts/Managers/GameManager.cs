using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int targetFramerate = 60;

    [SerializeField]
    private bool showCursor = false;

    void Awake()
    {
        Application.targetFrameRate = targetFramerate;
        Cursor.visible = showCursor;
        DontDestroyOnLoad(this);
    }
}
