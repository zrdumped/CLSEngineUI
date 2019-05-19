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
            }
        }

        void OnTriggerExit(Collider other)
        {
            var burner = other.GetComponent<Burner>();
            if (burner)
            {
                burner.hasLid = false;
            }
        }
    }
}