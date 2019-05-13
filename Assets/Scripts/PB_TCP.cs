using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ProtoBuf;
using System.IO;

namespace PB {
    public class PB_TCP : MonoBehaviour {

        [SerializeField]
        private string ipString;
        [SerializeField]
        private int port = 0;

        private Socket serverSocket;
        private IPEndPoint ipEnd;
        private Thread connectThread;

        private byte[] recvData;
        private int recvLen;
        private byte[] sendData = { 0 };

        private PB_Interface receiver;

        private MemoryStream recvMs;


        // Use this for initialization
        void Start() {
            PB_Msg.Command cmd = new PB_Msg.Command();
            cmd.commandID = -1;
            MemoryStream ms = new MemoryStream();
            Serializer.Serialize(ms, cmd);
            sendData = ms.ToArray();

            InitSocket();
        }

        // Update is called once per frame
        void Update() {

        }

        public void InitSocket()
        {
            ipEnd = new IPEndPoint(IPAddress.Parse(ipString), port);
            connectThread = new Thread(new ThreadStart(SocketReceive));
            connectThread.Start();
        }

        void SocketConnect()
        {
            if (serverSocket == null)
            {
                serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }
            print("ready to connect");
            serverSocket.Connect(ipEnd);
        }

        void SocketReceive()
        {
            SocketConnect();
            while (true)
            {
                recvData = new byte[4096];
                recvLen = serverSocket.Receive(recvData);
                if (recvLen == 0)
                {
                    continue;
                }

                recvMs = new MemoryStream(recvData, 0, recvLen);
                receiver.receiveResult(recvMs);

                serverSocket.Send(sendData);
                Debug.Log(sendData);
            }
        }

        public void SocketSend(byte[] sendMsg)
        {
            serverSocket.Send(sendMsg);
        }

        void SocketQuit()
        {
            if (connectThread != null)
            {
                connectThread.Interrupt();
                connectThread.Abort();
            }
            if (serverSocket != null)
                serverSocket.Close();
            print("diconnect");
        }

        void OnApplicationQuit()
        {
            SocketQuit();
        }
    }

}
