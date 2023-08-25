using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goalCountDis : MonoBehaviour
{
    private Text scoreText = null;

    private TotalGM gm;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        this.gm = FindObjectOfType<TotalGM>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gm != null)
        {
            Display();
        }
    }

    private void Display()
    { 
        scoreText.text = gm.StageLeafCount + "/" + gm.MaxLeafCount;
    }
}
