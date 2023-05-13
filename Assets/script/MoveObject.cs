using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class MoveObject : MonoBehaviour
{
    #region//�v���C���[�֌W
    //x�����ɐi�ރX�s�[�h(��ʓI)
    private float xMoveFloorSpeed = 3.0f;
    //x�����ɐi�ރX�s�[�h(�X)
    private float xMoveIceSpeed = 5.0f;
    #endregion

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

    //RigidBody�ƃ{�b�N�X�R���C�_�[�̒�`
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    //�X�v���N�g�p
    private TotalGM gm;

    //�q�b�g�����I�u�W�F�N�g�̊p�x
    private Quaternion hitObjectRotaion;

    //�ǂ����̕����ɐ����Ђ�����
    private bool rightLine = true;

    //�^���W�F���g
    private float tan;

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
        gm = FindObjectOfType<TotalGM>();
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        //this.anime = GetComponent<Animator>();;

        //���������Ɏg�����l���Z�b�g
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


        //���C�̊p�x�v�Z
        RayAngleIns();

        //debug
        /*RaycastHit2D hit2D = Physics2D.Linecast((Vector2)rayPosition.position, (Vector2)rayPosition.position + Vector2.down * rayRange, LayerMask.GetMask("Ground"));
        Debug.DrawRay((Vector2)rayPosition.position, Vector2.down * rayRange, Color.red);
        if (hit2D.transform.gameObject.CompareTag("IceGround") || hit2D.transform.gameObject.CompareTag("Ground"))
        {
            Debug.Log("OK");
        }*/


        //���R����
        /*moveVelocity.y += -_graviry *Time.fixedDeltaTime;

        var p = transform.position;

        p+= _moveVelocity *Time.fixedDeltaTime;
        transform.position =p;*/

        //�����鏈��

        /*tan = 0f;
        RaycastHit2D hit2D = Physics2D.Raycast((Vector2)rayPosition.position, Vector2.up *rayRange);
        if(fallFlag == false && hit2D.transform.gameObject.CompareTag("Ground"))
        {
            
        }*/
        //�ړ�
        //true�Ȃ�E
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
        else//false�Ȃ獶
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

        //��Q���ɓ���������
        if (other.gameObject.CompareTag("Obstacle") || other.gameObject.CompareTag("OutSide"))
        {
            GameOverFlag = true;
            //�V�[���Ăяo��

        }

        //��&&�����܂�&&�؍ނɓ�����
        /*if (other.gameObject.CompareTag("FlameFloor")||
            other.gameObject.CompareTag("PuddleFloor")||
            other.gameObject.CompareTag("Obstacle")     )
        {
            
        }*/
    }

    //���C�̊p�x�v�Z�p
    public void RayAngleIns()
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
        
        var downObject = GetDownObject();

        if (fallFlag == true)
        {
            //���C���o��
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
