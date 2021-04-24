using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowController : MonoBehaviour
{

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private float distance = 8f;

    [SerializeField]
    private Vector2 tolerance = new Vector2(1, 1);

    private Transform target;
    private Vector3 pausePosition;

    void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        pausePosition = this.transform.position;
    }

    void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (GameController.Paused)
            MoveTowardPausePosition();
        else
            MoveTowardTarget();
    }

    private void MoveTowardPausePosition()
    {
        transform.position = Vector3.Slerp(transform.position, pausePosition, speed * Time.unscaledDeltaTime);
    }

    private void MoveTowardTarget()
    {
        Vector3 move = GetMove();
        transform.position = Vector3.Slerp(transform.position, transform.position + move, speed * Time.unscaledDeltaTime);
    }

    private Vector3 GetMove()
    {
        Vector3 move = Vector3.zero;
        Vector3 difference = target.position - transform.position;

        if (difference.x > tolerance.x)
            move.x = difference.x - tolerance.x;
        else if (-difference.x > tolerance.x)
            move.x = difference.x + tolerance.x;

        if (difference.y > tolerance.y)
            move.y = difference.y - tolerance.y;
        else if (-difference.y > tolerance.y)
            move.y = difference.y + tolerance.y;

        move.z = -transform.position.z - distance;

        return move;
    }
}
