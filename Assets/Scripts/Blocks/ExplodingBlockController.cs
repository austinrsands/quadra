using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ExplodingBlockController : MonoBehaviour, IBlockController
{

    [SerializeField]
    private float force = 50f;

    private new Rigidbody2D rigidbody;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Block"))
            collision.collider.GetComponent<Rigidbody2D>().AddForce(-collision.relativeVelocity.normalized * force);
    }

    public void Disable()
    {

    }

    public void Restore()
    {

    }
}
