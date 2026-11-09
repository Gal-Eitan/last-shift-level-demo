using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> emptyBars;

    [SerializeField]
    GameObject EnergyBar;

    [SerializeField]
    GameObject DistanceFromDissmisalBar;

    [SerializeField]
    GameObject WakefulnessBar;

    [SerializeField]
    private float Energy_initial_percent;
    [SerializeField]
    private float Wakefulness_initial_percent;
    [SerializeField]
    private float DistanceFromDissmisal_initial_percent;





    private const float BAR_HEIGHT = 20f;
    
    [SerializeField]
    private float full_bar_width;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        emptyBars.ForEach(b=> setSize(b,full_bar_width,BAR_HEIGHT));
        setSize(EnergyBar,full_bar_width*Energy_initial_percent/100,BAR_HEIGHT);
        setSize(DistanceFromDissmisalBar,full_bar_width*DistanceFromDissmisal_initial_percent/100,BAR_HEIGHT);
        setSize(WakefulnessBar,full_bar_width*Wakefulness_initial_percent/100,BAR_HEIGHT);
    }


    private void setSize(GameObject game_object_param, float width, float height)
    {
        game_object_param.GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    private void changeWidthByPercentOfFull(GameObject game_object_param, float percent)
    {
        float height=game_object_param.GetComponent<RectTransform>().sizeDelta.y;

        float width=game_object_param.GetComponent<RectTransform>().sizeDelta.x;

        width+=full_bar_width*percent/100;

Debug.Log(width);
        if (width >= full_bar_width)
        {
            width=full_bar_width;
        }else if (width <= 0)
        {
            width=0;
        }

        setSize(game_object_param,width,height);

    }

    // Update is called once per frame
    void Update()
    {
        changeWidthByPercentOfFull(EnergyBar,-1*Time.deltaTime);
        changeWidthByPercentOfFull(WakefulnessBar,-1*Time.deltaTime);
    }
}
