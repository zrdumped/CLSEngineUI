using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [CreateAssetMenu(fileName = "NewChemixConfig", menuName = "Chemix/Config", order = 3)]
    public class ChemixConfig : ScriptableObject
    {
        public float MinMultiplierSquare { get; }

        public float MaxMultiplierSquare { get; }

        [Header("Simulation")]
        public bool filterEquation = false;
        public float globalReactionRate = 0.4f;

        // for curve reaction rate
        public float middleMass = 0.25f;
        public float minMultiplier = 0.5f;
        public float maxMultiplier = 2.0f;

        [Header("Gas System")]
        public bool enableGasSystem = true;
        public float gasTransmitRate = 0.15f;
        public float airConstant = 0.1f;

        [Header("Task System")]
        public TaskFlowAsset taskFlowAsset;
        public int tipAnimCycle = 4; // letter animation is displayed every `animCycle` frames
        public int tipInitialStepIndex = 0;

        [Header("Miscellaneous")]
        public bool enableMouseControl = false;
        public bool enableLabel = true;

        public ChemixConfig()
        {
            MinMultiplierSquare = minMultiplier * minMultiplier;
            MaxMultiplierSquare = maxMultiplier * maxMultiplier;
        }
    }
}