using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeGM : MonoBehaviour
{
    //���Ԍv��
    private bool timeFlag;
    private float countTime = 0;

    private PasueDisplayC pDisplayC;
    private PenDisplay penDis;

    [SerializeField,Header("�G���A�i�s�x")]
    private GameObject eriaIcon;

    [SerializeField,Header("���v�A�C�R��")]
    private Image timeIcon;

    [SerializeField,Header("�e�L�X�g")]
    private Text countText = null;

    [SerializeField,Header("�������̔w�i")]
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
      
        //�ŏ��̏\�b���炢
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
