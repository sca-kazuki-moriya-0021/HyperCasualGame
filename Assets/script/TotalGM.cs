using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class TotalGM : MonoBehaviour
{
    //�S�[���J�E���g
    private int stageLeafCount = 0;
    private int maxLeafCount = 9;

    #region//�X�e�[�W�Ǘ�
    //�X�e�[�W�Ǘ�
    public enum StageCon
    { 
        Unknown=0,
        Title,
        StageSelect,
        Fiast,
        Second,
        Therd,
        Fouthe,
        GameOver,
        Clear,
    }

    private StageCon scene;
    private StageCon backScene;
    
    private Dictionary<string,StageCon> sceneDic = new Dictionary<string, StageCon>()
    {
        {"Title",StageCon.Title },
        {"StageSelect",StageCon.StageSelect },
        {"Stage",StageCon.Fiast },
        {"Stage1-2",StageCon.Second },
        {"Stage1-3",StageCon.Therd },
        {"GameOver",StageCon.GameOver },
        {"Gaol",StageCon.Clear },

    };
    #endregion

    private bool[] leafGetFlag = { false, false, false };

    private bool[] tmpGetFlag  = {false,false,false};

    public StageCon Scene
    {
        get { return this.scene; }
    }

    public StageCon BackScene
    {
        get {return this.backScene; }
        set {this.backScene = value; }
    }

    public  Dictionary<string,StageCon> SceneDic
    {
        get { return this.sceneDic; }
        set { this.sceneDic = value; }
    }

    public int StageLeafCount
    {
        get { return this.stageLeafCount; }
        set { this.stageLeafCount = value; }
    }

    public int MaxLeafCount
    {
        get { return this.maxLeafCount; }
        set { this.maxLeafCount = value; }
    }

    public bool[] LeafGetFlag
    {
        get { return this.leafGetFlag; }
        set { this.leafGetFlag = value; }
    }

    public bool[] TmpGetFlag
    {
        get { return this.tmpGetFlag; }
        set { this.tmpGetFlag = value; }
    }

    //�V���O���g��
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

   

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
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
