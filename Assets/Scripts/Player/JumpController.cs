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
        if (TryingToJump() && CanJump())
            Jump();
    }

    private bool TryingToJump()
    {
        return Input.GetKeyDown("space");
    }

    private bool CanJump()
    {
        return Physics2D.BoxCast(collider.bounds.center, sensorSize, 0, Vector2.down, sensorThreshold.y).collider != null;
    }

    private void Jump()
    {
        rigidbody.AddForce(Vector2.up * force);
    }
}
