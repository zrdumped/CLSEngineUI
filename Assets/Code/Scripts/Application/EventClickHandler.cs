using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    public class EventClickHandler : BaseClickHandler
    {
        public enum EventType
        {
            Default,
            SetPawnOnFire,
            SetMeOnFire,
            SetMeOffFire,
            GetSolid,
            PutSolid,
            GetLiquid,
            PutLiquid,
        }

        [System.Serializable]
        public class TypeAndBehaviour
        {
            public ChemixInstrument.Type expectedType;
            public EventType eventType;
        }

        [SerializeField]
        protected List<TypeAndBehaviour> expectedTypes;

        protected override bool HandleClick(GameObject pawn)
        {
            foreach (var ins in expectedTypes)
            {
                if (Chemix.CheckType(pawn, ins.expectedType))
                {
                    switch (ins.eventType)
                    {
                        case EventType.SetPawnOnFire:
                            pawn.GetComponent<BaseBurnable>().OnFire = true;
                            return false;
                        case EventType.SetMeOnFire:
                            if (pawn.GetComponent<BaseBurnable>().OnFire)
                            {
                                transform.parent.GetComponentInChildren<BaseBurnable>().OnFire = true;
                            }
                            return false;
                        case EventType.SetMeOffFire:
                            transform.parent.GetComponentInChildren<BaseBurnable>().OnFire = false;
                            return false;
                        case EventType.GetSolid:
                            {
                                var spoon = pawn.GetComponent<Instruments.MedicineSpoon>();
                                var medicine = GetComponent<ChemixObject>();
                                spoon.CollectOrPutback(medicine);
                                return false;
                            }
                        case EventType.PutSolid:
                            {
                                var spoon = pawn.GetComponent<Instruments.MedicineSpoon>();
                                var dropZone = GetComponent<Instruments.MedicineDropZone>();
                                spoon.TryPutInDropZone(dropZone);
                                return false;
                            }
                        case EventType.GetLiquid:
                            {
                                var dropper = pawn.GetComponent<Instruments.Dropper>();
                                var liquid = transform.parent.GetComponentInChildren<ChemixObject>();
                                dropper.CollectLiquidFrom(liquid);
                                return false;
                            }
                        case EventType.PutLiquid:
                            {
                                var dropper = pawn.GetComponent<Instruments.Dropper>();
                                dropper.DropLiquid();
                                return false;
                            }
                    }
                    return true;
                }
            }
            return true;
        }

        protected override bool isExpecting(GameObject go)
        {
            foreach (var ins in expectedTypes)
            {
                if (Chemix.CheckType(go, ins.expectedType))
                {
                    return true;
                }
            }
            return false;
        }
    }
}