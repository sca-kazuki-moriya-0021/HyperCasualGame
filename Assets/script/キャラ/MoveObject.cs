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

    #region//�v���C���[�֌W�ƃA�j���[�V����
    //x�����ɐi�ރX�s�[�h(��ʓI)
    [SerializeField,Header("���ʂ̏��Ői�ރX�s�[�h")]
    private float xMoveFloorSpeed = 10.0f;
    //x�����ɐi�ރX�s�[�h(�X)
    [SerializeField,Header("�X�̏��Ői�ރX�s�[�h")]
    private float xMoveIceSpeed = 15.0f;

    private GameObject child;

    //�I�u�W�F�N�g�ɐݒ肳��Ă���A�j���[�V����
    private SkeletonAnimation skeletonAnimation = default;
    //Spine�A�j���[�V��������N�p���邽�߂ɕK�v��State
    private Spine.AnimationState animationState = default;

    //�Đ�����A�j���[�V������
    [SerializeField,Header("���s�A�j���[�V����")]
    private string moveAnimation;
    [SerializeField, Header("�����A�j���[�V����")]
    private string fallAnimation;
    [SerializeField, Header("��э~��A�j���[�V����")]
    private string offJumpAnimation;
    [SerializeField,Header("���n���[�V����")]
    private string lindingAnimation;
    [SerializeField,Header("�W�����v���[�V����")]
    private string jumpAnimation;
    [SerializeField,Header("�W�����v�����[�V����")]
    private string jumpDuringA;

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
    //�W�����v��
    private bool jumpingFlag = false;

    #endregion

    #region//���C�֌W
    //�@���C���΂��ꏊ
    [SerializeField,Header("���C���΂��ʒu")]
    private Transform rayPosition;
    /*[SerializeField]
    private Vector2 upOffset;
    [SerializeField]
    private Vector2 downOffset;*/
    //�@���C���΂�����
    [SerializeField,Header("���ɔ�΂����C�̒���")]
    private float wRayRange;
    [SerializeField,Header("�c�ɔ�΂����C�̒���")]
    private float hRayRange;
    //�@������y���W
    private float fallenPosition;
    //�@�������Ă���n�ʂɗ�����܂ł̋���
    private float fallenDistance;
    //�q�b�g�����I�u�W�F�N�g�̊p�x
    private Quaternion hitObjectRotaion;
    //�q�b�g�����I�u�W�F�N�g�ۑ��p
    private RaycastHit2D headHitObject;
    private RaycastHit2D footHitObject;
    private RaycastHit2D forwardHitObject;
    #endregion

    //RigidBody�ƃJ�v�Z���R���C�_�[�̒�`
    private Rigidbody2D rb;
    private EdgeCollider2D col2D;

    //hit�����R���C�_�[���m�p
    private Collider2D hitCollider;
    private Collider2D hitBackCollider;

    //�X�v���N�g�p
    private TotalGM gm;
    private TimeGM timeGm;

    //�^���W�F���g
    private float tan;

    //�d�́@�g�����͂킩���
    [SerializeField,Header("�d�͗p�����ǎg���ĂȂ�")]
    private float _graviry = 9.80655f;
    private Vector3 _moveVelocity;

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

    private void Awake()
    {
        audios = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {
        child = transform.GetChild(0).gameObject;

        prevPos = this.transform.position;
        //�Ăяo��
        gm = FindObjectOfType<TotalGM>();
        timeGm = FindObjectOfType<TimeGM>();
        col2D = GetComponent<EdgeCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // �Q�[���I�u�W�F�N�g��SkeletonAnimation���擾
        skeletonAnimation =child.GetComponent<SkeletonAnimation>();
        // SkeletonAnimation����AnimationState���擾
        animationState =skeletonAnimation.AnimationState;

        //���C���΂�����
        lineCastF = StartCoroutine(StartLineCast());

        //���������Ɏg�����l���Z�b�g
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(fallFlag);
        //�Q�[���I�[�o�[�V�[���ɂ�������
        if (gameOverFlag == true)
        {
            gm.BackScene = gm.MyGetScene();
            gameOverFlag = false;
            SceneManager.LoadScene("GameOver");
        }

        //��p�x�v�Z�ƃW�����v����
        if (jumpFlag == false && fallFlag == false)
        {
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
                //Debug.Log("�����Ă���");
                //�ړ��̃A�j���[�V���������ƈړ�
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

            //���C�̊p�x�v�Z
            if (lineCastF != null || jumpFlag == true || fallFlag == true)
            {
                RayAngleIns();
            }
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
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * hRayRange, Color.red);
            
            //�n�ʂɐG�ꂽ���Ɋe��t���O�ƃA�j���[�V��������
            if (downObject && downObject.transform.gameObject.CompareTag("Ground"))
            {
                fallFlag = false;
                lindingFlag = true;
                Debug.Log("�R���[�`���O" + transform.position );
                LindingCoroutine();
            }
            else if(downObject && downObject.transform.gameObject.CompareTag("Wall"))
            {
                fallFlag = false;
                lindingFlag = true;
                LindingCoroutine();
            }
            //��Ɠ���(�������͕X�̏�Ȃ̂�iceWalkFlag�̂ݕύX)
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            { 
                fallFlag = false;
                iceWalkFlag = true;
                lindingFlag = true;
                LindingCoroutine();
            }
        }
        //�n�ʂ��痣�ꂽ���̏���
        else
        {
           Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * hRayRange, Color.blue);
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
        {
            return;
        } 
    }

    //���ɃI�u�W�F�N�g���������Ƃ�hit2D��Ԃ�
    private RaycastHit2D GetDownObject()
    {
        RaycastHit2D hit2D;
        
        hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * hRayRange, LayerMask.GetMask("Ground"));
        return hit2D;
       
    }

    //�������Ƀq�b�g�������̏ꍇ����
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

    //���C���΂��R���[�`��
    private IEnumerator StartLineCast()
    {
        while (true)
        {
            yield return null;
        }
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
        //���n�p�t���O�ύX
        lindingFlag = false;
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry moveTrackEntry = animationState.SetAnimation(0, lindingAnimation, false);
        moveTrackEntry.Complete += MoveSpineComplete;
        Debug.Log(transform.position);
        skeletonAnimation.skeleton.SetToSetupPose();

    }

    //���s�A�j���[�V��������p
    private void MoveSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 1;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry trackEntryA = animationState.SetAnimation(0, moveAnimation, true);
        moveAnimaFlag = false;
    }


    //����W�����v����Ƃ��Ɏg���֐�
    private float SlopeUp()
    {
      //if(fallFlag == false||)
      
        //�������̃I�u�W�F�N�g���m
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

    //�������ɔ�΂�
    private RaycastHit2D ForwardObject()
    {
        Debug.DrawRay((Vector2)rayPosition.position , Vector2.right * wRayRange, Color.green);
        forwardHitObject = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.right * wRayRange , LayerMask.GetMask("Ground"));
        //Debug.Log(forwardHitObject);
        return forwardHitObject;
    }
    
    //�^���W�F���g�v�Z
    private void return_tan()
    {
        //�^���W�F���g�v�Z
        if (ForwardObject().normal.x > 0f)
        {
            tan = Mathf.PI * 0.5f + Mathf.Atan(ForwardObject().normal.y / Mathf.Abs(ForwardObject().normal.x));
        }

        tan = Mathf.Tan(tan);
        //Debug.Log(tan);

        //�^���W�F���g��n�x�ȏ�Ȃ�W�����v�ڍs����
        if(tan <= Mathf.PI / 3 )
        {
            if(hitCollider.tag == "Wall")
            {
                //�W�����v���[�V����
                jumpFlag = true;
                skeletonAnimation.timeScale = 5;
                skeletonAnimation.state.ClearTrack(0);
                TrackEntry jumpTrackEntry = animationState.SetAnimation(0, jumpAnimation, false);
                jumpTrackEntry.Complete += JumpSpineComplete;
                skeletonAnimation.skeleton.SetToSetupPose();
                Debug.Log("�W�����v�ɂ͂�������");

            }
        }
    }

    private void JumpSpineComplete(TrackEntry trackEntry)
    {

        skeletonAnimation.timeScale = 5;
        skeletonAnimation.state.ClearTrack(0);
        animationState.SetAnimation(0, jumpDuringA, true);
        skeletonAnimation.skeleton.SetToSetupPose();
        Debug.Log("�W�����v������");
        
        StartCoroutine(JumpStart());
    }

    //�W�����v
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
        Vector3 miP = new Vector3(hitCollider.bounds.min.x,miS + 8f);
        Debug.Log(miP.x);
       

        //�p�x
        //var planeNormal = Vector3.up;

        //sleap�p�^�[���Ŏg������
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
            //�ړ����邽�߂̈���
            var a = Vector3.Lerp(myP, miP, sumTime);
            var b = Vector3.Lerp(miP, toP, sumTime);

            if (jumpFlag == false)
            {
                hitCollider = hitBackCollider;
            }


            if (jumpFlag == true && transform.position != toP)
            {
                // ��Ԉʒu�𔽉f
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



    //���̏ォ�牡�����Ƀ��C���΂�
    /*private RaycastHit2D HeadGetForwardObject()
    {
        Debug.DrawRay((Vector2)rayPosition.position + upOffset, Vector2.right * rayRange, Color.green);
        headHitObject = Physics2D.Linecast((Vector2)rayPosition.position + upOffset, (Vector2)rayPosition.position + Vector2.right * (rayRange * 0.5f), LayerMask.GetMask("Ground"));
        return headHitObject;
    }*/

    //�������牡�����Ƀ��C���΂�
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
    

    //��������ɓ���������
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

        //��Q���ɓ���������
        if (other.gameObject.CompareTag("Obstacle"))
        {
            audios.PlayOneShot(sound1);

            GameOverFlag = true;
        }

        if (other.gameObject.CompareTag("OutSide"))
        {
            GameOverFlag = true;
        }

        //�n�`�M�~�b�N�ɐG�ꂽ��
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
       // Debug.Log("���[�J�����W0" + localPoint0);
        var localPoint1 = (Vector2)transform.position + col2D.offset + capsuleDir * capsuleOffset;
        //Debug.Log("���[�J�����W1" + localPoint1);
        var point0 = (Vector2)transform.TransformPoint(localPoint0);
       // Debug.Log("���[���h���W:" + point0);
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

    //���ʉ��𗬂�����
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
