using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Chemix
{
    // TODO: not elegant. Other classes both reference object and mixture.
    [System.Serializable]
    public class Mixture : IRichText
    {
        #region Properties

        public static Mixture Air
        {
            get
            {
                return new Mixture("Air", Chemix.Config.airConstant * Time.fixedDeltaTime);
            }
        }

        public bool IsAir
        {
            get { return substances.Count == 1 && substances[0].formula.Equals("Air"); }
        }

        public float TotalMass
        {
            get
            {
                float totalMass = 0;
                foreach (var s in substances)
                {
                    totalMass += s.mass;
                }
                return totalMass;
            }
        }

        public ChemixEngine.Phase Phase
        {
            get { return phase; }
            set { phase = value; }
        }

        public List<Substance> Substances
        {
            get { return substances; }
        }

        #endregion

        #region Methods

        public Mixture() { }

        public Mixture(string formula, float mass = 0f)
        {
            substances.Add(new Substance(formula, mass));
        }

        bool Add(Substance substance)
        {
            foreach (var s in substances)
            {
                if (s.formula.Equals(substance.formula))
                {
                    if (s.mass > 0)
                    {
                        s.mass += substance.mass;
                        return false;
                    }
                    else
                    {
                        s.mass = substance.mass;
                        return true;
                    }
                }
            }

            substances.Add(substance);
            return true;
        }

        public void ClearAndAdd(Mixture mixture)
        {
            substances = new List<Substance>();
            Add(mixture);
        }

        /// <summary>
        /// Return true if new formula is introduced
        /// </summary>
        public bool Add(Mixture other)
        {
            bool hasNewFormula = false;

            foreach (var s in other.substances)
            {
                hasNewFormula |= Add(s);
            }

            return hasNewFormula;
        }

        public bool TryAdd(Substance substance)
        {
            if (ChemixEngine.Instance.LookupPhase(substance) == phase)
            {
                Add(substance);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Take certain mass of mixture out
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="takeAirFirst">Should we take air first if any</param>
        /// <returns></returns>
        public Mixture Take(float mass, bool takeAirFirst = true)
        {
            if (mass < 0)
            {
                Debug.LogError("Mixture/Take: invalid mass " + mass);
                return new Mixture("Unknown");
            }

            if (phase == ChemixEngine.Phase.Gas && takeAirFirst)
            {
                var mixture = TakeGas(mass);
                if (mixture != null)
                    return mixture;
            }

            return TakeByRatio(mass / TotalMass);
        }

        public Mixture TakeByRatio(float ratio = 0.5f)
        {
            if (ratio < 0)
            {
                ratio = Mathf.Clamp(ratio, 0, 1);
                Debug.LogError("Mixture/TakeByRatio: invalid ratio " + ratio);
            }
            else if (ratio > 1f)
            {
                ratio = 1f;
            }

            var mixture = new Mixture();
            foreach (var s in substances)
            {
                var substance = new Substance(s.formula);
                substance.mass = s.mass * ratio;
                s.mass *= (1 - ratio);
                mixture.substances.Add(substance);
            }

            return mixture;
        }

        /// <summary>
        /// We don't actually remove substance now to avoid constant adding and removing
        /// </summary>
        public bool TryRemove(Substance substance)
        {
            return true;
            //if (substances.Count > 0)
            //    return substances.Remove(substance);
            //else
            //    return false;
        }

        public Substance FindSubstance(string formula, bool allowEmpty = false)
        {
            foreach (var s in substances)
            {
                if (formula.Equals(s.formula) && (allowEmpty || s.mass > 0 || s.bornInThisFrame))
                {
                    return s;
                }
            }
            return null;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            int imax = substances.Count - 1;

            if (imax >= 0)
            {
                for (int i = 0; i < imax; i++)
                {
                    if (substances[i].mass > 0)
                    {
                        builder.Append(substances[i].ToString()).Append("+");
                    }
                }
                builder.Append(substances[imax].ToString());
                return builder.ToString();
            }
            else
            {
                return "(空)";
            }
        }

        public string ToRichString()
        {
            StringBuilder builder = new StringBuilder();
            int imax = substances.Count - 1;

            if (imax >= 0)
            {
                for (int i = 0; i < imax; i++)
                {
                    if (substances[i].mass > 0)
                    {
                        builder.Append(substances[i].ToRichString()).Append("+");
                    }
                }
                builder.Append(substances[imax].ToRichString());
                return builder.ToString();
            }
            else
            {
                return "(空)";
            }
        }

        Mixture TakeGas(float mass)
        {
            Mixture mixture = null;
            bool shouldRemoveAir = false;
            Substance substance = null;

            foreach (var s in substances)
            {
                if (s.formula == "Air")
                {
                    substance = new Substance(s.formula);
                    if (s.mass > mass)
                    {
                        substance.mass = mass;
                        s.mass -= mass;
                    }
                    else
                    {
                        substance = s;
                        shouldRemoveAir = true;
                    }

                    mixture = new Mixture();
                    mixture.Add(substance);
                    break;
                }
            }

            if (shouldRemoveAir)
            {
                substances.Remove(substance);
                mixture.Add(Take(mass - substance.mass));
            }

            return mixture;
        }
        #endregion

        #region Private

        ChemixEngine.Phase phase;

        [SerializeField]
        List<Substance> substances = new List<Substance>();

        #endregion
    }
}