using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenDisplay : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;

    //�R�[�h�Ăяo��
    private PasueDisplayC pasueDisplayC;
    private LineDrawCon lineDrawCon;

    /*[SerializeField, Header("����������邤����ǂ�")]
    private Canvas operationCanvas;
    [SerializeField]
    private List<string> texts;

    private bool waitingFlag = false;
    private PanelCon panelCon;*/

    //�|�[�Y���J�������̃t���O
    private bool penMenuFlag = false;

    //�L�����p�X�擾
    [SerializeField]
    private Canvas penMCanvas;
    private Canvas myCanvas;


    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;


    //�Q�b�^�[�Z�b�^�[
    public bool PenMenuFlag
    {
        get { return this.penMenuFlag; }
        set { this.penMenuFlag = value; }
    }

    /*
    public bool PenMWaitingFlag
    {
        get { return this.waitingFlag; }
        set { this.waitingFlag = value; }
    }

    private void Awake()
    {
        texts = new List<string>();
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();
        //operationCanvas = operationCanvas.GetComponent<Canvas>();

        audioSource = GetComponent<AudioSource>();

        //operationCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (penMenuFlag == true)
        {
            Time.timeScale = 0f;
        }

        //�|�[�Y��ʂ��o�Ă��鎞�̏���
        if (pasueDisplayC.MenuFlag == true)
        {
            myCanvas.enabled = false;
        }
        if(pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            myCanvas.enabled = true;
        }

    }

    //�y���̑I����ʏo��
    public void DisplayFlag()
    {
        if (pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            //waitingFlag = true;
            audioSource.PlayOneShot(sound1);
            //Time.timeScale = 0f;

            /*if (waitingFlag == true)
            {
                Debug.Log("asuki");
                operationCanvas.enabled = true;
                panelCon =  operationCanvas.GetComponent<PanelCon>();
                panelCon.SetText(texts[0]);
                
            }*/

            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            penMenuFlag = true;

        }
    }

  
}
