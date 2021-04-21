using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GrabberController : MonoBehaviour
{
    [SerializeField]
    private float grabberRadius = 1.75f;

    [SerializeField]
    private float sensitivity = 0.01f;

    [SerializeField]
    private float speed = 50f;

    [SerializeField]
    private float force = 80f;

    [SerializeField]
    private Sprite dot, selection, arrow;

    [SerializeField]
    private LayerMask blockLayer;

    private Vector3 grabberPosition;

    private new SpriteRenderer renderer;

    private Vector3 previousMousePosition, currentMousePosition;

    private enum State
    {
        Searching,
        Hovering,
        Holding
    }

    private State state;

    private Transform target;

    private GameObject block;
    private BlockController blockController;
    private BoxCollider2D parentCollider, blockCollider;

    private Rigidbody2D blockRigidbody;

    void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
        parentCollider = transform.parent.GetComponent<BoxCollider2D>();
        grabberPosition = Vector3.zero;
        previousMousePosition = Input.mousePosition;
        BeginSearching();
    }

    void FixedUpdate()
    {
        UpdateTarget();
    }

    void LateUpdate()
    {
        HandleMouseClick();
        UpdateCurrentMousePosition();
        UpdateGrabberPosition();
        UpdateSpritePositionAndRotation();
        UpdatePreviousMousePosition();
        UpdateBlock();
    }

    private void BeginSearching()
    {
        state = State.Searching;
        renderer.sprite = dot;
        renderer.size = Vector2.one;
        transform.rotation = Quaternion.identity;
    }

    private void BeginHovering()
    {
        state = State.Hovering;
        renderer.sprite = selection;
        renderer.size = target.localScale;
        transform.rotation = Quaternion.identity;
    }

    private void BeginHolding()
    {
        state = State.Holding;
        renderer.sprite = arrow;
        renderer.size = Vector2.one;
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonUp(0))
            if (state == State.Hovering)
                GrabBlock();
            else if (state == State.Holding)
                ThrowBlock();

        if (Input.GetMouseButtonUp(1))
            if (state == State.Holding)
                DropBlock();
    }

    private void GrabBlock()
    {
        block = target.gameObject;
        blockController = block.GetComponent<BlockController>();
        blockCollider = block.GetComponent<BoxCollider2D>();
        blockRigidbody = block.GetComponent<Rigidbody2D>();
        blockController.OnGrab();
        block.transform.parent = transform.parent;
        BeginHolding();
    }

    private void ReleaseBlock()
    {
        blockController.OnRelease();
        Physics2D.IgnoreCollision(parentCollider, blockCollider, true);
        BeginHovering();
    }

    private void ThrowBlock()
    {
        ReleaseBlock();
        blockRigidbody.AddForce(grabberPosition * force);
    }

    private void DropBlock()
    {
        ReleaseBlock();
    }

    private void UpdateBlock()
    {
        if (state == State.Holding)
            RetractBlock();

        if (state == State.Hovering && block != null && BlockIsOutsideParent())
            ExpelBlock();
    }

    private void RetractBlock()
    {
        block.transform.position = Vector3.Lerp(block.transform.position, transform.parent.position, speed * Time.unscaledDeltaTime);
    }

    private void ExpelBlock()
    {
        Physics2D.IgnoreCollision(parentCollider, blockCollider, false);
        block = null;
        blockController = null;
        blockCollider = null;
        blockRigidbody = null;
    }

    private bool BlockIsOutsideParent()
    {
        return !parentCollider.Distance(blockCollider).isOverlapped;
    }

    private void UpdateTarget()
    {
        target = block?.transform ?? Physics2D.Raycast(transform.parent.position, grabberPosition, grabberPosition.magnitude, blockLayer).collider?.transform;

        if (state == State.Searching && target != null)
            BeginHovering();
        else if (state == State.Hovering && target == null)
            BeginSearching();
    }

    private void UpdateCurrentMousePosition()
    {
        currentMousePosition = Input.mousePosition;
    }

    private void UpdatePreviousMousePosition()
    {
        previousMousePosition = currentMousePosition;
    }

    private void UpdateGrabberPosition()
    {
        Vector3 mouseDelta = currentMousePosition - previousMousePosition;
        Vector3 grabberDelta = mouseDelta * sensitivity;
        grabberPosition = Vector3.ClampMagnitude(grabberPosition + grabberDelta, grabberRadius);
    }

    private void UpdateSpritePositionAndRotation()
    {
        switch (state)
        {
            case State.Searching:
                UpdateDotPosition();
                break;
            case State.Hovering:
                UpdateSelectionPosition();
                break;
            case State.Holding:
                UpdateArrowPositionAndRotation();
                break;
        }
    }

    private void UpdateDotPosition()
    {
        transform.localPosition = grabberPosition;
    }

    private void UpdateSelectionPosition()
    {
        transform.position = target.position;
    }

    private void UpdateArrowPositionAndRotation()
    {
        transform.localPosition = grabberPosition;
        float targetAngle = Mathf.Atan2(grabberPosition.y, grabberPosition.x) * Mathf.Rad2Deg;
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle - 45);
    }
}