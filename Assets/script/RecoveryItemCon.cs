using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoveryItemCon : MonoBehaviour
{
    private bool recoveryFlag = false;

    public bool RecoveryFlag
    {
        get { return this.recoveryFlag; }
        set { this.recoveryFlag = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("MoveObject"))
        {
            recoveryFlag = true;
            this.gameObject.SetActive(false);
        }
    }
}