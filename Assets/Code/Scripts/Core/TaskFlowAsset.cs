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

        static string[] taskEventChineses = new string[]
        {
            "默认",
            "预热检查",
            "显示气泡",
            "酒精灯点燃",
            "棉花在试管中",
            "导管连接试管",
            "盖玻片封闭集气瓶",
            "集气瓶在水槽中",
            "已添加药品",
            "集气完成",
        };

        public enum EventType
        {
            Normal,
            StateTrue,
            StateFalse,
        }

        [System.Serializable]
        public class EventInfo
        {
            public string chineseName;
            public TaskEvent taskEvent;
            public bool eventOrCondition;
        }
        #endregion

        // Intermodule
        public static List<EventInfo> GetAllEventInfos()
        {
            List<EventInfo> result = new List<EventInfo>();
            var eventNames = System.Enum.GetValues(typeof(TaskEvent));
            foreach (TaskEvent te in eventNames)
            {
                EventInfo item = new EventInfo();
                item.chineseName = taskEventChineses[(int)te];
                item.taskEvent = te;
                item.eventOrCondition = ((int)te) < 2; // dangerous
                result.Add(item);
            }
            return result;
        }

        public string title = "Unknown";
        public List<Step> steps = new List<Step>();
        public string completeMessage = "Congratulation!";
    }
}