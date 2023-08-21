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

    //�X�v���N�g�p
    private TotalGM gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void NextStage()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("Title");
    }

    public void StageSlect()
    {
        audioSource.PlayOneShot(sound1);
        gm.ReloadClearSchene();
    }
}
