using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using Spine.Unity;

public class SpineObjectCon : MonoBehaviour
{
    /// <summary> 再生するアニメーション名 </summary>
    [SerializeField]
    private string testAnimationName = "janken/idle";

    /// <summary> ゲームオブジェクトに設定されているSkeletonAnimation </summary>
    private SkeletonAnimation skeletonAnimation = default;

    /// <summary> Spineアニメーションを適用するために必要なAnimationState </summary>
    private Spine.AnimationState spineAnimationState = default;

    // Start is called before the first frame update
    void Start()
    {
        // ゲームオブジェクトのSkeletonAnimationを取得
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        // SkeletonAnimationからAnimationStateを取得
        spineAnimationState = skeletonAnimation.AnimationState;
    }

    // Update is called once per frame
    void Update()
    {
        // アニメーション「testAnimationName」を再生
        spineAnimationState.SetAnimation(0, testAnimationName,true);
    }
}
