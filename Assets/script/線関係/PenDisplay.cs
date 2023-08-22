using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenDisplay : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;

    private PasueDisplayC pasueDisplayC;
    private LineDrawCon lineDrawCon;


    //�|�[�Y���J�������̃t���O
    private bool penMenuFlag = false;


    [SerializeField]
    private Canvas penMCanvas;

    private Canvas myCanvas;

    //�{�^���p
    [SerializeField]
    private Button penMButton;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Sprite fireSprite;
    [SerializeField]
    private Sprite iceSprite;

    //shader�ۊǗp
    private Shader lineShader;
    //shader�p
    [SerializeField]
    private Shader iceShader;
    [SerializeField]
    private Shader generalShader;
    [SerializeField]
    private Shader fireShader;

    //���Ԍv���p
    private float lineTime = 10.0f;

    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    private PenM penM;

    //�Q�b�^�[�Z�b�^�[
    public bool PenMenuFlag
    {
        get { return this.penMenuFlag; }
        set { this.penMenuFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();

        penM = FindObjectOfType<PenM>();

        penMButton = penMButton.GetComponent<Button>();

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if(pasueDisplayC.MenuFlag == true)
        {
            myCanvas.enabled = false;
        }
        if(pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            myCanvas.enabled = true;
        }


        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                Debug.Log("oir");
                penMButton.GetComponent<Image>().sprite = iceSprite;
                break;

            case PenM.PenCom.Fire:
                penMButton.GetComponent<Image>().sprite = fireSprite;
                break;

            case PenM.PenCom.General:
                penMButton.GetComponent<Image>().sprite = sprite;
                break;
        }

        if(lineDrawCon.LineFlag == true)
        {
            StartCoroutine(LineTime());
        }
    }

    public void DisplayFlag()
    {
        //�y���̑I����ʏo��
        if (pasueDisplayC.MenuFlag == false && penMenuFlag == false)
        {
            audioSource.PlayOneShot(sound1);

            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            Time.timeScale = 0f;
            penMenuFlag = true;
        }
    }

    //���������Ɏg���R���[�`��
    IEnumerator LineTime()
    {
        while(lineTime >0f)
        {

        }
        yield return null;
    }
}
