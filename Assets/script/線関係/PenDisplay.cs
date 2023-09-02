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


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();

        audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(sound1);
            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            penMenuFlag = true;
        }
    }
}
