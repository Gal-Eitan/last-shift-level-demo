
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WorldItem : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    public ItemData Data => itemData;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (itemData != null)
            SetItem(itemData);
    }

    public void SetItem(ItemData data)
    {
        if (data == null)
        {
            Debug.LogWarning("ItemData is null!");
            return;
        }

        itemData = data;
        spriteRenderer.sprite = data.icon;

        gameObject.name = data.itemName;
    }

    public string GetItemName()
    {
        return itemData != null ? itemData.itemName : "";
    }
}
