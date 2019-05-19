using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Utils
{
    public class TestHelper : BaseSlave
    {
        public override void ReceiveCommand()
        {
            if (!hasRun)
            {
                if (!Chemix.CustomMode)
                {
                    StartCoroutine(StartAutoTest());
                    hasRun = true;
                }
                else
                {
                    Debug.Log("AutoTest: not allowed in custom mode");
                }
            }
            else
            {
                Debug.Log("AutoTest: already begin");
            }
        }

        public void ContinueCoroutine()
        {
            isWaitingForEvent = false;
        }

#if UNITY_EDITOR
        private void Start()
        {
            if (allowControl)
                StartCoroutine(SetupDefaultStates());
        }
#endif

        IEnumerator StartAutoTest()
        {
            yield return new WaitForSeconds(beginLatency);
            foreach (var step in steps)
            {
                if (step.finishOnEvent.taskEvent != TaskFlow.TaskEvent.Default)
                {
                    step.instrument.ReceiveCommand();

                    isWaitingForEvent = true;
                    ChemixEventManager.Instance.On(step.finishOnEvent, ContinueCoroutine);
                    while (isWaitingForEvent)
                    {
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else if (step.instrument)
                {
                    step.instrument.ReceiveCommand();
                    yield return new WaitForSeconds(step.duration * timeMultiplier);
                }
            }
            Debug.LogFormat("AutoTest: Complete");
        }

        IEnumerator SetupDefaultStates()
        {
            yield return 0;

            foreach (var e in experiments)
            {
                if (e.enabled)
                {
                    foreach (var i in e.instruments)
                    {
                        if (i.instrument && i.enabled)
                        {
                            i.instrument.ReceiveCommand();
                        }
                    }
                }
            }
        }

        [System.Serializable]
        public class ExperimentSetup
        {
            [System.Serializable]
            public class InstrumentAndState
            {
                public BaseSlave instrument;
                public bool enabled = true;
            }

            public string name;
            public bool enabled = true;
            public List<InstrumentAndState> instruments;
        }

        [System.Serializable]
        public class TestStep
        {
            public BaseSlave instrument;
            public float duration = 1.0f;
            public TaskFlow.Substep finishOnEvent = null;
        }

        #region Privates

        bool hasRun = false;
        bool isWaitingForEvent = true;

        [Header("Default state")]
#if UNITY_EDITOR
        [SerializeField]
        bool allowControl = false;
#endif
        [SerializeField]
        List<ExperimentSetup> experiments;

        [Header("Auto test")]
        [SerializeField]
        float beginLatency = 0.2f;
        [SerializeField]
        float timeMultiplier = 1.0f;
        [SerializeField]
        List<TestStep> steps;

        #endregion
    }
}