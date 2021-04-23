using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StoppingBlockController : MonoBehaviour, IBlockController
{
    [SerializeField]
    private float stoppingSpeed = 0.001f;


    private new Rigidbody2D rigidbody;
    private bool stopped;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Stop()
    {
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;
        stopped = true;
    }

    void FixedUpdate()
    {
        if (ShouldStop())
            Stop();
    }

    private bool ShouldStop()
    {
        return !stopped && rigidbody.velocity.magnitude < stoppingSpeed;
    }

    public void Disable()
    {

    }

    public void Restore()
    {
        stopped = false;
    }
}
