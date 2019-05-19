using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Gas
{
    [RequireComponent(typeof(ParticleSystem))]
    public class Bubble : MonoBehaviour
    {
        public GasSource Target
        {
            get;
            private set;
        }

        public void ToggleAnimation(bool enabled)
        {
            if (enabled)
            {
                bubbleEffect.Play();
            }
            else
            {
                bubbleEffect.Stop();
            }
        }

        private void Start()
        {
            bubbleEffect = GetComponent<ParticleSystem>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var gasSource = other.GetComponent<GasSource>();
            if (gasSource)
            {
                Target = gasSource;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var gasSource = other.GetComponent<GasSource>();
            if (gasSource == Target)
            {
                Target = null;
            }
        }

        ParticleSystem bubbleEffect;
    }
}