using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeGM : MonoBehaviour
{
    //ŽžŠÔŒv‘ª
    private bool timeFlag;
    private float countTime = 0;

    public bool TimeFlag
    {
        get { return this.timeFlag; }
        set { this.timeFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        timeFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("ŽžŠÔŒo‰ß—p" + timeFlag);
        if (timeFlag == false)
        {
            countTime += Time.unscaledDeltaTime;
            if (countTime > 10.0f)
            {
                timeFlag = true;
                countTime = 0;
            }
        }
    }
}
