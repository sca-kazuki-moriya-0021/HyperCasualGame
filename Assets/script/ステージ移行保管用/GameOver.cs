using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //効果音用
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    //スプリクト用
    private TotalGM gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //前のシーンに戻る
    public void ReloadStage()
    {
        audioSource.PlayOneShot(sound1);
        gm.ReloadClearScene();
    }

    //ゲーム終了
    public void GameEnd()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //エディタ上の動作
    #else
            Application.Quit();
            //エディタ以外の操作
    #endif
    }
}
