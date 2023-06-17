using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenM : MonoBehaviour
{
    //ゲーマネ呼び出し
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
        //ペンの入れ替え

        if(penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        //タイム戻す
        Time.timeScale = 1f;

        this.gameObject.SetActive(false);

    }

    public void IcePen()
    {
        //ペンの入れ替え

        //タイム戻す
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
        //ペンの入れ替え

        if (penDis.PenMenuFlag == true)
        {
            Debug.Log("haiyo");
            penDis.gameObject.GetComponent<CanvasGroup>().alpha = 1;
        }

        //タイム戻す
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

        //タイム戻す
        Time.timeScale = 1f;

        this.gameObject.SetActive(false);
    }
}
