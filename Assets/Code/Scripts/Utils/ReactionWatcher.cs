using UnityEngine;
using System.Collections.Generic;

namespace Chemix.Utils
{
    /// <summary>
    /// ReactionWather enables you to see how reaction is going over a graph.
    /// Note that it will *NOT* record anything if no reaction happen.
    /// </summary>
    [RequireComponent(typeof(ChemixObject))]
    public class ReactionWatcher : MonoBehaviour
    {
#if UNITY_EDITOR
        [System.Serializable]
        class NameAndCurve
        {
            public string name;
            public AnimationCurve curve = new AnimationCurve();
        }

        [SerializeField]
        List<NameAndCurve> graphs = new List<NameAndCurve>();
        ChemixObject cobject;
        int frameCnt = 0;

        AnimationCurve GetGraph(Substance substance)
        {
            foreach (var g in graphs)
            {
                if (substance.formula == g.name)
                {
                    return g.curve;
                }
            }
            var newGraph = new NameAndCurve();
            newGraph.name = substance.formula;
            graphs.Add(newGraph);
            return newGraph.curve;
        }

        private void Start()
        {
            cobject = GetComponent<ChemixObject>();
        }

        private void FixedUpdate()
        {
            if (cobject.System.IsReacting)
            {
                foreach (var s in cobject.Mixture.Substances)
                {
                    var graph = GetGraph(s);
                    graph.AddKey(frameCnt, s.mass);
                }

                frameCnt++;
            }
        }
#endif
    }
}