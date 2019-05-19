using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    public abstract class BaseClickHandler : MonoBehaviour
    {
        protected abstract bool isExpecting(GameObject go);

        protected abstract bool HandleClick(GameObject pawn);

        private void Start()
        {
            //gameObject.layer = InputController.Instance.expectedLayer;
        }

        public bool OnClick(GameObject pawn) // Todo: Rename: TryClick
        {
            if (isExpecting(InputController.Instance.currentPawn))
            {
                return HandleClick(pawn);
            }
            return true;
        }

        private void OnMouseEnter()
        {
            if (isExpecting(InputController.Instance.currentPawn))
            {
                UI.UIManager.Instance.DisplayFocus(gameObject);
                InputController.Instance.handler = this;
            }
            else
            {
                if (m_AlwaysShowFocus)
                {
                    UI.UIManager.Instance.DisplayFocus(gameObject);
                }
            }
        }

        private void OnMouseExit()
        {
            UI.UIManager.Instance.HideFocus(gameObject);
            if (InputController.Instance.handler == this)
                InputController.Instance.handler = null;
        }

        [SerializeField]
        private bool m_AlwaysShowFocus = false;
    }
}