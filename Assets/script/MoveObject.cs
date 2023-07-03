using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;

public class MoveObject : MonoBehaviour
{
    #region//�v���C���[�֌W
    //x�����ɐi�ރX�s�[�h(��ʓI)
    private float xMoveFloorSpeed = 6.0f;
    //x�����ɐi�ރX�s�[�h(�X)
    private float xMoveIceSpeed = 7.0f;
    //�f�t�H���g�̊p�x
    private Quaternion defeltRotation;
    #endregion

    private float time = 400f;
    private float countTime;

    //�v���C���[�A�j���[�V�����p�ϐ�
    //[SerializeField]
    //private Animator anime = null;

    //�X�̏�ɏ���Ă��邩�ǂ���
    private bool iceWalkFlag = false;

    #region//���C�֌W
    //�@���C���΂��ꏊ
    [SerializeField]
    private Transform rayPosition;
    //�@���C���΂�����
    [SerializeField]
    private float rayRange;
    //���C���΂��p�x�v�Z
    /*[SerializeField]
    private float _Velocity_0;
    [SerializeField]
    private float degree;
    [SerializeField]
    private float angle_Split;
    //�e�v�Z�p�ϐ�
    float _theta;
    float PI = Mathf.PI;
    //���C���΂��p�x�ۑ��p
    Vector2 rayVector2;*/
    //�@������y���W
    private float fallenPosition;
    //�@�������Ă���n�ʂɗ�����܂ł̋���
    private float fallenDistance;
    //�@�ǂ̂��炢�̍�������_���[�W��^���邩
    //[SerializeField]
    //private float takeDamageDistance = 3f;
    #endregion

    #region//�󋵂ɉ����Ďg�p����t���O�֌W
    //�ړ�
    private bool moveFlag = false;
    //������
    private bool fallFlag = false;
    //���n��
    private bool landFlag = false;
    //�Q�[���I�[�o�[
    private bool gameOverFlag = false;
    //�N���A
    private bool clearFlag = false;
    #endregion

    //RigidBody�ƃJ�v�Z���R���C�_�[�̒�`
    private Rigidbody2D rb;
    private CapsuleCollider2D col2D;

    //
    readonly Collider[] _result = new Collider[5];

    //�X�v���N�g�p
    private TotalGM gm;

    //�q�b�g�����I�u�W�F�N�g�̊p�x
    private Quaternion hitObjectRotaion;

    //��������
    private bool dirSwitchFlag = false;
    //�ǂ����̕����ɐ����Ђ�����
    private bool rightLine = true;

    //�^���W�F���g
    private float tan;
    private RaycastHit2D upHit2D;

    //�d�́@�g�����͂킩���
    [SerializeField]
    private float _graviry = 9.80655f;
    private Vector3 _moveVelocity;

    #region//���ʉ��֌W
    private AudioSource audios = null;
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
        Time.timeScale = 0.1f;
        gm = FindObjectOfType<TotalGM>();
        col2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        //this.anime = GetComponent<Animator>();;

        //���������Ɏg�����l���Z�b�g
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;

        defeltRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOverFlag == true)
        {
            gm.BackScene = gm.MyGetScene();
            gameOverFlag = false;
            SceneManager.LoadScene("GameOver");
        }
        //�ړ�
        //true�Ȃ�E
        if (rightLine == true)
        {
            if (iceWalkFlag == false)
            {
                transform.Translate(xMoveFloorSpeed * Time.deltaTime, 0, 0);
            }
            if (iceWalkFlag == true)
            {
                transform.Translate(xMoveIceSpeed * Time.deltaTime, 0, 0);
            }
        }
        if (rightLine == false)//false�Ȃ獶
        {
            if (iceWalkFlag == false)
            {
                transform.Translate(-xMoveFloorSpeed * Time.deltaTime, 0, 0);
            }
            if (iceWalkFlag == true)
            {
                transform.Translate(-xMoveIceSpeed * Time.deltaTime, 0, 0);
            }
        }

    }

    private void FixedUpdate()
    {
        //���C�̊p�x�v�Z
        RayAngleIns();

        //�����鏈��
        SlopeUp();

        //�R���C�_�[���߂荞�񂾎�
        //CollderMerging();

        //���R����
        { 
            /*moveVelocity.y += -_graviry *Time.fixedDeltaTime;

            var p = transform.position;

            p+= _moveVelocity *Time.fixedDeltaTime;
            transform.position =p;*/
        }

        //�ǂ���ɂ������̔���
        {
            if (dirSwitchFlag == true)
            {
                Debug.Log("aik");
                countTime++;
                if (countTime >= time && rightLine == true)
                {
                    transform.Rotate(0,180f,0);
                    rightLine = false;
                    countTime = 0;
                }
                if (countTime >= time && rightLine == false)
                {
                    transform.Rotate(0, 0f, 0);
                    rightLine = true;
                    countTime = 0;
                }
                dirSwitchFlag = false;
            }
        }

        //�R���C�_�[���m���߂肱�񂾂Ƃ�
        //Physics2D.OverlapPointAll()
        

    }

    //���������C�̊p�x�v�Z�p
    public void RayAngleIns()
    {
        { 
        /*for (int i = 0; i <= (angle_Split - 1); i++)
            {
                //���C�̒[����[�܂ł̊p�x
                float AngleRange = PI * (degree / 180);

                //���C�ɓn���p�x�̌v�Z
                if (angle_Split > 1) _theta = (AngleRange / (angle_Split - 1)) * i + 0.5f * (PI - AngleRange);
                else _theta = 0.5f * PI;

                //�擾�����p�x��ۑ�
                rayVector2.x = _Velocity_0 * Mathf.Cos(_theta);
                rayVector2.y = _Velocity_0 * Mathf.Sin(_theta);
            }*/
        }
        
        var downObject = GetDownObject();

        if (fallFlag == true)
        {
            //���C���o��
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.red);
            if (downObject && downObject.transform.gameObject.CompareTag("Ground"))
            {
                { 
                /*hitObjectRotaion = hit2D.transform.rotation;
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    transform.rotation = hitObjectRotaion;
                    objectRotaion = hitObjectRotaion;*/
                }
                //Debug.Log("ki");
                fallFlag = false;
            }
            else if (downObject && downObject.transform.gameObject.CompareTag("IceGround"))
            {
                { 
                    /*hitObjectRotaion = hit2D.transform.rotation;
                    transform.rotation = new Quaternion(0, 0, 0, 0);
                    transform.rotation = hitObjectRotaion;
                    objectRotaion = hitObjectRotaion;*/
                }
                Debug.Log("ari");
                fallFlag = false;
                iceWalkFlag = true;
            }
        }
        else
        {
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.blue);

            //�n�ʂ���󒆂ɂ�������(fallFlag == false�@����@true�@�ɂȂ鎞)
            if (!IsOnGrounds(downObject))
            {
                //�n�ʂ�����ł�LineCast�̐������ꂽ�Ƃ� = ������ԂƂ���
                //���̎��ɗ�����Ԃ𔻕ʂ��邽��fallFlag��true�ɂ���
                //�ŏ��̗����n�_��ݒ�
                fallenPosition = transform.position.y;
                fallenDistance = 0;
                //hitObjectRotaion = default;
                //�t���O�𗧂Ă�
                fallFlag = true;
                iceWalkFlag = false;
                Debug.Log("�n�ʂ��痣�ꂽ��");
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
        RaycastHit2D hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange, LayerMask.GetMask("Ground"));

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

    //������Ƃ��Ɏg���֐�
    private float SlopeUp()
    {
      if(fallFlag == false)
      {
            tan = 0f;
            var forwardObject = GetForwardObject();

            if (forwardObject && forwardObject.transform.gameObject.CompareTag("Ground"))
            {
                return_tan();
            }
            else if (forwardObject && forwardObject.transform.gameObject.CompareTag("IceGround"))
            {
                return_tan();
            }
            if (forwardObject.normal.x == 1f)
            {
                Debug.Log("�i�s�����ύX�t���O�ς������");
                tan = 0f;
                dirSwitchFlag = true;
            }
      }
        return tan;
    }
    //�O�����Ƀ��C���΂�
    private RaycastHit2D GetForwardObject()
    {
        var direciton = transform.rotation;
        if(direciton != defeltRotation)
        {
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.left * rayRange, Color.green);
            upHit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.left * rayRange, LayerMask.GetMask("Ground"));
           
        }
        if(direciton == defeltRotation)
        {
            Debug.DrawRay((Vector2)rayPosition.position, Vector2.right * rayRange, Color.green);
            upHit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.right * rayRange, LayerMask.GetMask("Ground"));
         
        }
        return upHit2D;
    }

    //�^���W�F���g�v�Z����������
    private void return_tan()
    {
        //�^���W�F���g�v�Z
        if (GetForwardObject().normal.x > 0f)
        {
            tan = Mathf.PI * 0.5f + Mathf.Atan(GetForwardObject().normal.y / Mathf.Abs(GetForwardObject().normal.x));
        }

        tan = Mathf.Tan(tan);
        Debug.Log(tan);

        //�^���W�F���g��n�x�ȏ�Ȃ�i�s������ς���
        if(tan == 0 || tan >= Mathf.PI /9)
        {
            Debug.Log("�i�s�����ύX�t���O�ς������");
            dirSwitchFlag = true;
        }
    }

    //�R���C�_�[���߂荞�ޔ���
   /*private Vector2 CollderMerging()
    {
        

    }*/

    //��������ɓ���������
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {

           
        }

        if (other.gameObject.CompareTag("IceGround"))
        {
            iceWalkFlag = true;
        }

        if (other.gameObject.CompareTag("IceGround") && other.gameObject.CompareTag("Ground"))
        {
            iceWalkFlag = true;
        }

        if (other.gameObject.CompareTag("GoalPoint"))
        {
            gm.BackScene = gm.MyGetScene();
            clearFlag = true;
            SceneManager.LoadScene("Goal");
            
        }

        //��Q���ɓ���������
        if (other.gameObject.CompareTag("Obstacle")
            || other.gameObject.CompareTag("PuddleFloor")
            || other.gameObject.CompareTag("FlameFloor")
            || other.gameObject.CompareTag("FlameGround"))
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
             Vector3 a =  collision.collider.ClosestPoint(transform.position);
             Vector3 b = (a - transform.position);
             Vector3 c = b.normalized;
             float returnDistance = col2D.size.y * transform.localScale.y / 2 - b.magnitude;
            Debug.Log(a);
            Debug.Log("sizey" + col2D.size.y *transform.localScale.y);
            Debug.Log(b.magnitude);
            Debug.Log(returnDistance);

            transform.position = (transform.position - returnDistance * c);
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
