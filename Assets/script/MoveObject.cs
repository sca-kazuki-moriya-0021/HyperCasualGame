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
    #region//�v���C���[�֌W�ƃA�j���[�V����
    //x�����ɐi�ރX�s�[�h(��ʓI)
    [SerializeField,Header("���ʂ̏��Ői�ރX�s�[�h")]
    private float xMoveFloorSpeed = 6.0f;
    //x�����ɐi�ރX�s�[�h(�X)
    [SerializeField,Header("�X�̏��Ői�ރX�s�[�h")]
    private float xMoveIceSpeed = 7.0f;
    //�W�����v���Ɏg�����x
    private float moveSpeed = 2f;
    //�f�t�H���g�̊p�x
    //private Quaternion defeltRotation;

    //�Đ�����A�j���[�V������
    [SerializeField,Header("���s�A�j���[�V����")]
    private string moveAnimation;
    [SerializeField, Header("�����A�j���[�V����")]
    private string fallAnimation;
    [SerializeField, Header("��э~��A�j���[�V����")]
    private string offJumpAnimation;
    [SerializeField,Header("���n���[�V����")]
    private string lindingAnimation;

    //�I�u�W�F�N�g�ɐݒ肳��Ă���A�j���[�V����
    private SkeletonAnimation skeletonAnimation = default;
    //Spine�A�j���[�V��������N�p���邽�߂ɕK�v��State
    private Spine.AnimationState animationState = default;
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
    [SerializeField,Header("���C�̒���")]
    private float rayRange;
    //�@������y���W
    private float fallenPosition;
    //�@�������Ă���n�ʂɗ�����܂ł̋���
    private float fallenDistance;
    //�@�ǂ̂��炢�̍�������_���[�W��^���邩
    //[SerializeField]
    //private float takeDamageDistance = 3f;

    //�q�b�g�����I�u�W�F�N�g�̊p�x
    private Quaternion hitObjectRotaion;
    //�q�b�g�����I�u�W�F�N�g�ۑ��p
    private RaycastHit2D headHitObject;
    private RaycastHit2D footHitObject;
    private RaycastHit2D forwardHitObject;

    //hit�����R���C�_�[����
    private float xLen;
    private float yLen;
    private Vector2 xVLen;
    private Vector2 yVLen;
    //�L��������hit�����I�u�W�F�N�g�̋���
    private float pDis;
    private Vector2 pVDis;
    #endregion

    //RigidBody�ƃJ�v�Z���R���C�_�[�̒�`
    private Rigidbody2D rb;
    private CapsuleCollider2D col2D;

    //readonly Collider[] _result = new Collider[5];

    //hit�����R���C�_�[���m�p
    private Collider2D hitCollider;
    private Collider2D hitBackCollider;

    //�X�v���N�g�p
    private TotalGM gm;

    //��������
    //private bool dirSwitchFlag = false;

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
        //�Ăяo��
        gm = FindObjectOfType<TotalGM>();
        col2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // �Q�[���I�u�W�F�N�g��SkeletonAnimation���擾
        skeletonAnimation = GetComponent<SkeletonAnimation>();
        // SkeletonAnimation����AnimationState���擾
        animationState = skeletonAnimation.AnimationState;

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
        //�Q�[���I�[�o�[�V�[���ɂ�������
        if (gameOverFlag == true)
        {
            gm.BackScene = gm.MyGetScene();
            gameOverFlag = false;
            SceneManager.LoadScene("GameOver");
        }
  
    }

    private void FixedUpdate()
    {

        //�ړ�
        if (jumpFlag == false && offJumpFlag == false && lindingFlag == false)
        {
            Debug.Log("�����Ă���");
            //�ړ��̃A�j���[�V���������ƈړ�
            if (iceWalkFlag == false)
            {
                transform.Translate(-xMoveFloorSpeed * Time.deltaTime * 0.2f, 0, 0);
                if(moveAnimaFlag == false)
                {
                    OnCompleteAnimation();
                }

            }
            if (iceWalkFlag == true)
            {
                transform.Translate(-xMoveIceSpeed * Time.deltaTime * 0.2f, 0, 0);
                if (moveAnimaFlag == false)
                {
                    OnCompleteAnimation();
                }
            }
        }

        //��p�x�v�Z�ƃW�����v����
        if (jumpFlag == false)
        {
            SlopeUp();
        }

        //���C�̊p�x�v�Z
        if(lineCastF != null)
        {
            RayAngleIns();;
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
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.red);
            
            //�n�ʂɐG�ꂽ���Ɋe��t���O�ƃA�j���[�V��������
            if (downObject && downObject.transform.gameObject.CompareTag("Ground"))
            {
                fallFlag = false;
                lindingFlag = true;
                StartCoroutine(LindingCoroutine());
            }
            //��Ɠ���(�������͕X�̏�Ȃ̂�iceWalkFlag�̂ݕύX)
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            { 
                fallFlag = false;
                iceWalkFlag = true;
                lindingFlag = true;
                StartCoroutine(LindingCoroutine());
            }
        }
        //�n�ʂ��痣�ꂽ���̏���
        else
        {
           Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.blue);
           //�n�ʂ���󒆂ɂ�������(fallFlag == false�@����@true�@�ɂȂ鎞)
           if (!IsOnGrounds(downObject))
           {
             //�n�ʂ�����ł�LineCast�̐������ꂽ�Ƃ� = ������ԂƂ���
             //���̎��ɗ�����Ԃ𔻕ʂ��邽��fallFlag��true�ɂ���
             fallFlag = true;
             iceWalkFlag = false;
             offJumpFlag = true;
             //�A�j���[�V����
             StartCoroutine(StartoffJump());
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
        
        hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange, LayerMask.GetMask("Ground"));
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
        if (h.transform.gameObject.CompareTag("IceGround"))
        {
            return true;
        }
        return false;
    }

    //���s�A�j���[�V��������p
    private void MoveSpineComplete(TrackEntry trackEntry)
    {
        skeletonAnimation.timeScale = 1;
        TrackEntry trackEntryA  = animationState.SetAnimation(0, moveAnimation, true);
        moveAnimaFlag = false;
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
    private IEnumerator LindingCoroutine()
    {
        skeletonAnimation.timeScale = 2;
        skeletonAnimation.state.ClearTrack(0);
        TrackEntry moveTrackEntry = animationState.SetAnimation(0, lindingAnimation, false);
        moveTrackEntry.Complete += MoveSpineComplete;
        skeletonAnimation.skeleton.SetToSetupPose();

        yield return new WaitForSeconds(0.5f);

        //���n�p�t���O�ύX
        lindingFlag = false;

        //�R���[�`���X�g�b�v
        StopCoroutine(LindingCoroutine());

    }

    //����W�����v����Ƃ��Ɏg���֐�
    private float SlopeUp()
    {
      //if(fallFlag == false||)
      
        //�������̃I�u�W�F�N�g���m
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

        return tan;
    }

    //�������ɔ�΂�
    private RaycastHit2D ForwardObject()
    {
        Debug.DrawRay((Vector2)rayPosition.position , Vector2.right * rayRange, Color.green);
        forwardHitObject = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.right * rayRange , LayerMask.GetMask("Ground"));
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
            Debug.Log("�^�O�ύX");
            hitBackCollider = hitCollider;
            hitCollider.gameObject.tag = "Wall";
            if(hitCollider.tag == "Wall")
            {
                jumpFlag = true;
                StartCoroutine(JumpStart());
            }
        }
    }

    //�W�����v
    private IEnumerator JumpStart()
    {
        var d = transform.position;
        Debug.Log(d);
        var t = new Vector2(hitCollider.bounds.max.x,hitCollider.bounds.max.y +3.0f);
        Debug.Log(t);
        var sumTime = 0f;
        while (true)
        {
            sumTime += Time.deltaTime;
            var ratio = sumTime / 3;
            if (ratio < 1.0f)
            {
                transform.position = Vector3.Lerp(d, t, ratio);
            }

            if (ratio > 1.0f)
            {
                // �ڕW�̒l�ɓ��B�����炱��Coroutine���I������
                // ~.Lerp�͊���������������0 ~ 1�̊Ԃ�Clamp�����̂�1���傫���Ă����Ȃ�
                break;
            }
            yield return null;
        }

        jumpFlag = false;
        hitCollider.gameObject.tag = hitBackCollider.gameObject.tag;

        StopCoroutine(JumpStart());
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
        }

        if (other.gameObject.CompareTag("IceGround"))
        {
            iceWalkFlag = true;
        }

        if (other.gameObject.CompareTag("IceGround") && other.gameObject.CompareTag("Ground"))
        {
            iceWalkFlag = true;
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
