using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LineDraw : MonoBehaviour
{

    //���̍ގ�
    [SerializeField]
    private Material lineMaterial;
    //���̐F
    [SerializeField]
    private Color lineColor;
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
    
    private PhysicsMaterial2D sMaterial;
    private EdgeCollider2D edge;

    private void Start()
    {
        //List�̏�����
        linePoints = new List<Vector2>();
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
        lineObj.AddComponent<EdgeCollider2D>();
        sMaterial = GetComponent<EdgeCollider2D>().sharedMaterial;
        //lineObj�����g�iStroke�j�̎q�v�f�ɐݒ�
        lineObj.transform.SetParent(transform);
        _initRenderer();
    }

    //lineObj����������
    void _initRenderer()
    {
        //LineRenderer���擾
        lineRenderer = lineObj.GetComponent<LineRenderer>();
        //�|�W�V�����J�E���g�����Z�b�g
        lineRenderer.positionCount = 0;
        //�}�e���A����ݒ�
        lineRenderer.material = lineMaterial;
        //�}�e���A���̐F��ݒ�
        lineRenderer.material.color = lineColor;
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
