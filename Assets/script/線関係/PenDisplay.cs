using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenDisplay : MonoBehaviour
{
    //ゲーマネ呼び出し
    private TotalGM gm;

    private PasueDisplayC pasueDisplayC;

    //ポーズが開いたかのフラグ
    private bool penMenuFlag = false;


    [SerializeField]
    private Canvas penMCanvas;

    private Canvas myCanvas;

    //ボタン用
    [SerializeField]
    private Button penMButton;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Sprite fireSprite;
    [SerializeField]
    private Sprite iceSprite;


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

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();

        penM = FindObjectOfType<PenM>();

        penMButton = penMButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

        if(pasueDisplayC.MenuFlag == true)
        {
            myCanvas.enabled = false;
        }
        if(pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            myCanvas.enabled = true;
        }


        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                Debug.Log("oir");
                penMButton.GetComponent<Image>().sprite = iceSprite;
                break;

            case PenM.PenCom.Fire:
                penMButton.GetComponent<Image>().sprite = fireSprite;
                break;

            case PenM.PenCom.General:
                penMButton.GetComponent<Image>().sprite = sprite;
                break;
        }
    }

    public void DisplayFlag()
    {
        //ペンの選択画面出す
        if (pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            Debug.Log("a");
            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            Time.timeScale = 0f;
            penMenuFlag = true;
        }
    }
}
