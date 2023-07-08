using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenM : MonoBehaviour
{
    //ゲーマネ呼び出し
    private TotalGM gm;
    private PenDisplay penDis;
    [SerializeField]
    private Canvas penC;
    
    private Canvas c;

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
        c = this.GetComponent<Canvas>();
        penC = penC.GetComponent<Canvas>();

        c.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirePen()
    {
        //ペンの入れ替え

        /**/


        nowPen = PenCom.Fire;

        penDis.PenMenuFlag = false;
        //タイム戻す
        Time.timeScale = 1f;

        penC.enabled = true;
        c.enabled = false;

    }

    public void IcePen()
    {
        //ペンの入れ替え

        nowPen = PenCom.Ice;


        penDis.PenMenuFlag = false;

        //タイム戻す
        Time.timeScale = 1f;

        penC.enabled = true;
        c.enabled = false;
    }

    public void GeneralPen()
    {
        //ペンの入れ替え



        nowPen = PenCom.General;

        //タイム戻す
        Time.timeScale = 1f;

        penDis.PenMenuFlag = false;

        penC.enabled = true;
        c.enabled = false;
    }

    public void GameBark()
    {


        //タイム戻す
        Time.timeScale = 1f;

        penDis.PenMenuFlag = false;

        penC.enabled = true;
        c.enabled = false;
    }
}
