using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenDisplay : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;

    //�|�[�Y���J�������̃t���O
    private bool penMenuFlag = false;

    [SerializeField]
    private GameObject penM;


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
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DisplayFlag()
    {
        //�y���̑I����ʏo��
        if ( penMenuFlag == false)
        {
            penM.gameObject.GetComponent<CanvasGroup>().alpha = 1;
            Time.timeScale = 0f;
            this.gameObject.GetComponent<CanvasGroup>().alpha = 0;
            penMenuFlag = true;
        }
    }
}
