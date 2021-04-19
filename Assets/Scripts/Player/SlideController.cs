using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SlideController : MonoBehaviour
{

    [SerializeField]
    private float force = 15f;
    [SerializeField]

    private float maxSpeed = 3f;

    private new Rigidbody2D rigidbody;
    private float horizontalInput;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
    }

    void FixedUpdate()
    {
        if (TryingToSlide() && CanSlide())
            Slide();
    }

    private bool TryingToSlide()
    {
        return Mathf.Abs(horizontalInput) != 0;
    }

    private bool CanSlide()
    {
        return Mathf.Abs(rigidbody.velocity.x) < maxSpeed;
    }

    private void Slide()
    {
        rigidbody.AddForce(Vector2.right * force * horizontalInput);
    }
}
