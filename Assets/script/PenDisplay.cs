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
    //�|�[�Y�������ɕ\������UI�̃v���n�u
    private GameObject penUIPrefab;
    //�|�[�YUI�̃C���X�^���X
    private GameObject penUIInstance;


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

    void PenDisplayFlag()
    {
        //�y���̑I����ʏo��
        if (penUIInstance == null && penUIPrefab == false)
        {
            penUIInstance = GameObject.Instantiate(penUIPrefab) as GameObject;
            Time.timeScale = 0f;
            this.gameObject.SetActive(false);
            //penMenuFlag = true;
        }
    }
}
