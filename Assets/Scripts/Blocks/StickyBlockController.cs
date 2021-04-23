using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class StickyBlockController : MonoBehaviour, IBlockController
{

    private new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Stick()
    {
        rigidbody.isKinematic = true;
        rigidbody.velocity = Vector2.zero;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Environment"))
            Stick();
    }

    public void Disable()
    {

    }

    public void Restore()
    {

    }
}
