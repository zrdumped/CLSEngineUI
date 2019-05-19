using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    public class ChemixInstrument : MonoBehaviour
    {
        public enum Type
        {
            Default,
            Cotton,
            Spoon, 
            DeliveryTube,
            Match,
            BurnerLid,
            Fe,
            GlassCover,
            Burner,
            GasJar,
            Dropper,
        }

        public Type type
        {
            get { return m_Type; }
        }

        private void Start()
        {
            if (m_CanBePossessed)
            {
                gameObject.layer = InputController.Instance.expectedLayer;
            }
        }
        
        public bool TryUnpossess()
        {
            return true;
        }

        private void OnMouseEnter()
        {
            if (InputController.Instance.currentPawn == null)
            {
                UI.UIManager.Instance.DisplayFocus(gameObject);
            }
        }

        private void OnMouseExit()
        {
            UI.UIManager.Instance.HideFocus(gameObject);
        }
        
        [SerializeField]
        private bool m_CanBePossessed = true;
        [SerializeField]
        private Type m_Type = Type.Default;
    }
}