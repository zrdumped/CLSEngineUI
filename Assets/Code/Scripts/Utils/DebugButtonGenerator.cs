using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Chemix.UI;

namespace Chemix.Utils
{
    public class DebugButtonGenerator : MonoBehaviour
    {
        #region subclassess
        public enum ButtonType
        {
            // OnClick will trigger certain event
            Event,
            // OnClick will trigger IStateful.SetState(state)
            Stateful,
        }

        [System.Serializable]
        public class DebugButton
        {
            public string text;
            public BaseSlave slave;

            public void handleOnClick()
            {
                if (slave)
                {
                    slave.ReceiveCommand();
                }
            }
        }
        #endregion

        public Color eventButtonColor = new Color(1f, 0.8862f, 0);
        public List<DebugButton> debugButtons;

        void Start()
        {
            if (Chemix.CustomMode)
            {
                return;
            }

            foreach (var btn in debugButtons)
            {
                CreateBtn(btn);
            }
        }

        void CreateBtn(DebugButton btn)
        {
            GameObject go = Instantiate(UIManager.Instance.debugButtonPrefab);
            go.transform.SetParent(this.transform);
            go.transform.localScale = new Vector3(1f, 1f, 1f);

            go.name = "[Btn] " + btn.text;
            go.GetComponentInChildren<Text>().text = btn.text;

            var button = go.GetComponent<Button>();
            button.onClick.AddListener(btn.handleOnClick);
            //switch (btn.type)
            //{
            //    case ButtonType.Event:
            //        go.GetComponent<Image>().color = eventButtonColor;
            //        break;
            //}
        }
    }
}