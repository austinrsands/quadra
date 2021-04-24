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
    private LayerMask raycastLayer;

    private Vector3 grabberPosition;

    private new SpriteRenderer renderer;

    private enum State
    {
        Searching,
        Hovering,
        Holding
    }

    private State state;

    private Transform target, blockParent;

    private GameObject block;
    private IBlockController blockController;
    private BoxCollider2D parentCollider, blockCollider;

    private Rigidbody2D parentRigidbody, blockRigidbody;

    private float parentGravityScale, parentDrag;

    void Awake()
    {
        Initialize();
        BeginSearching();
    }

    void FixedUpdate()
    {
        UpdateTarget();
    }

    void LateUpdate()
    {
        if (GameController.Playing)
        {
            renderer.enabled = true;
            HandleMouseClick();
        }
        else
            renderer.enabled = false;

        UpdateGrabberPosition();
        UpdateSpritePositionAndRotation();
        UpdateBlock();
    }

    private void Initialize()
    {
        renderer = GetComponent<SpriteRenderer>();
        parentCollider = transform.parent.GetComponent<BoxCollider2D>();
        parentRigidbody = transform.parent.GetComponent<Rigidbody2D>();
        parentGravityScale = parentRigidbody.gravityScale;
        parentDrag = parentRigidbody.drag;
        grabberPosition = Vector3.zero;
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

    private void SaveBlock()
    {
        block = target.gameObject;
        blockParent = block.transform.parent;
        blockController = block.GetComponent<IBlockController>();
        blockCollider = block.GetComponent<BoxCollider2D>();
        blockRigidbody = block.GetComponent<Rigidbody2D>();
    }

    private void DiscardBlock()
    {
        block = null;
        blockController = null;
        blockCollider = null;
        blockRigidbody = null;
    }

    private void DisableBlockProperties()
    {
        blockCollider.enabled = false;
        blockRigidbody.isKinematic = true;
        block.transform.parent = transform.parent;
        blockRigidbody.velocity = Vector2.zero;
        blockController.Disable();
    }

    private void RestoreBlockProperties()
    {
        blockCollider.enabled = true;
        blockRigidbody.isKinematic = false;
        block.transform.parent = blockParent;
        blockRigidbody.velocity = parentRigidbody.velocity;
        blockController.Restore();
    }

    private void InheritBlockProperties()
    {
        parentRigidbody.mass += blockRigidbody.mass;
        parentRigidbody.gravityScale = blockRigidbody.gravityScale;
        parentRigidbody.drag = blockRigidbody.drag;
    }

    private void RemoveInheritedBlockProperties()
    {
        parentRigidbody.mass -= blockRigidbody.mass;
        parentRigidbody.gravityScale = parentGravityScale;
        parentRigidbody.drag = parentDrag;
    }

    private void AddForceToBlock()
    {
        blockRigidbody.AddForce(grabberPosition * force);
    }

    private void DisableParentCollisions()
    {
        Physics2D.IgnoreCollision(parentCollider, blockCollider, true);
    }

    private void EnableParentCollisions()
    {
        Physics2D.IgnoreCollision(parentCollider, blockCollider, false);
    }

    private void GrabBlock()
    {
        SaveBlock();
        DisableBlockProperties();
        InheritBlockProperties();
        BeginHolding();
    }

    private void ReleaseBlock()
    {
        RemoveInheritedBlockProperties();
        RestoreBlockProperties();
        DisableParentCollisions();
        BeginHovering();
    }

    private void ThrowBlock()
    {
        ReleaseBlock();
        AddForceToBlock();
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
        EnableParentCollisions();
        DiscardBlock();
    }

    private bool BlockIsOutsideParent()
    {
        return !parentCollider.Distance(blockCollider).isOverlapped;
    }

    private void UpdateTarget()
    {

        Transform previousTarget = target;

        if (block != null)
            target = block.transform;
        else
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.parent.position, grabberPosition, grabberPosition.magnitude, raycastLayer);
            if (hit.collider != null && hit.collider.CompareTag("Block"))
                target = hit.collider.transform;
            else
                target = null;
        }


        if (state == State.Searching && target != null)
            BeginHovering();
        else if (state == State.Hovering && (target == null || previousTarget != target))
            BeginSearching();

    }

    private void UpdateGrabberPosition()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
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