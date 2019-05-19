using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    public class BaseBurnable : BaseSlave, IHeatableObject
    {
        #region variables
        public ParticleSystem flame;

        protected bool onFire = false;

        List<IHeatableObject> collidingObjects = new List<IHeatableObject>();

        public virtual bool OnFire
        {
            get
            {
                return onFire;
            }

            set
            {
                if (onFire != value && collidingObjects.Count > 0)
                {
                    foreach (var h in collidingObjects)
                    {
                        if (h != null)
                        {
                            h.SetIsHeating(value);
                        }
                    }
                }
                onFire = value;

                if (value)
                {
                    flame.Play();
                }
                else
                {
                    flame.Stop();
                }
            }
        }
        #endregion

        public override void ReceiveCommand()
        {
            OnFire = true;
        }

        public void SetIsHeating(bool isHeated)
        {
            if (isHeated)
                OnFire = true;
        }

        void Start()
        {
            if (flame == null)
            {
                Debug.LogWarning("BaseBurnable: no flame");
                enabled = false;
                return;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var heatable = other.GetComponent<IHeatableObject>();
            if (heatable != null)
            {
                if (onFire)
                {
                    heatable.SetIsHeating(true);
                }
                collidingObjects.Add(heatable);
            }
        }

        void OnTriggerExit(Collider other)
        {
            var heatable = other.GetComponent<IHeatableObject>();
            if (heatable != null)
            {
                if (onFire)
                {
                    heatable.SetIsHeating(false);
                }
                collidingObjects.Remove(heatable);
            }
        }
    }
}