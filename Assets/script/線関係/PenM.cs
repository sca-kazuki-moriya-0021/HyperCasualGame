using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenM : MonoBehaviour
{
    //使うスクリプト呼び出し
    private TotalGM gm;
    private PenDisplay penDis;
    private LineDrawCon lineDrawCon;
    private RecoveryItemCon recovery;

    [SerializeField,Header("ペンボタンを表示するキャンパス")]
    private Canvas penCanvas;
    
    //自分のキャンパス
    private Canvas myCanvas;

    //効果音用
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    private bool iceDrawFlag = true;
    private bool fireDrawFlag = true;
    private bool generalDrawFlag = true;

    //ボタン用
    [SerializeField,Header("ペンのイラスト")]
    private Sprite[] penSprites;
    [SerializeField,Header("インクのイラスト")]
    private Sprite[] inkSprites;

    //取得するスプライト
    [SerializeField,Header("取得したいペンのイラスト")]
    private Image getPenSprite;
    [SerializeField,Header("取得したいペンのイラスト")]
    private Image getInkSprite;


    //ペンで描いている長さ
    private float iceDrawTime;
    private float fireDrawTime;
    private float generalDrawTime;

    //ペンの種類判定
    public enum PenCom
    {
        Ice = 0,
        Fire,
        General,
    }

    private PenCom nowPen;

    public PenCom NowPen
    {
        get { return this.nowPen; }
        set { this.nowPen = value; }
    }

    public bool IceDrawFlag
    {
        get { return this.iceDrawFlag; }
        set { this.iceDrawFlag = value; }
    }

    public bool FireDrawFlag
    {
        get { return this.fireDrawFlag; }
        set { this.fireDrawFlag = value; }
    }

    public bool GeneralDrawFlag
    {
        get { return this.generalDrawFlag; }
        set { this.generalDrawFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //取得・sprite宣言・自分のキャンパスを非表示

        gm = FindObjectOfType<TotalGM>();
        penDis = FindObjectOfType<PenDisplay>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();
        recovery = FindObjectOfType<RecoveryItemCon>();

        audioSource = GetComponent<AudioSource>();
        myCanvas = this.GetComponent<Canvas>();
        penCanvas = penCanvas.GetComponent<Canvas>();

        nowPen = PenM.PenCom.General;

        myCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {


        //押されたペンボタンと同じイラストにする
        switch (nowPen)
        {
            case PenCom.Ice:
                getPenSprite.sprite = penSprites[0];
                getInkSprite.sprite = inkSprites[0];
                InkDown(getInkSprite,iceDrawTime,5);
                break;
            case PenCom.Fire:
                getPenSprite.sprite = penSprites[1];
                getInkSprite.sprite = inkSprites[1];
                InkDown(getInkSprite, fireDrawTime, 5);
                break;
            case PenCom.General:
                getPenSprite.sprite = penSprites[2];
                getInkSprite.sprite = inkSprites[2];
                InkDown(getInkSprite,generalDrawTime,5);
                break;
        }

        //線がひかれたら
        if (lineDrawCon.LineFlag == true)
        {
            LineTime();
        }

        if (recovery.RecoveryFlag == true)
        {
            switch (nowPen)
            {
                case PenCom.Ice:
                    if (iceDrawTime > 0)
                    {
                        iceDrawTime = 0;
                        recovery.RecoveryFlag = false;
                    }
                    break;
                case PenCom.Fire:
                    if (fireDrawTime > 0)
                    {
                        fireDrawTime = 0;
                        recovery.RecoveryFlag = false;
                    }
                    break;
                case PenCom.General:
                    if (generalDrawTime > 0)
                    {
                        generalDrawTime = 0;
                        recovery.RecoveryFlag = false;
                    }
                    break;
            }
        }

    }
    
    //炎のペンボタンが押された時の処理
    public void FirePen()
    {
        if(fireDrawFlag == true)
        {
            //ペンの入れ替え
            audioSource.PlayOneShot(sound1);

            nowPen = PenCom.Fire;


            penDis.PenMenuFlag = false;
            //タイム戻す
            Time.timeScale = 1f;

            penCanvas.enabled = true;
            myCanvas.enabled = false;
        }
    }

    //氷のペンボタンが押された時の処理
    public void IcePen()
    {
        if (IceDrawFlag ==  true)
        {
            //ペンの入れ替え
            audioSource.PlayOneShot(sound1);

            nowPen = PenCom.Ice;

            penDis.PenMenuFlag = false;

            //タイム戻す
            Time.timeScale = 1f;

            penCanvas.enabled = true;
            myCanvas.enabled = false;
        }
     
    }

    //普通のペンボタンが押された時の処理
    public void GeneralPen()
    {
        if(generalDrawFlag == true)
        {
            //ペンの入れ替え
            audioSource.PlayOneShot(sound1);

            nowPen = PenCom.General;

            //タイム戻す
            Time.timeScale = 1f;

            penDis.PenMenuFlag = false;

            penCanvas.enabled = true;
            myCanvas.enabled = false;
        }

    }

    //戻るボタンを押したときの処理
    public void GameBark()
    {
        audioSource.PlayOneShot(sound1);

        //タイム戻す
        Time.timeScale = 1f;

        penDis.PenMenuFlag = false;

        penCanvas.enabled = true;
        myCanvas.enabled = false;
    }

    //線引く用
    private void LineTime()
    {
        if (generalDrawTime <= 5f || iceDrawTime <= 5f || fireDrawTime <= 5f)
        {
            if (lineDrawCon.LineFlag == true)
            {
                switch (nowPen)
                {
                    case PenCom.Ice:
                        iceDrawTime += Time.deltaTime;
                        //InkDown(getInkSprite,iceDrawTime,5);
                    break;

                    case PenCom.Fire:
                        fireDrawTime += Time.deltaTime;
                        //InkDown(getInkSprite,fireDrawTime,5);

                        break;

                    case PenCom.General:
                        generalDrawTime += Time.deltaTime;
                        //getInkSprite.fillAmount = 1 - generalDrawTime / 5;
                        //InkDown(getInkSprite,generalDrawTime,5);
                        break;
                }
            }
        }

        //線を引ける時間が過ぎたら選べなくする
        if (nowPen == PenCom.General&& generalDrawTime > 5f)
        {
            lineDrawCon.LineFlag =false;
            generalDrawFlag = false;
        }
        if (nowPen == PenCom.Ice && iceDrawTime > 5f)
        {
            lineDrawCon.LineFlag = false;
            iceDrawFlag = false;
        }
        if (nowPen == PenCom.Fire && fireDrawTime > 5f)
        {
            lineDrawCon.LineFlag = false;
            fireDrawFlag = false;
        }
    }

    //引いた時間にあわせてインク減少
    public void InkDown(Image image,float time,float maxTime)
    {
       image.fillAmount = 1-time/maxTime;
       
    }

}
