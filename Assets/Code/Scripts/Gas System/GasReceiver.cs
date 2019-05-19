using UnityEngine;

namespace Chemix.Gas
{
    public abstract class GasReceiver : MonoBehaviour
    {
        /// <summary>
        /// False to indicate the tail is Air
        /// </summary>
        public abstract bool GetTail(out GasReceiver gasReceiver);
    }
}