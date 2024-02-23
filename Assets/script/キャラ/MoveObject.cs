using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Spine.Unity;
using Spine;

public class MoveObject : MonoBehaviour
{
    #region//�v���C���[�֌W�ƃA�j���[�V����
    //x�����ɐi�ރX�s�[�h(��ʓI)
    [SerializeField,Header("���ʂ̏��Ői�ރX�s�[�h")]
    private float xMoveFloorSpeed;
    //x�����ɐi�ރX�s�[�h(�X)
    [SerializeField,Header("�X�̏��Ői�ރX�s�[�h")]
    private float xMoveIceSpeed;

    private GameObject child;

    //�I�u�W�F�N�g�ɐݒ肳��Ă���A�j���[�V����
    private SkeletonAnimation skeletonAnimation = default;
    //Spine�A�j���[�V��������N�p���邽�߂ɕK�v��State
    private Spine.AnimationState animationState = default;

    //�Đ�����A�j���[�V������
    [Header("�A�j���[�V����")]
    [SerializeField, Tooltip("���s�A�j���[�V����")]
    private string moveAnimation;
    [SerializeField, Tooltip("�����A�j���[�V����")]
    private string fallAnimation;
    [SerializeField, Tooltip("��э~��A�j���[�V����")]
    private string offJumpAnimation;
    [SerializeField, Tooltip("���n���[�V����")]
    private string lindingAnimation;
    [SerializeField, Tooltip("�W�����v���[�V����")]
    private string jumpAnimation;
    [SerializeField, Tooltip("�W�����v�����[�V����")]
    private string jumpDuringAnimation;

    //�����A�j���[�V��������
    private bool moveAnimaFlag = false;
    //�X�̏�ɏ���Ă��邩�ǂ���
    private bool iceWalkFlag = false;
    //������
    private bool fallFlag = false;
    //�Q�[���I�[�o�[
    private bool gameOverFlag = false;
    //�W�����v
    private bool jumpFlag = false;
    //��э~��
    private bool offJumpFlag = false;
    //���n��
    private bool lindingFlag = false;
    //
    private bool jumpingFlag;
    #endregion

    #region//���C�֌W
    //�@���C���΂��ꏊ
    [Header("���C���΂��ʒu")]
    [SerializeField, Tooltip("��������o�Ă郌�C")]
    private Transform centerRayPosition;
    [SerializeField, Tooltip("������o�Ă郌�C")]
    private Transform downRayPosition;
    private RaycastHit2D centerHitObject;

    //�@���C���΂�����
    [SerializeField,Header("���ɔ�΂����C�̒���")]
    private float wRayRange;
    [SerializeField,Header("�c�ɔ�΂����C�̒���")]
    private float hRayRange;
    #endregion

    //RigidBody�ƃJ�v�Z���R���C�_�[�̒�`
    private Rigidbody2D rb;
    //private EdgeCollider2D col2D;

    //hit�����R���C�_�[���m�p
    private Collider2D hitCollider;
    private Collider2D hitBackCollider;

    //�X�v���N�g�p
    private TotalGM gm;
    private HearPositionCon positionCon;
    //private TimeGM timeGm;


    #region//���ʉ��֌W
    private AudioSource audios = null;
    [SerializeField,Header("�؂ɓ���������")]
    private AudioClip sound1;
    /*[SerializeField]
    private AudioClip runSE;//�ړ��p
    [SerializeField]
    private AudioClip iceRunSE;//�X�ړ��p
    [SerializeField]
    private AudioClip itemGetSE;//�A�C�e���Ƃ�����
    [SerializeField]
    private AudioClip gameOverSE;//�Q�[���I�[�o�[�ɂȂ�����
    //���ʉ����Ȃ�����
    private bool soundFlag = true;*/
    #endregion

    //�R���[�`���߂�l�p
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

        //�Ăяo��
        gm = FindObjectOfType<TotalGM>();
        positionCon = FindObjectOfType<HearPositionCon>();
        //timeGm = FindObjectOfType<TimeGM>();
        //col2D = GetComponent<EdgeCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // �Q�[���I�u�W�F�N�g��SkeletonAnimation���擾
        skeletonAnimation =child.GetComponent<SkeletonAnimation>();
        // SkeletonAnimation����AnimationState���擾
        animationState =skeletonAnimation.AnimationState;

        //���C���΂�����
        lineCastF = StartCoroutine(StartLineCast());

        fallFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //�Q�[���I�[�o�[�V�[���ɂ�������
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
            //�ړ�
            if (jumpFlag == false && offJumpFlag == false && lindingFlag == false)
            {
                //�ړ��̃A�j���[�V���������ƈړ�
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

            //���C�̊p�x�v�Z
            if ((lineCastF != null || jumpFlag == true || fallFlag == true))// && positionCon.SetFlag == false)
                RayAngleIns();
        }
    }

    //�ړ��̃A�j���[�V��������
    private void OnCompleteAnimation()
    {
        moveAnimaFlag = true;
        skeletonAnimation.timeScale = 1;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry trackEntry = animationState.SetAnimation(0, moveAnimation, true);
        skeletonAnimation.skeleton.SetToSetupPose();
    }

    //���������C�̊p�x�v�Z�p
    public void RayAngleIns()
    {
        var downObject = GetDownObject();
        if (fallFlag == true)
        {
            //���C���o��
            Debug.DrawRay((Vector2)downRayPosition.position, Vector2.down * hRayRange, Color.red);
            
            //�n�ʂɐG�ꂽ���Ɋe��t���O�ƃA�j���[�V��������
            if (downObject && downObject.transform.gameObject.CompareTag("Ground") 
                || downObject && downObject.transform.gameObject.CompareTag("AreaGround")
                || downObject && downObject.transform.gameObject.CompareTag("Wall"))
            {
                jumpFlag = false;
                fallFlag = false;
                lindingFlag = true;
                LindingCoroutine();
            }
            //��Ɠ���(�������͕X�̏�Ȃ̂�iceWalkFlag�̂ݕύX)
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            {
                jumpFlag = false;
                fallFlag = false;
                iceWalkFlag = true;
                lindingFlag = true;
                LindingCoroutine();
            }
        }
        //�n�ʂ��痣�ꂽ���̏���
        else
        {
           Debug.DrawRay((Vector2)downRayPosition.position, Vector2.down * hRayRange, Color.blue);
           //�n�ʂ���󒆂ɂ�������(fallFlag == false�@����@true�@�ɂȂ鎞)
           if (!IsOnGrounds(downObject))
           {
                //�n�ʂ�����ł�LineCast�̐������ꂽ�Ƃ� = ������ԂƂ���
                //���̎��ɗ�����Ԃ𔻕ʂ��邽��fallFlag��true�ɂ���
                fallFlag = true;
                iceWalkFlag = false;
             
                if(jumpFlag == false)
                {
                     offJumpFlag = true;
                     //�A�j���[�V����
                    StartCoroutine(StartoffJump());
                }
           }
        }
        //���C�����ɂ��������Ă��Ȃ��Ƃ��͋������^�[��
        if (!downObject)
            return;
    }

    //���ɃI�u�W�F�N�g���������Ƃ�hit2D��Ԃ�
    private RaycastHit2D GetDownObject()
    {
        RaycastHit2D hit2D;
        
        hit2D = Physics2D.Linecast((Vector2)downRayPosition.position, (Vector2)downRayPosition.position + Vector2.down * hRayRange, LayerMask.GetMask("Ground"));
        return hit2D;
    }

    //�������Ƀq�b�g�������̏ꍇ����
    private bool IsOnGrounds(RaycastHit2D h)
    {
        if (!h)
         return false;

        if (h.transform.gameObject.CompareTag("AreaGround") || h.transform.gameObject.CompareTag("IceGround")
            || h.transform.gameObject.CompareTag("Ground")|| h.transform.gameObject.CompareTag("Wall"))
            return true;

        return false;
    }

    //���C���΂��R���[�`��
    private IEnumerator StartLineCast()
    {
        while (true)
          yield return null;
    }

    //��э~��̃A�j���[�V��������
    private IEnumerator StartoffJump()
    {
        //��э~��̃A�j���[�V���������A���̌㗎�����̃A�j���[�V�����𗬂�
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry fallTrackEntry = animationState.SetAnimation(0, offJumpAnimation, false);
        fallTrackEntry.Complete += FallSpineComplete;
        skeletonAnimation.skeleton.SetToSetupPose();

        //��U���C�𖳂���
        lineCastF = null;

        yield return new WaitForSeconds(0.01f);

        offJumpFlag = false;
        //���C�𕜊�
        lineCastF = StartCoroutine(StartLineCast());
        //�R���[�`���X�g�b�v
        StopCoroutine(StartoffJump());
    }

    //�������̃A�j���[�V�����𗬂�
    private void FallSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        animationState.SetAnimation(0, fallAnimation, false);
        skeletonAnimation.skeleton.SetToSetupPose();
    }

    //���n�̃A�j���[�V����
    private void LindingCoroutine()
    {
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry moveTrackEntry = animationState.SetAnimation(0, lindingAnimation, false);
        moveTrackEntry.Complete += MoveSpineComplete;
        skeletonAnimation.skeleton.SetToSetupPose();

        //���n�p�t���O�ύX
        lindingFlag = false;
    }

    //���s�A�j���[�V��������p
    private void MoveSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 1;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry trackEntryA = animationState.SetAnimation(0, moveAnimation, true);
        moveAnimaFlag = false;
    }

    //�������ɔ�΂�
    private RaycastHit2D CenterHitObj()
    {
        Debug.DrawRay((Vector2)centerRayPosition.position, Vector2.right * wRayRange, Color.green);
        centerHitObject = Physics2D.Linecast((Vector2)centerRayPosition.position, (Vector2)centerRayPosition.position + Vector2.right * wRayRange, LayerMask.GetMask("Ground"));
        return centerHitObject;
    }

    //�ǂ��W�����v����Ƃ��Ɏg���֐�
    private void SlopeUp()
    {
        //�������̃I�u�W�F�N�g���m
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

    //�^���W�F���g�v�Z
    private void return_tan(float ang)
    {
        //�^���W�F���g�v�Z
        if (CenterHitObj().normal.x > 0f)
            ang = Mathf.PI * 0.5f + Mathf.Atan(CenterHitObj().normal.y / Mathf.Abs(CenterHitObj().normal.x));

         var tan = Mathf.Tan(ang);

        //�^���W�F���g��n�x�ȏ�Ȃ�W�����v�ڍs����
        if(tan <= Mathf.PI / 3 )
        {
            //�W�����v���[�V����
            jumpFlag = true;
            skeletonAnimation.timeScale = 5;
            skeletonAnimation.state.ClearTrack(0);
            TrackEntry jumpTrackEntry = animationState.SetAnimation(0, jumpAnimation, false);
            jumpTrackEntry.Complete += JumpSpineComplete;
            skeletonAnimation.skeleton.SetToSetupPose();
        }
    }

    //�W�����v���̃A�j���[�V����
    private void JumpSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 5;
        skeletonAnimation.state.ClearTrack(0);
        animationState.SetAnimation(0, jumpDuringAnimation, true);
        skeletonAnimation.skeleton.SetToSetupPose();
        StartCoroutine(JumpStart());
    }

    //�W�����v�̏���
    private IEnumerator JumpStart()
    {
        //�񎟌��x�W�F�Ȑ��p�^�[��
        //�����̈ʒu
        var myP =transform.position;
        //����̈ʒu
        var x = Mathf.Abs(hitCollider.bounds.min.x + Mathf.Abs(myP.x));
        Vector3 toP =new Vector3(x,myP.y);
        //���Ԃ̈ʒu
        var miS = Mathf.Abs(hitCollider.bounds.max.y);
        Vector3 miP = new Vector3(hitCollider.bounds.min.x,miS + 3f);
        var flag = false;

        var sumTime = 0f;

        while (true)
        {
            sumTime += Time.deltaTime /3;
            //�ړ����邽�߂̈���
            var a = Vector3.Lerp(myP, miP, sumTime);
            var b = Vector3.Lerp(miP, toP, sumTime);

            if (jumpFlag == true && transform.position != toP)
            {
                // ��Ԉʒu�𔽉f
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

    //�W�����v���̊p�x����
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
    //��������ɓ���������
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

        //��Q���ɓ���������
        if (other.gameObject.CompareTag("Obstacle"))
        {
            audios.PlayOneShot(sound1);
            GameOverFlag = true;
        }

        //�n�`�M�~�b�N�ɐG�ꂽ��
        if (other.gameObject.CompareTag("PuddleFloor")
        ||other.gameObject.CompareTag("FlameFloor")
        ||other.gameObject.CompareTag("FlameGround")
        || other.gameObject.CompareTag("OutSide"))
             GameOverFlag = true;
    }

    //���ʉ��𗬂�����
    public void PlaySE(AudioClip clip)
    {
        if (audios != null)
            audios.PlayOneShot(clip);
    }
}
