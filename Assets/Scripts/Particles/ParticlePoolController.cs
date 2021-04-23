using UnityEngine;

public class ParticlePoolController : MonoBehaviour
{
    [SerializeField] private GameObject particleSystemPrefab;
    [SerializeField] private int poolSize = 8;

    private ParticleSystem[] pool;
    private int index;

    void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        pool = new ParticleSystem[poolSize];
        index = 0;
        for (int i = 0; i < pool.Length; i++)
        {
            GameObject system = Instantiate(particleSystemPrefab);
            system.transform.SetParent(transform);
            system.SetActive(false);
            pool[i] = system.GetComponent<ParticleSystem>();
        }
    }

    public ParticleSystem GetNextParticleSystem()
    {
        ParticleSystem system = pool[index];
        system.Clear();
        system.Stop();
        system.gameObject.SetActive(false);
        index = (index + 1) % pool.Length;
        return system;
    }
}
