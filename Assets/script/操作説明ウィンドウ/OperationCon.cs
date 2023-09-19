using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OperationCon : MonoBehaviour
{
    [SerializeField, Header("ëÄçÏê‡ñæÇ∑ÇÈÇ§Ç°ÇÒÇ«Ç§")]
    private Canvas operationCanvas;
    [SerializeField]
    private List<string> texts = new List<string>();

    [SerializeField]
    private PanelCon panelCon;

    private float time = 0;

    [SerializeField] private Animator _animator;

    private TotalGM totalGM;
    private LineDrawCon lineDraw;
    private PenDisplay penDisplay;

    bool a = false;
    bool b = false;

    private void Awake()
    {
        operationCanvas = operationCanvas.GetComponent<Canvas>();
        totalGM = FindObjectOfType<TotalGM>();
    }

    // Start is called before the first frame update
    void Start()
    {
        panelCon = FindObjectOfType<PanelCon>();
        lineDraw = FindObjectOfType<LineDrawCon>();
        penDisplay = FindObjectOfType<PenDisplay>();

        operationCanvas.enabled = true;
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        panelCon.SetText(texts[0]);
        
    }

    // Update is called once per frame
    void Update()
    {
        var scene = totalGM.MyGetScene();

        if(scene == TotalGM.StageCon.TutorialF)
        {
            if (lineDraw.LineFlag == true && a == false)
            {
                panelCon.SetText(texts[1]);
                a = true;
            }

            if (penDisplay.PenMenuFlag == true && b == false)
            {
                panelCon.SetText(texts[2]);
                b = true;
            }

            if (penDisplay.PenMenuFlag == false && b == true)
            {
                time += Time.unscaledDeltaTime;
                panelCon.SetText(texts[3]);
                if (time > 5f)
                {
                    panelCon.SetText(texts[4]);
                }
                if (time > 10f)
                {
                    panelCon.SetText(texts[5]);
                }
                if(time > 15f)
                {
                    panelCon.SetText(texts[6]);

                }
                if(time > 20f)
                {
                    panelCon.SetText(texts[6]);
                }

                if(time > 25f)
                {
                    SceneManager.LoadScene("TutorialStage2");
                    time = 0;
                }
            }
        }

        if (scene == TotalGM.StageCon.TutorialS)
        {
            time += Time.unscaledDeltaTime;
            if(time > 5f)
            {
                panelCon.SetText(texts[1]);
            }
            if (time > 10f)
            {
                time = 0;
                SceneManager.LoadScene("TutorialStage3");
            }
        }

        if(scene == TotalGM.StageCon.TutorialT)
        {
            time += Time.unscaledTime;

            if(time > 5f)
            {
                panelCon.SetText(texts[1]);
            }

            if(time > 10f)
            {
                panelCon.SetText(texts[2]);
            }

            if(time > 15f)
            {
                panelCon.SetText(texts[3]);
            }

            if(time > 20f)
            {
                SceneManager.LoadScene("StageSelect");
                time = 0;
            }
        }

    }

   /* public void DisplayTap()
    {
        operationCanvas.enabled = true;
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        panelCon.SetText(texts[0]);
    }*/

    /*public void Push()
    {
       panelCon.backText(texts[0]);
    }

    public void Push2()
    {
       panelCon.backText(texts[1]);
    }

    public void Push3()
    {
       panelCon.backText(texts[2]);
    }

    public void Push4()
    {
       panelCon.backText(texts[3]);
    }*/

}
