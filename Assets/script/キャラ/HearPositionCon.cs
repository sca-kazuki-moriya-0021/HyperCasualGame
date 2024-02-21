using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearPositionCon : MonoBehaviour
{

    [SerializeField]
    private GameObject moveObject;
    private float yAdjustMaxDistance = 0.5f;
    private MoveObject moveObject_cs;

    // Start is called before the first frame update
    void Start()
    {
        moveObject_cs = FindObjectOfType<MoveObject>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if(moveObject_cs.JumpFlag == false)
        {

            if(moveObject_cs.JumpFlag == true)
                return;

            var castResult = Physics2D.Linecast(transform.position, (Vector2)transform.position + Vector2.down * yAdjustMaxDistance, LayerMask.GetMask("Ground"));
            Debug.DrawRay((Vector2)transform.position, Vector2.down * yAdjustMaxDistance, Color.black);
            // 何とも命中しなかった場合
            Debug.Log(castResult.point);

            if (castResult.collider == null || castResult.collider == castResult.collider.CompareTag("AreaGround")
                || castResult.collider == castResult.collider.CompareTag("Wall"))
            {
                Debug.Log("何も入ってないよ");
                return;
            }

            if (castResult.point.y >= transform.position.y)
            {
                Debug.Log("壁になっているよ");
                return;
            }

            var normal = Vector2.Dot(Vector2.up, castResult.normal);
            if (normal <= 0.25f)
                return;

            Debug.Log("最後の処理だよ");
            var yAdjustDistance = yAdjustMaxDistance;
            moveObject.transform.position = new Vector2(transform.position.x, transform.position.y + yAdjustDistance);
        }
    }
}
