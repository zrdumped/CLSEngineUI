using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Chemix.UI;

namespace Chemix
{
    //[System.Serializable]
    public class ChemixReactionSystem : IRichText
    {
        #region Properties

        public List<ChemixObject> cobjects = new List<ChemixObject>();
        public bool IsHeating
        {
            get
            {
                return isHeating;
            }

            set
            {
                if (isHeating != value)
                {
                    isHeating = value;
                    FindAndSetupReactions();
                }
            }
        }
        public bool IsReacting => reactions.Count != 0;

        #endregion

        #region Methods

        public ChemixReactionSystem(ChemixObject cobject)
        {
            cobjects.Add(cobject);
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            int imax = cobjects.Count - 1;

            for (int i = 0; i < imax; i++)
            {
                builder.Append(cobjects[i].ToString()).Append("+");
            }
            builder.Append(cobjects[imax].ToString());

            if (IsReacting)
            {
                builder.Append("*"); // indicate reaction is undergoing
            }

            if (isHeating)
            {
                builder.Append("^"); // indicate system is heating up 
            }

            return builder.ToString();
        }

        public string ToRichString()
        {
            StringBuilder builder = new StringBuilder();
            int imax = cobjects.Count - 1;

            for (int i = 0; i < imax; i++)
            {
                builder.Append(cobjects[i].ToRichString()).Append("+");
            }
            builder.Append(cobjects[imax].ToRichString());

            if (IsReacting)
            {
                builder.Append("*"); // indicate reaction is undergoing
            }

            if (isHeating)
            {
                builder.Append("^"); // indicate system is heating up 
            }

            return builder.ToString();
        }

        public bool IsOwner(ChemixObject cobject)
        {
            return cobjects[0] == cobject;
        }

        public void Add(ChemixObject cobject)
        {
            if (cobject.System == this)
            {
                Debug.LogFormat("ReactionSystem: {0} Add {1} twice.", this.ToString(), cobject.ToString());
                return;
            }

            foreach (var s in cobject.System.cobjects)
            {
                cobjects.Add(s);
                s.System = this;
            }
            FindAndSetupReactions();
        }

        public void Remove(ChemixObject cobject)
        {
            if (cobjects.Remove(cobject))
            {
                cobject.System = new ChemixReactionSystem(cobject);
            }
            FindAndSetupReactions();
        }

        public void FindAndSetupReactions()
        {
            reactions = ChemixEngine.Instance.FindPossibleReactions(this);

            //bool reactionsChanged = false;
            //if (new_reactions.Count == reactions.Count)
            //{
            //    for (int i = 0; i < new_reactions.Count; i++)
            //    {
            //        if (new_reactions[i].equation != reactions[i].equation)
            //        {
            //            reactionsChanged = true;
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    reactionsChanged = true;
            //}

            //reactions = new_reactions;

            string equationInfo = "";
            foreach (var r in reactions)
            {
                Debug.LogFormat("ReactionSystem/equation: {0}", r.equation.ToString());
                equationInfo += "\n" + r.equation.ToRichString();
            }

            if (reactions.Count > 0)
            {
                UIManager.Instance.DisplayEquationBox(cobjects, equationInfo);
                //UIManager.Instance.DisplayTest(cobjects[0].transform);
            }
        }

        public void AddProduct(Substance substance)
        {
            bool foundObject = false;

            foreach (var cobject in cobjects)
            {
                if (cobject.Mixture.TryAdd(substance))
                {
                    foundObject = true;
                    break;
                }
            }

            // TODO: create new cobject if there's no cbject with the same state
            if (!foundObject)
            {
                Debug.LogFormat("ReactionSystem: product {0} goes into the air", substance.formula);
            }
        }

        public void OnSystemUpdate(float deltaTime)
        {
            if (!IsReacting)
            {
                return;
            }

            bool allReactionComplete = true;

            foreach (var r in reactions)
            {
                bool oneReactionComplete = false;
                float rateMultiplier = CalculateRateMultiplier(r);
                // simulate reaction and update states
                float baseRate = Chemix.Config.globalReactionRate * deltaTime * rateMultiplier;
                float compensate = 0;
                foreach (var slot in r.slots)
                {
                    float deltaMass = baseRate * slot.tickMass;
                    float newMass = slot.substance.mass - deltaMass;

                    slot.substance.mass = newMass;
                    if (newMass < 0)
                    {
                        oneReactionComplete = true;
                        compensate = Mathf.Max(compensate, -newMass / slot.tickMass);
                    }
                }
                allReactionComplete &= oneReactionComplete;

                // when over-react occur, we should compensate for it 
                if (compensate > 0)
                {
                    foreach (var slot in r.slots)
                    {
                        slot.substance.mass += compensate * slot.tickMass;

                        if (slot.substance.mass <= 0)
                        {
                            foreach (var cobject in cobjects)
                            {
                                if (cobject.Mixture.TryRemove(slot.substance))
                                    break;
                            }
                        }
                    }
                }
            }

            if (allReactionComplete)
            {
                reactions.Clear();
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Calculate multiplier according to mole mass
        /// </summary>
        float CalculateRateMultiplier(ChemixEngine.Reaction reaction)
        {
            var config = Chemix.Config;
            float rateMultiplier = 1f;
            if (reaction.equation.reversable)
            {
                float forwardRate = 1f, backwardRate = 1f;
                foreach (var slot in reaction.slots)
                {
                    if (ChemixEngine.Instance.LookupPhase(slot.substance) != ChemixEngine.Phase.Solid)
                    {
                        float slotFactor = Mathf.Clamp(slot.substance.mass / config.middleMass, config.minMultiplier, config.maxMultiplier);
                        if (slot.IsReactant)
                        {
                            forwardRate *= slotFactor;
                        }
                        else
                        {
                            backwardRate *= slotFactor;
                        }
                    }
                }

                rateMultiplier = reaction.equation.reversableFactor * forwardRate - (1 - reaction.equation.reversableFactor) * backwardRate;
                if (rateMultiplier > 0)
                {
                    rateMultiplier = Mathf.Clamp(rateMultiplier, config.MinMultiplierSquare, config.MaxMultiplierSquare);
                }
                else
                {
                    rateMultiplier = Mathf.Clamp(rateMultiplier, -config.MaxMultiplierSquare, -config.MinMultiplierSquare);
                }
            }
            else
            {
                foreach (var slot in reaction.slots)
                {
                    if (ChemixEngine.Instance.LookupPhase(slot.substance) != ChemixEngine.Phase.Solid)
                    {
                        if (slot.IsReactant)
                        {
                            rateMultiplier *= Mathf.Clamp(slot.substance.mass / config.middleMass, config.minMultiplier, config.maxMultiplier);
                        }
                    }
                }
                rateMultiplier = Mathf.Clamp(rateMultiplier, config.MinMultiplierSquare, config.MaxMultiplierSquare);
            }
            return rateMultiplier;
        }

        bool isHeating = false;
        List<ChemixEngine.Reaction> reactions = new List<ChemixEngine.Reaction>();

        #endregion
    }
}