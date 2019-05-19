using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [CreateAssetMenu(fileName = "NewChemixDatabase", menuName = "Chemix/Database", order = 3)]
    public class ChemixDatabase : ScriptableObject
    {
        [System.Serializable]
        public class SubstanceInfos
        {
            public List<ChemixEngine.SubstanceInfo> solid = new List<ChemixEngine.SubstanceInfo>() { new ChemixEngine.SubstanceInfo() };
            public List<ChemixEngine.SubstanceInfo> liquid = new List<ChemixEngine.SubstanceInfo>() { new ChemixEngine.SubstanceInfo() };
            public List<ChemixEngine.SubstanceInfo> gas = new List<ChemixEngine.SubstanceInfo>() { new ChemixEngine.SubstanceInfo() };
        }

        public List<ChemixEngine.Equation> equations = new List<ChemixEngine.Equation>() { new ChemixEngine.Equation() };

        public SubstanceInfos substanceInfos;
    }
}