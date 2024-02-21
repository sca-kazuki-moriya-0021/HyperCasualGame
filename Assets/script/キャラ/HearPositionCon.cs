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
            // ‰½‚Æ‚à–½’†‚µ‚È‚©‚Á‚½ê‡
            Debug.Log(castResult.point);

            if (castResult.collider == null || castResult.collider == castResult.collider.CompareTag("AreaGround")
                || castResult.collider == castResult.collider.CompareTag("Wall"))
            {
                Debug.Log("‰½‚à“ü‚Á‚Ä‚È‚¢‚æ");
                return;
            }

            if (castResult.point.y >= transform.position.y)
            {
                Debug.Log("•Ç‚É‚È‚Á‚Ä‚¢‚é‚æ");
                return;
            }

            var normal = Vector2.Dot(Vector2.up, castResult.normal);
            if (normal <= 0.25f)
                return;

            Debug.Log("ÅŒã‚Ìˆ—‚¾‚æ");
            var yAdjustDistance = yAdjustMaxDistance;
            moveObject.transform.position = new Vector2(transform.position.x, transform.position.y + yAdjustDistance);
        }
    }
}
