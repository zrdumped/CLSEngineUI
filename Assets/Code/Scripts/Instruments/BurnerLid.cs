using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Instruments
{
    // ChemixBurnerLid
    // test: no
    public class BurnerLid : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            var burner = other.GetComponent<Burner>();
            if (burner)
            {
                burner.OnFire = false;
                burner.hasLid = true;

                var il = GetComponent<InputLogger>();
                if (!il)
                {
                    il = gameObject.AddComponent<InputLogger>();
                }
                il.previousParent = transform.parent;
                transform.parent = other.transform.parent;
            }
        }

        void OnTriggerExit(Collider other)
        {
            var burner = other.GetComponent<Burner>();
            if (burner)
            {
                burner.hasLid = false;
                
                var p = GetComponent<InputLogger>().previousParent;
                if (p)
                    transform.parent = p;
            }
        }
    }
}