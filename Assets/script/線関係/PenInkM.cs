using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenInkM : MonoBehaviour
{
    public static PenInkM instance;
    
    private TotalGM gm;

    //ƒyƒ“‚Å•`‚¢‚Ä‚¢‚é’·‚³
    private float iceDrawTime = 0;
    private int fireDrawCount = 0;
    private float generalDrawTime = 0;

    private float backIceDrawTime = 0;
    private int backFireDrawCount = 0;
    private float backGeneralDrawTime = 0;

    public float IceDrawTime
    {
        get { return this.iceDrawTime; }
        set { this.iceDrawTime = value; }
    }

    public int FireDrawCount {
        get { return this.fireDrawCount; }
        set { this.fireDrawCount = value; }
    }

    public float GeneralDrawTime {
        get { return this.generalDrawTime; }
        set { this.generalDrawTime = value; }
    }

    private void Awake()
    {
        gm = FindObjectOfType<TotalGM>();

        Debug.Log(fireDrawCount);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        var scene = gm.MyGetScene();

        if(scene == gm.BackScene)
        {
            iceDrawTime = backIceDrawTime;
            fireDrawCount = backFireDrawCount;
            generalDrawTime = backGeneralDrawTime;

            if (scene == TotalGM.StageCon.Clear)
            {
                Destroy(gameObject);
            }

            if (scene == TotalGM.StageCon.GameOver)
            {
                iceDrawTime = 0;
                fireDrawCount = 0;
                generalDrawTime = 0;
                backIceDrawTime = iceDrawTime;
                backFireDrawCount = fireDrawCount;
                backGeneralDrawTime = generalDrawTime;
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        backIceDrawTime = iceDrawTime;
        backFireDrawCount = fireDrawCount;
        backGeneralDrawTime = generalDrawTime;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
