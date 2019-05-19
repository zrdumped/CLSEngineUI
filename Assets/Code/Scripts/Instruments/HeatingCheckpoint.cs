using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Instruments
{
    public class HeatingCheckpoint : MonoBehaviour, IHeatableObject
    {
        bool heated = false;

        public void SetIsHeating(bool isHeating)
        {
            if (isHeating && !heated)
            {
                if (ChemixEventManager.Instance.TriggerEvent(TaskFlow.TaskEvent.HeatingCheckpoint))
                {
                    //Debug.Log("<color=green>HeatingCheckpoint</color>: heated");
                    heated = true;
                }
            }
        }
    }
}