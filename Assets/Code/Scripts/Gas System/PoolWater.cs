using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Gas
{
    public class PoolWater : GasReceiver, IGasBlock
    {
        #region Methods

        public override bool GetTail(out GasReceiver gasReceiver)
        {
            gasReceiver = this;
            return true;
        }

        public void ReceiveGas(Mixture gas)
        {
            if (gas.TotalMass > 0.0001f)
            {
                if (gasTransmitter)
                {
                    bubble.transform.position = gasTransmitter.transform.position;
                }
                else
                {
                    Debug.LogWarning("PoolWater: receive gas without gas transmitter.");
                }

                if (bubble.Target)
                    bubble.Target.ReceiveBubble(gas);

                deadline = Time.time + interval;
                //multiplier = Mathf.Max(1, gas.TotalMass / 0.001f);
                SetBubbleState(true);
            }
        }

        #endregion

        #region Messages

        private void Start()
        {
            bubble = Instantiate(bubblePrefab, transform).GetComponent<Bubble>();
        }

        private void FixedUpdate()
        {
            if (deadline > 0 && Time.time > deadline)
            {
                SetBubbleState(false);
                deadline = 0;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherTransmitter = other.GetComponent<GasTransmitter>();
            if (otherTransmitter)
            {
                gasTransmitter = otherTransmitter;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var otherTransmitter = other.GetComponent<GasTransmitter>();
            if (otherTransmitter == gasTransmitter)
            {
                gasTransmitter = null;
            }
        }

        #endregion

        #region Privates

        void SetBubbleState(bool state)
        {
            if (state != lastState)
            {
                ChemixEventManager.Instance.NotifyChangeState(TaskFlow.TaskEvent.BubbleVisible, state);

                if (state)
                {
                    //Debug.Log("PoolWater: play bubble");
                    bubble.ToggleAnimation(true);

                    //var emission = currentBubble.emission;
                    //emission.rateOverTimeMultiplier = multiplier;
                }
                else
                {
                    if (bubble)
                    {
                        bubble.ToggleAnimation(false);
                        //Debug.Log("PoolWater: stop bubble");
                    }
                }
                lastState = state;
            }
        }

        bool lastState = false;
        float deadline = 0;
        Bubble bubble;
        GasTransmitter gasTransmitter;

        [SerializeField]
        GameObject bubblePrefab;
        [SerializeField]
        float interval = 0.5f;

        #endregion
    }
}