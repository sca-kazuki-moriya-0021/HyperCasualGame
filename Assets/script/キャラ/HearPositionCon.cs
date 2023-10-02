using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearPositionCon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("IceGround"))
        {
            Vector3 postion = other.ClosestPoint(transform.position);
            transform.root.gameObject.GetComponent<MoveObject>().LineSensor(postion);
        }
      
    }
}
