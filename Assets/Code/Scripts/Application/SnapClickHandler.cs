using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    public class SnapClickHandler : BaseClickHandler
    {
        [System.Serializable]
        public class TypeAndBehaviour
        {
            public ChemixInstrument.Type expectedType;
            public Transform snapTransform;
        }

        [SerializeField]
        protected List<TypeAndBehaviour> expectedTypes;

        protected override bool isExpecting(GameObject go)
        {
            foreach (var ins in expectedTypes)
            {
                if (Chemix.CheckType(go, ins.expectedType))
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool HandleClick(GameObject pawn)
        {
            foreach (var ins in expectedTypes)
            {
                if (Chemix.CheckType(pawn, ins.expectedType))
                {
                    //pawn.GetComponent<InputLogger>().previousParent = pawn.transform.parent;
                    //pawn.transform.parent = ins.snapTransform;
                    StartCoroutine(LerpToTarget(pawn.transform, ins.snapTransform));
                    return true;
                }
            }
            return true;
        }

        IEnumerator LerpToTarget(Transform pawn, Transform target)
        {
            var initialPosition = pawn.position;
            var initialRotation = pawn.rotation;
            float progress = 0;

            while (progress < 1f)
            {
                progress += Time.deltaTime / lerpDuration;
                pawn.position = Vector3.Lerp(initialPosition, target.position, progress);
                pawn.rotation = Quaternion.Lerp(initialRotation, target.rotation, progress);
                yield return null;
            }
        }

        [SerializeField]
        private float lerpDuration = 0.5f;
    }
}
