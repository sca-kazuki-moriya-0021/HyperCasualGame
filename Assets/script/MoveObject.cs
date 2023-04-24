using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class MoveObject : MonoBehaviour
{
    #region//プレイヤー関係
    //プレイヤーのHpを保管する変数
    private int oldHp;
    //x方向に進むスピード(一般的)
    private float xMoveFloorSpeed = 3.0f;
    //x方向に進むスピード(氷)
    private float xMoveIceSpeed = 5.0f;
    #endregion

    //プレイヤーアニメーション用変数
    //[SerializeField]
    //private Animator anime = null;

    //氷の上に乗っているかどうか
    private bool iceWalkFlag = false;

    #region//レイ関係
    //　レイを飛ばす場所
    [SerializeField]
    private Transform rayPosition;
    //　レイを飛ばす距離
    [SerializeField]
    private float rayRange;
    //　落ちたy座標
    private float fallenPosition;
    //　落下してから地面に落ちるまでの距離
    private float fallenDistance;
    //　どのぐらいの高さからダメージを与えるか
    //[SerializeField]
    //private float takeDamageDistance = 3f;
    #endregion

    #region//状況に応じて使用するフラグ関係
    //移動
    private bool moveFlag = false;
    //落下中
    private bool fallFlag = false;
    //着地中
    private bool landFlag = false;
    //ゲームオーバー
    private bool gameOverFlag = false;
    //クリア
    private bool clearFlag = false;
    #endregion

    //RigidBodyとボックスコライダーの定義
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    //スプリクト用
    private TotalGM gm;

    #region//効果音関係
    private AudioSource audios = null;
    /*[SerializeField]
    private AudioClip runSE;//移動用
    [SerializeField]
    private AudioClip iceRunSE;//氷移動用
    [SerializeField]
    private AudioClip itemGetSE;//アイテムとった時
    [SerializeField]
    private AudioClip gameOverSE;//ゲームオーバーになった時
    //効果音がなったら
    private bool soundFlag = true;*/
    #endregion

    //コルーチン戻り値用
    private Coroutine lineCast;

    public bool GameOverFlag
    {
        get { return this.gameOverFlag; }
        set { this.gameOverFlag = value; }
    }

    public bool ClearFlag
    {
        get { return this.clearFlag; }
        set { this.clearFlag = value; }
    }

    

    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        //this.anime = GetComponent<Animator>();

        //hp初期化
        oldHp = gm.PlayerHp;

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;

    }

    // Update is called once per frame
    void Update()
    {

        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.down * rayRange, Color.red, 1.0f);
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.forward * rayRange, Color.red, 1.0f);
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.left * rayRange, Color.red, 1.0f);
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.right * rayRange, Color.red, 1.0f);
        Debug.DrawLine(rayPosition.position, rayPosition.position + Vector3.back * rayRange, Color.red, 1.0f);

        Debug.Log(fallFlag);

        if (gm.PlayerHp < oldHp)
        {
            if (gm.PlayerHp == 0)
            {
                gameOverFlag = true;
            }
        }

        RaycastHit hit;
        if (Physics.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange ,out hit,LayerMask.GetMask("Ground")))
        {
            Debug.Log("ari");
            fallFlag = false;
        }
        else
        {
            //レイが届かないなら
            if (!Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange, LayerMask.GetMask("Ground")))
            {
                //地面から一回でもLineCastの線が離れたとき = 落下状態とする
                //その時に落下状態を判別するためfallFlagをtrueにする
                //最初の落下地点を設定
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                //フラグを立てる
                fallFlag = true;
                iceWalkFlag = false;
                //Debug.Log("地面から離れたよ");
            }
        }

        if(fallFlag == false &&iceWalkFlag == false)
        {
            transform.Translate(transform.rotation.x * xMoveFloorSpeed, 0,0);
        }
        if(fallFlag == false && iceWalkFlag == true)
        {
            transform.Translate(transform.rotation.x * xMoveIceSpeed,0,0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Ground"))
        {
            iceWalkFlag = false;
        }

        if (other.gameObject.CompareTag("IceGround"))
        {
            iceWalkFlag = true;
        }


        if (other.gameObject.CompareTag("IceGround")&&other.gameObject.CompareTag("Ground"))
        {
            iceWalkFlag = true;
        }


        if (other.gameObject.CompareTag("GoalPoint"))
        {

        }
    }

    //効果音を流す処理
    public void PlaySE(AudioClip clip)
    {
        if (audios != null)
        {
            audios.PlayOneShot(clip);
        }
        else
        {

        }
    }

    /*public void WalkSE()
    {
        if(iceWalkFlag == true)
        {
            PlaySE(iceRunSE);
        }
        if(iceWalkFlag == false)
        {
            PlaySE(runSE);
        }
    }*/

}
