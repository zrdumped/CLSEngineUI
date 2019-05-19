using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    [CreateAssetMenu(fileName = "NewTaskFlow", menuName = "Chemix/TaskFlow", order = 3)]
    public class TaskFlowAsset : ScriptableObject
    {
        public TaskFlow taskFlow;
    }

    [System.Serializable]
    public class TaskFlow
    {
        #region subclasses
        [System.Serializable]
        public class Step
        {
            public string detail = "Do a big thing";
            public List<Substep> substeps = new List<Substep>() { new Substep() };

            public string GetSubstepsAsString(int stepIndex)
            {
                string s = "";

                int imax = substeps.Count - 1;
                for (int i = 0; i < imax; i++)
                {
                    s += substeps[i].detail + ", ";
                }
                s += substeps[imax].detail;

                return s;
            }
        }

        [System.Serializable]
        public class Substep
        {
            public string detail = "Substep";
            public TaskEvent taskEvent;
            public EventType eventType;
        }

        public enum TaskEvent
        {
            // One-time Events
            Default,
            HeatingCheckpoint,

            // State Events
            BubbleVisible,
            BurnerOnFire,
            CottonInPosition,
            DeliveryTubeInPosition,
            GlassCoverInPosition,
            GasJarInPosition,
            MedicineInPosition,
            CollectingComplete,
        }

        public enum EventType
        {
            Normal,
            StateTrue,
            StateFalse,
        }

        public class EventInfo
        {
            public string chineseName;
            public TaskEvent taskEvent;
            public bool eventOrCondition;
        }
        #endregion

        // Intermodule
        static List<EventInfo> GetAllEventInfos()
        {
            List<EventInfo> result = new List<EventInfo>();
            string[] eventNames = System.Enum.GetNames(typeof(TaskEvent));
            for (int i = 0; i < eventNames.Length; i++)
            {
                EventInfo item = new EventInfo();
                item.chineseName = eventNames[i];
                item.taskEvent = (TaskEvent)i;
                item.eventOrCondition = i < 2; // dangerous
                result.Add(item);
            }
            return result;
        }

        public string title = "Unknown";
        public List<Step> steps = new List<Step>();
        public string completeMessage = "Congratulation!";
    }
}