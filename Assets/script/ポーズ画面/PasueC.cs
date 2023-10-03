using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PasueC : MonoBehaviour
{
    private PasueDisplayC pDisplayC;
    private TotalGM gm;
    //private TimeGM timeGM;

    [SerializeField]
    private Canvas pDisplayCanvas;

    private GameObject bgm_Con;

    private AudioSource bgmCon;

    private Canvas myCanvas;

    [SerializeField]
    private AudioClip sound1;
    private AudioSource audioSource;

    [SerializeField]
    private Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pDisplayC = FindObjectOfType<PasueDisplayC>();
        //timeGM = FindObjectOfType<TimeGM>();

        slider = GetComponent<Slider>();
        bgm_Con =GameObject.Find("BGMObject");
        bgmCon = bgm_Con.GetComponent<AudioSource>();
        myCanvas = this.GetComponent<Canvas>();
        pDisplayCanvas = pDisplayCanvas.GetComponent<Canvas>();
        audioSource = GetComponent<AudioSource>();

        //slider.value = AudioListener.volume;

        myCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        float v = slider.value;
        bgmCon.volume = v;
    }

    public void GameEnd()
    {
      
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            //エディタ上の動作
        #else
            Application.Quit();
            //エディタ以外の操作
        #endif
            myCanvas.enabled = false;
            pDisplayC.MenuFlag = false;
        
    }

    public void StageSelectButton()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("StageSelect");
        pDisplayCanvas.enabled = true;

        //if(timeGM.TimeFlag == false)
        {
            Time.timeScale = 1f;
        }
       
        myCanvas.enabled = false;
        pDisplayC.MenuFlag = false;
    }

    public void StageReload()
    {
        audioSource.PlayOneShot(sound1);

        //if (timeGM.TimeFlag == false)
        {
            Time.timeScale = 1f;
        }

        gm.BackScene = gm.MyGetScene();
        gm.ReloadCurrentSchene();

        pDisplayCanvas.enabled = true;
        myCanvas.enabled = false;

        pDisplayC.MenuFlag = false;
    }

    public void BackButton()
    {
        audioSource.PlayOneShot(sound1);

        //if (timeGM.TimeFlag == false)
        {
            Time.timeScale = 1f;
        }

        pDisplayCanvas.enabled = true;
        myCanvas.enabled = false;

        pDisplayC.MenuFlag = false;
    }
    

}
