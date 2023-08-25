using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StageSelectCon : MonoBehaviour
{
    //Œø‰Ê‰¹—p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    [SerializeField]
    private GameObject food;

    private TotalGM gm;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gm = FindObjectOfType<TotalGM>();
        food.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(gm.LeafGetFlag[0]);
        if (gm.LeafGetFlag[0] == true)
        {

            food.SetActive(true);
        }
    }


    public void Stage1_1()
    {
        audioSource.PlayOneShot(sound1);
        for(int i = 0; gm.TmpGetFlag[2]; i++)
        {
            gm.TmpGetFlag[i] = false;
        }
        SceneManager.LoadScene("Stage");
    }

    public void Stage2_1()
    {
        audioSource.PlayOneShot(sound1);
    }

    public void Stage3_1()
    {
        audioSource.PlayOneShot(sound1);
    }

    public void Title()
    {
        audioSource.PlayOneShot(sound1);
        SceneManager.LoadScene("Title");
    }
}
