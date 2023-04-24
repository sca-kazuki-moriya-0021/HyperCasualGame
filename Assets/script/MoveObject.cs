using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;


public class MoveObject : MonoBehaviour
{
    #region//�v���C���[�֌W
    //�v���C���[��Hp��ۊǂ���ϐ�
    private int oldHp;
    //x�����ɐi�ރX�s�[�h(��ʓI)
    private float xMoveFloorSpeed = 3.0f;
    //x�����ɐi�ރX�s�[�h(�X)
    private float xMoveIceSpeed = 5.0f;
    #endregion

    //�v���C���[�A�j���[�V�����p�ϐ�
    [SerializeField]
    private Animator anime = null;

    //�X�̏�ɏ���Ă��邩�ǂ���
    private bool IceWalkFlag = false;


    #region//���C�֌W
    //�@���C���΂��ꏊ
    [SerializeField]
    private Transform rayPosition;
    //�@���C���΂�����
    [SerializeField]
    private float rayRange;
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

    #region//���ʉ��֌W
    private AudioSource audios = null;
    [SerializeField]
    private AudioClip runSE;//�ړ��p
    [SerializeField]
    private AudioClip iceRunSE;//�X�ړ��p
    [SerializeField]
    private AudioClip itemGetSE;//�A�C�e���Ƃ�����
    [SerializeField]
    private AudioClip gameOverSE;//�Q�[���I�[�o�[�ɂȂ�����
    //���ʉ����Ȃ�����
    private bool soundFlag = true;
    #endregion

    //�R���[�`���߂�l�p
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

        //hp������
        oldHp = gm.PlayerHp;

        //���������Ɏg�����l���Z�b�g
        fallenDistance = 0f;
        fallenPosition = transform.position.y;
        fallFlag = false;

        //ray���ˊJ�n
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
