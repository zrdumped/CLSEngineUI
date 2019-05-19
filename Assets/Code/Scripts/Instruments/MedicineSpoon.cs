using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Instruments
{
    public class MedicineSpoon : MonoBehaviour, IUsable
    {
        public GameObject powder;

        ChemixObject objectToCollect;
        Mixture mixture;
        MedicineDropZone dropZone;

        public void StartUsing()
        {
            if (objectToCollect)
            {
                CollectOrPutback(objectToCollect);
            }
            else
            {
                TryPutInDropZone(dropZone);
            }
        }

        public void CollectOrPutback(ChemixObject cobject)
        {
            if (mixture == null)
            {
                mixture = cobject.Mixture.TakeByRatio();
                powder.SetActive(true);
            }
            else
            {
                cobject.AddAndUpdate(mixture);
                mixture = null;
                powder.SetActive(false);
            }
        }

        public void TryPutInDropZone(MedicineDropZone mdz)
        {
            if (mixture != null && mdz)
            {
                mdz.OnDrop(mixture);
                mixture = null;
                powder.SetActive(false);
            }
        }

        public void StopUsing() { }

        void OnTriggerEnter(Collider other)
        {
            var solid = other.GetComponent<ChemixSolidObject>();
            if (solid)
            {
                if (solid.CanSpoonCollect)
                {
                    objectToCollect = solid;
                }
                return;
            }

            var mdz = other.GetComponent<MedicineDropZone>();
            if (mdz)
            {
                dropZone = mdz;
            }
        }

        void OnTriggerExit(Collider other)
        {
            var solid = other.GetComponent<ChemixSolidObject>();
            if (solid)
            {
                if (solid.CanSpoonCollect)
                {
                    objectToCollect = null;
                }
                return;
            }

            if (other.GetComponent<MedicineDropZone>())
            {
                dropZone = null;
            }
        }

        void OnGUI()
        {
            if (mixture == null)
                return;

            Chemix.DrawTextOnTransform(transform, mixture.ToString());
        }
    }
}