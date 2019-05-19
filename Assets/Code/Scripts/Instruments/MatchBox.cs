using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Instruments
{
    // ChemixMatch
    // functionality: yes
    // test: no
    public class MatchBox : MonoBehaviour
    {
        public float sensitivity = 0.4f;

        float enterTime;
        Vector3 enterPos;

        void OnTriggerEnter(Collider other)
        {
            var match = other.GetComponent<Match>();
            if (match)
            {
                enterTime = Time.time;
                enterPos = other.transform.position;
            }
        }

        void OnTriggerExit(Collider other)
        {
            var match = other.GetComponent<Match>();
            if (match)
            {
                float dx = (other.transform.position - enterPos).magnitude;
                float dt = Time.time - enterTime;
                if (dx / dt > sensitivity)
                {
                    match.OnFire = true;
                }
            }
        }
    }
}