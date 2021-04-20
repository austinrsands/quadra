using UnityEngine;

public class GrabberController : MonoBehaviour
{
    [SerializeField]
    private float grabberRadius = 1.75f;

    [SerializeField]
    private float sensitivity = 0.01f;

    private Vector3 previousMousePosition;

    void Awake()
    {
        previousMousePosition = Input.mousePosition;
    }

    void Update()
    {
        UpdatePositionAndRotation();
    }

    private void UpdatePositionAndRotation()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - previousMousePosition;
        Vector3 grabberDelta = mouseDelta * sensitivity;
        Vector3 targetPosition = Vector3.ClampMagnitude(transform.localPosition + grabberDelta, grabberRadius);
        float targetAngle = Mathf.Atan2(targetPosition.y, targetPosition.x) * Mathf.Rad2Deg;
        transform.localPosition = targetPosition;
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
        previousMousePosition = currentMousePosition;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Block"))
            Debug.Log("Block");
    }
}
