using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class DestructionController : MonoBehaviour
{

    [SerializeField]
    private int chunkFactor = 5;

    private new Rigidbody2D rigidbody;
    private new SpriteRenderer renderer;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    public void Destruct()
    {
        gameObject.SetActive(false);
        SpawnChunks();
        Debug.Log("hello");
    }

    public void SpawnChunks()
    {
        Vector3 startingPosition = new Vector3(transform.position.x - transform.localScale.x / 2, transform.position.y - transform.localScale.y / 2, transform.position.z);
        Vector3 chunkScale = new Vector3(1 / (float)chunkFactor, 1 / (float)chunkFactor, 1);
        float chunkMass = rigidbody.mass / (chunkFactor * chunkFactor);
        for (int y = 0; y < chunkFactor; y++)
        {
            for (int x = 0; x < chunkFactor; x++)
            {
                GameObject chunk = new GameObject("chunk");
                chunk.transform.localScale = chunkScale;
                chunk.transform.parent = transform.parent;
                chunk.transform.position = startingPosition + new Vector3(chunkScale.x * x, chunkScale.y * y, 0);
                chunk.tag = gameObject.tag;

                SpriteRenderer chunkRenderer = chunk.AddComponent<SpriteRenderer>();
                chunkRenderer.sprite = renderer.sprite;
                chunkRenderer.color = renderer.color;
                chunkRenderer.sortingLayerID = renderer.sortingLayerID;

                chunk.AddComponent<BoxCollider2D>();

                Rigidbody2D chunkRigidbody = chunk.AddComponent<Rigidbody2D>();
                chunkRigidbody.mass = chunkMass;
                chunkRigidbody.bodyType = rigidbody.bodyType;
                chunkRigidbody.sharedMaterial = rigidbody.sharedMaterial;
                chunkRigidbody.drag = rigidbody.drag;
                chunkRigidbody.gravityScale = rigidbody.gravityScale;
                chunkRigidbody.collisionDetectionMode = rigidbody.collisionDetectionMode;
                chunkRigidbody.interpolation = rigidbody.interpolation;
                chunkRigidbody.constraints = rigidbody.constraints;
                chunkRigidbody.velocity = rigidbody.velocity;
            }
        }
    }
}
