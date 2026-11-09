using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class ItemInteractor : MonoBehaviour
{
    private Collider2D other_collider;
    [SerializeField]
    private TextMeshProUGUI item_name_view;
    private GameObject itemHeld;

    private void Start()
    {
        other_collider=null;
        itemHeld=null;
        GetComponent<NursePlayerController>().onInteract.AddListener(holdItem);
    }

// When the player enters the item's trigger collider
private void OnTriggerEnter2D(Collider2D other) // Use OnTriggerEnter for 3D
{
    
    if (other.CompareTag("ItemToHold"))
    {
        other_collider=other;
    }
    
}

// When the player leaves the item's trigger collider
private void OnTriggerExit2D(Collider2D other) // Use OnTriggerExit for 3D
{
    if (other.CompareTag("ItemToHold"))
    {
        other_collider=null;
    }
}

public void holdItem()
    {
        if (other_collider != null)
        {
            itemHeld= other_collider.GetComponent<ItemHolder>().holdThisItem();
            item_name_view.SetText(itemHeld.GetComponent<ItemHolder>().getName());
        }
    }

// Inside your existing update or input method:
void Update()
{
    
}
}
