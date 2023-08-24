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
        slider.value = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(a);
        slider.value = (float)a/(float)gm.MaxClearCount;
    }
}
