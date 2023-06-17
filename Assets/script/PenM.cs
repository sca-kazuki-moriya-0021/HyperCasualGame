using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenM : MonoBehaviour
{
    //ゲーマネ呼び出し
    private TotalGM gm;



    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FirePen()
    {
        //ペンの入れ替え

        //タイム戻す
        Time.timeScale = 1f;

        Destroy(this.gameObject);

    }

    private void IcePen()
    {
        //ペンの入れ替え

        //タイム戻す
        Time.timeScale = 1f;

        Destroy(this.gameObject);

    }

    private void GeneralPen()
    {
        //ペンの入れ替え

        //タイム戻す
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    private void GameBark()
    {
        //タイム戻す
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }
}
