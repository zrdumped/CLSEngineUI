using System.Collections.Generic;
using System.Text;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Chemix
{
    /// <summary>
    /// ChemixEngine manages reactions
    /// </summary> 
    public class ChemixEngine : Singleton<ChemixEngine>
    {
        #region Subclasses

        public enum Condition
        {
            None,
            Heat,
        }

        public enum Phase
        {
            Liquid,
            Solid,
            Gas,
        }

        [System.Serializable]
        public class SubstanceInfo
        {
            public string formula;
            [HideInInspector]
            public Phase state;
        }

        [System.Serializable]
        public class EquationSlot : IRichText
        {
            public bool IsReactant
            {
                get { return constant > 0; }
            }

            public string formula = "Unknown";
            public int constant = 1;

            public override string ToString()
            {
                int absConstant = Mathf.Abs(constant);
                return absConstant == 1 ? formula : (absConstant.ToString() + formula);
            }

            public string ToRichString()
            {
                int absConstant = Mathf.Abs(constant);
                string processedFormula = Chemix.InsertSubscriptTag(formula);
                return absConstant == 1 ? processedFormula : (absConstant.ToString() + processedFormula);
            }
        }

        [System.Serializable]
        public class ReactionSlot
        {
            public bool IsReactant
            {
                get { return tickMass > 0; }
            }

            public Substance substance;
            public float tickMass;

            public ReactionSlot(Substance s, float t)
            {
                substance = s;
                tickMass = t;
            }
        }

        [System.Serializable]
        public class Equation : IRichText
        {
            public string name = "NewEquation";
            public float reactionRate = 1f;
            public Condition condition = Condition.None;
            public bool reversable = false;
            [Range(0f, 1f)]
            public float reversableFactor = 0.5f;
            public List<EquationSlot> equationSlots = new List<EquationSlot>() { new EquationSlot() };

            public override string ToString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(equationSlots[0].ToString());
                int i = 1;
                for (; i < equationSlots.Count; i++)
                {
                    if (equationSlots[i].constant < 0)
                        break;
                    builder.Append(" + ").Append(equationSlots[i].ToString());
                }

                builder.Append(" == ").Append(equationSlots[i++]);

                for (; i < equationSlots.Count; i++)
                {
                    builder.Append(" + ").Append(equationSlots[i].ToString());
                }
                return builder.ToString();
            }

            public string ToRichString()
            {
                StringBuilder builder = new StringBuilder();
                builder.Append(equationSlots[0].ToRichString());
                int i = 1;
                for (; i < equationSlots.Count; i++)
                {
                    if (equationSlots[i].constant < 0)
                        break;
                    builder.Append(" + ").Append(equationSlots[i].ToRichString());
                }

                builder.Append(" == ").Append(equationSlots[i++].ToRichString());

                for (; i < equationSlots.Count; i++)
                {
                    builder.Append(" + ").Append(equationSlots[i].ToRichString());
                }
                return builder.ToString();
            }

            public bool Match(ChemixReactionSystem system, out Reaction reaction)
            {
                // match conditions
                if (condition == Condition.Heat && !system.IsHeating)
                {
                    //Debug.Log("ChemixEngine: reject matching cuz heating is required.");
                    reaction = new Reaction();
                    return false;
                }

                // match substances
                if (reversable)
                {
                    if (MatchSubstances(system, out reaction, false))
                        return true;
                    return MatchSubstances(system, out reaction, true);
                }
                else
                {
                    return MatchSubstances(system, out reaction, false);
                }
            }

            public bool MatchSubstances(HashSet<string> formulas)
            {
                int imax = reversable ? 2 : 1;
                for (int i = 0; i < imax; i++)
                {
                    bool reverse = i > 0;
                    bool success = true;
                    foreach (var slot in equationSlots)
                    {
                        if (slot.IsReactant ^ reverse)
                        {
                            if (!formulas.Contains(slot.formula))
                            {
                                success = false;
                                break;
                            }
                        }
                    }

                    if (success)
                    {
                        return success;
                    }
                }
                return false;
            }

            bool MatchSubstances(ChemixReactionSystem system, out Reaction reaction, bool reverse)
            {
                reaction = new Reaction();
                foreach (var slot in equationSlots)
                {
                    bool slotMatch = false;

                    foreach (var cobject in system.cobjects)
                    {
                        var substance = cobject.Mixture.FindSubstance(slot.formula, (!slot.IsReactant) ^ reverse);
                        if (substance != null)
                        {
                            slotMatch = true;
                            reaction.slots.Add(new ReactionSlot(substance, slot.constant * reactionRate));
                            break;
                        }
                    }

                    if (!slotMatch)
                    {
                        // add product in new substance
                        if (slot.IsReactant ^ reverse)
                        {
                            return false;
                        }
                        else
                        {
                            var substance = new Substance(slot.formula);
                            system.AddProduct(substance);
                            reaction.slots.Add(new ReactionSlot(substance, slot.constant * reactionRate));
                            reaction.hasNewProduct = true;
                        }
                    }
                }

                reaction.equation = this;
                return true;
            }
        }

        public class Reaction
        {
            public Equation equation;
            public List<ReactionSlot> slots = new List<ReactionSlot>();
            public bool hasNewProduct = false;
        }

        #endregion

        #region Properties

        public ChemixConfig Config
        {
            get;
            private set;
        }

        public TaskFlow customTaskFlow
        {
            get;
            private set;
        }
        
        public bool CustomMode = false;

        [HideInInspector]
        public Camera mainCamera; // cache this for better performance

        #endregion

        #region Messages

        protected override void Awake()
        {
            base.Awake();

#if UNITY_EDITOR
            Config = debugConfig;
#else
            Config = releaseConfig;
#endif

            // cache main camera
            mainCamera = Camera.main;

            // setup dictionary
            foreach (var info in database.substanceInfos.solid)
            {
                info.state = Phase.Solid;
                formula2info.Add(info.formula, info);
            }
            foreach (var info in database.substanceInfos.liquid)
            {
                info.state = Phase.Liquid;
                formula2info.Add(info.formula, info);
            }
            foreach (var info in database.substanceInfos.gas)
            {
                info.state = Phase.Gas;
                formula2info.Add(info.formula, info);
            }

            if (Chemix.Config.filterEquation)
            {
                FilterEquation();
            }

            //StartCoroutine(SetupInCustomMode());
        }

        //IEnumerator SetupInCustomMode()
        void Start()
        {
            if (!GM.GM_Core.instance)
                return;

            var setup = GameManager.Instance.GetExperimentalSetup();
            
            if (CustomMode)
            {
                customTaskFlow = setup.taskFlow;

                Debug.Log("ChemixEngine: Load instrument info");
                foreach (var info in setup.instrumentInfos)
                {
                    var instrument = GameManager.Instance.GetInstrumentByType(info.type);

                    var go = Instantiate(instrument.simulatingPrefab);
                    go.transform.position = info.position + new Vector3(0, instrument.offsetY, 0);
                    go.transform.rotation = info.quaternion;
                    go.transform.localScale *= instrument.scaleMultiplier;

                    var sc = go.GetComponent<Utils.SplineController>();
                    if (sc)
                        sc.enabled = false;

                    var si = go.GetComponent<Utils.SplineInterpolator>();
                    if (si)
                        si.enabled = false;


                    if (instrument.isSubstanceContainer)
                    {
                        var co = go.GetComponentInChildren<ChemixObject>();
                        if (co)
                        {
                            co.Mixture.ClearAndAdd(new Mixture(info.formula, info.mass));
                        }
                        else
                        {
                            Debug.LogErrorFormat("ChemixEngine: {0} have no ChemixObject for container", go.name);
                        }
                    }
                    else
                    {
                        if (info.mass != 0)
                        {
                            Debug.LogWarningFormat("ChemixEngine: {0} is configured to be non-container", info.type);
                        }
                    }
                }
            }
        }

        #endregion

        #region Methods

        public List<Reaction> FindPossibleReactions(ChemixReactionSystem system)
        {
            bool hasNewProduct = true;
            List<Reaction> reactions = new List<Reaction>();
            List<Equation> equations = Chemix.Config.filterEquation ? filteredEquations : database.equations;

            while (hasNewProduct)
            {
                hasNewProduct = false;
                foreach (var equation in equations)
                {
                    // Ignore already matched equations
                    bool hasMatched = false;
                    foreach (var r in reactions)
                    {
                        if (r.equation == equation)
                        {
                            hasMatched = true;
                            break;
                        }
                    }
                    if (hasMatched)
                    {
                        continue;
                    }

                    Reaction reaction;
                    if (equation.Match(system, out reaction))
                    {
                        reactions.Add(reaction);
                        hasNewProduct |= reaction.hasNewProduct;
                    }
                }
            }
            return reactions;
        }

        public Phase LookupPhase(Substance substance)
        {
            SubstanceInfo info;
            if (formula2info.TryGetValue(substance.formula, out info))
            {
                return info.state;
            }
            else
            {
                Debug.LogWarningFormat("ChemixEngine: GetState({0}) not found", substance.formula);
                return Phase.Liquid;
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void FilterEquation()
        {
            float startTime = Time.realtimeSinceStartup;
            //Debug.Log("ChemixEngine: Discard impossible reactions");

            // Get initial possible formulas
            HashSet<string> possibleFormulas = new HashSet<string>();
            var cobjects = Object.FindObjectsOfType<ChemixObject>();
            foreach (var cobject in cobjects)
            {
                //Debug.LogFormat("ChemixEngine: Find ChemixObject {0}", cobject.name);
                foreach (var substance in cobject.Mixture.Substances)
                {
                    possibleFormulas.Add(substance.formula);
                }
            }

            // Calculate possible reactions
            bool hasNewFormula = true;
            HashSet<Equation> possibleEquationSet = new HashSet<Equation>();
            while (hasNewFormula)
            {
                hasNewFormula = false;
                foreach (var equation in database.equations)
                {
                    if (!possibleEquationSet.Contains(equation) && equation.MatchSubstances(possibleFormulas))
                    {
                        possibleEquationSet.Add(equation);
                        //Debug.LogFormat("ChemixEngine: Find equation {0}", equation.ToString());

                        foreach (var equationSlot in equation.equationSlots)
                        {
                            if (possibleFormulas.Add(equationSlot.formula))
                            {
                                //Debug.LogFormat("ChemixEngine: Find product {0}", equationSlot.formula);
                                hasNewFormula = true;
                            }
                        }
                    }
                }
            }
            filteredEquations = possibleEquationSet.ToList();
            Debug.LogFormat("<color=green>ChemixEngine: FilterEquation</color> {0}/{1} in {2}s", filteredEquations.Count, database.equations.Count, (Time.realtimeSinceStartup - startTime).ToString("0.0000"));
        }

        #endregion

        #region Privates

        Dictionary<string, SubstanceInfo> formula2info = new Dictionary<string, SubstanceInfo>();
        List<Equation> filteredEquations;

        [SerializeField]
        ChemixDatabase database;
        [SerializeField]
        ChemixConfig debugConfig;
        [SerializeField]
        ChemixConfig releaseConfig;

        #endregion
    }
}