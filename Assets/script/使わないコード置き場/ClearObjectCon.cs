using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearObjectCon : MonoBehaviour
{
    private TotalGM gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("MoveObject"))
        {
            Debug.Log("haitteru");
            //var scene = gm.MyGetScene();
            //gm.BackScene = scene;
            SceneManager.LoadScene("Goal");
        }
    }
}
