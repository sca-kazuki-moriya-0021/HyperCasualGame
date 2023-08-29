using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleNexrStage : MonoBehaviour
{
    //効果音用
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
            StageSelect();
        }
    }

    public void StageSelect()
    {
        Debug.Log("選んだよ");
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("StageSelect",LoadSceneMode.Single);
    }

    public void TutorialStage()
    {
        Debug.Log("選んだよ");
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("TutorialStage", LoadSceneMode.Single);
    }
    
}
