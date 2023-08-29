using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Video;
using System.Linq;
using Spine.Unity;
using UnityEngine.EventSystems;

public class LineDrawCon : MonoBehaviour
{
    private Camera mainCamera;
    RaycastHit[] checkTouchResults = new RaycastHit[1];
    //�J��������^�b�`������΂��ő�̋���
    float checkTouchMaxDistance = 100f;
    //�I��Ώۂ̃��C���[
    LayerMask targetLayer;
    //�^�b�`�ʒu
    Vector3 touchPoint;
    //�J��������^�b�`�ʒu�ւ�Ray
    Ray checkTouchRay;
    //�^�b�`�ʒu��UI�v�f���Ԃ����List
    List<RaycastResult> isPointerOverUIResults = new List<RaycastResult>(10);
    

    //���̑���
    [Range(0.1f, 0.5f)]
    [SerializeField]
    private float lineWidth;

    //���ƂȂ�Q�[���I�u�W�F�N�g�p�ϐ�
    private GameObject lineObj;
    //lineObj��lineRenderer�p�ϐ�
    private LineRenderer lineRenderer;
    //�R���C�_�[�̂��߂̍��W��ێ����郊�X�g�^�̕ϐ�
    private List<Vector2> linePoints;

    //�F
    [SerializeField]
    private Material iceTexture;
    [SerializeField]
    private Material fierTexture;
    [SerializeField]
    private Material ironTexture;

    //�F��ۑ����Ă����p�ϐ�
    private Material lineMaterial;
    private Material _myMat;

    //PhysicsMaterial2D��Color���܂Ƃ߂Ă����p
    protected PhysicsMaterial2D sMaterial;
    protected Color lineColor;
    [SerializeField]
    private PhysicsMaterial2D iceMaterial;
    [SerializeField]
    private PhysicsMaterial2D generalMaterial;
    [SerializeField]
    private Color iceColor;
    [SerializeField]
    private Color generalColor;

    //���������Ă��鎞
    private bool lineFlag = false;

    //[SerializeField]
    //private GameObject nullObject;

    //�X�̃A�j���[�V����
    [SerializeField]
    private GameObject instansIcePrefab;
    private SkeletonAnimation iceSkelton;
    private string ice;


    //���̃A�j���[�V����
    [SerializeField]
    private GameObject instansfirePrefab;
    private SkeletonAnimation fireSkelton;
    private string fire;


    //�A�j���[�V������K�p���邽�߂ɕK�v��AnimationState
    private Spine.AnimationState iceAnimationState = default;
    private Spine.AnimationState fireAnimationState = default;

    //private SkeletonAnimation nowSkeletonAnima;

    //�X�N���v�g�ǂݎ��p
    private PasueDisplayC pasueDisplayC;
    private PenDisplay penDisplayC;

    //�y���̃X�v���N�g
    private PenM penM;

    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;
    private bool sEffectFlag = false;

    public bool LineFlag
    {
        get { return this.lineFlag; }
        set { this.lineFlag = value; }
    }

    private void Awake()
    {
        mainCamera = Camera.main;
        //�Ώۂ̃��C���[���w��B������ݒ肷��ꍇ�́u1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ground")�v�݂����Ɂu|�v�ŋ�؂�B
        targetLayer = LayerMask.NameToLayer("UI");
        //eventSystem = EventSystem.current;
    }


    // Start is called before the first frame update
    void Start()
    {
        //������
        penM = FindObjectOfType<PenM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        penDisplayC = FindObjectOfType<PenDisplay>();

        iceSkelton = instansIcePrefab.GetComponent<SkeletonAnimation>();
        fireSkelton = instansfirePrefab.GetComponent<SkeletonAnimation>();
        audioSource = GetComponent<AudioSource>();


        //iceAnimationState = iceSkelton.AnimationState
        //fireAnimationState = fireSkelton.AnimationState;

        //�_�u���^�b�`����
        Input.multiTouchEnabled = false;

        //�A�j���[�V����������
        iceSkelton.AnimationName = "None";
        fireSkelton.AnimationName = "None";

        //���̃|�C���g������
        linePoints = new List<Vector2>();

    }

    // Update is called once per frame
    void Update()
    {

        //�y���̎�ނɂ���Đ؂�ւ���v���O����
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                sMaterial = iceMaterial;
                lineMaterial = iceTexture;
                lineColor = iceColor;
                //nowSkeletonAnima = iceSkelton;
                iceSkelton.AnimationName = "animetion";

                //Debug.Log(lineColor);
                break;

            case PenM.PenCom.Fire:
                //nowSkeletonAnima = fireSkelton;
                fireSkelton.AnimationName = "animetion";
                break;

            case PenM.PenCom.General:
                sMaterial = generalMaterial;
                lineMaterial = ironTexture;
                lineColor = generalColor;
                //nowSkeletonAnima = null;
                break;
        }

        //���������鎞
        if(pasueDisplayC.MenuFlag == false && penDisplayC.PenMenuFlag == false )
        {
                //���N���b�N���ꂽ��
                if (Input.GetMouseButtonDown(0))
                {
                    touchPoint = Input.mousePosition;
                    checkTouchRay = mainCamera.ScreenPointToRay(touchPoint);
                    
                    //if(checkTouchRay)
                    {

                        switch (penM.NowPen)
                        {
                            case PenM.PenCom.Ice:
                            if (penM.IceDrawFlag == true)
                            {
                                lineFlag = true;

                                Debug.Log(lineFlag);
                                _addLineObject();
                            }
                            break;

                            case PenM.PenCom.Fire:
                            if (penM.FireDrawFlag == true)
                            {
                                lineFlag = true;
                                _addLineObject();
                            }
                            break;

                            case PenM.PenCom.General:
                            if (penM.GeneralDrawFlag == true)
                            {
                                lineFlag = true;
                                _addLineObject();
                            }
                            break;
                        }
                    }
                }
                //�N���b�N���i�X�g���[�N���j
                if (Input.GetMouseButton(0) && lineFlag == true)
                {
                    if (sEffectFlag == false)
                    {
                        audioSource.PlayOneShot(sound1);
                        sEffectFlag = true;
                    }
                    _addPositionDataToLineRenderer();
                }

                //�N���b�N�����ɂȂ�����
                if (Input.GetMouseButtonUp(0))
                {
                        lineFlag = false;
                        sEffectFlag = false;
                        linePoints = new List<Vector2>();
                }
        }
    }

    //�N���b�N�����甭��
    void _addLineObject()
    {
        //�Q�[���I�u�W�F�N�g�쐬
        lineObj = new GameObject();
        //���O��Line�ɂ���
        lineObj.name = "Line";
        //lineObj��LineRenderer�R���|�[�l���g��ǉ�
        lineObj.AddComponent<LineRenderer>();
        //lineObj��EdgeCollider2D�R���|�[�l���g��ǉ�
        EdgeCollider2D c = lineObj.AddComponent<EdgeCollider2D>();
        //�}�e���A���̃R���C�_�[�̒ǉ�
        c.sharedMaterial = sMaterial;
        //Debug.Log(lineObj.AddComponent<EdgeCollider2D>().sharedMaterial = lineDrawCon.SMaterial);
        //lineObj�����g�iStroke�j�̎q�v�f�ɐݒ�
        lineObj.transform.SetParent(transform);
        //���C���[��Ground�ɂ���
        lineObj.layer = 6;

        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:

                lineObj.tag = "IceGround";

                break;

            case PenM.PenCom.Fire:

                lineObj.tag = "FlameGround";

                break;

            case PenM.PenCom.General:

                lineObj.tag = "Ground";

                break;
        }

        _initRenderer();
    }


    //lineObj����������
    void _initRenderer()
    {
        //LineRenderer���擾
        lineRenderer = lineObj.GetComponent<LineRenderer>();
        _myMat = new Material(lineMaterial);
        lineRenderer.material = _myMat;

        
        //�}�e���A���̐F��ݒ�
        //lineRenderer.material.color = lineDrawCon.LineColor;
        //Debug.Log(lineRenderer.material.color);
        lineMaterial.SetColor("_Color",lineColor);
        lineRenderer.textureMode = LineTextureMode.Tile;


        //�|�W�V�����J�E���g�����Z�b�g
        lineRenderer.positionCount = 0;

        //�n�_�̑�����ݒ�
        lineRenderer.startWidth = lineWidth;
        //�I�_�̑�����ݒ�
        lineRenderer.endWidth = lineWidth;
        //���C���[�w��
        lineRenderer.renderingLayerMask = 2;

        //�y���̎�ނɂ���Đ؂�ւ���v���O����
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:

                lineRenderer.tag = "IceGround";

                break;

            case PenM.PenCom.Fire:

                lineRenderer.tag = "FlameGround";

                break;

            case PenM.PenCom.General:

                lineRenderer.tag = "Ground";

                break;
        }

    }


    void _addPositionDataToLineRenderer()
    {
        /*���W�Ɋւ��鏈��*/
        //�}�E�X���W�擾
        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1.0f);
        //���[���h���W�֕ϊ�
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        /*LineRenderer�Ɋւ��鏈��*/
        //LineRenderer�̃|�C���g�𑝂₷
        lineRenderer.positionCount += 1;
        //LineRenderer�̃|�W�V������ݒ�
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, worldPos);

        /*EdgeCollider2D�Ɋւ��鏈��*/
        //���[���h���W�����X�g�ɒǉ�
        linePoints.Add(worldPos);
        //EdgeCollider2D�̃|�C���g��ݒ�
        lineObj.GetComponent<EdgeCollider2D>().SetPoints(linePoints);


    }

}
