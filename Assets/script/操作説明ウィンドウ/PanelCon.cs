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
        Text t = transform.GetComponent<Text>();
        t.text = text;
    }

    public void backText(string text)
    {
        Canvas canvas = GetComponentInParent<Canvas>();
        Text t = transform.GetComponent<Text>();
        t.text = text;
        StartCoroutine(ResetText(canvas));
    }

    private IEnumerator ResetText(Canvas c)
    {
        while (true)
        {
            //操作説明中は時止めをしているのでリアルタイム方式で待たないとだめ
            yield return new WaitForSecondsRealtime(4.0f);

            Debug.Log("消える");
            tutorialCon.TutorialDFlag = false;
            c.enabled = false;
            break;

        }
        StopCoroutine(ResetText(c));

    }

}
