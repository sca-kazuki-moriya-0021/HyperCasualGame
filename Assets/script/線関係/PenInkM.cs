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

    public float BackIDrawTime {
        get { return this.backIceDrawTime; }
        set { this.backIceDrawTime = value; }
    }

    public int BackFDrawCount {
        get { return this.backFireDrawCount; }
        set { this.backFireDrawCount = value; }
    }

    public float BackGDrawTime {
        get { return this.backGeneralDrawTime; }
        set { this.backGeneralDrawTime = value; }
    }


    private void Awake()
    {
        gm = FindObjectOfType<TotalGM>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

      
    }

    private void OnValidate()
    {

    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        var scene = gm.MyGetScene();

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
