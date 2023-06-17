using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineDraw : MonoBehaviour
{

    //線の材質
    [SerializeField]
    private Material lineMaterial;
    //線の色
    [SerializeField]
    private Color lineColor;
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
    
    private PhysicsMaterial2D sMaterial;
    private EdgeCollider2D edge;

    private void Start()
    {
        //Listの初期化
        linePoints = new List<Vector2>();
    }

    private void Update()
    {
        //左クリックされたら
        if (Input.GetMouseButtonDown(0))
        {
            _addLineObject();
        }

        //クリック中（ストローク中）
        if (Input.GetMouseButton(0))
        {
            _addPositionDataToLineRenderer();
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
        lineObj.AddComponent<EdgeCollider2D>();
        sMaterial = GetComponent<EdgeCollider2D>().sharedMaterial;
        //lineObjを自身（Stroke）の子要素に設定
        lineObj.transform.SetParent(transform);
        _initRenderer();
    }

    //lineObj初期化処理
    void _initRenderer()
    {
        //LineRendererを取得
        lineRenderer = lineObj.GetComponent<LineRenderer>();
        //ポジションカウントをリセット
        lineRenderer.positionCount = 0;
        //マテリアルを設定
        lineRenderer.material = lineMaterial;
        //マテリアルの色を設定
        lineRenderer.material.color = lineColor;
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
