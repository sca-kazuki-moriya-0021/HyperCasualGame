using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalPSliderCon : MonoBehaviour
{

    [SerializeField]
    private Slider slider;
    private int a =0;

    private TotalGM gm;


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        slider.value =gm.StageLeafCount/gm.MaxLeafCount;
        
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = (float)gm.StageLeafCount / (float)gm.MaxLeafCount;
    }
}
