using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AdaptivePerformance;

public class ItemHolder : MonoBehaviour
{
    [SerializeField]
    private string item_name ="item";
    
    public GameObject holdThisItem()
    {
        gameObject.SetActive(false);
        Debug.Log("held");
        return gameObject;
    }
    public string getName()
    {
        return item_name;
    }
}
