using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class BlockController : MonoBehaviour
{

    private new BoxCollider2D collider;
    private new Rigidbody2D rigidbody;
    private Transform parent;

    void Awake()
    {
        collider = GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        parent = transform.parent;
    }

    public void OnGrab()
    {
        collider.enabled = false;
        rigidbody.isKinematic = true;
    }

    public void OnRelease()
    {
        collider.enabled = true;
        rigidbody.isKinematic = false;
        transform.parent = parent;
    }
}
