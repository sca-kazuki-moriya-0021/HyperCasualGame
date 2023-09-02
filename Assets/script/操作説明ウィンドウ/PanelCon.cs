using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelCon : MonoBehaviour
{
    private TutorialCon tutorialCon;

    // Start is called before the first frame update
    void Start()
    {
        tutorialCon = FindObjectOfType<TutorialCon>();
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
     
        StartCoroutine(ResetText(canvas));
        
    }

    private IEnumerator ResetText(Canvas c)
    {

        Debug.Log("mi");
        Debug.Log(c);

        //‘€ìà–¾’†‚Í~‚ß‚ğ‚µ‚Ä‚¢‚é‚Ì‚ÅƒŠƒAƒ‹ƒ^ƒCƒ€•û®‚Å‘Ò‚½‚È‚¢‚Æ‚¾‚ß
        yield return new WaitForSecondsRealtime(4.0f);

        Debug.Log("Á‚¦‚é");
        tutorialCon.TutorialDFlag = false;
        c.enabled = false;


        StopCoroutine(ResetText(c));

    }

}
