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

    //色
    [SerializeField]
    private Material iceTexture;
    [SerializeField]
    private Material fierTexture;
    [SerializeField]
    private Material ironTexture;

    //色を保存しておく用変数
    private Material lineMaterial;
    private Material _myMat;

    //PhysicsMaterial2DとColorをまとめておく用
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

    //線を引いている時
    private bool lineFlag = false;

    //[SerializeField]
    //private GameObject nullObject;

    //氷のアニメーション
    [SerializeField]
    private GameObject instansIcePrefab;
    private SkeletonAnimation iceSkelton;
    private string ice;


    //炎のアニメーション
    [SerializeField]
    private GameObject instansfirePrefab;
    private SkeletonAnimation fireSkelton;
    private string fire;


    //アニメーションを適用するために必要なAnimationState
    private Spine.AnimationState iceAnimationState = default;
    private Spine.AnimationState fireAnimationState = default;

    //private SkeletonAnimation nowSkeletonAnima;

    //スクリプト読み取り用
    private PasueDisplayC pasueDisplayC;
    private PenDisplay penDisplayC;

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

    public bool LineFlag
    {
        get { return this.lineFlag; }
        set { this.lineFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        //初期化
        penM = FindObjectOfType<PenM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        penDisplayC = FindObjectOfType<PenDisplay>();

        iceSkelton = instansIcePrefab.GetComponent<SkeletonAnimation>();
        fireSkelton = instansfirePrefab.GetComponent<SkeletonAnimation>();
        audioSource = GetComponent<AudioSource>();


        //iceAnimationState = iceSkelton.AnimationState
        //fireAnimationState = fireSkelton.AnimationState;

        //ダブルタッチ無効
        Input.multiTouchEnabled = false;

        //アニメーション初期化
        iceSkelton.AnimationName = "None";
        fireSkelton.AnimationName = "None";

        //線のポイント初期化
        linePoints = new List<Vector2>();


    }

    // Update is called once per frame
    void Update()
    {

        //ペンの種類によって切り替えるプラグラム
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                sMaterial = iceMaterial;
                lineMaterial = iceTexture;
                lineColor = iceColor;
                //nowSkeletonAnima = iceSkelton;
                iceSkelton.AnimationName = "animetion";

                //Debug.Log(lineColor);
                break;

            case PenM.PenCom.Fire:
                //nowSkeletonAnima = fireSkelton;
                fireSkelton.AnimationName = "animetion";
                break;

            case PenM.PenCom.General:
                sMaterial = generalMaterial;
                lineMaterial = ironTexture;
                lineColor = generalColor;
                //nowSkeletonAnima = null;
                break;
        }

        //線が引ける時
        if(pasueDisplayC.MenuFlag == false && penDisplayC.PenMenuFlag == false )
        {
                //左クリックされたら
                if (Input.GetMouseButtonDown(0))
                {
                    switch (penM.NowPen)
                    {
                         case PenM.PenCom.Ice:
                         if(penM.IceDrawFlag == true)
                         {
                            Debug.Log("asuke");
                            lineFlag = true;

                            Debug.Log(lineFlag);
                            _addLineObject();
                         }
                         break;

                         case PenM.PenCom.Fire:
                         if(penM.FireDrawFlag == true)
                         {
                            lineFlag = true;
                            _addLineObject();
                         }
                         break;

                         case PenM.PenCom.General:
                         if(penM.GeneralDrawFlag == true)
                         {
                            lineFlag = true;
                            _addLineObject();
                         }
                         break;
                    }
                }
                //クリック中（ストローク中）
                if (Input.GetMouseButton(0) && lineFlag == true)
                {
                    if (sEffectFlag == false)
                    {
                        audioSource.PlayOneShot(sound1);
                        sEffectFlag = true;
                    }
                    _addPositionDataToLineRenderer();
                }

                //クリック解除になったら
                if (Input.GetMouseButtonUp(0))
                {
                    lineFlag = false;
                    sEffectFlag = false;
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

        //ペンの種類によって切り替えるプラグラム
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:

               lineObj.tag = "IceGround";

            break;

            case PenM.PenCom.Fire:

                lineObj.tag = "FlameGround";

           break;

            case PenM.PenCom.General:

                lineObj.tag = "Ground";

            break;
        }

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
