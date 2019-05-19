using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.UI
{
    public class FormulaLabel : UIController
    {
        public ChemixObject owner
        {
            get;
            set;
        }

        private TMPro.TextMeshProUGUI m_Text;

        private void Awake()
        {
            m_Text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            if (!owner)
            {
                Destroy(gameObject);
                return;
            }

            if (!owner.System.IsOwner(owner) || !Chemix.Config.enableLabel)
            {
                m_Text.text = "";
            }
            else
            {
                m_Text.text = owner.System.ToRichString();
            }
        }
    }
}