using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenM : MonoBehaviour
{
    //�g���X�N���v�g�Ăяo��
    private TotalGM gm;
    private PenDisplay penDis;
    private LineDrawCon lineDrawCon;
    private RecoveryItemCon recovery;
    private PenInkM penInkM;

    [SerializeField,Header("�y���{�^����\������L�����p�X")]
    private Canvas penCanvas;
    
    //�����̃L�����p�X
    private Canvas myCanvas;

    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    private bool iceDrawFlag = true;
    private bool fireDrawFlag = true;
    private bool generalDrawFlag = true;

    //�{�^���p
    [SerializeField,Header("�y���̃C���X�g")]
    private Sprite[] penSprites;
    [SerializeField,Header("�C���N�̃C���X�g")]
    private Sprite[] inkSprites;

    //�擾����X�v���C�g
    [SerializeField,Header("�擾�������y���̃C���X�g")]
    private Image getPenSprite;
    [SerializeField,Header("�擾�������y���̃C���X�g")]
    private Image getInkSprite;


    //�y���̎�ޔ���
    public enum PenCom
    {
        Ice = 0,
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
        //�擾�Esprite�錾�E�����̃L�����p�X���\��

        gm = FindObjectOfType<TotalGM>();
        penDis = FindObjectOfType<PenDisplay>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();
        penInkM = FindObjectOfType<PenInkM>();
        recovery = FindObjectOfType<RecoveryItemCon>();

        audioSource = GetComponent<AudioSource>();
        myCanvas = this.GetComponent<Canvas>();
        penCanvas = penCanvas.GetComponent<Canvas>();

        //panelCon = FindObjectOfType<PanelCon>();

        nowPen = PenM.PenCom.General;

        myCanvas.enabled = false;

    }

    // Update is called once per frame
    void Update()
    { 
       

        //�����ꂽ�y���{�^���Ɠ����C���X�g�ɂ���
        switch (nowPen)
        {
            case PenCom.Ice:
                getPenSprite.sprite = penSprites[0];
                getInkSprite.sprite = inkSprites[0];
                InkDown(getInkSprite,penInkM.IceDrawTime,5);
                break;
            case PenCom.Fire:
                getPenSprite.sprite = penSprites[1];
                getInkSprite.sprite = inkSprites[1];
                InkDown(getInkSprite, penInkM.FireDrawCount, 5);
                break;
            case PenCom.General:
                getPenSprite.sprite = penSprites[2];
                getInkSprite.sprite = inkSprites[2];
                InkDown(getInkSprite,penInkM.GeneralDrawTime,5);
                break;
        }

        //�����Ђ��ꂽ��
        if (lineDrawCon.LineFlag == true)
        {
            LineTime();
        }

        //���̃y����I��������
        if (nowPen == PenCom.Fire && penInkM.FireDrawCount >= 5)
        {
            lineDrawCon.LineFlag = false;
            fireDrawFlag = false;
        }

        //�y���̉񕜃A�C�e������肵���Ƃ�
        if (recovery.RecoveryFlag == true)
        {
          if (recovery.Item.name == "RecoveryIce")
          {
                if (penInkM.IceDrawTime > 0)
                {
                    penInkM.IceDrawTime = 0;
                }
          }
          
          if(recovery.Item.name == "RecoveryFire")
          {
                if (penInkM.FireDrawCount > 0)
                {
                    penInkM.FireDrawCount = 0;
                }
          }

          if(recovery.Item.name == "RecoveryGeneral")
          {
                if (penInkM.GeneralDrawTime > 0)
                {
                    penInkM.GeneralDrawTime = 0;
                }
          }
          recovery.RecoveryFlag = false;
        }
    }

    //���̃y���{�^���������ꂽ���̏���
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

    //�X�̃y���{�^���������ꂽ���̏���
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

    //���ʂ̃y���{�^���������ꂽ���̏���
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

    //�߂�{�^�����������Ƃ��̏���
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
        if (penInkM.GeneralDrawTime <= 5f ||penInkM.IceDrawTime <= 5f)
        {
            if (lineDrawCon.LineFlag == true)
            {
                switch (nowPen)
                {
                    case PenCom.Ice:
                        penInkM.IceDrawTime += Time.unscaledDeltaTime;
                        //InkDown(getInkSprite,iceDrawTime,5);
                        break;
                    case PenCom.General:
                        penInkM.GeneralDrawTime += Time.unscaledDeltaTime;
                        //getInkSprite.fillAmount = 1 - generalDrawTime / 5;
                        //InkDown(getInkSprite,generalDrawTime,5);
                        break;
                }
            }
        }

        //���������鎞�Ԃ��߂�����I�ׂȂ�����
        if (nowPen == PenCom.General &&penInkM.GeneralDrawTime > 5f)
        {
            lineDrawCon.LineFlag = false;
            generalDrawFlag = false;
        }
        if (nowPen == PenCom.Ice && penInkM.IceDrawTime > 5f)
        {
            lineDrawCon.LineFlag = false;
            iceDrawFlag = false;
        }
    }

    //���������Ԃɂ��킹�ăC���N����
    public void InkDown(Image image, float time, float maxTime)
    {
        image.fillAmount = 1 - time / maxTime;

    }

    public void InkDown(Image image,int count ,int maxCount)
    {

        //Debug.Log(image.fillAmount);
        image.fillAmount = 1 - (float)count/ (float)maxCount;
    }


}
