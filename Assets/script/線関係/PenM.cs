using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenM : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;
    private PenDisplay penDis;

    public enum PenCom
    {
        Unknown = 0,
        Ice,
        Fire,
        General,
    }

    private PenCom nowPen;

    public PenCom NowPen
    {
        get { return this.nowPen; }
        set { this.nowPen = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        penDis = FindObjectOfType<PenDisplay>();
        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirePen()
    {
        //�y���̓���ւ�

        if(penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        nowPen = PenCom.Fire;

        penDis.PenMenuFlag = false;
        //�^�C���߂�
        Time.timeScale = 1f;

        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;

    }

    public void IcePen()
    {
        //�y���̓���ւ�

        nowPen = PenCom.Ice;


        if (penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        penDis.PenMenuFlag = false;

        //�^�C���߂�
        Time.timeScale = 1f;

        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void GeneralPen()
    {
        //�y���̓���ւ�

        if (penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        nowPen = PenCom.General;

        //�^�C���߂�
        Time.timeScale = 1f;

        penDis.PenMenuFlag = false;

        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }

    public void GameBark()
    {
        if (penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        //�^�C���߂�
        Time.timeScale = 1f;

        penDis.PenMenuFlag = false;

        this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
    }
}
