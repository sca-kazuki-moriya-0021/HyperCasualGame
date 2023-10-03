using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGMCon : MonoBehaviour
{

    //public static BGMCon instance;

    private TotalGM gm;

    private GameObject pasue;
    private Slider pasueSlider;

    private AudioSource audioSource;


    //シングルトン
    /*private void Awake()
    {
        gm = FindObjectOfType<TotalGM>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }*/

    // Start is called before the first frame update
    void Start()
    {
        //pasue = GameObject.Find("Slider");
        //pasueSlider = pasue.GetComponent<Slider>();
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //audioSource.volume = pasueSlider.value;

        //var scene = gm.MyGetScene();

        /*if (scene == TotalGM.StageCon.Clear)
        {
            Destroy(gameObject);
        }

        if (scene == TotalGM.StageCon.GameOver)
        {
             Destroy(gameObject);
        }*/
    }

    /// <summary>
	/// スライドバー値の変更イベント
	/// </summary>
	/// <param name="newSliderValue">スライドバーの値(自動的に引数に値が入る)</param>
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        // 音楽の音量をスライドバーの値に変更
        audioSource.volume = newSliderValue;
    }
}
