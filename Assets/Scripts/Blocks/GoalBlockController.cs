using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class GoalBlockController : MonoBehaviour, IBlockController
{
    private GameController gameController;
    private AudioSource audioSource;

    void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Disable()
    {
        audioSource.Play();
        gameController.Outro();
    }

    public void Restore()
    {

    }
}
