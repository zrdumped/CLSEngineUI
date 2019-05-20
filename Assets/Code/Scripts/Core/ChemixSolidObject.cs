using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Chemix/Chemix Solid Object")]
    public class ChemixSolidObject : ChemixObject
    {
        public bool CanSpoonCollect
        {
            get { return canSpoonCollect; }
        }

        protected override void Start()
        {
            base.Start();

            if (enableResize)
            {
                initialScale = transform.localScale.x;
                initialMass = TotalMass;
            }
            mixture.Phase = ChemixEngine.Phase.Solid;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (enableResize)
            {
                float totalMass = TotalMass;

                if (totalMass > 0)
                {
                    float percent = totalMass / initialMass * initialScale;
                    transform.localScale = new Vector3(percent, percent, percent);
                }
                else if (initialMass > 0)
                {
                    system.Remove(this);
                    Destroy(this.gameObject);
                }
            }
        }

        float initialScale = 0;
        float initialMass = 0;

        [Header("Solid")]
        [SerializeField]
        bool enableResize = false;
        [SerializeField]
        bool canSpoonCollect = false;
    }
}