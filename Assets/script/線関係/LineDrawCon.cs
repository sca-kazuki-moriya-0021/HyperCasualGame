using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Spine.Unity;

public class LineDrawCon : MonoBehaviour
{

    protected PhysicsMaterial2D sMaterial;
    protected Color lineColor;
    [SerializeField]
    private PhysicsMaterial2D iceMaterial;
    [SerializeField]
    private PhysicsMaterial2D generalMaterial;
    [SerializeField]
    private Color iceColor;
    [SerializeField]
    private Color generalColor;

    private SkeletonAnimation iceSkelton;

    private SkeletonAnimation fireSkelton;
    [SerializeField]
    private Sprite ironSprite;

    private SkeletonAnimation nowSkeletonAnima;
    private Sprite nowSprite;

    private string name;

    
    private PenM penM;

    public Color LineColor
    {
        get { return this.lineColor; }
        set { this.lineColor = value; }
    }

    public PhysicsMaterial2D SMaterial
    {
        get { return this.sMaterial; }
        set { this.sMaterial = value; }
    }

    public SkeletonAnimation NowSkeletonAnima
    {
        get { return this.nowSkeletonAnima; }
        set { this.nowSkeletonAnima = value; }
    }

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public Sprite NowSprite
    {
        get { return this.nowSprite; }
        set { this.nowSprite = value; }
    }

    bool iceflag = false;
    public bool IceFlag
    {
        get { return this.iceflag; }
        set { this.iceflag = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        penM = FindObjectOfType<PenM>();
        sMaterial = generalMaterial;
        
        //penM.NowPen = PenM.PenCom.General;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(sMaterial);
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                sMaterial = iceMaterial;
                lineColor = iceColor;
                nowSkeletonAnima = iceSkelton;
                name = "animetion";
                iceflag = true;
                //Debug.Log(lineColor);
                lineName(name);
                break;

            case PenM.PenCom.Fire:
                nowSkeletonAnima = fireSkelton;
                name = "animetion";
                fireName(name);
                break;

            case PenM.PenCom.General:
                sMaterial = generalMaterial;
                lineColor = generalColor;
                nowSkeletonAnima = null;
                nowSprite = ironSprite;
                break;
        }
    }

    public string lineName(string name)
    {
        return name;
    }

    public string fireName(string name)
    {
        return name;
    }
}
