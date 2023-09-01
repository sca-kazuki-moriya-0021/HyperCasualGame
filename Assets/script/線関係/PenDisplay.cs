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

    /*[SerializeField, Header("操作説明するうぃんどう")]
    private Canvas operationCanvas;
    [SerializeField]
    private List<string> texts;

    private bool waitingFlag = false;
    private PanelCon panelCon;*/

    //ポーズが開いたかのフラグ
    private bool penMenuFlag = false;

    //キャンパス取得
    [SerializeField]
    private Canvas penMCanvas;
    private Canvas myCanvas;


    //効果音用
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;


    //ゲッターセッター
    public bool PenMenuFlag
    {
        get { return this.penMenuFlag; }
        set { this.penMenuFlag = value; }
    }

    /*
    public bool PenMWaitingFlag
    {
        get { return this.waitingFlag; }
        set { this.waitingFlag = value; }
    }

    private void Awake()
    {
        texts = new List<string>();
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();
        //operationCanvas = operationCanvas.GetComponent<Canvas>();

        audioSource = GetComponent<AudioSource>();

        //operationCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (penMenuFlag == true)
        {
            Time.timeScale = 0f;
        }

        //ポーズ画面が出ている時の処理
        if (pasueDisplayC.MenuFlag == true)
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
            //waitingFlag = true;
            audioSource.PlayOneShot(sound1);
            //Time.timeScale = 0f;

            /*if (waitingFlag == true)
            {
                Debug.Log("asuki");
                operationCanvas.enabled = true;
                panelCon =  operationCanvas.GetComponent<PanelCon>();
                panelCon.SetText(texts[0]);
                
            }*/

            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            penMenuFlag = true;

        }
    }

  
}
