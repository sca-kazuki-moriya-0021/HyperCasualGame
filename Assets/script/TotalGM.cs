using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class TotalGM : MonoBehaviour
{
    //�S�[���J�E���g
    private int stageGoalCount = 0;
    private int maxClearCount = 3;

    private bool[] stageClearFlag = {false,false,false};


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

    public  Dictionary<string,StageCon> SceneDic
    {
        get { return this.sceneDic; }
        set { this.sceneDic = value; }
    }

    public int StageGoalCount
    {
        get { return this.stageGoalCount; }
        set { this.stageGoalCount = value; }
    }

    public int MaxClearCount
    {
        get { return this.maxClearCount; }
        set { this.maxClearCount = value; }
    }

    public bool[] StageClearFlag
    {
        get { return this.stageClearFlag; }
        set { this.stageClearFlag = value; }
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
