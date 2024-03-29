using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleNexrStage : MonoBehaviour
{
    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //TutorialStage();
            //StageSelect();
            Stage();
        }
    }

    public void StageSelect()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("StageSelect",LoadSceneMode.Single);
    }

    public void TutorialStage()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("TutorialStage", LoadSceneMode.Single);
    }

    public void Stage()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("Stage", LoadSceneMode.Single);
    }
    
}
