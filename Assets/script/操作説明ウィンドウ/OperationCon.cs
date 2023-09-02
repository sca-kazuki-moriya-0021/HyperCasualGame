using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperationCon : MonoBehaviour
{
    [SerializeField, Header("操作説明するうぃんどう")]
    private Canvas operationCanvas;
    [SerializeField]
    private List<string> texts = new List<string>();

    private bool waitingFlag = false;
    [SerializeField]
    private PanelCon panelCon;

    private TutorialCon tutorialCon;

    private void Awake()
    {
        operationCanvas = operationCanvas.GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        panelCon = FindObjectOfType<PanelCon>();
        tutorialCon = FindObjectOfType<TutorialCon>();

        operationCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
 
    }

    public void DisplayTap()
    {
        operationCanvas.enabled = true;
        panelCon.SetText(texts[0]);
    }

    public void Push()
    {
       //operationCanvas.enabled = true;
       panelCon.backText(texts[0]);
    }

    public void Push2()
    {
       
            //operationCanvas.enabled = true;
            panelCon.backText(texts[1]);
    }


    public void Push3()
    {
            //operationCanvas.enabled = true;
            panelCon.backText(texts[2]);
    }

    public void Push4()
    {
            //operationCanvas.enabled = false;
           panelCon.backText(texts[3]);
    }

}
