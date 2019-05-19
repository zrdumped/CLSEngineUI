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

        [Header("Authoring System")]
        public GameObject authoringPrefab;
        // public Image icon;
        // public enum Type type;
        // public float offsetY;

        [Header("Simulating System")]
        public GameObject simulatingPrefab;
        [Tooltip("Whether or not can we add medicine in this instrument")]
        public bool isSubstanceContainer;
    }

    public List<Instrument> instruments;
}
