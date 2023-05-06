using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class GoalNextStage : MonoBehaviour
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
        SceneManager.LoadScene("Title");
    }

    public void StageSlect()
    {
        SceneManager.LoadScene("StageSlect");
    }
}
