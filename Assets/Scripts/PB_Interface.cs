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
        private int tracedNum;


        public bool testing = true;
        // Use this for initialization
        void Start()
        {
            socket = this.GetComponent<PB_TCP>();
        }

        // Update is called once per frame
        void Update()
        {

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
            PB_Msg.TracingResult p = Serializer.Deserialize<PB_Msg.TracingResult>(recvMs);
            for (int i = 0; i < tracedNum; i++)
            {
                Debug.Log(p.result[0]);
            }
        }
    }
}
