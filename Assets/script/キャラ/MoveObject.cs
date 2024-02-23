using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;
using Spine;

public class MoveObject : MonoBehaviour
{
    #region//プレイヤー関係とアニメーション
    //x方向に進むスピード(一般的)
    [SerializeField,Header("普通の床で進むスピード")]
    private float xMoveFloorSpeed;
    //x方向に進むスピード(氷)
    [SerializeField,Header("氷の床で進むスピード")]
    private float xMoveIceSpeed;

    private GameObject child;

    //オブジェクトに設定されているアニメーション
    private SkeletonAnimation skeletonAnimation = default;
    //Spineアニメーションを艇起用するために必要なState
    private Spine.AnimationState animationState = default;

    //再生するアニメーション名
    [Header("アニメーション")]
    [SerializeField, Tooltip("歩行アニメーション")]
    private string moveAnimation;
    [SerializeField, Tooltip("落下アニメーション")]
    private string fallAnimation;
    [SerializeField, Tooltip("飛び降りアニメーション")]
    private string offJumpAnimation;
    [SerializeField, Tooltip("着地モーション")]
    private string lindingAnimation;
    [SerializeField, Tooltip("ジャンプモーション")]
    private string jumpAnimation;
    [SerializeField, Tooltip("ジャンプ中モーション")]
    private string jumpDuringAnimation;

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
    //
    private bool jumpingFlag;
    #endregion

    #region//レイ関係
    //　レイを飛ばす場所
    [Header("レイを飛ばす位置")]
    [SerializeField, Tooltip("中央から出てるレイ")]
    private Transform centerRayPosition;
    [SerializeField, Tooltip("下から出てるレイ")]
    private Transform downRayPosition;
    private RaycastHit2D centerHitObject;

    //　レイを飛ばす距離
    [SerializeField,Header("横に飛ばすレイの長さ")]
    private float wRayRange;
    [SerializeField,Header("縦に飛ばすレイの長さ")]
    private float hRayRange;
    #endregion

    //RigidBodyとカプセルコライダーの定義
    private Rigidbody2D rb;
    //private EdgeCollider2D col2D;

    //hitしたコライダー検知用
    private Collider2D hitCollider;
    private Collider2D hitBackCollider;

    //スプリクト用
    private TotalGM gm;
    private HearPositionCon positionCon;
    //private TimeGM timeGm;


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

    public bool JumpFlag
    {
        get { return this.jumpFlag; }
        set { this.jumpFlag = value; }
    }

    public bool FallFlag { get => fallFlag; set => fallFlag = value; }
    public bool LindingFlag { get => lindingFlag; set => lindingFlag = value; }

    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;

        //呼び出し
        gm = FindObjectOfType<TotalGM>();
        positionCon = FindObjectOfType<HearPositionCon>();
        //timeGm = FindObjectOfType<TimeGM>();
        //col2D = GetComponent<EdgeCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // ゲームオブジェクトのSkeletonAnimationを取得
        skeletonAnimation =child.GetComponent<SkeletonAnimation>();
        // SkeletonAnimationからAnimationStateを取得
        animationState =skeletonAnimation.AnimationState;

        //レイを飛ばす処理
        lineCastF = StartCoroutine(StartLineCast());

        fallFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //ゲームオーバーシーンにいく処理
        if (gameOverFlag == true)
        {
            gm.BackScene = gm.MyGetScene();
            gameOverFlag = false;
            SceneManager.LoadScene("GameOver");
        }
        
        if(jumpingFlag == false)
        {
            jumpingFlag = true;
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
                //移動のアニメーション流しと移動
                if (iceWalkFlag == false)
                {
                    transform.Translate(xMoveFloorSpeed * Time.deltaTime * 0.3f, 0, 0);
                    if (moveAnimaFlag == false)
                        OnCompleteAnimation();
                }
                if (iceWalkFlag == true)
                {
                    transform.Translate(xMoveIceSpeed * Time.deltaTime * 0.5f, 0, 0);
                    if (moveAnimaFlag == false)
                        OnCompleteAnimation();
                }
            }

            //レイの角度計算
            if ((lineCastF != null || jumpFlag == true || fallFlag == true))// && positionCon.SetFlag == false)
                RayAngleIns();
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
            Debug.DrawRay((Vector2)downRayPosition.position, Vector2.down * hRayRange, Color.red);
            
            //地面に触れた時に各種フラグとアニメーション制御
            if (downObject && downObject.transform.gameObject.CompareTag("Ground") 
                || downObject && downObject.transform.gameObject.CompareTag("AreaGround")
                || downObject && downObject.transform.gameObject.CompareTag("Wall"))
            {
                jumpFlag = false;
                fallFlag = false;
                lindingFlag = true;
                LindingCoroutine();
            }
            //上と同じ(こっちは氷の上なのでiceWalkFlagのみ変更)
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            {
                jumpFlag = false;
                fallFlag = false;
                iceWalkFlag = true;
                lindingFlag = true;
                LindingCoroutine();
            }
        }
        //地面から離れた時の処理
        else
        {
           Debug.DrawRay((Vector2)downRayPosition.position, Vector2.down * hRayRange, Color.blue);
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
            return;
    }

    //下にオブジェクトがあったときhit2Dを返す
    private RaycastHit2D GetDownObject()
    {
        RaycastHit2D hit2D;
        
        hit2D = Physics2D.Linecast((Vector2)downRayPosition.position, (Vector2)downRayPosition.position + Vector2.down * hRayRange, LayerMask.GetMask("Ground"));
        return hit2D;
    }

    //下方向にヒットした時の場合分け
    private bool IsOnGrounds(RaycastHit2D h)
    {
        if (!h)
         return false;

        if (h.transform.gameObject.CompareTag("AreaGround") || h.transform.gameObject.CompareTag("IceGround")
            || h.transform.gameObject.CompareTag("Ground")|| h.transform.gameObject.CompareTag("Wall"))
            return true;

        return false;
    }

    //レイを飛ばすコルーチン
    private IEnumerator StartLineCast()
    {
        while (true)
          yield return null;
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
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry moveTrackEntry = animationState.SetAnimation(0, lindingAnimation, false);
        moveTrackEntry.Complete += MoveSpineComplete;
        skeletonAnimation.skeleton.SetToSetupPose();

        //着地用フラグ変更
        lindingFlag = false;
    }

    //歩行アニメーション制御用
    private void MoveSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 1;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry trackEntryA = animationState.SetAnimation(0, moveAnimation, true);
        moveAnimaFlag = false;
    }

    //横方向に飛ばす
    private RaycastHit2D CenterHitObj()
    {
        Debug.DrawRay((Vector2)centerRayPosition.position, Vector2.right * wRayRange, Color.green);
        centerHitObject = Physics2D.Linecast((Vector2)centerRayPosition.position, (Vector2)centerRayPosition.position + Vector2.right * wRayRange, LayerMask.GetMask("Ground"));
        return centerHitObject;
    }

    //壁をジャンプするときに使う関数
    private void SlopeUp()
    {
        //横方向のオブジェクト検知
        var tan = 0f;
        var getObject = CenterHitObj();
        if(getObject.collider != null)
            //positionCon.RayHit = getObject;

        if(getObject.collider == null || getObject.collider == getObject.collider.CompareTag("Ground"))
        {
            jumpingFlag = false;
            return;
        }
        else
        {
            hitCollider = getObject.collider;
            return_tan(tan);
        }
    }

    //タンジェント計算
    private void return_tan(float ang)
    {
        //タンジェント計算
        if (CenterHitObj().normal.x > 0f)
            ang = Mathf.PI * 0.5f + Mathf.Atan(CenterHitObj().normal.y / Mathf.Abs(CenterHitObj().normal.x));

         var tan = Mathf.Tan(ang);

        //タンジェントがn度以上ならジャンプ移行する
        if(tan <= Mathf.PI / 3 )
        {
            //ジャンプモーション
            jumpFlag = true;
            skeletonAnimation.timeScale = 5;
            skeletonAnimation.state.ClearTrack(0);
            TrackEntry jumpTrackEntry = animationState.SetAnimation(0, jumpAnimation, false);
            jumpTrackEntry.Complete += JumpSpineComplete;
            skeletonAnimation.skeleton.SetToSetupPose();
        }
    }

    //ジャンプ中のアニメーション
    private void JumpSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 5;
        skeletonAnimation.state.ClearTrack(0);
        animationState.SetAnimation(0, jumpDuringAnimation, true);
        skeletonAnimation.skeleton.SetToSetupPose();
        StartCoroutine(JumpStart());
    }

    //ジャンプの処理
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
        Vector3 miP = new Vector3(hitCollider.bounds.min.x,miS + 3f);
        var flag = false;

        var sumTime = 0f;

        while (true)
        {
            sumTime += Time.deltaTime /3;
            //移動するための引数
            var a = Vector3.Lerp(myP, miP, sumTime);
            var b = Vector3.Lerp(miP, toP, sumTime);

            if (jumpFlag == true && transform.position != toP)
            {
                // 補間位置を反映
                transform.position = Vector3.Lerp(a, b, sumTime);
                if(flag == false)
                {
                    flag = true;
                    StartCoroutine(SetQuaternion());
                }  
            }
            if (jumpFlag == true && transform.position == toP)
                jumpFlag = false;

            if(jumpFlag == false)
              break;

            yield return null;
        }

        hitCollider = hitBackCollider;
        jumpingFlag = false;
        StopCoroutine(JumpStart());
    }

    //ジャンプ中の角度調整
    private IEnumerator SetQuaternion()
    {
        while (true)
        {
            if (jumpFlag == false)
                break;

            var t = transform.position;
            yield return new WaitForSeconds(0.01f);
            var t2 = transform.position;

            Vector2 m = t2 -t;
            transform.rotation = Quaternion.FromToRotation(Vector2.right, m);
        }
        this.transform.rotation = Quaternion.Euler(0, 0, 0);
        StopCoroutine(SetQuaternion());
    }
    //何かしらに当たった時
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("AreaGround") || other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Wall"))
        {
            iceWalkFlag = false;
            jumpFlag = false;
        }

        if (other.gameObject.CompareTag("IceGround") || other.gameObject.CompareTag("IceGround") && other.gameObject.CompareTag("AreaGround")
            || other.gameObject.CompareTag("IceGround") && other.gameObject.CompareTag("Ground"))
        {
            iceWalkFlag = true;
            jumpFlag = false;
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
        ||other.gameObject.CompareTag("FlameGround")
        || other.gameObject.CompareTag("OutSide"))
             GameOverFlag = true;
    }

    //効果音を流す処理
    public void PlaySE(AudioClip clip)
    {
        if (audios != null)
            audios.PlayOneShot(clip);
    }
}
