using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    public class GameManager : Singleton<GameManager>
    {
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
            public TaskFlow taskFlow;
            public TextInfo title;
            public TextInfo detail;
        }

        protected override void Awake()
        {
            base.Awake();

            // setup type2instrument
            foreach (var instrument in GM.GM_Core.instance.instrumentListAsset.instruments)
            {
                type2instrument.Add(instrument.type, instrument);
            }
        }

        public ExperimentalSetup GetExperimentalSetup()
        {
            return GM.GM_Core.instance.experimentalSetup;
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

        private Dictionary<string, InstrumentsListAsset.Instrument> type2instrument = new Dictionary<string, InstrumentsListAsset.Instrument>();
    }
}