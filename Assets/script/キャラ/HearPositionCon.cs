using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearPositionCon : MonoBehaviour
{

    [SerializeField]
    private GameObject moveObject;
    private Vector2 pos;

    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);


        // ���g�̍��W
        var selfPosition = pos;
        var frontPosition = selfPosition + Vector2.right; // �O�� �����ɉ�����right�𔽓]
        var yAdjustMaxDistance = 0.2f; // �␳�\�ȍ��� 
        var rayOrigin = frontPosition + (Vector2.up * yAdjustMaxDistance); // Ray�̔��ˈʒu
                                                                           // ���g�̑O���̏����ォ��^���Ɍ�����Ray���΂�
        var castResult = Physics2D.Raycast(rayOrigin, Vector2.down, distance: yAdjustMaxDistance, 6);
        // ���Ƃ��������Ȃ������ꍇ
        if (castResult.collider == null)
            return;
        // ��������collider�̖@���Ǝ��g�̏�����̓��ς�����Ĉ��̌X���ȉ��Ȃ�o��Ȃ� or �����������ӂƔ���
        var normal = Vector2.Dot(Vector2.up, castResult.normal);
        if (normal <= 0.25f)
            return;

        // ���g��y���W����␳����ׂ�����
        var yAdjustDistance = castResult.distance - yAdjustMaxDistance;
        moveObject.transform.position = new Vector2(transform.position.x,transform.position.y + yAdjustDistance);
    }

}
