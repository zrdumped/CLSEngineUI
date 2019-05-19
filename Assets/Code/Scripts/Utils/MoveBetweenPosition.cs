using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Utils
{
    public class MoveBetweenPosition : BaseSlave
    {
        public float Hermite(float value)
        {
            return value * value * (3.0f - 2.0f * value);
        }

        public override void ReceiveCommand()
        {
            StartMovingToAnother();
        }

        public void StartMovingToAnother()
        {
            initial2target = !initial2target;
            lerpPercent = lerpPercent >= 1f ? 0 : 1f - lerpPercent;

            if (initial2target)
            {
                if (initialEvent)
                {
                    initialEvent.onLeave.Invoke();
                }
            }
            else
            {
                if (targetEvent)
                {
                    targetEvent.onLeave.Invoke();
                }
            }
        }

        void Start()
        {
            initial = new GameObject();
            initial.transform.parent = transform.parent;
            initial.name = "Initial - " + this.name;
            initial.transform.localPosition = transform.localPosition;
            initial.transform.localRotation = transform.localRotation;
            lerpPercent = 1f;

            targetEvent = target.GetComponent<SplineEvent>();
            initialEvent = GetComponent<SplineEvent>();
        }

        void Update()
        {
            if (lerpPercent >= 1f || !target || !initial)
            {
                return;
            }

            lerpPercent += Time.deltaTime / lerpTime;
            float actualLerp = Hermite(lerpPercent);

            if (initial2target)
            {
                transform.position = Vector3.Lerp(initial.transform.position, target.transform.position, actualLerp);
                transform.rotation = Quaternion.Lerp(initial.transform.rotation, target.transform.rotation, actualLerp);
            }
            else
            {
                transform.position = Vector3.Lerp(target.transform.position, initial.transform.position, actualLerp);
                transform.rotation = Quaternion.Lerp(target.transform.rotation, initial.transform.rotation, actualLerp);
            }

            if (lerpPercent >= 1f)
            {
                if (initial2target)
                {
                    if (targetEvent)
                    {
                        targetEvent.onArrive.Invoke();
                    }
                }
                else
                {
                    if (initialEvent)
                    {
                        initialEvent.onArrive.Invoke();
                    }
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            if (target)
            {
                Gizmos.DrawSphere(target.transform.position, 0.05f);
            }
        }

        #region Privates

        bool initial2target = false;
        float lerpPercent = 0;
        SplineEvent targetEvent, initialEvent;
        GameObject initial;

        [SerializeField]
        GameObject target = null;
        [SerializeField]
        float lerpTime = 0.6f;

        #endregion
    }
}