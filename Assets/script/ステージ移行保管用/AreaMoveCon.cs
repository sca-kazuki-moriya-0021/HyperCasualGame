using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaMoveCon : MonoBehaviour
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

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("MoveObject"))
        {

            var scene = gm.MyGetScene();
            switch (scene)
            {
                case TotalGM.StageCon.Fiast:

                gm.BackScene = scene;

                SceneManager.LoadScene("Stage1-2");

                break;
                
                case TotalGM.StageCon.Second:

                SceneManager.LoadScene("Stage1-3");

                break;

                case TotalGM.StageCon.Therd:
                
                SceneManager.LoadScene("Goal");

                break;
            }
        }
    }
}