using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.UI
{
    public class UIController : MonoBehaviour
    {
        public Transform followTarget
        {
            get;
            set;
        } = null;

        [SerializeField]
        private float m_OffsetY = 0.3f;

        public void ToggleOpen()
        {
            var animator = GetComponent<Animator>();
            bool isOpen = animator.GetBool("isOpen");
            animator.SetBool("isOpen", !isOpen);
        }

        protected virtual void Update()
        {
            if (followTarget)
            {
                Vector3 worldPosition = followTarget.position;
                worldPosition.y += m_OffsetY;
                var screenPoint = ChemixEngine.Instance.mainCamera.WorldToScreenPoint(worldPosition);
                transform.position = screenPoint;
            }
        }
    }
}