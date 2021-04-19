using UnityEngine;

public class JumpController : MonoBehaviour
{
  [SerializeField]
  private float force = 10f;

  [SerializeField]
  private float sensorThreshold = 0.001f;

  private new Rigidbody2D rigidbody;
  private new BoxCollider2D collider;
  private Vector2 sensorSize;

  void Awake()
  {
    rigidbody = GetComponent<Rigidbody2D>();
    collider = GetComponent<BoxCollider2D>();
    sensorSize = new Vector2(collider.bounds.size.x - sensorThreshold, collider.bounds.size.y - sensorThreshold);
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
    return Physics2D.BoxCast(collider.bounds.center, sensorSize, 0, -transform.up, sensorThreshold).collider != null;
  }

  private void Jump()
  {
    rigidbody.AddForce(transform.up * force);
  }
}
