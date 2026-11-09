using Unity.VisualScripting;
using UnityEngine;

public class MobileSettings : MonoBehaviour
{
    [SerializeField]
    bool SHOW_TOUCH_SCREEN=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(SHOW_TOUCH_SCREEN);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
