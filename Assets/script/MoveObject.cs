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
    [SerializeField]
    private Animator anime = null;

    //氷の上に乗っているかどうか
    private bool IceWalkFlag = false;


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
    [SerializeField]
    private AudioClip runSE;//移動用
    [SerializeField]
    private AudioClip iceRunSE;//氷移動用
    [SerializeField]
    private AudioClip itemGetSE;//アイテムとった時
    [SerializeField]
    private AudioClip gameOverSE;//ゲームオーバーになった時
    //効果音がなったら
    private bool soundFlag = true;
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
        this.anime = GetComponent<Animator>();

        //hp初期化
        oldHp = gm.PlayerHp;

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;

        //ray投射開始
        lineCast = StartCoroutine("StartLineCast");
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.PlayerHp < oldHp)
        {
            if (gm.PlayerHp == 0)
            {
                gameOverFlag = true;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {

        }

        if (other.gameObject.CompareTag("IceGorund"))
        {

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

    public void WalkSE()
    {
        if(IceWalkFlag == true)
        {
            PlaySE(iceRunSE);
        }
        if(IceWalkFlag == false)
        {
            PlaySE(runSE);
        }
    }

}
