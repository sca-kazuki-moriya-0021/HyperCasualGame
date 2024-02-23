using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearPositionCon : MonoBehaviour
{

    [SerializeField]
    private GameObject moveObject;
    private float yAdjustMaxDistance = 0.6f;
    private MoveObject moveObject_cs;
    private RaycastHit2D rayHit;
    private bool setFlag;

    public RaycastHit2D RayHit { get => rayHit; set => rayHit = value; }


    // Start is called before the first frame update
    void Start()
    {
        moveObject_cs = FindObjectOfType<MoveObject>();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);

        if(moveObject_cs.JumpFlag == false || setFlag == false)
        {
            
            var castResult = Physics2D.Linecast((Vector2)transform.position,(Vector2)transform.position + Vector2.down * yAdjustMaxDistance, LayerMask.GetMask("Ground"));
            Debug.DrawRay((Vector2)transform.position, Vector2.down * yAdjustMaxDistance, Color.black);
            // âΩÇ∆Ç‡ñΩíÜÇµÇ»Ç©Ç¡ÇΩèÍçá

            if(rayHit.collider != null)
            {
                if (moveObject_cs.JumpFlag == true || rayHit.collider == rayHit.collider.CompareTag("Ground")
                    || rayHit.collider == rayHit.collider.CompareTag("Wall"))
                    return;
            }
      
            if (castResult.collider == null || castResult.collider == castResult.collider.CompareTag("AreaGround")
                || castResult.collider == castResult.collider.CompareTag("Wall"))
                return;

            var normal = Vector2.Dot(Vector2.up, castResult.normal);
            if (normal <= 0.25f)
                return;
            if(setFlag == false)
            {
                Debug.Log("ç≈å„ÇÃèàóùÇæÇÊ!!!!");
                StartCoroutine(SetTrans(castResult));
                setFlag = true;
            }

        }
    }


    private IEnumerator SetTrans(RaycastHit2D hit)
    {
        var yAdjustDistance = 0.3f;
        yield return new WaitForSeconds(0.2f);
        moveObject.transform.position = new Vector2(transform.position.x, transform.position.y + yAdjustDistance);
        setFlag = false;
        Debug.Log("ç≈å„ÇÃèàóùÇæÇÊ");
        StopAllCoroutines();
    }
}
