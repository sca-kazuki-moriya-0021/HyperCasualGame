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
        //Debug.Log(canvas);
        t.text = text;
     
        StartCoroutine(ResetText(canvas));
        
    }

    private IEnumerator ResetText(Canvas c)
    {

        Debug.Log("mi");
        Debug.Log(c);

        //操作説明中は時止めをしているのでリアルタイム方式で待たないとだめ
        yield return new WaitForSecondsRealtime(2.0f);

        Debug.Log("消える");
        c.enabled = false;


        StopCoroutine(ResetText(c));

    }

}
