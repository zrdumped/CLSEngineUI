using System.Collections.Generic;
using UnityEngine;

public class GameManager : Chemix.Singleton<GameManager>
{
#if HAIL_HYDRA
    private void Start()
    {
        SwitchToScene(m_DefaultSceneName);
    }

    private void SwitchToScene(string sceneName)
    {
        if (m_CurrentSceneName != null)
        {
            SceneManager.UnloadSceneAsync(m_CurrentSceneName);
        }
        m_CurrentSceneName = sceneName;
        StartCoroutine(LoadSceneJob(sceneName));
    }

    IEnumerator LoadSceneJob(string sceneName)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    private string m_CurrentSceneName;

    [SerializeField]
    private string m_DefaultSceneName = "Default";
#endif
    
    /// <summary>
    /// InstrumentInfo contains the type, transform, and stattus of instrument
    /// </summary>
    [System.Serializable]
    public class InstrumentInfo
    {
        public string type;
        public Vector3 position;
        public Quaternion quaternion;
        public string formula;
        public float mass;
    }

    [System.Serializable]
    public class TextInfo
    {
        public Vector3 position;
        public Color color;
        public float size;
    }

    /// <summary>
    /// ExperimentalSetup contains the experimental setup which user created
    /// </summary>
    [System.Serializable]
    public class ExperimentalSetup
    {
        public List<InstrumentInfo> instrumentInfos;
        public Chemix.TaskFlow taskFlow;
        public TextInfo title;
        public TextInfo detail;
    }

    protected override void Awake()
    {
        base.Awake();

        // setup type2instrument
        foreach (var instrument in instrumentListAsset.instruments)
        {
            type2instrument.Add(instrument.type, instrument);
        }
    }

    public ExperimentalSetup GetExperimentalSetup()
    {
        return m_ExperimentalSetup;
    }

    public InstrumentsListAsset.Instrument GetInstrumentByType(string type)
    {
        if (type2instrument.ContainsKey(type))
        {
            return type2instrument[type];
        }
        Debug.LogErrorFormat("GameManager: no instrument for type {0}", type);
        return null;
    }
    
    public InstrumentsListAsset instrumentListAsset
    {
        get { return m_InstrumentListAsset; }
    }

    [SerializeField] // TODO: test only
    private ExperimentalSetup m_ExperimentalSetup = new ExperimentalSetup();

    [SerializeField]
    private InstrumentsListAsset m_InstrumentListAsset;

    private Dictionary<string, InstrumentsListAsset.Instrument> type2instrument = new Dictionary<string, InstrumentsListAsset.Instrument>();
}