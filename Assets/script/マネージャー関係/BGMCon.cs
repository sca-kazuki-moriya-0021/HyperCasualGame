using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMCon : MonoBehaviour
{

    public static BGMCon instance;

    private TotalGM gm;

    private AudioSource audioSource;


    //�V���O���g��
    private void Awake()
    {
        gm = FindObjectOfType<TotalGM>();

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var scene = gm.MyGetScene();

        if (scene == TotalGM.StageCon.Clear)
        {
            Destroy(gameObject);
        }

        if (scene == TotalGM.StageCon.GameOver)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
	/// �X���C�h�o�[�l�̕ύX�C�x���g
	/// </summary>
	/// <param name="newSliderValue">�X���C�h�o�[�̒l(�����I�Ɉ����ɒl������)</param>
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        // ���y�̉��ʂ��X���C�h�o�[�̒l�ɕύX
        
    }
}
