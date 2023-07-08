using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueDisplayC : MonoBehaviour
{

    //�Q�[�}�l�Ăяo��
    private TotalGM gm;

    //�|�[�Y���J�������̃t���O
    private bool menuFlag = false;

    //��������J�����Ƃ��̃t���O
    //private bool opierationExpFlag = false;

    //��񂾂�����Ƃ��̃t���O
    private bool onlyFlag = false;

    [SerializeField]
    //�|�[�Y�������ɕ\������UI�̃v���n�u
    private GameObject pasueUIPrefab;
    //�|�[�YUI�̃C���X�^���X
    private GameObject pauseUIInstance;
    /*
    //�������UI�̃C���X�^���X
    private GameObject playOperateUIInstance;
    [SerializeField]
    //�������UI�̃v���n�u
    private GameObject playOperatePrafab;
    */

    //���X�^�[�g����t���O
    private bool restartFlag = false;

    //�Q�b�^�[�Z�b�^�[
    public bool MenuFlag
    {
        get { return this.menuFlag; }
        set { this.menuFlag = value; }
    }

    public bool RestartFlag
    {
        get { return this.restartFlag; }
        set { this.restartFlag = value; }
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
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PauseButton()
    {

        if (pauseUIInstance != null && menuFlag == true)
        {
            Debug.Log("asiki");
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
        }

        //�|�[�Y��ʏo��
        if (pauseUIInstance == null && menuFlag == false)
        {
            Debug.Log("�|�[�Y��ʉ����ꂽ��");
           pauseUIInstance = GameObject.Instantiate(pasueUIPrefab) as GameObject;
           Time.timeScale = 0f;
           menuFlag = true;
        }

        //���j���[���J���ꂽ��
        if (menuFlag == true)
        {
            PauseMenu();
        }

    }

    private void PauseMenu()
    {
        //esc�L�[�������Ƃ�
        if (Input.GetKey(KeyCode.Escape))
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            //�G�f�B�^��̓���
            #else
            Application.Quit();
            //�G�f�B�^�ȊO�̑���
            #endif
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            menuFlag = false;
        }
      
        if (Input.GetKey(KeyCode.Q))
        {
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            SceneManager.LoadScene("StageSelect");
        }
        if (Input.GetKey(KeyCode.E))
        {
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            restartFlag = true;
            gm.ReloadCurrentSchene();
        }
    }
}
