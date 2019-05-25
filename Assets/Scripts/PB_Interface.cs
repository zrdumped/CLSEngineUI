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

            float qw = res[0] + res[5] + res[10];
            float qx = res[0] - res[5] - res[10];
            float qy = res[5] - res[0] - res[10];
            float qz = res[10] - res[0] - res[5];

            float[] q = new float[4] { qw, qx, qy, qz };
            int biggestID = 0;
            float biggestNum = qw;

            for (int i = 1; i < 4; i++)
            {
                if (q[i] > q[biggestID])
                {
                    biggestID = i;
                    biggestNum = q[i];
                }
            }

            float v = Mathf.Sqrt(biggestNum + 1.0f) * 0.5f;
            float mult = 0.25f / v;

            float w = 0, x = 0, y = 0, z = 0;
            switch (biggestID)
            {
                case 0:
                    w = v;
                    x = (res[6] - res[9]) * mult;
                    y = (res[8] - res[2]) * mult;
                    z = (res[1] - res[4]) * mult;
                    break;
                case 1:
                    x = v;
                    w = (res[6] - res[9]) * mult;
                    z = (res[8] + res[2]) * mult;
                    y = (res[1] + res[4]) * mult;
                    break;
                case 2:
                    y = v;
                    z = (res[6] + res[9]) * mult;
                    w = (res[8] - res[2]) * mult;
                    x = (res[1] + res[4]) * mult;
                    break;
                case 3:
                    z = v;
                    y = (res[6] + res[9]) * mult;
                    x = (res[8] + res[2]) * mult;
                    w = (res[1] - res[4]) * mult;
                    break;
                default:
                    break;
            }

            traceObj.transform.rotation = new Quaternion(x, y, z, w);
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
