using UnityEngine;

namespace Chemix.Instruments
{
    /// <summary>
    /// MedicineDropZone specify the valid zone for dropping medecine
    /// </summary>
    public class MedicineDropZone : BaseSlave
    {
        public GameObject prefab;

        GameObject medicine = null;

        public override void ReceiveCommand()
        {
            OnDrop(new Mixture("KMnO4", 3));
        }

        public void OnDrop(Mixture mixture)
        {
            if (!medicine)
            {
                medicine = Instantiate(prefab, transform);
            }
            medicine.GetComponent<ChemixObject>().AddAndUpdate(mixture);
            ChemixEventManager.Instance.NotifyChangeState(TaskFlow.TaskEvent.MedicineInPosition, true);
        }
    }
}