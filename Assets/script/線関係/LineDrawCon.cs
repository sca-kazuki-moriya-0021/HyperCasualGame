using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Video;
using System.Linq;
using Spine.Unity;

public class LineDrawCon : MonoBehaviour
{
    //ペンで描いている長さ
    private float drawTime;
    private float iceDrawTime;
    private float fireDrawTime;

    //色
    [SerializeField]
    private Material iceTexture;
    [SerializeField]
    private Material fierTexture;
    [SerializeField]
    private Material ironTexture;

    private Material lineMaterial;
    private Material _myMat;

    //線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField]
    private float lineWidth;

    //線となるゲームオブジェクト用変数
    private GameObject lineObj;
    //lineObjのlineRenderer用変数
    private LineRenderer lineRenderer;
    //コライダーのための座標を保持するリスト型の変数
    private List<Vector2> linePoints;

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


    [SerializeField]
    private GameObject nullObject;

    [SerializeField]
    private GameObject instansIcePrefab;
    private SkeletonAnimation iceSkelton;
    private string ice;


    [SerializeField]
    private GameObject instansfirePrefab;
    private SkeletonAnimation fireSkelton;
    private string fire;


    //アニメーションを適用するために必要なAnimationState
    private Spine.AnimationState iceAnimationState = default;
    private Spine.AnimationState fireAnimationState = default;

    //private SkeletonAnimation nowSkeletonAnima;

    private PasueDisplayC pasueDisplayC;
    private PenDisplay penDisplayC;

    private bool iceflag = false;
    private bool fireflag = false;

    //ペンのスプリクト
    private PenM penM;

    //効果音用
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    private bool sEffectFlag = false;

    /*public Color LineColor
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

    /*public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public Sprite NowSprite
    {
        get { return this.nowSprite; }
        set { this.nowSprite = value; }
    }


    public bool IceFlag
    {
        get { return this.iceflag; }
        set { this.iceflag = value; }
    }*/


    // Start is called before the first frame update
    void Start()
    {
        penM = FindObjectOfType<PenM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        penDisplayC = FindObjectOfType<PenDisplay>();

        iceSkelton = instansIcePrefab.GetComponent<SkeletonAnimation>();
        fireSkelton = instansfirePrefab.GetComponent<SkeletonAnimation>();
        audioSource = GetComponent<AudioSource>();


        //iceAnimationState = iceSkelton.AnimationState
        //fireAnimationState = fireSkelton.AnimationState;


        iceSkelton.AnimationName = "None";
        fireSkelton.AnimationName = "None";


        sMaterial = generalMaterial;

        linePoints = new List<Vector2>();

        penM.NowPen = PenM.PenCom.General;
        lineColor = generalColor;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(sMaterial);
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                sMaterial = iceMaterial;
                lineMaterial = iceTexture;
                lineColor = iceColor;
                //nowSkeletonAnima = iceSkelton;
                iceSkelton.AnimationName = "animetion";
                iceflag = true;
                //Debug.Log(lineColor);
                break;

            case PenM.PenCom.Fire:
                //nowSkeletonAnima = fireSkelton;
                fireSkelton.AnimationName = "animetion";
                fireflag = true;
                break;

            case PenM.PenCom.General:
                sMaterial = generalMaterial;
                lineMaterial = ironTexture;
                lineColor = generalColor;
                //nowSkeletonAnima = null;
                break;
        }

        if(pasueDisplayC.MenuFlag == false && penDisplayC.PenMenuFlag == false )
        {
            //左クリックされたら
            if (Input.GetMouseButtonDown(0))
            {
              
                _addLineObject();
            }

            //クリック中（ストローク中）
            if (Input.GetMouseButton(0))
            {
                if(sEffectFlag == false)
                {
                    audioSource.PlayOneShot(sound1);
                    sEffectFlag = true;
                }
                _addPositionDataToLineRenderer();
                
            }

            if (Input.GetMouseButtonUp(0))
            {
               sEffectFlag =false;
                linePoints = new List<Vector2>();
            }
        }


    }


    //クリックしたら発動
    void _addLineObject()
    {
        //ゲームオブジェクト作成
        lineObj = new GameObject();
        //名前をLineにする
        lineObj.name = "Line";
        //lineObjにLineRendererコンポーネントを追加
        lineObj.AddComponent<LineRenderer>();
        //lineObjにEdgeCollider2Dコンポーネントを追加
        EdgeCollider2D c = lineObj.AddComponent<EdgeCollider2D>();
        //マテリアルのコライダーの追加
        c.sharedMaterial = sMaterial;
        //Debug.Log(lineObj.AddComponent<EdgeCollider2D>().sharedMaterial = lineDrawCon.SMaterial);
        //lineObjを自身（Stroke）の子要素に設定
        lineObj.transform.SetParent(transform);

        _initRenderer();
    }


    //lineObj初期化処理
    void _initRenderer()
    {
        //LineRendererを取得
        lineRenderer = lineObj.GetComponent<LineRenderer>();
        _myMat = new Material(lineMaterial);
        lineRenderer.material = _myMat;
        
        //マテリアルの色を設定
        //lineRenderer.material.color = lineDrawCon.LineColor;
        //Debug.Log(lineRenderer.material.color);
        lineMaterial.SetColor("_Color",lineColor);
        lineRenderer.textureMode = LineTextureMode.Tile;

        //ポジションカウントをリセット
        lineRenderer.positionCount = 0;

        //始点の太さを設定
        lineRenderer.startWidth = lineWidth;
        //終点の太さを設定
        lineRenderer.endWidth = lineWidth;
        //レイヤー指定
        lineRenderer.renderingLayerMask = 2;
        //レイヤーをGroundにする
        lineObj.layer = 6;
        //タグ変更
        lineObj.tag = "Ground";
        //Material変更
    }


    void _addPositionDataToLineRenderer()
    {
        /*座標に関する処理*/
        //マウス座標取得
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        //ワールド座標へ変換
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        /*LineRendererに関する処理*/
        //LineRendererのポイントを増やす
        lineRenderer.positionCount += 1;
        //LineRendererのポジションを設定
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPos);

        /*EdgeCollider2Dに関する処理*/
        //ワールド座標をリストに追加
        linePoints.Add(worldPos);
        //EdgeCollider2Dのポイントを設定
        lineObj.GetComponent<EdgeCollider2D>().SetPoints(linePoints);


    }

}
