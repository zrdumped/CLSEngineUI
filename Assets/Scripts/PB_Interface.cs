using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System.IO;

namespace PB
{
    public class PB_Interface : MonoBehaviour
    {
        private PB_Msg.Command cmd;
        private PB_Msg.Command result;
        private MemoryStream ms;
        private PB_TCP socket;

        private List<GameObject> tracedObjects;
        private List<float> res;
        private int tracedNum;


        public bool testing = true;
        public GameObject traceObj;

        // Use this for initialization
        void Start()
        {
            socket = this.GetComponent<PB_TCP>();
        }

        // Update is called once per frame
        void Update()
        {
            if (res == null) return;
            //rotation
            float qw = Mathf.Sqrt(1f + res[0] + res[5] + res[10]) / 2;
            float w = 4 * qw;
            float qx = (res[9] - res[6]) / w;
            float qy = (res[3] - res[8]) / w;
            float qz = (res[4] - res[1]) / w;

            traceObj.transform.rotation = new Quaternion(qx, qy, qz, qw);
            traceObj.transform.position = new Vector3(res[12], res[13], res[14]);
        }

        public void sendCommand(string command)
        {
            cmd = new PB_Msg.Command();
            if(command == "program")
            {
                cmd.commandID = 0;
                if (testing)
                {
                    tracedNum = cmd.tracedObjNum = 1;
                    cmd.tracedObjName.Add("teacan");
                }
                else
                {
                    Lab.Lab_Table curObject = GameObject.Find("Table").GetComponent<Lab.Lab_Table>();
                    List<GameObject> tracedObjects = curObject.curObject();
                    cmd.tracedObjNum = tracedObjects.Count;
                    for(int i = 0; i < tracedObjects.Count; i++)
                    {
                        cmd.tracedObjName.Add(tracedObjects[i].name);
                    }
                }
                Debug.Log("start program");
            }
            else if(command == "recording")
            {
                cmd.commandID = 1;
                Debug.Log("start recording");
            }else if(command == "tracing")
            {
                cmd.commandID = 2;
                //StartCoroutine("receiveResult");
                Debug.Log("start tracing");
            }else if(command == "exit")
            {
                cmd.commandID = 3;
                Debug.Log("exit");
            }
            serializeCommandAndSend();
        }

        public void serializeCommandAndSend()
        {
            using (ms = new MemoryStream())
            {
                // Save the person to a stream
                Serializer.Serialize(ms, cmd);
                byte[] bytes = ms.ToArray();
                socket.SocketSend(bytes);
            }
        }

        public void receiveResult(MemoryStream recvMs)
        {
            //Debug.Log(recvMs.Length);
            PB_Msg.TracingResult p = Serializer.Deserialize<PB_Msg.TracingResult>(recvMs);
            res = p.result;
            //Debug.Log("yya");
            string resStr = "";
            for (int i = 0; i < 16; i++)
            {
                resStr += p.result[i] + " ";
            }
            Debug.Log(resStr);
        }

        void OnApplicationQuit()
        {
            sendCommand("exit");
            socket.SocketQuit();
        }
    }
}
