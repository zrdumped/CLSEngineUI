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

                    if (changeParent)
                    {
                        var il = other.GetComponent<InputLogger>();
                        if (!il)
                        {
                            il = other.gameObject.AddComponent<InputLogger>();
                        }
                        il.previousParent = other.transform.parent;
                        other.transform.parent = transform;
                    }

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

                    if (changeParent)
                    {
                        var p = other.GetComponent<InputLogger>().previousParent;
                        if (p)
                            other.transform.parent = p;
                    }
                    return;
                }
            }
        }

        #endregion

        #region Private

        [SerializeField]
        bool changeParent = false;
        [SerializeField]
        List<ExpectedObject> expectedObjects;

        #endregion
    }
}