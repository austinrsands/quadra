using UnityEngine;

public class GoalBlockController : MonoBehaviour, IBlockController
{
    private GameController gameController;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    public void Disable()
    {
        gameController.Outro();
    }

    public void Restore()
    {

    }
}
