using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Gas
{
    [RequireComponent(typeof(ChemixGasObject))]
    public class GasSource : GasReceiver
    {
        public enum Status
        {
            Blocked,
            WithAir,
            WithGasSource,
            WithPool,
        }
        #region Methods

        public override bool GetTail(out GasReceiver gasReceiver)
        {
            gasReceiver = this;
            return true;
        }

        public void ReceiveBubble(Mixture gas)
        {
            if (gasObject.Pressure < ChemixGasObject.AirPressure)
            {
                gasObject.AddAndUpdate(gas);
            }
        }

        #endregion

        #region Messages

        private void Start()
        {
            gasObject = GetComponent<ChemixGasObject>();

            if (!Chemix.Config.enableGasSystem)
            {
                enabled = false;
            }

            // eliminate warning
            switch (status)
            {
                default:
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var otherBlock = other.GetComponent<IGasBlock>();
            if (otherBlock != null)
            {
                gasBlocks.Add(otherBlock);

                // Corner case: Remove air from mixture if there are only a little bit of air
                if (gasObject.Mixture.IsAir && gasObject.Mixture.TotalMass < 0.01f)
                {
                    gasObject.Mixture.Take(gasObject.Mixture.TotalMass);
                }
            }

            var otherTransmitter = other.GetComponent<GasTransmitter>();
            if (otherTransmitter)
            {
                gasTransmitters.Add(otherTransmitter);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var otherBlock = other.GetComponent<IGasBlock>();
            gasBlocks.Remove(otherBlock);

            var otherTransmitter = other.GetComponent<GasTransmitter>();
            if (otherTransmitter)
            {
                gasTransmitters.Remove(otherTransmitter);
            }
        }

        private void FixedUpdate()
        {
            GasReceiver tail = null;
            bool hasTransmitter = (gasTransmitters.Count > 0);
            bool isAir;

            isAir = hasTransmitter ? !gasTransmitters[gasTransmitters.Count - 1].GetTail(out tail) : true;

            if (isAir)
            {
                if (gasBlocks.Count == 0)
                {
                    status = Status.WithAir;
                    BalanceGasWithAir(gasObject);
                }
                else
                {
                    status = Status.Blocked;
                }
            }
            else
            {
                if (tail is GasSource)
                {
                    status = Status.WithGasSource;
                    var gasSource = (GasSource)tail;
                    if (GetInstanceID() > gasSource.GetInstanceID())
                    {
                        BalanceGas(gasObject, gasSource.gasObject);
                    }
                }
                else if (tail is PoolWater)
                {
                    status = Status.WithPool;
                    var poolWater = (PoolWater)tail;
                    if (gasObject.Pressure > ChemixGasObject.AirPressure)
                    {
                        poolWater.ReceiveGas(ExtractGas());
                    }
                }
                else
                {
                    Debug.LogWarningFormat("GasSource: {0} is not expected to be tail", tail.name);
                }
            }
        }

        #endregion

        #region Privates

        Mixture ExtractGas(bool takeAirFirst = true)
        {
            float deltaMass = Chemix.Config.gasTransmitRate * Time.fixedDeltaTime * gasObject.Pressure;
            return gasObject.Mixture.Take(deltaMass, takeAirFirst);
        }

        void BalanceGas(ChemixGasObject g1, ChemixGasObject g2)
        {
            if (g1.Pressure > g2.Pressure)
            {
                float deltaMass = Chemix.Config.gasTransmitRate * Time.fixedDeltaTime * (g1.Pressure - g2.Pressure);
                g2.AddAndUpdate(g1.Mixture.Take(deltaMass));
            }
            else
            {
                float deltaMass = Chemix.Config.gasTransmitRate * Time.fixedDeltaTime * (g2.Pressure - g1.Pressure);
                g1.AddAndUpdate(g2.Mixture.Take(deltaMass));
            }
        }

        void BalanceGasWithAir(ChemixGasObject g1)
        {
            ExtractGas(false);
            g1.AddAndUpdate(Mixture.Air);
        }

        ChemixGasObject gasObject;
        List<IGasBlock> gasBlocks = new List<IGasBlock>();
        List<GasTransmitter> gasTransmitters = new List<GasTransmitter>();
        Status status;

        #endregion
    }
}