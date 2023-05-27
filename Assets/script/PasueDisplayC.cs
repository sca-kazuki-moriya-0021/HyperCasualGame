using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueDisplayC : MonoBehaviour
{

    //ゲーマネ呼び出し
    private TotalGM totalGM;

    //ポーズが開いたかのフラグ
    private bool menuFlag = false;

    //操作説明開いたときのフラグ
    //private bool opierationExpFlag = false;

    //一回だけ入るときのフラグ
    private bool onlyFlag = false;

    [SerializeField]
    //ポーズした時に表示するUIのプレハブ
    private GameObject pasueUIPrefab;
    //ポーズUIのインスタンス
    private GameObject pauseUIInstance;
    /*
    //操作説明UIのインスタンス
    private GameObject playOperateUIInstance;
    [SerializeField]
    //操作説明UIのプレハブ
    private GameObject playOperatePrafab;
    */

    //操作説明フラグを立てるフラグ
    private bool stageSelect = false;
    //ゲームに戻る動作を行うフラグ
    private bool returnGame = false;

    //ゲッターセッター
    public bool MenuFlag
    {
        get { return this.menuFlag; }
        set { this.menuFlag = value; }
    }

    public bool OpenManual
    {
        get { return this.stageSelect; }
        set { this.stageSelect = value; }
    }

    public bool ReturnGame
    {
        get { return this.returnGame; }
        set { this.returnGame = value; }
    }

    public bool OnlyFlag
    {
        get { return this.onlyFlag; }
        set { this.onlyFlag = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
         
            //ポーズ画面出す
            if (pauseUIInstance == null && menuFlag == false)
            {
                pauseUIInstance = GameObject.Instantiate(pasueUIPrefab) as GameObject;
                Time.timeScale = 0f;
                menuFlag = true;
            }
        }
        //メニューが開かれたら
        if (menuFlag == true)
        {
            PauseMenu();
        }
    }

    private void PauseMenu()
    {
        //escキーおしたとき
        if (Input.GetKey(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            //エディタ上の動作
            #else
            Application.Quit();
            //エディタ以外の操作
            #endif
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            menuFlag = false;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("asiki");
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            SceneManager.LoadScene("StageSelect");
        }
    }
}
