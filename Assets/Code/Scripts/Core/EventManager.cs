using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Chemix
{
    public class EventManager
    {
        private Dictionary<string, UnityEvent> name2event = new Dictionary<string, UnityEvent>();

        private Dictionary<string, int> name2count = new Dictionary<string, int>();

        public void StartListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (name2event.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);

                int count;
                name2count.TryGetValue(eventName, out count);
                name2count[eventName] = count + 1;
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);

                name2event.Add(eventName, thisEvent);
                name2count.Add(eventName, 1);
            }
        }

        public void StopListening(string eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (name2event.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);

                int count;
                name2count.TryGetValue(eventName, out count);
                name2count[eventName] = count - 1;
            }
        }

        public bool TriggerEvent(string eventName)
        {
            UnityEvent thisEvent = null;
            if (name2event.TryGetValue(eventName, out thisEvent))
            {
                int count;
                name2count.TryGetValue(eventName, out count);

                thisEvent.Invoke();

                if (count > 0)
                    return true;
            }
            return false;
        }
    }
}