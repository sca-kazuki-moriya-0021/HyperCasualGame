using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using System.Linq;
using UnityEngine.UI;
using UnityEditor;
using Spine.Unity;

public class LineDraw : MonoBehaviour

{
    //
    [SerializeField]
    private Material lineMaterial;
    private Material _myMat;

    private EdgeCollider2D c;

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

    //�A�j���[�V������K�p���邽�߂ɕK�v��AnimationState
    private Spine.AnimationState spineAnimationState = default;

    private EdgeCollider2D edge;

    private LineDrawCon lineDrawCon;

    [SerializeField]
    private GameObject nullObject;

    [SerializeField]
    private GameObject instansIcePrefab;
    SkeletonAnimation ice;
    [SerializeField]
    private GameObject instansfirePrefab;
    SkeletonAnimation fire;

    private void Start()
    {
        ice = instansIcePrefab.GetComponent<SkeletonAnimation>();
        fire = instansfirePrefab.GetComponent<SkeletonAnimation>();
        ice.AnimationName = "None";
        //List�̏�����
        linePoints = new List<Vector2>();
        lineDrawCon = FindObjectOfType<LineDrawCon>();

    }

    private void Update()
    {
        //���N���b�N���ꂽ��
        if (Input.GetMouseButtonDown(0))
        {
            _addLineObject();
        }

        //�N���b�N���i�X�g���[�N���j
        if (Input.GetMouseButton(0))
        {
            _addPositionDataToLineRenderer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            linePoints = new List<Vector2>();
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
        c.sharedMaterial =lineDrawCon.SMaterial;
        //Debug.Log(lineObj.AddComponent<EdgeCollider2D>().sharedMaterial = lineDrawCon.SMaterial);
        //lineObj�����g�iStroke�j�̎q�v�f�ɐݒ�
        lineObj.transform.SetParent(transform);

        //�A�j���[�V�����Z�b�g
        /*if (lineDrawCon.NowSkeletonAnima != null)
        {
            Debug.Log(lineDrawCon.NowSkeletonAnima.name);
            spineAnimationState.SetAnimation(1, lineDrawCon.NowSkeletonAnima.name, true);
        }*/

        //�A�C�X�������ꂽ
        if(lineDrawCon.IceFlag == true)
        {
            Instantiate(instansIcePrefab ,new Vector3(0, 0, 1.0f),Quaternion.identity);
            ice.AnimationName = lineDrawCon.lineName(lineDrawCon.Name);
            lineDrawCon.IceFlag = false;
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
         lineMaterial.SetColor("_Color", lineDrawCon.LineColor);

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
