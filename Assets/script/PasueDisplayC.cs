using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueDisplayC : MonoBehaviour
{

    //ゲーマネ呼び出し
    private TotalGM gm;

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

    //リスタートするフラグ
    private bool restartFlag = false;

    //ゲッターセッター
    public bool MenuFlag
    {
        get { return this.menuFlag; }
        set { this.menuFlag = value; }
    }

    public bool RestartFlag
    {
        get { return this.restartFlag; }
        set { this.restartFlag = value; }
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseButton()
    {

        if (pauseUIInstance != null && menuFlag == true)
        {
            Debug.Log("asiki");
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
        }

        //ポーズ画面出す
        if (pauseUIInstance == null && menuFlag == false)
        {
            Debug.Log("ポーズ画面押されたよ");
           pauseUIInstance = GameObject.Instantiate(pasueUIPrefab) as GameObject;
           Time.timeScale = 0f;
           menuFlag = true;
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
      
        if (Input.GetKey(KeyCode.Q))
        {
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            SceneManager.LoadScene("StageSelect");
        }
        if (Input.GetKey(KeyCode.E))
        {
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            restartFlag = true;
            gm.ReloadCurrentSchene();
        }
    }
}
