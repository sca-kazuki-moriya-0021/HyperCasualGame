using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectCon : MonoBehaviour
{
    //Œø‰Ê‰¹—p
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
        
    }


    public void Stage1_1()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("Stage");
    }

    public void Title()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("Title");
    }
}
