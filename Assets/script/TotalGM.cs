using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TotalGM : MonoBehaviour
{
    #region//プレイヤー関係
    //引き継ぐプレイヤーのhp
    private int playerHp= 1;
    #endregion

    public int PlayerHp//プレイヤーのＨｐ
    {
        get { return this.playerHp; }
        set { this.playerHp = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
