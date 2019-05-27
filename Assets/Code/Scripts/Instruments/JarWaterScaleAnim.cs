using UnityEngine;

namespace Chemix.Instruments
{
    public class JarWaterScaleAnim : MonoBehaviour
    {
        #region Messages

        void FixedUpdate()
        {
            float ratio = gasJar.TotalMass / gasJar.StableAirMass;
            if (ratio < 0.9999f)
            {
                float scale = 1 - ratio;
                transform.localScale = new Vector3(1, scale, 1);
            }
            else
            {
                ChemixEventManager.Instance.NotifyChangeState(TaskFlow.TaskEvent.CollectingComplete, true);
                bottomHalf.SetActive(false);
                enabled = false;
            }
        }

        #endregion

        #region Private

        [SerializeField]
        GameObject bottomHalf;
        [SerializeField]
        ChemixGasObject gasJar;

        #endregion
    }
}