using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class SpineMove : MonoBehaviour
{
    [SerializeField] GameObject h;
    SkeletonAnimation g;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        g = h.GetComponent<SkeletonAnimation>();
        g.AnimationName = "None";
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            g.AnimationName = "animation";

        }
    }
}
