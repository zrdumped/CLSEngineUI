using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Chemix/Chemix Liquid Object")]
    public class ChemixLiquidObject : ChemixObject
    {
        protected override void Awake()
        {
            base.Awake();
            mixture.Phase = ChemixEngine.Phase.Liquid;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            var liquid = other.GetComponent<ChemixLiquidObject>();
            if (liquid)
            {
                if (liquid.mergeOnCollide)
                {
                    //Debug.LogFormat("ChemixLiquid: {1} enter {0}", this.name, other.name);
                    // merge two liquid
                    if (!liquid.system.IsOwner(liquid))
                    {
                        liquid.system.Remove(liquid);
                    }
                    AddAndUpdate(liquid.mixture);
                    Destroy(liquid.gameObject);
                }
            }
            else
            {
                base.OnTriggerEnter(other);
            }
        }

        [Header("Liquid")]
        [SerializeField]
        bool mergeOnCollide = false;
        // TODO: use volumn
        //[SerializeField]
        //float volumn = 1f;
    }
}