using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;

public class TotalGM : MonoBehaviour
{
    //ゴールカウント
    private int stageLeafCount = 0;
    private int maxLeafCount = 9;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] audioClips;

    #region//ステージ管理
    //ステージ管理
    public enum StageCon
    { 
        Start=0,
        Title,
        StageSelect,
        TutorialF,
        TutorialS,
        TutorialT,
        Fiast,
        Second,
        Therd,
        Fouthe,
        GameOver,
        Clear,
    }

    private StageCon scene;
    private StageCon tentative;
    private StageCon backScene;
    private StageCon clearBackScene;
    
    private Dictionary<string,StageCon> sceneDic = new Dictionary<string, StageCon>()
    {
        {"StartScene",StageCon.Start },
        {"Title",StageCon.Title },
        {"StageSelect",StageCon.StageSelect },
        {"TutorialStage",StageCon.TutorialF },
        {"TutorialStage2",StageCon.TutorialS },
        {"TutorialStage3",StageCon.TutorialT },
        {"Stage",StageCon.Fiast },
        {"Stage1-2",StageCon.Second },
        {"Stage1-3",StageCon.Therd },
        {"GameOver",StageCon.GameOver },
        {"Goal",StageCon.Clear },

    };
    #endregion

    private bool[] leafGetFlag = { false, false, false };

   //ゲームクリア画面で表示するもの
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

    public StageCon ClearBackScene
    {
        get { return this.clearBackScene; }
        set { this.clearBackScene = value; }
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

    //シングルトン
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        tentative = MyGetScene();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
       var scene = MyGetScene();
       if(scene != tentative)
            BgmSelert(scene);
    }

    //現在のステージを返す
    public StageCon MyGetScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        scene = sceneDic[sceneName];
        return scene;
    }

    // enumのシーンで指定したシーンをロードする
    public void MyLoadScene(StageCon scene)
    {
        SceneManager.LoadScene(sceneDic.FirstOrDefault(x => x.Value == scene).Key);
    }

    // 現在のシーンを再度ロードする
    public void ReloadCurrentScene()
    {
        StageCon scene = MyGetScene();
        MyLoadScene(scene);
    }

    //前のシーンに戻る時に使う
    public void ReloadClearScene()
    {
        MyLoadScene(backScene); 
    }

    public void ClearBack()
    {
        MyLoadScene(clearBackScene);
    }

    public void BgmSelert(StageCon s)
    {
        switch (s)
        {
            case StageCon.Title:
                audioSource.clip = audioClips[0];
                audioSource.Play();
                tentative = s;
                break;
            case StageCon.GameOver:
                audioSource.clip = audioClips[1];
                audioSource.Play();
                tentative = s;
                break;
            case StageCon.Clear:
                audioSource.clip = audioClips[2];
                audioSource.Play();
                tentative = s;
                break;
            default:
                audioSource.clip = audioClips[3];
                audioSource.Play();
                tentative = s;
                break;
        }
    }

    /// <summary>
    /// スライドバー値の変更イベント
    /// </summary>
    /// <param name="newSliderValue">スライドバーの値(自動的に引数に値が入る)</param>
    public void SoundSliderOnValueChange(float newSliderValue)
    {
        // 音楽の音量をスライドバーの値に変更
        audioSource.volume = newSliderValue;
    }
}
