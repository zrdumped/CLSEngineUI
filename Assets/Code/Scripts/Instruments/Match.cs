using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Instruments
{
    // ChemixMatch
    // functionality: yes
    // test: no
    public class Match : BaseBurnable
    {
        public float putOutDelay = 2f;

        public override bool OnFire
        {
            get
            {
                return onFire;
            }

            set
            {
                base.OnFire = value;

                if (value)
                {
                    StopCoroutine("WaitAndPutOutFire");
                    StartCoroutine("WaitAndPutOutFire");
                }
            }
        }

        IEnumerator WaitAndPutOutFire()
        {
            yield return new WaitForSeconds(putOutDelay);
            OnFire = false;
            //Debug.Log("ChemixMatch: put off");
        }
    }
}