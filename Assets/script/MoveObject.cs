using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class MoveObject : MonoBehaviour
{
    #region//プレイヤー関係
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
    //レイを飛ばす角度計算
    /*[SerializeField]
    private float _Velocity_0;
    [SerializeField]
    private float degree;
    [SerializeField]
    private float angle_Split;
    //各計算用変数
    float _theta;
    float PI = Mathf.PI;
    //レイを飛ばす角度保存用
    Vector2 rayVector2;*/
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

    //ヒットしたオブジェクトの角度
    private Quaternion hitObjectRotaion;

    //どっちの方向に線をひいたか
    private bool rightLine = true;

    //タンジェント
    private float tan;

    //重力　使うかはわからん
    [SerializeField]
    private float _graviry = 9.80655f;
    private Vector3 _moveVelocity;

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

    SelectAngle selectAngle;

    enum SelectAngle
    {
        
    }

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
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        //this.anime = GetComponent<Animator>();;

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;
    }

    // Update is called once per frame
    /*void Update()
    {


    }*/

    private void FixedUpdate()
    {
        Debug.Log(fallFlag);


        //レイの角度計算
        RayAngleIns();

        //debug
        /*RaycastHit2D hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange, LayerMask.GetMask("Ground"));
        Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.red);
        if (hit2D.transform.gameObject.CompareTag("IceGround") || hit2D.transform.gameObject.CompareTag("Ground"))
        {
            Debug.Log("OK");
        }*/


        //自由落下
        /*moveVelocity.y += -_graviry *Time.fixedDeltaTime;

        var p = transform.position;

        p+= _moveVelocity *Time.fixedDeltaTime;
        transform.position =p;*/

        //坂を上る処理

        /*tan = 0f;
        RaycastHit2D hit2D = Physics2D.Raycast((Vector2)rayPosition.position, Vector2.up *rayRange);
        if(fallFlag == false && hit2D.transform.gameObject.CompareTag("Ground"))
        {
            
        }*/
        //移動
        //trueなら右
        if (rightLine == true)
        {

            if (fallFlag == false && iceWalkFlag == false)
            {
                transform.Translate(xMoveFloorSpeed*Time.fixedDeltaTime, 0, 0);
            }
            if (fallFlag == false && iceWalkFlag == true)
            {
                transform.Translate(xMoveIceSpeed*Time.fixedDeltaTime, 0, 0);
            }
        }
        else//falseなら左
        {

            if (fallFlag == false && iceWalkFlag == false)
            {
                transform.Translate(-xMoveFloorSpeed, 0, 0);
            }
            if (fallFlag == false && iceWalkFlag == true)
            {
                transform.Translate(-xMoveIceSpeed, 0, 0);
            }
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
            SceneManager.LoadScene("Goal");
        }

        //障害物に当たったら
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("OutSide"))
        {
            GameOverFlag = true;
            //シーン呼び出し

        }

        //炎&&水たまり&&木材に当たる
        /*if (other.gameObject.CompareTag("FlameFloor")||
            other.gameObject.CompareTag("PuddleFloor")||
            other.gameObject.CompareTag("Obstacle")     )
        {
            
        }*/
    }

    //レイの角度計算用
    public void RayAngleIns()
    {
        /*for (int i = 0; i <= (angle_Split - 1); i++)
        {
            //レイの端から端までの角度
            float AngleRange = PI * (degree / 180);

            //レイに渡す角度の計算
            if (angle_Split > 1) _theta = (AngleRange / (angle_Split - 1)) * i + 0.5f * (PI - AngleRange);
            else _theta = 0.5f * PI;

            //取得した角度を保存
            rayVector2.x = _Velocity_0 * Mathf.Cos(_theta);
            rayVector2.y = _Velocity_0 * Mathf.Sin(_theta);
        }*/
        
        var downObject = GetDownObject();

        if (fallFlag == true)
        {
            //レイを出す
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.red);
            if (downObject && downObject.transform.gameObject.CompareTag("Ground"))
            {
                /*hitObjectRotaion = hit2D.transform.rotation;
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.rotation = hitObjectRotaion;
                objectRotaion = hitObjectRotaion;*/
                Debug.Log("ki");
                fallFlag = false;
            }
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            {
                /*hitObjectRotaion = hit2D.transform.rotation;
                transform.rotation = new Quaternion(0, 0, 0, 0);
                transform.rotation = hitObjectRotaion;
                objectRotaion = hitObjectRotaion;*/
                Debug.Log("ari");
                fallFlag = false;
                iceWalkFlag = true;
            }

        }
        else
        {
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.blue);

            //地面から空中にいった時(fallFlag == false　から　true　になる時)
            if (!IsOnGrounds(downObject))
            {
                //地面から一回でもLineCastの線が離れたとき = 落下状態とする
                //その時に落下状態を判別するためfallFlagをtrueにする
                //最初の落下地点を設定
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                //hitObjectRotaion = default;
                //フラグを立てる
                fallFlag = true;
                iceWalkFlag = false;
                Debug.Log("地面から離れたよ");
            }

        }

        //レイが何にも当たっていないときは強制リターン
        if (!downObject)
        {
            return ;
        }
    }

    private RaycastHit2D GetDownObject()
    {
        RaycastHit2D hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange, LayerMask.GetMask("Ground"));

        return hit2D;
    }

    private bool IsOnGrounds(RaycastHit2D h)
    {
        if (!h)
        {
            return false;
        }
        if (h.transform.gameObject.CompareTag("Ground"))
        {
            return true;
        }
        if (h.transform.gameObject.CompareTag("IceGround"))
        {
            return true;
        }
        return false;
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
