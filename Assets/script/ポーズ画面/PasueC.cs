using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueC : MonoBehaviour
{
    private PasueDisplayC pDisplayC;
    private TotalGM gm;

    [SerializeField]
    private Canvas pDisplayCanvas;

    private Canvas myCanvas;

    [SerializeField]
    private AudioClip sound1;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pDisplayC = FindObjectOfType<PasueDisplayC>();

        myCanvas = this.GetComponent<Canvas>();
        pDisplayCanvas = pDisplayCanvas.GetComponent<Canvas>();
        audioSource = GetComponent<AudioSource>();

        myCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

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

            myCanvas.enabled = false;
            pDisplayC.MenuFlag = false;
            pDisplayC.MenuFlag = false;
        }
    }

    public void StageSelectButton()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("StageSelect");
        pDisplayCanvas.enabled = true;

        Time.timeScale = 1f;

        myCanvas.enabled = false;
        pDisplayC.MenuFlag = false;
    }

    public void StageReload()
    {
        audioSource.PlayOneShot(sound1);

        Time.timeScale = 1f;

        gm.ReloadCurrentSchene();

        pDisplayCanvas.enabled = true;
        myCanvas.enabled = false;

        pDisplayC.MenuFlag = false;
    }

    public void BackButton()
    {
        audioSource.PlayOneShot(sound1);

        Time.timeScale = 1f;

        pDisplayCanvas.enabled = true;
        myCanvas.enabled = false;

        pDisplayC.MenuFlag = false;
    }
    

}
