using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeGM : MonoBehaviour
{
    //時間計測
    private bool timeFlag;
    private float countTime = 0;

    private PasueDisplayC pDisplayC;
    private PenDisplay penDis;

    [SerializeField,Header("エリア進行度")]
    private GameObject eriaIcon;

    [SerializeField,Header("時計アイコン")]
    private Image timeIcon;

    [SerializeField,Header("テキスト")]
    private Text countText = null;

    [SerializeField,Header("文字後ろの背景")]
    private Image image;
    
    public bool TimeFlag
    {
        get { return this.timeFlag; }
        set { this.timeFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        pDisplayC = FindObjectOfType<PasueDisplayC>();
        penDis = FindObjectOfType<PenDisplay>();

        eriaIcon.SetActive(false);
        timeFlag = true;
        countTime = 0;

    }

    // Update is called once per frame
    void Update()
    {
      
        //最初の十秒ぐらい
        if (timeFlag == true)
        {
            float s = 5f;
            Time.timeScale = 0f;
            if(pDisplayC.MenuFlag == false && penDis.PenMenuFlag == false)
            {
                countTime += Time.unscaledDeltaTime;
            }
            s -= countTime;
            countText.text = s.ToString("f2");
            if (countTime > 5.0f)
            {
                eriaIcon.SetActive(true);
                timeIcon.enabled = false;
                countText.enabled = false;
                image.enabled = false;
                Time.timeScale = 1f;
                timeFlag = false;
                countTime = 0;
            }
        }
    }
}
