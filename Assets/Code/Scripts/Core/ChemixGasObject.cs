using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [DisallowMultipleComponent]
    [AddComponentMenu("Chemix/Chemix Gas Object")]
    public class ChemixGasObject : ChemixObject
    {
        public static float AirPressure
        {
            get { return Chemix.Config.airConstant / Chemix.Config.gasTransmitRate; }
        }

        public float Volumn
        {
            get { return volumn; }
        }

        public float Pressure
        {
            get { return TotalMass / volumn * (IsHeating ? 2 : 1); }
        }

        public float StableAirMass
        {
            get { return AirPressure * volumn; }
        }

        public bool IsHeating
        {
            get { return system.IsHeating; }
        }

        protected override void Awake()
        {
            base.Awake();
            mixture.Phase = ChemixEngine.Phase.Gas;

            if (volumn <= 0)
            {
                Debug.LogError("ChemixGasObject: volumn <= 0");
            }
        }

        protected override void Start()
        {
            base.Start();

            if (fillWithAir && Chemix.Config.enableGasSystem)
            {
                mixture.Add(new Mixture("Air", StableAirMass));
            }
        }

        [Header("Gas")]
        [SerializeField]
        float volumn = 1f;
        [SerializeField]
        bool fillWithAir = false;
    }
}