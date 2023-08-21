using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Video;
using System.Linq;
using Spine.Unity;

public class LineDrawCon : MonoBehaviour
{
    //�y���ŕ`���Ă��钷��
    private float drawTime;
    private float iceDrawTime;
    private float fireDrawTime;

    //�F
    [SerializeField]
    private Material iceTexture;
    [SerializeField]
    private Material fierTexture;
    [SerializeField]
    private Material ironTexture;

    private Material lineMaterial;
    private Material _myMat;

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


    [SerializeField]
    private GameObject nullObject;

    [SerializeField]
    private GameObject instansIcePrefab;
    private SkeletonAnimation iceSkelton;
    private string ice;


    [SerializeField]
    private GameObject instansfirePrefab;
    private SkeletonAnimation fireSkelton;
    private string fire;


    //�A�j���[�V������K�p���邽�߂ɕK�v��AnimationState
    private Spine.AnimationState iceAnimationState = default;
    private Spine.AnimationState fireAnimationState = default;

    //private SkeletonAnimation nowSkeletonAnima;

    private PasueDisplayC pasueDisplayC;
    private PenDisplay penDisplayC;

    private bool iceflag = false;
    private bool fireflag = false;

    //�y���̃X�v���N�g
    private PenM penM;

    //���ʉ��p
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip sound1;

    private bool sEffectFlag = false;

    /*public Color LineColor
    {
        get { return this.lineColor; }
        set { this.lineColor = value; }
    }

    public PhysicsMaterial2D SMaterial
    {
        get { return this.sMaterial; }
        set { this.sMaterial = value; }
    }

    public SkeletonAnimation NowSkeletonAnima
    {
        get { return this.nowSkeletonAnima; }
        set { this.nowSkeletonAnima = value; }
    }

    /*public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public Sprite NowSprite
    {
        get { return this.nowSprite; }
        set { this.nowSprite = value; }
    }


    public bool IceFlag
    {
        get { return this.iceflag; }
        set { this.iceflag = value; }
    }*/


    // Start is called before the first frame update
    void Start()
    {
        penM = FindObjectOfType<PenM>();
        pasueDisplayC = FindObjectOfType<PasueDisplayC>();
        penDisplayC = FindObjectOfType<PenDisplay>();

        iceSkelton = instansIcePrefab.GetComponent<SkeletonAnimation>();
        fireSkelton = instansfirePrefab.GetComponent<SkeletonAnimation>();
        audioSource = GetComponent<AudioSource>();


        //iceAnimationState = iceSkelton.AnimationState
        //fireAnimationState = fireSkelton.AnimationState;


        iceSkelton.AnimationName = "None";
        fireSkelton.AnimationName = "None";


        sMaterial = generalMaterial;

        linePoints = new List<Vector2>();

        penM.NowPen = PenM.PenCom.General;
        lineColor = generalColor;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(sMaterial);
        switch (penM.NowPen)
        {
            case PenM.PenCom.Ice:
                sMaterial = iceMaterial;
                lineMaterial = iceTexture;
                lineColor = iceColor;
                //nowSkeletonAnima = iceSkelton;
                iceSkelton.AnimationName = "animetion";
                iceflag = true;
                //Debug.Log(lineColor);
                break;

            case PenM.PenCom.Fire:
                //nowSkeletonAnima = fireSkelton;
                fireSkelton.AnimationName = "animetion";
                fireflag = true;
                break;

            case PenM.PenCom.General:
                sMaterial = generalMaterial;
                lineMaterial = ironTexture;
                lineColor = generalColor;
                //nowSkeletonAnima = null;
                break;
        }

        if(pasueDisplayC.MenuFlag == false && penDisplayC.PenMenuFlag == false )
        {
            //���N���b�N���ꂽ��
            if (Input.GetMouseButtonDown(0))
            {
              
                _addLineObject();
            }

            //�N���b�N���i�X�g���[�N���j
            if (Input.GetMouseButton(0))
            {
                if(sEffectFlag == false)
                {
                    audioSource.PlayOneShot(sound1);
                    sEffectFlag = true;
                }
                _addPositionDataToLineRenderer();
                
            }

            if (Input.GetMouseButtonUp(0))
            {
               sEffectFlag =false;
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
        //���C���[��Ground�ɂ���
        lineObj.layer = 6;
        //�^�O�ύX
        lineObj.tag = "Ground";
        //Material�ύX
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
