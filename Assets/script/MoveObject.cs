using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using Spine.Unity;
using Spine;

public class MoveObject : MonoBehaviour
{
    #region//プレイヤー関係
    //x方向に進むスピード(一般的)
    [SerializeField]
    private float xMoveFloorSpeed = 6.0f;
    //x方向に進むスピード(氷)
    [SerializeField]
    private float xMoveIceSpeed = 7.0f;
    //ジャンプ中に使う速度
    private float moveSpeed = 10f;
    //デフォルトの角度
    //private Quaternion defeltRotation;

    //再生するアニメーション名
    [SerializeField,Header("歩行アニメーション")]
    private string moveAnimation;
    [SerializeField, Header("落下アニメーション")]
    private string fallAnimation;
    [SerializeField, Header("飛び降りアニメーション")]
    private string offJumpAnimation;
    [SerializeField,Header("着地モーション")]
    private string lindingAnimation;

    //オブジェクトに設定されているアニメーション
    private SkeletonAnimation skeletonAnimation = default;
    //Spineアニメーションを艇起用するために必要なState
    private Spine.AnimationState animationState = default;
    //歩くアニメーション制御
    private bool moveAnimaFlag = false;
    //飛び降りアニメーション制御
    private bool offJumpAnimaFlag = false;
    #endregion


    //氷の上に乗っているかどうか
    private bool iceWalkFlag = false;

    #region//レイ関係
    //　レイを飛ばす場所
    [SerializeField]
    private Transform rayPosition;
    /*[SerializeField]
    private Vector2 upOffset;
    [SerializeField]
    private Vector2 downOffset;*/
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

    //ヒットしたオブジェクトの角度
    private Quaternion hitObjectRotaion;

    private RaycastHit2D headHitObject;
    private RaycastHit2D footHitObject;
    private RaycastHit2D forwardHitObject;

    //hitしたコライダー長さ
    private float xLen;
    private float yLen;
    private Vector2 xVLen;
    private Vector2 yVLen;
    //キャラからhitしたオブジェクトの距離
    private float pDis;
    private Vector2 pVDis;
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
    //ジャンプ
    private bool jumpFlag = false;

    #endregion

    //RigidBodyとカプセルコライダーの定義
    private Rigidbody2D rb;
    private CapsuleCollider2D col2D;

    //
    readonly Collider[] _result = new Collider[5];

    //hitしたコライダー検知用
    private Collider2D hitCollider;
    private Collider2D hitBackCollider;

    //スプリクト用
    private TotalGM gm;

    //方向判別
    //private bool dirSwitchFlag = false;

    //タンジェント
    private float tan;

    //重力　使うかはわからん
    [SerializeField]
    private float _graviry = 9.80655f;
    private Vector3 _moveVelocity;

    #region//効果音関係
    private AudioSource audios = null;
    [SerializeField]
    private AudioClip sound1;
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
    private Coroutine lineCastF;

    public bool GameOverFlag
    {
        get { return this.gameOverFlag; }
        set { this.gameOverFlag = value; }
    }

    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<TotalGM>();
        col2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // ゲームオブジェクトのSkeletonAnimationを取得
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        // SkeletonAnimationからAnimationStateを取得
        animationState = skeletonAnimation.AnimationState;

        lineCastF = StartCoroutine(StartLineCast());

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(jumpFlag);

        if(gameOverFlag == true)
        {
            gm.BackScene = gm.MyGetScene();
            gameOverFlag = false;
            SceneManager.LoadScene("GameOver");
        }
  
    }

    private void FixedUpdate()
    {
        Debug.Log("飛び降り:" +offJumpAnimaFlag);

        //移動
        //trueなら右
        if (jumpFlag == false)
        {

            if (iceWalkFlag == false)
            {
                transform.Translate(xMoveFloorSpeed * Time.deltaTime * 0.2f, 0, 0);
                if(moveAnimaFlag == false)
                {
                    OnCompleteAnimation();
                }

            }
            if (iceWalkFlag == true)
            {
                transform.Translate(xMoveIceSpeed * Time.deltaTime * 0.2f, 0, 0);
                if (moveAnimaFlag == false)
                {
                    OnCompleteAnimation();
                }
            }
        }

        //坂角度計算とジャンプ処理
        if (jumpFlag == false)
        {
            SlopeUp();

        }

        //レイの角度計算
        if(lineCastF != null)
        {
            RayAngleIns();
            Debug.Log("確かめよう");
        }


        //ジャンプの急降下
        if(hitCollider != null)
        {
            if (jumpFlag == true && transform.position.x < hitCollider.bounds.max.x &&
            transform.position.y < hitCollider.bounds.max.y + 2f)
            {
                //Debug.Log("suka");
                rb.AddForce(Vector2.down * moveSpeed * 10, ForceMode2D.Force);
                hitCollider.tag = hitBackCollider.tag;
                jumpFlag = false;
            }
        }
    }

    //下方向レイの角度計算用
    public void RayAngleIns()
    {
        var downObject = GetDownObject();
        if (fallFlag == true)
        {
            //レイを出す
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.red);
            
            //地面に触れた時に各種フラグとアニメーション制御
            if (downObject && downObject.transform.gameObject.CompareTag("Ground"))
            {
                skeletonAnimation.state.ClearTrack(0);
                TrackEntry moveTrackEntry = animationState.SetAnimation(0, lindingAnimation, false);
                moveTrackEntry.Complete += MoveSpineComplete;
                //歩行アニメーション制御
                skeletonAnimation.skeleton.SetToSetupPose();
                fallFlag = false;
                jumpFlag = false;
            }
            //上と同じ
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            {
                skeletonAnimation.state.ClearTrack(0);
                TrackEntry moveTrackEntry = animationState.SetAnimation(0, lindingAnimation, false);
                moveTrackEntry.Complete += MoveSpineComplete;
                skeletonAnimation.skeleton.SetToSetupPose();
                fallFlag = false;
                iceWalkFlag = true;
                jumpFlag = false;
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

                    fallFlag = true;
                    iceWalkFlag = false;

                    //アニメーション
                    StartCoroutine(StartoffJump());
                }
            
        }
        //レイが何にも当たっていないときは強制リターン
        if (!downObject)
        {
            return;
        }
    }

    //下にオブジェクトがあったときhit2Dを返す
    private RaycastHit2D GetDownObject()
    {
        RaycastHit2D hit2D;
        
        hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange, LayerMask.GetMask("Ground"));
        return hit2D;
       
    }

    //下方向にヒットした時の場合分け
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

    //着地後の歩行アニメーション制御用
    private void MoveSpineComplete(TrackEntry trackEntry)
    {
        animationState.SetAnimation(0, moveAnimation, true);
        moveAnimaFlag = false;
    }


    //坂をジャンプするときに使う関数
    private float SlopeUp()
    {
      //if(fallFlag == false||)
      {

            tan = 0f;
            var GetObject = ForwardObject();

            hitCollider = GetObject.collider;
           
           if (GetObject && GetObject.transform.gameObject.CompareTag("Ground"))
           {
              return_tan();

           }
           else if (GetObject && GetObject.transform.gameObject.CompareTag("IceGround"))
           {
              return_tan();
           }

           if (GetObject.normal.x == 1f)
           {
                    tan = 0f;
           }
      }
        return tan;
    }

    //横方向に飛ばす
    private RaycastHit2D ForwardObject()
    {
        Debug.DrawRay((Vector2)rayPosition.position , Vector2.right * rayRange, Color.green);
        forwardHitObject = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.right * rayRange, LayerMask.GetMask("Ground"));
        Debug.Log(forwardHitObject);
        return forwardHitObject;
    }
    
    //タンジェント計算
    private void return_tan()
    {
        //タンジェント計算
        if (ForwardObject().normal.x > 0f)
        {
            tan = Mathf.PI * 0.5f + Mathf.Atan(ForwardObject().normal.y / Mathf.Abs(ForwardObject().normal.x));
        }

        tan = Mathf.Tan(tan);
        //Debug.Log(tan);

        //タンジェントがn度以上なら進行方向を変える
        //if(tan <= Mathf.PI / )
        {
            Debug.Log("タグ変更");
            hitBackCollider = hitCollider;
            hitCollider.gameObject.tag = "Wall";
            if(hitCollider.tag == "Wall")
            {
                JumpCon();
            }
        }

    }
    
    //ジャンプ操作
    private void JumpCon()
    {
        Debug.Log("確かめ中");
        Debug.Log("キャラ:"+transform.position.y);
        Debug.Log("ヒットしたオブジェクト:"+hitCollider.bounds.max.y);


        xLen = hitCollider.bounds.max.x - hitCollider.bounds.min.x;
        yLen = hitCollider.bounds.max.y - hitCollider.bounds.min.y;
        //pDis =hitCollider.bounds.min.x - transform.position.x;
        pVDis = new Vector2((hitCollider.bounds.max.x - transform.position.x), (hitCollider.bounds.max.y - hitCollider.bounds.min.y));
        //pVDis =  pVDis.normalized;
        Debug.Log(pVDis);

        if(transform.position.x< hitCollider.bounds.max.x &&
            transform.position.y < hitCollider.bounds.max.y +2f)
        {
            Debug.Log("入ってる");
            rb.AddForce(pVDis * moveSpeed * 100);
            jumpFlag = true;
        }
    }


    //頭の上から横方向にレイを飛ばす
    /*private RaycastHit2D HeadGetForwardObject()
    {
        Debug.DrawRay((Vector2)rayPosition.position + upOffset, Vector2.right * rayRange, Color.green);
        headHitObject = Physics2D.Linecast((Vector2)rayPosition.position + upOffset, (Vector2)rayPosition.position + Vector2.right * (rayRange * 0.5f), LayerMask.GetMask("Ground"));
        return headHitObject;
    }*/

    //足元から横方向にレイを飛ばす
    /*private Vector2 FootGetForwardObject()
    {
        Debug.DrawRay((Vector2)rayPosition.position + downOffset, Vector2.right * rayRange, Color.green);
        var footHitObject = Physics2D.Linecast((Vector2)rayPosition.position + downOffset, (Vector2)rayPosition.position + Vector2.right * (rayRange * 2f), LayerMask.GetMask("Ground"));
        Vector2 a = Vector2.zero;
        
        while(footHitObject.collider != null)
        {
            jumpTime = true;
            footHitObject= new RaycastHit2D();
            a.y +=0.01f;
            if(a.y >= 5.0f)
            {
                break;
            }
            footHitObject = Physics2D.Linecast((Vector2)rayPosition.position + downOffset + a, (Vector2)rayPosition.position + Vector2.right * (rayRange * 2f), LayerMask.GetMask("Ground"));
        }
        return a + (Vector2)transform.position;

    }*/
    


    //何かしらに当たった時
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            offJumpAnimaFlag = false;
            iceWalkFlag = false;
        }

        if (other.gameObject.CompareTag("IceGround"))
        {
            offJumpAnimaFlag = false;
            iceWalkFlag = true;
        }

        if (other.gameObject.CompareTag("IceGround") && other.gameObject.CompareTag("Ground"))
        {
            iceWalkFlag = true;
        }

        //障害物に当たったら
        if (other.gameObject.CompareTag("Obstacle"))
        {
            audios.PlayOneShot(sound1);

            GameOverFlag = true;
        }

        //地形ギミックに触れたら
        if (other.gameObject.CompareTag("PuddleFloor")
        ||other.gameObject.CompareTag("FlameFloor")
        ||other.gameObject.CompareTag("FlameGround"))
        {
             GameOverFlag = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        /*var capsuleDir = new Vector2 { [(int)col2D.direction] = 1 };
        //Debug.Log("Dir:" + capsuleDir);
        var capsuleOffset = col2D.size.y / 2 - (col2D.size.x / 2);
       // Debug.Log("Offset:" + capsuleOffset);
        var localPoint0 = (Vector2)transform.position + col2D.offset - capsuleDir * capsuleOffset;
       // Debug.Log("ローカル座標0" + localPoint0);
        var localPoint1 = (Vector2)transform.position + col2D.offset + capsuleDir * capsuleOffset;
        //Debug.Log("ローカル座標1" + localPoint1);
        var point0 = (Vector2)transform.TransformPoint(localPoint0);
       // Debug.Log("ワールド座標:" + point0);
        var point1 = (Vector2)transform.TransformPoint(localPoint1);
        var r = transform.TransformVector(col2D.size.x / 2, col2D.size.y / 2, 0);
        var radius = Enumerable.Range(0, 2).Select(xy => xy == (int)col2D.direction ? 0 : r[xy]).Select(Mathf.Abs).Max();
        //Debug.Log((CapsuleDirection2D)radius);

        var merging = Physics2D.OverlapCapsule(point0, point1, (CapsuleDirection2D)radius, LayerMask.GetMask("Ground"));
        Debug.Log(collision.gameObject.name);*/
        {
             /*Vector3 a =  collision.collider.ClosestPoint(transform.position);
             Vector3 b = (a - transform.position);
             Vector3 c = b.normalized;
             float returnDistance = col2D.size.y * transform.localScale.y / 2 - b.magnitude;
            //Debug.Log(a);
            //Debug.Log("sizey" + col2D.size.y *transform.localScale.y);
            //Debug.Log(b.magnitude);
            //Debug.Log(returnDistance);

            transform.position = (transform.position - returnDistance * c);*/
        }
    }


    //移動のアニメーション処理
    private void OnCompleteAnimation()
    {
       moveAnimaFlag = true;
       skeletonAnimation.state.ClearTrack(0);
       animationState.SetAnimation(0, moveAnimation, true);
       Debug.Log("アニメーション");
       skeletonAnimation.skeleton.SetToSetupPose();
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

    private IEnumerator StartLineCast()
    {
        while (true)
        {
          yield return null;
        }
    }

    private IEnumerator StartoffJump()
    {
        if (offJumpAnimaFlag == false)
        {
                offJumpAnimaFlag = true;
                skeletonAnimation.state.ClearTrack(0);
                TrackEntry fallTrackEntry =  animationState.SetAnimation(0, offJumpAnimation, false);
                fallTrackEntry.Complete += FallSpineComplete;
                skeletonAnimation.skeleton.SetToSetupPose();
        }

        lineCastF = null;

        yield return new WaitForSeconds(1f);

        lineCastF = StartCoroutine(StartLineCast());
        Debug.Log("aiu");

        StopCoroutine(StartoffJump());

    }

    private void FallSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.state.ClearTrack(0);
        animationState.SetAnimation(0, fallAnimation, true);
        skeletonAnimation.skeleton.SetToSetupPose();
    }

}
