﻿#if UNITY_EDITOR
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
            if (GUILayout.Button("接口测试"))
            {
                nm.TestInterface();
            }
            if (GUILayout.Button("注册"))
            {
                nm.Signup();
            }
            if (GUILayout.Button("保存"))
            {
                nm.SaveData();
            }
            if (GUILayout.Button("加载"))
            {
                nm.LoadData();
            }
            if (GUILayout.Button("邀请码"))
            {
                nm.Invite();
            }
            if (GUILayout.Button("序列化测试"))
            {
                nm.TestSerialization();
            }
        }
    }
}
#endif