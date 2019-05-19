using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Instruments
{
    // ChemixBurner
    // functionality: yes
    // test: no
    public class Burner : BaseBurnable
    {
        #region variables
        [HideInInspector]
        public bool hasLid;

        public override bool OnFire
        {
            get
            {
                return onFire;
            }

            set
            {
                if (value && hasLid)
                {
                    return;
                }

                if (onFire != value)
                {
                    ChemixEventManager.Instance.NotifyChangeState(TaskFlow.TaskEvent.BurnerOnFire, value);
                }

                base.OnFire = value;
            }
        }
        #endregion
    }
}