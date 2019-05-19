using UnityEngine;

namespace Chemix.Instruments
{
    public class Dropper : MonoBehaviour, IUsable
    {
        public GameObject waterInside;
        public GameObject waterDropPrefab;
        public float dropOffset;

        ChemixObject objectToCollect;
        Mixture mixture;

        public void StartUsing()
        {
            if (objectToCollect)
            {
                CollectLiquidFrom(objectToCollect);
            }
            else
            {
                DropLiquid();
            }
        }

        public void CollectLiquidFrom(ChemixObject cobject)
        {
            if (mixture == null)
            {
                mixture = cobject.Mixture.TakeByRatio();
                waterInside.SetActive(true);
            }
            else
            {
                cobject.AddAndUpdate(mixture);
                mixture = null;
                waterInside.SetActive(false);
            }
        }

        public void DropLiquid()
        {
            if (mixture != null)
            {
                var drop = Instantiate(waterDropPrefab, transform.position + new Vector3(0, dropOffset, 0), new Quaternion());
                var cobject = drop.GetComponent<ChemixObject>();
                cobject.AddAndUpdate(mixture);
                mixture = null;
                waterInside.SetActive(false);
            }

        }

        public void StopUsing()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            var liquid = other.GetComponent<ChemixLiquidObject>();

            if (liquid)
            {
                objectToCollect = liquid;
            }
        }

        void OnTriggerExit(Collider other)
        {
            var liquid = other.GetComponent<ChemixLiquidObject>();

            if (liquid)
            {
                objectToCollect = null;
            }
        }

        void OnGUI()
        {
            if (mixture == null)
                return;

            Chemix.DrawTextOnTransform(transform, mixture.ToString());
        }

        // display where the water drop will appear
        void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position + new Vector3(0, dropOffset, 0), 0.04f);
        }
    }
}