using System.Collections;
using UnityEngine;

namespace Chemix.Instruments
{
    public class TubeExplodeAnim : MonoBehaviour, IHeatableObject
    {
        public float explosionForce = 70f;
        public float explosionRadius = 1.6f;
        public float waitTime = 2f;
        public GameObject tube;
        public GameObject brokePrefab;

        bool hasExploded = false;
        bool isHeating = false;

        public void SetIsHeating(bool isHeating)
        {
            if (isHeating != this.isHeating)
            {
                if (!isHeating)
                {
                    bool isDeliveryTubeConnected = ChemixEventManager.Instance.GetState(TaskFlow.TaskEvent.DeliveryTubeInPosition);
                    if (isDeliveryTubeConnected)
                    {
                        if (!hasExploded)
                        {
                            StartCoroutine(WaitAndExplode());
                        }
                    }
                }
                else
                {
                    StopCoroutine(WaitAndExplode());
                }
                this.isHeating = isHeating;
            }
        }

        IEnumerator WaitAndExplode()
        {
            yield return new WaitForSeconds(waitTime);
            if (hasExploded)
            {
                yield break;
            }

            bool isDeliveryTubeConnected = ChemixEventManager.Instance.GetState(TaskFlow.TaskEvent.DeliveryTubeInPosition);
            if (!isDeliveryTubeConnected)
            {
                yield break;
            }

            TipBoard.Instance.Warning("提示：应该先移除导管再熄灭酒精灯，否则会水倒吸引起试管破裂。");
            hasExploded = true;
            var broke = Instantiate(brokePrefab, transform.position, transform.rotation);
            foreach (Transform child in broke.transform)
            {
                var rb = child.gameObject.AddComponent<Rigidbody>();
                rb.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }

            broke.AddComponent<WaitAndDestroy>().lifetime = 3f;
            Destroy(tube);
        }
    }
}