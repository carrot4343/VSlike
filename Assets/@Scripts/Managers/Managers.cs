using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers m_instance;
    static bool is_generated = false;

    #region Contents
    GameManager gameManager = new GameManager();
    ObjectManager objectManager = new ObjectManager();
    PoolManager poolManager = new PoolManager();
    public static GameManager _Game { get { return Instance?.gameManager; } }
    public static ObjectManager _Object { get { return Instance?.objectManager; } }
    public static PoolManager _Pool { get { return Instance?.poolManager; } }
    #endregion

    #region Core
    DataManager dataManager = new DataManager();
    ResourceManager resourceManager = new ResourceManager();
    SceneManagerEx sceneManagerEx = new SceneManagerEx();
    SoundManager soundManager = new SoundManager();
    UIManager uiManager = new UIManager();
    public static DataManager _Data { get { return Instance?.dataManager; } }
    public static ResourceManager _Resource { get { return Instance?.resourceManager; } }
    public static SceneManagerEx _Scene { get { return Instance?.sceneManagerEx; } }
    public static SoundManager _Sound { get { return Instance?.soundManager; } }
    public static UIManager _UI { get { return Instance?.uiManager; } }
    #endregion

    public static Managers Instance
    {
        get
        {
            if(is_generated == false)
            {
                is_generated = true;

                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject() { name = "@Managers" };
                    go.AddComponent<Managers>();
                }

                DontDestroyOnLoad(go);
                m_instance = go.GetComponent<Managers>();
            }
            return m_instance;
        }
    }
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}