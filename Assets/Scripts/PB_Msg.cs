using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtoBuf;

namespace PB
{
    public class PB_Msg : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [ProtoContract]
        public class Command
        {
            [ProtoMember(1)]
            //0 - start program; 1 - start recording(b); 2 - start tracing(r); 3 - exit(e); -1 idle
            public int commandID { get; set; }

            //start program
            [ProtoMember(2)]
            public int tracedObjNum { get; set; }
            [ProtoMember(3)]
            public List<string> tracedObjName { get; set; }
        }

        [ProtoContract]
        public class TracingResult
        {
            [ProtoMember(1)]
            public List<float> result;
            ////[x, y, z, r1, r2, r3][][]...
            //public List<float> x;
            //[ProtoMember(2)]
            //public List<float> y;
            //[ProtoMember(3)]
            //public List<float> z;
            //[ProtoMember(4)]
            //public List<float> r1;
            //[ProtoMember(5)]
            //public List<float> r2;
            //[ProtoMember(6)]
            //public List<float> r3;
        }
    }
}
