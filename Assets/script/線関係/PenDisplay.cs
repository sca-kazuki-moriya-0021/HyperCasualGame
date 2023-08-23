using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenDisplay : MonoBehaviour
{
    //ゲーマネ呼び出し
    private TotalGM gm;

    //コード呼び出し
    private PasueDisplayC pasueDisplayC;
    private LineDrawCon lineDrawCon;

    //ペンで描いている長さ
    //private float iceDrawTime;
    //private float fireDrawTime;
    //private float generalDrawTime;

    //ポーズが開いたかのフラグ
    private bool penMenuFlag = false;


    [SerializeField]
    private Canvas penMCanvas;
    private Canvas myCanvas;

    //shader保管用
    /*private Shader lineShader;
    //shader用
    [SerializeField]
    private Shader iceShader;
    [SerializeField]
    private Shader generalShader;
    [SerializeField]
    private Shader fireShader;*/

    //効果音用
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    private PenM penM;

    //ゲッターセッター
    public bool PenMenuFlag
    {
        get { return this.penMenuFlag; }
        set { this.penMenuFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();
        penM = FindObjectOfType<PenM>();

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();


        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ画面が出ている時の処理
        if(pasueDisplayC.MenuFlag == true)
        {
            myCanvas.enabled = false;
        }
        if(pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            myCanvas.enabled = true;
        }
    }

    //ペンの選択画面出す
    public void DisplayFlag()
    {
        if (pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            audioSource.PlayOneShot(sound1);

            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            Time.timeScale = 0f;
            penMenuFlag = true;
        }
    }

    //ペンのクールタイム用関数
    /*private void LineCoolTime()
    {
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                if(fireDrawTime > 0)
                {
                    fireDrawTime -= Time.deltaTime;
                }
                if(generalTime > 0)
                {
                    generalTime -= Time.deltaTime;
                }
                break;

            case PenM.PenCom.Fire:
                if (iceDrawTime > 0)
                {
                    iceDrawTime -= Time.deltaTime;
                }
                if (generalTime > 0)
                {
                    generalTime -= Time.deltaTime;
                }
                break;

            case PenM.PenCom.General:
                if (fireDrawTime > 0)
                {
                    fireDrawTime -= Time.deltaTime;
                }
                if (iceDrawTime > 0)
                {
                    iceDrawTime -= Time.deltaTime;
                }
                break;
        }
    }*/

}
