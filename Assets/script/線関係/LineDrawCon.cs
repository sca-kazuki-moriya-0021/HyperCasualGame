using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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


    // Start is called before the first frame update
    void Start()
    {
        penM = FindObjectOfType<PenM>();
        sMaterial = generalMaterial;
        penM.NowPen = PenM.PenCom.General;
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

                //Debug.Log(lineColor);
                break;

            case PenM.PenCom.Fire:
                break;

            case PenM.PenCom.General:
                sMaterial = generalMaterial;
                lineColor = generalColor;
                //Debug.Log(lineColor);
                break;
        }
    }
}
