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


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();

        penMCanvas = penMCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();

        audioSource = GetComponent<AudioSource>();
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
            audioSource.PlayOneShot(sound1);
            penMCanvas.enabled = true;
            myCanvas.enabled = false;

            penMenuFlag = true;
        }
    }
}
