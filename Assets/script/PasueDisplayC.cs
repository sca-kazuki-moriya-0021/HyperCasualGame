using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PasueDisplayC : MonoBehaviour
{

    //�Q�[�}�l�Ăяo��
    private TotalGM totalGM;

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

    //��������t���O�𗧂Ă�t���O
    private bool stageSelect = false;
    //�Q�[���ɖ߂铮����s���t���O
    private bool returnGame = false;

    //�Q�b�^�[�Z�b�^�[
    public bool MenuFlag
    {
        get { return this.menuFlag; }
        set { this.menuFlag = value; }
    }

    public bool OpenManual
    {
        get { return this.stageSelect; }
        set { this.stageSelect = value; }
    }

    public bool ReturnGame
    {
        get { return this.returnGame; }
        set { this.returnGame = value; }
    }

    public bool OnlyFlag
    {
        get { return this.onlyFlag; }
        set { this.onlyFlag = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
         
            //�|�[�Y��ʏo��
            if (pauseUIInstance == null && menuFlag == false)
            {
                pauseUIInstance = GameObject.Instantiate(pasueUIPrefab) as GameObject;
                Time.timeScale = 0f;
                menuFlag = true;
            }
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
        if (Input.GetKey(KeyCode.Z))
        {
            Debug.Log("asiki");
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            menuFlag = false;
            Destroy(pauseUIInstance);
            Time.timeScale = 1f;
            SceneManager.LoadScene("StageSelect");
        }
    }
}
