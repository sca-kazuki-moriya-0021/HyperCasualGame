using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenM : MonoBehaviour
{
    //�Q�[�}�l�Ăяo��
    private TotalGM gm;



    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FirePen()
    {
        //�y���̓���ւ�

        //�^�C���߂�
        Time.timeScale = 1f;

        Destroy(this.gameObject);

    }

    private void IcePen()
    {
        //�y���̓���ւ�

        //�^�C���߂�
        Time.timeScale = 1f;

        Destroy(this.gameObject);

    }

    private void GeneralPen()
    {
        //�y���̓���ւ�

        //�^�C���߂�
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }

    private void GameBark()
    {
        //�^�C���߂�
        Time.timeScale = 1f;
        Destroy(this.gameObject);
    }
}
