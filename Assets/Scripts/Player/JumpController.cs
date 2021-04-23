using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class JumpController : MonoBehaviour
{
    [SerializeField]
    private float force = 400f;

    [SerializeField]
    private Vector2 sensorThreshold = new Vector2(0.001f, 0.2f);

    private new Rigidbody2D rigidbody;
    private new BoxCollider2D collider;
    private Vector2 sensorSize;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        sensorSize = (Vector2)collider.bounds.size - sensorThreshold;
    }

    void Update()
    {
        if (!GameController.InputIsEnabled)
            return;

        if (TryingToJumpUp() && CanJumpUp())
            JumpUp();
        else if (TryingToJumpDown() && CanJumpDown())
            JumpDown();
    }

    private bool TryingToJumpUp()
    {
        return Input.GetKeyDown("space") || Input.GetKeyDown("w") || Input.GetKeyDown("up");
    }

    private bool TryingToJumpDown()
    {
        return Input.GetKeyDown("space") || Input.GetKeyDown("s") || Input.GetKeyDown("down");
    }

    private bool CanJumpUp()
    {
        return Physics2D.BoxCast(collider.bounds.center, sensorSize, 0, Vector2.down, sensorThreshold.y).collider != null;
    }

    private bool CanJumpDown()
    {
        return Physics2D.BoxCast(collider.bounds.center, sensorSize, 0, Vector2.up, sensorThreshold.y).collider != null;
    }

    private void JumpUp()
    {
        rigidbody.AddForce(Vector2.up * force);
    }

    private void JumpDown()
    {
        rigidbody.AddForce(Vector2.down * force);

    }
}
