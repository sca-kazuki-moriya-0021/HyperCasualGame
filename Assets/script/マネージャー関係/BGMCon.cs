using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMCon : MonoBehaviour
{

    //public static BGMCon instance;

    //private TotalGM gm;

   // private GameObject pasue;
    //private Slider pasueSlider;




    //ƒVƒ“ƒOƒ‹ƒgƒ“
    /*private void Awake()
    {
        gm = FindObjectOfType<TotalGM>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }*/

    // Start is called before the first frame update
    void Start()
    {
        //pasue = GameObject.Find("Slider");
        //pasueSlider = pasue.GetComponent<Slider>();
    
    }

    // Update is called once per frame
    void Update()
    {
        //audioSource.volume = pasueSlider.value;

        //var scene = gm.MyGetScene();

        /*if (scene == TotalGM.StageCon.Clear)
        {
            Destroy(gameObject);
        }

        if (scene == TotalGM.StageCon.GameOver)
        {
             Destroy(gameObject);
        }*/
    }

}
