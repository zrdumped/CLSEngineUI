using System.Text;
using UnityEngine;

namespace Chemix
{
    [System.Serializable]
    public class Substance : IRichText
    {
        public string richFormula
        {
            get
            {
                if (m_FormulaForPrint == null)
                {
                    m_FormulaForPrint = Chemix.InsertSubscriptTag(formula);
                }
                return m_FormulaForPrint;
            }
        }

        public bool isProduct
        {
            get
            {
                return mass <= 0f;
            }
        }

        public bool bornInThisFrame
        {
            get { return m_BornFrame == Time.frameCount; }
        }

        public string formula = "Unknown";
        public float mass = 0f;

        private string m_FormulaForPrint;
        private int m_BornFrame;

        public Substance(string formula, float mass = 0)
        {
            this.formula = formula;
            this.mass = mass;
            m_BornFrame = Time.frameCount;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[").Append(formula).Append("]").Append(mass.ToString("0.00"));
            return builder.ToString();
        }

        public string ToRichString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("[").Append(richFormula).Append("]").Append(mass.ToString("0.00"));
            return builder.ToString();
        }
    }
}