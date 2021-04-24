using UnityEngine;
using TMPro;

[RequireComponent(typeof(BoxCollider2D))]
public class TutorialTextZoneController : MonoBehaviour
{
    [SerializeField]
    private string message;

    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Animator animator;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            text.SetText(message);
            animator.SetTrigger("Fade");
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
            animator.SetTrigger("Fade");

    }
}
