using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleNexrStage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextStage()
    {
       SceneManager.LoadScene("Stage");
    }

    public void StageSelect()
    {
        SceneManager.LoadScene("StageSelect");
    }
    
}
