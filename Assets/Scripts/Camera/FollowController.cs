using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowController : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float speed = 2f;

    [SerializeField]
    private Vector2 tolerance = new Vector2(1, 1);

    void LateUpdate()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
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

        return move;
    }
}
