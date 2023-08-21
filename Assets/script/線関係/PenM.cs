using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenM : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;
    private PenDisplay penDis;
    [SerializeField]
    private Canvas penCanvas;
    
    private Canvas myCanvas;

    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;


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
    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        penDis = FindObjectOfType<PenDisplay>();

        audioSource = GetComponent<AudioSource>();
        myCanvas = this.GetComponent<Canvas>();
        penCanvas = penCanvas.GetComponent<Canvas>();

        myCanvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FirePen()
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

    public void IcePen()
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

    public void GeneralPen()
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

    public void GameBark()
    {
        audioSource.PlayOneShot(sound1);

        //�^�C���߂�
        Time.timeScale = 1f;

        penDis.PenMenuFlag = false;

        penCanvas.enabled = true;
        myCanvas.enabled = false;
    }
}
