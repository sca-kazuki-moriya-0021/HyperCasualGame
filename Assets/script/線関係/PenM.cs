using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenM : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;
    private PenDisplay penDis;
    private LineDrawCon lineDrawCon;

    [SerializeField]
    private Canvas penCanvas;
    
    private Canvas myCanvas;

    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    private bool iceDrawFlag = true;
    private bool fireDrawFlag = true;
    private bool generalDrawFlag = true;

    //�{�^���p
    [SerializeField]
    private Button penMButton;
    [SerializeField]
    private Sprite generalSprite;
    [SerializeField]
    private Sprite fireSprite;
    [SerializeField]
    private Sprite iceSprite;

    //�y���ŕ`���Ă��钷��
    private float iceDrawTime;
    private float fireDrawTime;
    private float generalDrawTime;

    public enum PenCom
    {
        Unknown = 0,
        Ice,
        Fire,
        General,
    }

    private PenCom nowPen;

    public PenCom NowPen
    {
        get { return this.nowPen; }
        set { this.nowPen = value; }
    }

    public bool IceDrawFlag
    {
        get { return this.iceDrawFlag; }
        set { this.iceDrawFlag = value; }
    }

    public bool FireDrawFlag
    {
        get { return this.fireDrawFlag; }
        set { this.fireDrawFlag = value; }
    }

    public bool GeneralDrawFlag
    {
        get { return this.generalDrawFlag; }
        set { this.generalDrawFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        penDis = FindObjectOfType<PenDisplay>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();

        audioSource = GetComponent<AudioSource>();
        myCanvas = this.GetComponent<Canvas>();
        penCanvas = penCanvas.GetComponent<Canvas>();

        penMButton = penMButton.GetComponent<Button>();

        myCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (nowPen)
        {
            case PenCom.Ice:
                penMButton.GetComponent<Image>().sprite = iceSprite;
                //LineCoolTime();
                break;

            case PenCom.Fire:
                penMButton.GetComponent<Image>().sprite = fireSprite;
                //LineCoolTime();
                break;

            case PenCom.General:
                penMButton.GetComponent<Image>().sprite = generalSprite;
                //LineCoolTime();
                break;
        }

        if (lineDrawCon.LineFlag == true)
        {
            LineTime();
        }
    }

    public void FirePen()
    {
        if(fireDrawFlag == true)
        {
            //�y���̓���ւ�
            audioSource.PlayOneShot(sound1);

            nowPen = PenCom.Fire;

            penDis.PenMenuFlag = false;
            //�^�C���߂�
            Time.timeScale = 1f;

            penCanvas.enabled = true;
            myCanvas.enabled = false;
        }
    }

    public void IcePen()
    {
        if (IceDrawFlag ==  true)
        {
            //�y���̓���ւ�
            audioSource.PlayOneShot(sound1);

            nowPen = PenCom.Ice;

            penDis.PenMenuFlag = false;

            //�^�C���߂�
            Time.timeScale = 1f;

            penCanvas.enabled = true;
            myCanvas.enabled = false;
        }
     
    }

    public void GeneralPen()
    {
        if(generalDrawFlag == true)
        {
            //�y���̓���ւ�
            audioSource.PlayOneShot(sound1);

            nowPen = PenCom.General;

            //�^�C���߂�
            Time.timeScale = 1f;

            penDis.PenMenuFlag = false;

            penCanvas.enabled = true;
            myCanvas.enabled = false;
        }

    }

    public void GameBark()
    {
        audioSource.PlayOneShot(sound1);

        //�^�C���߂�
        Time.timeScale = 1f;

        penDis.PenMenuFlag = false;

        penCanvas.enabled = true;
        myCanvas.enabled = false;
    }

    //�������p
    private void LineTime()
    {
        if (generalDrawTime <= 5f || iceDrawTime <= 5f || fireDrawTime <= 5f)
        {
            if (lineDrawCon.LineFlag == true)
            {
                switch (nowPen)
                {
                    case PenCom.Ice:

                        iceDrawTime += Time.deltaTime;

                    break;

                    case PenCom.Fire:

                        fireDrawTime += Time.deltaTime;

                   break;

                    case PenCom.General:
                        generalDrawTime += Time.deltaTime;

                   break;
                }
            }
        }

        if (nowPen == PenCom.General&& generalDrawTime > 5f)
        {
            lineDrawCon.LineFlag =false;
            generalDrawFlag = false;
        }
        if (nowPen == PenCom.Ice && iceDrawTime > 5f)
        {
            lineDrawCon.LineFlag = false;
            iceDrawFlag = false;
        }
        if (nowPen == PenCom.Fire && fireDrawTime > 5f)
        {
            lineDrawCon.LineFlag = false;
            fireDrawFlag = false;
        }
    }

}
