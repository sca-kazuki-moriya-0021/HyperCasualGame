using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeText : MonoBehaviour
{
    private Text countText = null;
    float s = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(float time)
    {
        s -= time;
        countText.text = s.ToString("f0");
        
    } 

}
