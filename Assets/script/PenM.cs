using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenM : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;
    private PenDisplay penDis;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        penDis = FindObjectOfType<PenDisplay>();
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

        //�^�C���߂�
        Time.timeScale = 1f;

        this.gameObject.SetActive(false);

    }

    public void IcePen()
    {
        //�y���̓���ւ�

        //�^�C���߂�
        Time.timeScale = 1f;


        if (penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        this.gameObject.SetActive(false);
    }

    public void GeneralPen()
    {
        //�y���̓���ւ�

        if (penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        //�^�C���߂�
        Time.timeScale = 1f;

        this.gameObject.SetActive(false);
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

        this.gameObject.SetActive(false);
    }
}
