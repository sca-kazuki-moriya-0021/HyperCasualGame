using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

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
        
    }

    //�O�̃V�[���ɖ߂�
    public void ReloadStage()
    {
        audioSource.PlayOneShot(sound1);
        gm.ReloadClearScene();
    }

    //�Q�[���I��
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
}
