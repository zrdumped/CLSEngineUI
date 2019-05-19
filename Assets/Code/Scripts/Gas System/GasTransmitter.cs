using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Gas
{
    public class GasTransmitter : GasReceiver
    {
        #region Methods

        public override bool GetTail(out GasReceiver gasReceiver)
        {
            if (sibling.gasReceivers.Count > 0)
            {
                return sibling.gasReceivers[0].GetTail(out gasReceiver);
            }
            else
            {
                gasReceiver = null;
                return false;
            }
        }

        public void ReceiveGas(Mixture gas)
        {

        }

        #endregion

        #region Messages

        private void Start()
        {
            if (!sibling)
            {
                Debug.LogErrorFormat("GasTransmitter: {0} has no sibling", name);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            var otherReceiver = other.GetComponent<GasReceiver>();
            if (otherReceiver != null)
            {
                gasReceivers.Add(otherReceiver);
            }
        }

        void OnTriggerExit(Collider other)
        {
            var otherReceiver = other.GetComponent<GasReceiver>();
            if (otherReceiver != null)
            {
                gasReceivers.Remove(otherReceiver);
            }
        }

        #endregion

        #region Private

        List<GasReceiver> gasReceivers = new List<GasReceiver>();

        [SerializeField]
        GasTransmitter sibling;

        #endregion
    }
}