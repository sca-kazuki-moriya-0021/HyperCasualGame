using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeafCon : MonoBehaviour
{
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MoveObject"))
        {
            this.gameObject.SetActive(false);

            var s = gm.MyGetScene();
            //取った枚数を数えて、ステージセレクト画面に表示する
            switch (s)
            {
                case TotalGM.StageCon.Fiast:

                if(gm.LeafGetFlag[0] == false)
                {
                  gm.StageLeafCount++;
                  gm.LeafGetFlag[0] = true;
                }
                //ゲームクリアで表示するためのフラグ
                gm.TmpGetFlag[0] = true;

                break;

                case TotalGM.StageCon.Second:

                if (gm.LeafGetFlag[1] == false)
                {
                  gm.StageLeafCount++;
                  gm.LeafGetFlag[1] = true;
                }

                gm.TmpGetFlag[1] = true;

                break;

                case TotalGM.StageCon.Therd:

                if(gm.LeafGetFlag[2] == false)
                {
                  gm.StageLeafCount++;
                  gm.LeafGetFlag[2] = true;
                }

                 gm.TmpGetFlag[2] = true;

                 break;
            }
        }
    }
}
