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
        Debug.Log("時間経過用" + timeFlag);
        //最初の十秒ぐらい
        if (timeFlag == true)
        {
            float s = 12f;
            Time.timeScale = 0f;
            if(pDisplayC.MenuFlag == false && penDis.PenMenuFlag == false)
            {
                countTime += Time.unscaledDeltaTime;
            }
            s -= countTime;
            countText.text = s.ToString("f2");
            if (countTime > 12.0f)
            {
                eriaIcon.SetActive(true);
                timeIcon.enabled = false;
                countText.enabled = false;
                Time.timeScale = 1f;
                timeFlag = false;
                countTime = 0;
            }
        }
    }
}
