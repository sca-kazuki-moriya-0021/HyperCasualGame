using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueDisplayC : MonoBehaviour
{
    [SerializeField]
    private Canvas pauseCanvas;

    private Canvas myCanvas;

    //�Q�[�}�l�Ăяo��
    private TotalGM gm;
    private PenDisplay penDisplay;

    //�|�[�Y���J�������̃t���O
    private bool menuFlag = false;

    //��������J�����Ƃ��̃t���O
    //private bool opierationExpFlag = false;

    //��񂾂�����Ƃ��̃t���O
    private bool onlyFlag = false;

    //�Q�b�^�[�Z�b�^�[
    public bool MenuFlag
    {
        get { return this.menuFlag; }
        set { this.menuFlag = value; }
    }


    public bool OnlyFlag
    {
        get { return this.onlyFlag; }
        set { this.onlyFlag = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        penDisplay = FindObjectOfType<PenDisplay>();

        pauseCanvas = pauseCanvas.GetComponent<Canvas>();
        myCanvas = this.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if(menuFlag == false)
        {
            Debug.Log("asiki");
            Time.timeScale = 1f;
        }

        if(penDisplay.PenMenuFlag == true)
        {
            myCanvas.enabled = false;
        }

        if(penDisplay.PenMenuFlag == false)
        {
            myCanvas.enabled = true;
        }

    }

    public void PauseButton()
    {

        //�|�[�Y��ʏo��
        if (menuFlag == false  && penDisplay.PenMenuFlag == false)
        {
            Debug.Log("�|�[�Y��ʉ����ꂽ��");
           Time.timeScale = 0f;

            pauseCanvas.enabled = true;
            myCanvas.enabled = false;

            menuFlag = true;
        }

    }

}
