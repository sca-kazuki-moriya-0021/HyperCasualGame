using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class GoalNextStage : MonoBehaviour
{
    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    [SerializeField]
    private GameObject[] images;

    //�X�v���N�g�p
    private TotalGM gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        if(gm.TmpGetFlag[0] == true)
            images[0].SetActive(true);
        if (gm.TmpGetFlag[1] == true)
            images[1].SetActive(true);
        if(gm.TmpGetFlag[2] == true)
            images[2].SetActive(true);
    }

    public void NextStage()
    {
        audioSource.PlayOneShot(sound1);
        gm.ClearBack();
    }

    public void GameEnd()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        //�G�f�B�^��̓���
        #else
            Application.Quit();
            //�G�f�B�^�ȊO�̑���
        #endif
    }

    public void StageSlect()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("StageSelect");
    }
}
