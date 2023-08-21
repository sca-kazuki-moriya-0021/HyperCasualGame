using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueDisplayC : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseCanvas;

    private Canvas myCanvas;

    //ゲーマネ呼び出し
    private TotalGM gm;
    private PenDisplay penDisplay;

    //ポーズが開いたかのフラグ
    private bool menuFlag = false;

    //操作説明開いたときのフラグ
    //private bool opierationExpFlag = false;

    //一回だけ入るときのフラグ
    private bool onlyFlag = false;

    [SerializeField]
    private AudioClip sound1;
    private AudioSource audioSource;

    //ゲッターセッター
    public bool MenuFlag
    {
        get { return this.menuFlag; }
        set { this.menuFlag = value; }
    }


    public bool OnlyFlag
    {
        get { return this.onlyFlag; }
        set { this.onlyFlag = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        penDisplay = FindObjectOfType<PenDisplay>();

        pauseCanvas = pauseCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(menuFlag == false)
        {
            Time.timeScale = 1f;
        }

        if(penDisplay.PenMenuFlag == true)
        {
            myCanvas.enabled = false;
        }

        if(penDisplay.PenMenuFlag == false)
        {
            myCanvas.enabled = true;
        }

    }

    public void PauseButton()
    {

        //ポーズ画面出す
        if (menuFlag == false  && penDisplay.PenMenuFlag == false)
        {
            audioSource.PlayOneShot(sound1);

            Debug.Log("ポーズ画面押されたよ");
           Time.timeScale = 0f;

            pauseCanvas.enabled = true;
            myCanvas.enabled = false;

            menuFlag = true;
        }

    }

}
