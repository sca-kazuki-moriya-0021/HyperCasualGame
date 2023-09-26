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
    Vector3 prevPos;

    #region//プレイヤー関係とアニメーション
    //x方向に進むスピード(一般的)
    [SerializeField,Header("普通の床で進むスピード")]
    private float xMoveFloorSpeed = 10.0f;
    //x方向に進むスピード(氷)
    [SerializeField,Header("氷の床で進むスピード")]
    private float xMoveIceSpeed = 15.0f;

    private GameObject child;

    //オブジェクトに設定されているアニメーション
    private SkeletonAnimation skeletonAnimation = default;
    //Spineアニメーションを艇起用するために必要なState
    private Spine.AnimationState animationState = default;

    //再生するアニメーション名
    [SerializeField,Header("歩行アニメーション")]
    private string moveAnimation;
    [SerializeField, Header("落下アニメーション")]
    private string fallAnimation;
    [SerializeField, Header("飛び降りアニメーション")]
    private string offJumpAnimation;
    [SerializeField,Header("着地モーション")]
    private string lindingAnimation;
    [SerializeField,Header("ジャンプモーション")]
    private string jumpAnimation;
    [SerializeField,Header("ジャンプ中モーション")]
    private string jumpDuringA;

    //歩くアニメーション制御
    private bool moveAnimaFlag = false;
    //氷の上に乗っているかどうか
    private bool iceWalkFlag = false;
    //落下中
    private bool fallFlag = false;
    //ゲームオーバー
    private bool gameOverFlag = false;
    //ジャンプ
    private bool jumpFlag = false;
    //飛び降り
    private bool offJumpFlag = false;
    //着地中
    private bool lindingFlag = false;
    //ジャンプ中
    private bool jumpingFlag = false;

    #endregion

    #region//レイ関係
    //　レイを飛ばす場所
    [SerializeField,Header("レイを飛ばす位置")]
    private Transform rayPosition;
    /*[SerializeField]
    private Vector2 upOffset;
    [SerializeField]
    private Vector2 downOffset;*/
    //　レイを飛ばす距離
    [SerializeField,Header("横に飛ばすレイの長さ")]
    private float wRayRange;
    [SerializeField,Header("縦に飛ばすレイの長さ")]
    private float hRayRange;
    //　落ちたy座標
    private float fallenPosition;
    //　落下してから地面に落ちるまでの距離
    private float fallenDistance;
    //ヒットしたオブジェクトの角度
    private Quaternion hitObjectRotaion;
    //ヒットしたオブジェクト保存用
    private RaycastHit2D headHitObject;
    private RaycastHit2D footHitObject;
    private RaycastHit2D forwardHitObject;
    #endregion

    //RigidBodyとカプセルコライダーの定義
    private Rigidbody2D rb;
    private EdgeCollider2D col2D;

    //hitしたコライダー検知用
    private Collider2D hitCollider;
    private Collider2D hitBackCollider;

    //スプリクト用
    private TotalGM gm;
    private TimeGM timeGm;

    //タンジェント
    private float tan;

    //重力　使うかはわからん
    [SerializeField,Header("重力用だけど使ってない")]
    private float _graviry = 9.80655f;
    private Vector3 _moveVelocity;

    #region//効果音関係
    private AudioSource audios = null;
    [SerializeField,Header("木に当たった音")]
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
        child = transform.GetChild(0).gameObject;

        prevPos = this.transform.position;
        //呼び出し
        gm = FindObjectOfType<TotalGM>();
        timeGm = FindObjectOfType<TimeGM>();
        col2D = GetComponent<EdgeCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // ゲームオブジェクトのSkeletonAnimationを取得
        skeletonAnimation =child.GetComponent<SkeletonAnimation>();
        // SkeletonAnimationからAnimationStateを取得
        animationState =skeletonAnimation.AnimationState;

        //レイを飛ばす処理
        lineCastF = StartCoroutine(StartLineCast());

        //落ちた時に使う数値リセット
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(fallFlag);
        //ゲームオーバーシーンにいく処理
        if (gameOverFlag == true)
        {
            gm.BackScene = gm.MyGetScene();
            gameOverFlag = false;
            SceneManager.LoadScene("GameOver");
        }

        //坂角度計算とジャンプ処理
        if (jumpFlag == false && fallFlag == false)
        {
            SlopeUp();
        }

       
    }

    private void FixedUpdate()
    {
        //if(timeGm.TimeFlag == false)
        {
            //移動
            if (jumpFlag == false && offJumpFlag == false && lindingFlag == false)
            {
                //Debug.Log("動いている");
                //移動のアニメーション流しと移動
                if (iceWalkFlag == false)
                {
                    transform.Translate(xMoveFloorSpeed * Time.deltaTime * 0.2f, 0, 0);
                    if (moveAnimaFlag == false)
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

            //レイの角度計算
            if (lineCastF != null || jumpFlag == true || fallFlag == true)
            {
                RayAngleIns();
            }
        }
    }

    //移動のアニメーション処理
    private void OnCompleteAnimation()
    {
        moveAnimaFlag = true;
        skeletonAnimation.timeScale = 1;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry trackEntry = animationState.SetAnimation(0, moveAnimation, true);
        skeletonAnimation.skeleton.SetToSetupPose();
    }

    //下方向レイの角度計算用
    public void RayAngleIns()
    {

        var downObject = GetDownObject();
        if (fallFlag == true)
        {
            //レイを出す
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * hRayRange, Color.red);
            
            //地面に触れた時に各種フラグとアニメーション制御
            if (downObject && downObject.transform.gameObject.CompareTag("Ground"))
            {
                fallFlag = false;
                lindingFlag = true;
                Debug.Log("コルーチン前" + transform.position );
                LindingCoroutine();
            }
            else if(downObject && downObject.transform.gameObject.CompareTag("Wall"))
            {
                fallFlag = false;
                lindingFlag = true;
                LindingCoroutine();
            }
            //上と同じ(こっちは氷の上なのでiceWalkFlagのみ変更)
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            { 
                fallFlag = false;
                iceWalkFlag = true;
                lindingFlag = true;
                LindingCoroutine();
            }
        }
        //地面から離れた時の処理
        else
        {
           Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * hRayRange, Color.blue);
           //地面から空中にいった時(fallFlag == false　から　true　になる時)
           if (!IsOnGrounds(downObject))
           {
             //地面から一回でもLineCastの線が離れたとき = 落下状態とする
             //その時に落下状態を判別するためfallFlagをtrueにする
             fallFlag = true;
             iceWalkFlag = false;
             
             if(jumpFlag == false)
             {
               offJumpFlag = true;
               //アニメーション
              StartCoroutine(StartoffJump());
             }
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
        
        hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * hRayRange, LayerMask.GetMask("Ground"));
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

        if (h.transform.gameObject.CompareTag("Wall"))
        {
            return true;
        }

        if (h.transform.gameObject.CompareTag("IceGround"))
        {
            return true;
        }
        return false;
    }

    //レイを飛ばすコルーチン
    private IEnumerator StartLineCast()
    {
        while (true)
        {
            yield return null;
        }
    }

    //飛び降りのアニメーション制御
    private IEnumerator StartoffJump()
    {
        //飛び降りのアニメーション流し、その後落下中のアニメーションを流す
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry fallTrackEntry = animationState.SetAnimation(0, offJumpAnimation, false);
        fallTrackEntry.Complete += FallSpineComplete;
        skeletonAnimation.skeleton.SetToSetupPose();

        //一旦レイを無くす
        lineCastF = null;

        yield return new WaitForSeconds(0.01f);

        offJumpFlag = false;

        //レイを復活
        lineCastF = StartCoroutine(StartLineCast());

        //コルーチンストップ
        StopCoroutine(StartoffJump());

    }

    //落下中のアニメーションを流す
    private void FallSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        animationState.SetAnimation(0, fallAnimation, false);
        skeletonAnimation.skeleton.SetToSetupPose();
    }


    //着地のアニメーション
    private void LindingCoroutine()
    {
        //着地用フラグ変更
        lindingFlag = false;
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry moveTrackEntry = animationState.SetAnimation(0, lindingAnimation, false);
        moveTrackEntry.Complete += MoveSpineComplete;
        Debug.Log(transform.position);
        skeletonAnimation.skeleton.SetToSetupPose();

    }

    //歩行アニメーション制御用
    private void MoveSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 1;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry trackEntryA = animationState.SetAnimation(0, moveAnimation, true);
        moveAnimaFlag = false;
    }


    //坂をジャンプするときに使う関数
    private float SlopeUp()
    {
      //if(fallFlag == false||)
      
        //横方向のオブジェクト検知
        tan = 0f;
        var GetObject = ForwardObject();

        hitCollider = GetObject.collider;
           
        if (GetObject && GetObject.transform.gameObject.CompareTag("Wall"))
        {
           return_tan();

        }
        if (GetObject.normal.x == 1f)
        {
          tan = 0f;
        }

        return tan;
    }

    //横方向に飛ばす
    private RaycastHit2D ForwardObject()
    {
        Debug.DrawRay((Vector2)rayPosition.position , Vector2.right * wRayRange, Color.green);
        forwardHitObject = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.right * wRayRange , LayerMask.GetMask("Ground"));
        //Debug.Log(forwardHitObject);
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

        //タンジェントがn度以上ならジャンプ移行する
        if(tan <= Mathf.PI / 3 )
        {
            if(hitCollider.tag == "Wall")
            {
                //ジャンプモーション
                jumpFlag = true;
                skeletonAnimation.timeScale = 5;
                skeletonAnimation.state.ClearTrack(0);
                TrackEntry jumpTrackEntry = animationState.SetAnimation(0, jumpAnimation, false);
                jumpTrackEntry.Complete += JumpSpineComplete;
                skeletonAnimation.skeleton.SetToSetupPose();
                Debug.Log("ジャンプにはいったよ");

            }
        }
    }

    private void JumpSpineComplete(TrackEntry trackEntry)
    {

        skeletonAnimation.timeScale = 5;
        skeletonAnimation.state.ClearTrack(0);
        animationState.SetAnimation(0, jumpDuringA, true);
        skeletonAnimation.skeleton.SetToSetupPose();
        Debug.Log("ジャンプ中だよ");
        
        StartCoroutine(JumpStart());
    }

    //ジャンプ
    private IEnumerator JumpStart()
    {

        //二次元ベジェ曲線パターン
        //自分の位置
        var myP =transform.position;
        //特定の位置
        var x = Mathf.Abs(hitCollider.bounds.min.x + Mathf.Abs(myP.x));
        Vector3 toP =new Vector3(x,myP.y);
        //中間の位置
        var miS = Mathf.Abs(hitCollider.bounds.max.y);
        Vector3 miP = new Vector3(hitCollider.bounds.min.x,miS + 8f);
        Debug.Log(miP.x);
       

        //角度
        //var planeNormal = Vector3.up;

        //sleapパターンで使うもの
        {
            /*var center = hitCollider.bounds.center;
            center = Mathf.Abs(center);
            Vector3 vector = new Vector3(Mathf.Abs(hitCollider.bounds.center.x),center);
            myP -= center;
            toP -= center;*/
        }

        var sumTime = 0f;
        var miFlag = false;

        var y =transform.position;
        Debug.Log(y);

        while (true)
        {
            sumTime += Time.deltaTime /3;
            //移動するための引数
            var a = Vector3.Lerp(myP, miP, sumTime);
            var b = Vector3.Lerp(miP, toP, sumTime);

            if (jumpFlag == false)
            {
                hitCollider = hitBackCollider;
            }


            if (jumpFlag == true && transform.position != toP)
            {
                // 補間位置を反映
                transform.position = Vector3.Lerp(a, b, sumTime);
                if(miP .x >transform.position.x && miP.y>transform.position.y && miFlag == false)
                {
                    var r = y - miP;
                    float d =  - (Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
                    d = 180-d;
                    Quaternion quaternion =Quaternion.Euler(0,0,d);
                    this.transform.rotation = quaternion;
                    Debug.Log("as");
                   
                }

                if (transform.position.x >= miP.x)
                {
                    miFlag = true;
                    Debug.Log("mikasa");
                }

                if (miFlag == true)
                {

                        if (toP.x > transform.position.x && toP.y <transform.position.y)
                        {
                            var r = toP -y;
                            var d = (Mathf.Atan2(r.y, r.x) * Mathf.Rad2Deg);
                           
                            Debug.Log(d);
                            var quaternion = Quaternion.Euler(0, 0, d);
                            this.transform.rotation = quaternion;
                            Debug.Log("aski");
                        }
                    
                }
            }

            if (jumpFlag == true && transform.position == toP)
            {
                miFlag = false;
                jumpFlag = false;
            }

            yield return null;
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
            iceWalkFlag = false;
            jumpFlag = false;
        }

        if (other.gameObject.CompareTag("IceGround"))
        {
            iceWalkFlag = true;
            jumpFlag = false;
        }

        if (other.gameObject.CompareTag("IceGround") && other.gameObject.CompareTag("Ground"))
        {
            iceWalkFlag = true;
            jumpFlag = false;
        }

        if(other.gameObject.CompareTag("Wall"))
        {
            jumpFlag = false;
            iceWalkFlag = false;
        }

        //障害物に当たったら
        if (other.gameObject.CompareTag("Obstacle"))
        {
            audios.PlayOneShot(sound1);

            GameOverFlag = true;
        }

        if (other.gameObject.CompareTag("OutSide"))
        {
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
}
