using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Chemix
{
    /// <summary>
    /// InstrumentManager is a wrapper class for EventManager.
    /// </summary>
    public class ChemixEventManager : Singleton<ChemixEventManager>
    {
        #region Methods

        public bool GetState(TaskFlow.TaskEvent taskEvent)
        {
            bool state = false;
            return m_StateDictionary.TryGetValue(taskEvent, out state) && state;
        }

        public void On(TaskFlow.Substep substep, UnityAction action)
        {
            switch (substep.eventType)
            {
                case TaskFlow.EventType.Normal:
                    m_EventManager.StartListening(substep.taskEvent.ToString(), action);
                    break;
                case TaskFlow.EventType.StateTrue:
                    if (GetState(substep.taskEvent))
                    {
                        action.Invoke();
                    }
                    else
                    {
                        m_EventManager.StartListening(substep.taskEvent + "True", action);
                    }
                    break;
                case TaskFlow.EventType.StateFalse:
                    if (!GetState(substep.taskEvent))
                    {
                        action.Invoke();
                    }
                    else
                    {
                        m_EventManager.StartListening(substep.taskEvent + "False", action);
                    }
                    break;
            }
        }

        public void On(string eventName, UnityAction action)
        {
            m_EventManager.StartListening(eventName, action);
        }

        public void Off(TaskFlow.Substep substep, UnityAction action)
        {
            switch (substep.eventType)
            {
                case TaskFlow.EventType.Normal:
                    m_EventManager.StopListening(substep.taskEvent.ToString(), action);
                    break;
                case TaskFlow.EventType.StateTrue:
                    m_EventManager.StopListening(substep.taskEvent + "True", action);
                    break;
                case TaskFlow.EventType.StateFalse:
                    m_EventManager.StopListening(substep.taskEvent + "False", action);
                    break;
            }
        }

        public void Off(string eventName, UnityAction action)
        {
            m_EventManager.StopListening(eventName, action);
        }

        public void NotifyChangeState(TaskFlow.TaskEvent taskEvent, bool state)
        {
            bool value;
            m_StateDictionary.TryGetValue(taskEvent, out value);
            if (value == state)
            {
                return;
            }

            m_StateDictionary[taskEvent] = state;
            m_EventManager.TriggerEvent(taskEvent.ToString() + state);
            Debug.LogFormat("ChemixEvent: Set {0} to {1}", taskEvent, state);
        }

        public bool TriggerEvent(TaskFlow.TaskEvent taskEvent)
        {
            bool hasListener = m_EventManager.TriggerEvent(taskEvent.ToString());
            //if (hasListener)
            //{
            //    Debug.LogFormat("InstrumentManager: TriggerEvent [{0}]", eventName);
            //}
            //else
            //{
            //    Debug.LogFormat("InstrumentManager: TriggerEventSilently [{0}]", eventName);
            //}
            return hasListener;
        }

        public void TriggerEventForTest(string eventName)
        {
            m_EventManager.TriggerEvent(eventName);
        }

        #endregion

        #region Private

        EventManager m_EventManager = new EventManager();

        Dictionary<TaskFlow.TaskEvent, bool> m_StateDictionary = new Dictionary<TaskFlow.TaskEvent, bool>();

        #endregion
    }
}