using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ObstacleController : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        DestructionController controller = collision.collider.GetComponent<DestructionController>();
        if (controller != null)
            controller.Destruct();
    }
}
