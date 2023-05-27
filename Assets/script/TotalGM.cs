using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TotalGM : MonoBehaviour
{
    #region
    private enum stageCon{ 
        zero = 0,
        fiast,
        second,
        therd,
        fouthe,
        NO,
    }

    #endregion

    //ƒVƒ“ƒOƒ‹ƒgƒ“
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
