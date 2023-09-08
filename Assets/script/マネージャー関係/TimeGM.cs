using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeGM : MonoBehaviour
{
    //���Ԍv��
    private bool timeFlag;
    private float countTime = 0;

    [SerializeField,Header("�G���A�i�s�x")]
    private GameObject eriaIcon;

    [SerializeField,Header("���v�A�C�R��")]
    private Image timeIcon;

    [SerializeField,Header("�e�L�X�g")]
    private Text countText = null;
    
    public bool TimeFlag
    {
        get { return this.timeFlag; }
        set { this.timeFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        eriaIcon.SetActive(false);
        timeFlag = false;
        countTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("���Ԍo�ߗp" + timeFlag);
        if (timeFlag == false)
        {
            float s = 10f;
            countTime += Time.unscaledDeltaTime;
            s -= countTime;
            countText.text = s.ToString("f2");
            if (countTime > 10.0f)
            {
                eriaIcon.SetActive(true);
                timeIcon.enabled = false;
                countText.enabled = false;
                timeFlag = true;
                countTime = 0;
            }
        }
    }
}
