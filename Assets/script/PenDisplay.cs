using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PenDisplay : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;

    //�|�[�Y���J�������̃t���O
    private bool penMenuFlag = false;

    [SerializeField]
    private Canvas penMCanvas;

    private Canvas c;

    //�{�^���p
    [SerializeField]
    private Button penMButton;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private Sprite fireSprite;
    [SerializeField]
    private Sprite iceSprite;


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
        penMCanvas = penMCanvas.GetComponent<Canvas>();
        c = this.GetComponent<Canvas>();

        penM = FindObjectOfType<PenM>();

        penMButton = penMButton.GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {

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
    }

    public void DisplayFlag()
    {
        //�y���̑I����ʏo��
        if ( penMenuFlag == false)
        {
            Debug.Log("a");
            penMCanvas.enabled = true;
            c.enabled = false;

            Time.timeScale = 0f;
            penMenuFlag = true;
        }
    }
}
