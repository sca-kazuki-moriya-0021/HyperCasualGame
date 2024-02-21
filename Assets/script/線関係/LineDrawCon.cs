using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.EventSystems;

public class LineDrawCon : MonoBehaviour
{
    [SerializeField,Header("ペンの上限ライン")]
    private GameObject range;
    //線の太さ
    [Range(0.1f, 0.5f)]
    [SerializeField,Header("線の太さ")]
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
    private Material fireTexture;
    [SerializeField]
    private Material generalTexture;
    
    [SerializeField,Header("火")]
    private GameObject fireObject;

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
    private Color fireColor;
    [SerializeField]
    private Color generalColor;

    //線を引いている時
    private bool lineFlag = false;

    //[SerializeField]
    //private GameObject nullObject;

    //氷のアニメーション
    [SerializeField]
    private GameObject instansIcePrefab;
    private string ice;

    //炎のアニメーション
    [SerializeField]
    private GameObject instansfirePrefab;
    private string fire;


    //アニメーションを適用するために必要なAnimationState
    //private Spine.AnimationState iceAnimationState = default;
    //private Spine.AnimationState fireAnimationState = default;

    //private SkeletonAnimation nowSkeletonAnima;

    //スクリプト読み取り用
    private PasueDisplayC pasueDisplayC;
    private PenDisplay penDisplayC;
    private TimeGM timeGM;
    private PenInkM penInkM;

    //ペンのスプリクト
    private PenM penM;

    //効果音用
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;
    private bool sEffectFlag = false;

    public bool LineFlag
    {
        get { return this.lineFlag; }
        set { this.lineFlag = value; }
    }

    private void Awake()
    {
        penM = FindObjectOfType<PenM>();
    }


    // Start is called before the first frame update
    void Start()
    {
        //初期化

        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        penDisplayC = FindObjectOfType<PenDisplay>();
        timeGM = FindObjectOfType<TimeGM>();
        penInkM = FindObjectOfType<PenInkM>();

        //iceSkelton = instansIcePrefab.GetComponent<SkeletonAnimation>();
        //fireSkelton = instansfirePrefab.GetComponent<SkeletonAnimation>();
        audioSource = GetComponent<AudioSource>();

        //iceAnimationState = iceSkelton.AnimationState
        //fireAnimationState = fireSkelton.AnimationState;

        //ダブルタッチ無効
        Input.multiTouchEnabled = false;

        //アニメーション初期化
        //iceSkelton.AnimationName = "None";
        //fireSkelton.AnimationName = "None";

        //線のポイント初期化
        linePoints = new List<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {
        /*座標に関する処理*/
        //マウス座標取得
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        //ワールド座標へ変換
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        //線引ける線の上限
        Vector3 u = range.transform.position;

        //ペンの種類によって切り替えるプラグラム
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                sMaterial = iceMaterial;
                lineMaterial = iceTexture;
                lineColor = iceColor;
                //nowSkeletonAnima = iceSkelton;
                //iceSkelton.AnimationName = "animetion";

                //Debug.Log(lineColor);
                break;

            case PenM.PenCom.Fire:
                sMaterial = generalMaterial;
                lineMaterial = fireTexture;
                lineColor = fireColor;

                break;

            case PenM.PenCom.General:
                sMaterial = generalMaterial;
                lineMaterial = generalTexture;
                lineColor = generalColor;
                //nowSkeletonAnima = null;
                break;
        }

        //線が引ける時
        if(pasueDisplayC.MenuFlag == false && penDisplayC.PenMenuFlag == false && timeGM.TimeFlag == true)
        {
            //特定の座標以下なら線を引けるようにする
            if (u.y > worldPos.y)
            {
                //左クリックされたら
                if (Input.GetMouseButtonDown(0))
                {
                        switch (penM.NowPen)
                        {
                            case PenM.PenCom.Ice:
                            if (penM.IceDrawFlag == true)
                            {
                               lineFlag = true;
                                _addLineObject();
                            }
                            break;

                            case PenM.PenCom.Fire:
                            if (penM.FireDrawFlag == true)
                            {
                                _addFireData();
                            }
                            break;

                            case PenM.PenCom.General:
                            if (penM.GeneralDrawFlag == true)
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
            }
            else
            {
                lineFlag = false;
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
        //lineObjを自身（Stroke）の子要素に設定
        lineObj.transform.SetParent(transform);
        //レイヤーをGroundにする
        lineObj.layer = 6;

        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                lineObj.tag = "IceGround";
            break;

            case PenM.PenCom.General:
                lineObj.tag = "Ground";
            break;
        }
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
        lineRenderer.renderingLayerMask = 1;
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

    private void _addFireData()
    {
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        //ワールド座標へ変換
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        GameObject obj = Instantiate(fireObject,worldPos,Quaternion.identity);
        penInkM.FireDrawCount++;
 
    }
}
