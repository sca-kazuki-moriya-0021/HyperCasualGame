using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class TotalGM : MonoBehaviour
{
    #region//�X�e�[�W�Ǘ�
    //�X�e�[�W�Ǘ�
    public enum StageCon
    { 
        Unknown,
        Title,
        StageSelect,
        Fiast,
        Second,
        Therd,
        Fouthe,
        GameOver,
        Clear,
        NO,
    }

    private StageCon scene;
    private StageCon backScene;
    
    private Dictionary<string,StageCon> sceneDic = new Dictionary<string, StageCon>()
    {
        {"Title",StageCon.Title },
        {"StageSelect",StageCon.StageSelect },
        {"Stage",StageCon.Fiast },
        {"SecondStage",StageCon.Second },
        {"GameOver",StageCon.GameOver },
        {"Gaol",StageCon.Clear },

    };
    #endregion


    private PhysicsMaterial2D sMaterial;
    private BoxCollider2D boxC2D;
    private PenM penM;


    public StageCon Scene
    {
        get { return this.scene; }
        set { this.scene = value; }
    }

    public StageCon BackScene
    {
        get {return this.backScene; }
        set {this.backScene = value; }
    }

    public PhysicsMaterial2D SMaterial
    {
        get { return this.sMaterial; }
        set { this.sMaterial = value; }
    }

    public  Dictionary<string,StageCon> SceneDic
    {
        get { return this.sceneDic; }
        set { this.sceneDic = value; }
    }


    //�V���O���g��
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        boxC2D = GetComponent<BoxCollider2D>();
        penM = FindObjectOfType<PenM>();
    }

    // Update is called once per frame
    void Update()
    {
        /*switch (penM.NowPen)
        {
            case :
            sMaterial = boxC2D.sharedMaterial;
            sMaterial.friction = 0.001f;
            break;

            case 2:

            break;

            case 3:
            sMaterial = boxC2D.sharedMaterial;
            sMaterial.friction = 0.1f;
            break;


        }*/
    }

    //���݂̃X�e�[�W��Ԃ�
    public StageCon MyGetScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        scene = sceneDic[sceneName];
        return scene;
    }

    // enum�̃V�[���Ŏw�肵���V�[�������[�h����
    public void MyLoadScene(StageCon scene)
    {
        SceneManager.LoadScene(sceneDic.FirstOrDefault(x => x.Value == scene).Key);
    }

    // ���݂̃V�[�����ēx���[�h����
    public void ReloadCurrentSchene()
    {
        StageCon scene = MyGetScene();
        MyLoadScene(scene);
    }

    //�O�̃V�[���ɖ߂鎞�Ɏg��
    public void ReloadClearSchene()
    {
        MyLoadScene(backScene);
    }

}
