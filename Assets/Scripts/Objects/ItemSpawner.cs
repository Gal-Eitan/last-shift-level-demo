
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [Header("Item Settings")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private ItemData itemToSpawn;

    [Header("Respawn Settings")]
    [SerializeField, Min(0f)]
    private float respawnDelay = 0f;

    private GameObject currentItem;
    private float respawnTimer;
    private bool waitingToRespawn;

    private void Start()
    {
        SpawnItem();
    }

    private void Update()
    {
        if (!waitingToRespawn &&
            (currentItem == null || !currentItem.activeSelf))
        {
            waitingToRespawn = true;
            respawnTimer = respawnDelay;
        }

        if (waitingToRespawn)
        {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0f)
            {
                waitingToRespawn = false;
                SpawnItem();
            }
        }
    }

    private void SpawnItem()
    {
        if (itemPrefab == null || itemToSpawn == null)
        {
            Debug.LogWarning("Spawner is missing ItemPrefab or ItemData!");
            return;
        }

        currentItem = Instantiate(
            itemPrefab,
            transform.position,
            Quaternion.identity
        );

        WorldItem item = currentItem.GetComponent<WorldItem>();

        if (item != null)
        {
            item.SetItem(itemToSpawn);
        }
        else
        {
            Debug.LogError("Item prefab is missing WorldItem!");
        }
    }
}
