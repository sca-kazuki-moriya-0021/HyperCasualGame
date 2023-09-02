using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoveryItemCon : MonoBehaviour
{
    [SerializeField]
    private GameObject item;

    private bool recoveryFlag = false;

    public bool RecoveryFlag
    {
        get { return this.recoveryFlag; }
        set { this.recoveryFlag = value; }
    }

    public GameObject Item
    {
        get { return this.item; }
        set { this.item = value; }
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