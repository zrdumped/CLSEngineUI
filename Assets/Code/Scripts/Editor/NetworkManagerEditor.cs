#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Chemix.Network
{
    [CustomEditor(typeof(NetworkManager))]
    public class NetworkManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            NetworkManager nm = (NetworkManager)target;
            if (GUILayout.Button("Ping"))
            {
                nm.Ping();
            }
            if (GUILayout.Button("Interface"))
            {
                nm.TestInterface();
            }
            //if (GUILayout.Button("Login"))
            //{
            //    nm.Login();
            //}
            //if (GUILayout.Button("Save"))
            //{
            //    nm.SaveData();
            //}
            //if (GUILayout.Button("Load"))
            //{
            //    nm.LoadData();
            //}
        }
    }
}
#endif