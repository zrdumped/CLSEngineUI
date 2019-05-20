using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// InstrumentListAsset stores all the possible instruments that could be added
/// into scene, with its information for authoring system and simulating system.
/// </summary>
[CreateAssetMenu(fileName = "NewInstrumentList", menuName = "Chemix/InstrumentList", order = 3)]
public class InstrumentsListAsset : ScriptableObject
{
    [System.Serializable]
    public class Instrument
    {
        public string type;
        
        [Header("Simulating System")]
        public GameObject simulatingPrefab;
        [Tooltip("Whether or not can we add medicine in this instrument")]
        public bool isSubstanceContainer;
        public Vector3 offset;
        public float scaleMultiplier = 1f;
    }

    public List<Instrument> instruments;
}
