using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Instruments
{
    public class InstrumentDetector : MonoBehaviour
    {
        #region Subclasses

        [System.Serializable]
        class ExpectedObject
        {
            public TaskFlow.TaskEvent taskEvent = TaskFlow.TaskEvent.Default;
            public ChemixInstrument.Type type = ChemixInstrument.Type.Default;
        }

        #endregion

        #region Messages

        void Awake()
        {
            if (expectedObjects == null || expectedObjects.Count == 0)
            {
                enabled = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            //if (other.GetComponent<ChemixInstrument>())
            //{
            //    Debug.LogFormat("InsDetector: {0} enter {1}", other.name, name);
            //}
            foreach (var eo in expectedObjects)
            {
                if (Chemix.CheckType(other.gameObject, eo.type))
                {
                    ChemixEventManager.Instance.NotifyChangeState(eo.taskEvent, true);
                    return;
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            //if (other.GetComponent<ChemixInstrument>())
            //{
            //    Debug.LogFormat("InsDetector: {0} exit {1}", other.name, name);
            //}
            foreach (var eo in expectedObjects)
            {
                if (Chemix.CheckType(other.gameObject, eo.type))
                {
                    ChemixEventManager.Instance.NotifyChangeState(eo.taskEvent, false);
                    return;
                }
            }
        }

        #endregion

        #region Private

        [SerializeField]
        List<ExpectedObject> expectedObjects;

        #endregion
    }
}