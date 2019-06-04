using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;
using System;
using System.IO;
using UnityEngine.UI;

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

        private float signX = -1, signY = -1, signZ = 1;
        private float posX = -1, posY = -1, posZ = -1;
        public Text adjustR, adjustP, adjustY;

        private Vector3 anchor = new Vector3(0, 0, 0);

        //public GameObject adjustO;
        //public Vector3 nowPos;
        public float posScaler = 1;

        public Slider xS, yS, zS, scalerS, objScaleS;

        public float offZ = 0, offX = 0, offY = 0;

        public Text fps;
        public TextMesh substance;

        public float lastTime;

        public string outText;
        private int frames = 0, updateFrame = 0;

        public GameObject bottle;
        public List<GameObject> models;
        private int modelID = 0;

        // Use this for initialization
        void Start()
        {
            socket = this.GetComponent<PB_TCP>();
            //nowPos = adjustO.transform.localPosition;
            lastTime = DateTime.Now.Millisecond;
            StartCoroutine("CalFrame");
            if(GM.GM_Core.instance.showText != "")
                substance.text = GM.GM_Core.instance.showText;
        }

        IEnumerator CalFrame()
        {
            while (true)
            {
                outText = "FPS: " + frames + "App: " + updateFrame;
                frames = updateFrame = 0;
                yield return new WaitForSeconds(1);
            }
        }

        // Update is called once per frame
        void Update()
        {
            updateFrame++;
            fps.text = outText;
            adjustR.text = "Rot: " + signX + " " + signY + " " + signZ + " Pos; " + posX + " " + posY + " " + posZ;
            adjustP.text = "XOff " + offX + " YOff " + offY;
            adjustY.text = "ZOff: " + offZ + " Scaler" + posScaler;
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

            traceObj.transform.localRotation = new Quaternion(x, y, z, w);
            traceObj.transform.localPosition = new Vector3(posX * res[12], posY * res[13], posZ * res[14]) * posScaler + new Vector3(offX, offY, offZ);
            traceObj.transform.localEulerAngles = new Vector3(signX * traceObj.transform.localEulerAngles.x,
               signY *  traceObj.transform.localEulerAngles.y, signZ * traceObj.transform.localEulerAngles.z);


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
            frames++;
            //int ms = DateTime.Now.Millisecond;
             // outText = string.Format("FPS: {0}",( 1000.0f / (ms - lastTime)).ToString("##.##"));
//lastTime = ms;
            //Debug.Log(recvMs.Length);
            PB_Msg.TracingResult p = Serializer.Deserialize<PB_Msg.TracingResult>(recvMs);
            res = p.result;
            //Debug.Log("yya");
            string resStr = "";
            for (int i = 0; i < 16; i++)
            {
                resStr += p.result[i] + " ";
            }
            Debug.Log(outText);
        }

        void OnApplicationQuit()
        {
            sendCommand("exit");
            socket.SocketQuit();
        }

        public void modifyArgu(int input)
        {
            switch (input)
            {
                case 0:
                    signX *= -1;
                    break;
                case 1:
                    signY *= -1;
                    break;
                case 2:
                    signZ *= -1;
                    break;
                default:break;
            }
        }

        public void modifyPos(int input)
        {
            switch (input)
            {
                case 0:
                    posX *= -1;
                    break;
                case 1:
                    posY *= -1;
                    break;
                case 2:
                    posZ *= -1;
                    break;
                default: break;
            }
        }

        public void modifyY()
        {
            offY = yS.value;
        }

        public void modifyX()
        {
            offX = xS.value;
        }

        public void modifyZ()
        {
            offZ = zS.value;
        }

        public void modifyScaler()
        {
            posScaler = scalerS.value;
        }

        public  void modifyObjScale()
        {

            bottle.transform.localScale = new Vector3(1, 1, 1) * objScaleS.value;
        }

        public void switchObject()
        {
            models[modelID].SetActive(false);
            modelID++;
            if (modelID == models.Count) modelID = 0;
            models[modelID].SetActive(true);
        }

    }
}
