using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCon : MonoBehaviour
{

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
        Debug.Log(canvas);
        t.text = text;
     
        StartCoroutine(ResetText(canvas));
        
    }

    private IEnumerator ResetText(Canvas c)
    {
        Debug.Log("mi");

        yield return new WaitForSeconds(3.0f);

        c.enabled = false;

        yield break;

    }

}
