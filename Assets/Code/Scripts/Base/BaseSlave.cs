using UnityEngine;

namespace Chemix
{
    public abstract class BaseSlave : MonoBehaviour
    {
        public virtual void ReceiveCommand()
        {
            Debug.LogFormat("BaseSlave: {0} doens't respond to command", this.name);
        }
    }
}