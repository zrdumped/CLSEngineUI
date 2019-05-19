using System.Text.RegularExpressions;
using UnityEngine;
using System;

namespace Chemix
{
    /// <summary>
    /// Chemix is a wrapper for utility functions
    /// </summary>
    public class Chemix
    {
        public static ChemixConfig Config
        {
            get { return ChemixEngine.Instance.Config; }
        }

        public static bool CustomMode
        {
            get { return ChemixEngine.Instance.CustomMode; }
        }

        public static TaskFlow taskFlow
        {
            get
            {
                if (CustomMode)
                {
                    return ChemixEngine.Instance.customTaskFlow;
                }
                else
                {
                    return Config.taskFlowAsset.taskFlow;
                }
            }
        }

#if UNITY_EDITOR
        //private const string k_GuiSize = "<size=9>";
        private readonly static GUIStyle k_GuiStyle = new GUIStyle
        {
            fontSize = 14,
        };
#else
        //private const string k_GuiSize = "<size=12>";
        private readonly static GUIStyle k_GuiStyle = new GUIStyle
        {
            fontSize = 18,
        };
#endif

        public static bool CheckType(GameObject go, ChemixInstrument.Type type)
        {
            if (go)
            {
                var ci = go.GetComponent<ChemixInstrument>();
                if (ci && ci.type == type)
                {
                    return true;
                }
            }
            return false;
        }

        public static void SetLayerRecursively(Transform root, int layer)
        {
            var logger = root.gameObject.GetComponent<InputLogger>();
            if (!logger)
            {
                logger = root.gameObject.AddComponent<InputLogger>();
            }
            logger.previousLayer = root.gameObject.layer;
            root.gameObject.layer = layer;

            foreach (Transform child in root)
            {
                SetLayerRecursively(child, layer);
            }
        }

        public static void RestoreLayerRecursively(Transform root)
        {
            var logger = root.gameObject.GetComponent<InputLogger>();
            root.gameObject.layer = logger.previousLayer;

            foreach (Transform child in root)
            {
                RestoreLayerRecursively(child);
            }
        }

        public static string InsertSubscriptTag(string raw)
        {
            return Regex.Replace(raw, @"\d", delegate (System.Text.RegularExpressions.Match match)
            {
                return "<sub>" + match.ToString() + "</sub>";
            });
        }

        static public void DrawTextOnTransform(Transform trans, string text, float labelOffset = 0)
        {
            DrawTextOnTransform(trans, text, Color.black, labelOffset);
        }

        static public void DrawTextOnTransform(Transform trans, string text, Color color, float labelOffset = 0)
        {
            var worldPosition = new Vector3(trans.position.x, trans.position.y + labelOffset, trans.position.z);
            var viewportPoint = ChemixEngine.Instance.mainCamera.WorldToViewportPoint(worldPosition);

            if (viewportPoint.z > 0)
            {
                var screenPosition = new Vector2(viewportPoint.x * Screen.width, Screen.height * (1 - viewportPoint.y));
                Vector2 textSize = GUI.skin.label.CalcSize(new GUIContent(text));
                Rect rect = new Rect(screenPosition.x - (textSize.x / 2), screenPosition.y - textSize.y, textSize.x, textSize.y);

                GUI.color = color;
                GUI.Label(rect, text, k_GuiStyle);
            }
        }
    }
}