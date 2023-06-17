using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenDisplay : MonoBehaviour
{
    //ゲーマネ呼び出し
    private TotalGM gm;

    //ポーズが開いたかのフラグ
    private bool penMenuFlag = false;

    [SerializeField]
    //ポーズした時に表示するUIのプレハブ
    private GameObject penUIPrefab;
    //ポーズUIのインスタンス
    private GameObject penUIInstance;


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
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PenDisplayFlag()
    {
        //ペンの選択画面出す
        if (penUIInstance == null && penUIPrefab == false)
        {
            penUIInstance = GameObject.Instantiate(penUIPrefab) as GameObject;
            Time.timeScale = 0f;
            this.gameObject.SetActive(false);
            //penMenuFlag = true;
        }
    }
}
