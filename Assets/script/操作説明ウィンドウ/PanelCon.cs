using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCon : MonoBehaviour
{
    float x = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Text t = transform.GetComponent<Text>();
        t.text = text;
        while (true)
        {
            x += Time.deltaTime;

            if(x > 5)
            {
                canvas.enabled = false;
            }
        }


        Debug.Log(x);
        
    }

}
