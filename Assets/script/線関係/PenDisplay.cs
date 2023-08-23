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

    //�y���ŕ`���Ă��钷��
    //private float iceDrawTime;
    //private float fireDrawTime;
    //private float generalDrawTime;

    //�|�[�Y���J�������̃t���O
    private bool penMenuFlag = false;


    [SerializeField]
    private Canvas penMCanvas;
    private Canvas myCanvas;

    //shader�ۊǗp
    /*private Shader lineShader;
    //shader�p
    [SerializeField]
    private Shader iceShader;
    [SerializeField]
    private Shader generalShader;
    [SerializeField]
    private Shader fireShader;*/

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
        penM = FindObjectOfType<PenM>();

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();


        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //�|�[�Y��ʂ��o�Ă��鎞�̏���
        if(pasueDisplayC.MenuFlag == true)
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
            audioSource.PlayOneShot(sound1);

            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            Time.timeScale = 0f;
            penMenuFlag = true;
        }
    }

    //�y���̃N�[���^�C���p�֐�
    /*private void LineCoolTime()
    {
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                if(fireDrawTime > 0)
                {
                    fireDrawTime -= Time.deltaTime;
                }
                if(generalTime > 0)
                {
                    generalTime -= Time.deltaTime;
                }
                break;

            case PenM.PenCom.Fire:
                if (iceDrawTime > 0)
                {
                    iceDrawTime -= Time.deltaTime;
                }
                if (generalTime > 0)
                {
                    generalTime -= Time.deltaTime;
                }
                break;

            case PenM.PenCom.General:
                if (fireDrawTime > 0)
                {
                    fireDrawTime -= Time.deltaTime;
                }
                if (iceDrawTime > 0)
                {
                    iceDrawTime -= Time.deltaTime;
                }
                break;
        }
    }*/

}
