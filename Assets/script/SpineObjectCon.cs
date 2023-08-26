using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Linq;
using Spine.Unity;

public class SpineObjectCon : MonoBehaviour
{
    /// <summary> �Đ�����A�j���[�V������ </summary>
    [SerializeField]
    private string testAnimationName = "janken/idle";

    /// <summary> �Q�[���I�u�W�F�N�g�ɐݒ肳��Ă���SkeletonAnimation </summary>
    private SkeletonAnimation skeletonAnimation = default;

    /// <summary> Spine�A�j���[�V������K�p���邽�߂ɕK�v��AnimationState </summary>
    private Spine.AnimationState spineAnimationState = default;

    // Start is called before the first frame update
    void Start()
    {
        // �Q�[���I�u�W�F�N�g��SkeletonAnimation���擾
        skeletonAnimation = GetComponent<SkeletonAnimation>();

        // SkeletonAnimation����AnimationState���擾
        spineAnimationState = skeletonAnimation.AnimationState;
    }

    // Update is called once per frame
    void Update()
    {
        // �A�j���[�V�����utestAnimationName�v���Đ�
        spineAnimationState.SetAnimation(0, testAnimationName,true);
    }
}
