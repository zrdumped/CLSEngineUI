using UnityEngine;
using Chemix.UI;

namespace Chemix
{
    // This can disallow ChemixObject to be added as component
    [DisallowMultipleComponent]
    [AddComponentMenu("")]
    public class ChemixObject : MonoBehaviour, IHeatableObject, IRichText
    {
        #region Properties

        public ChemixReactionSystem System
        {
            get { return system; }
            set { system = value; }
        }

        public float TotalMass
        {
            get { return mixture.TotalMass; }
        }

        public Mixture Mixture
        {
            get { return mixture; }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return mixture.ToString();
        }

        public string ToRichString()
        {
            return mixture.ToRichString();
        }

        public void AddAndUpdate(Mixture other)
        {
            if (other.IsAir)
            {
                // Corner case: Deal with problem with constantly adding air
                if (!system.IsReacting)
                {
                    mixture.Add(other);
                }
                return;
            }
            else
            {
                if (mixture.Add(other))
                {
                    system.FindAndSetupReactions();
                }
            }
        }

        #endregion

        #region Messages

        public void SetIsHeating(bool isHeating)
        {
            //Debug.LogFormat("ChemixSubstance: {0} is heated", this);
            if (system.IsOwner(this))
            {
                system.IsHeating = isHeating;
            }
        }

        protected virtual void Awake()
        {
            system = new ChemixReactionSystem(this);
        }

        protected virtual void Start()
        {
            UIManager.Instance.CreateFormulaLabel(this);
        }

        protected virtual void FixedUpdate()
        {
            if (system.IsOwner(this))
            {
                system.OnSystemUpdate(Time.fixedDeltaTime);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            var cobject = other.GetComponent<ChemixObject>();
            // Compare instance id to avoid `OnTriggerEnter` being called twice
            if (cobject && this.GetInstanceID() > cobject.GetInstanceID())
            {
                //Debug.LogFormat("ChemixObject: {1} enter {0}", this.name, other.name);
                system.Add(cobject);
            }
        }

        void OnTriggerExit(Collider other)
        {
            var cobject = other.GetComponent<ChemixObject>();

            if (cobject && this.GetInstanceID() > cobject.GetInstanceID())
            {
                //Debug.LogFormat("ChemixObject: {1} exit {0}", this.name, other.name);
                system.Remove(cobject);
            }
        }

#if UNITY_EDITOR
        // this is empty but necessarry to add gizmo icon in scene view
        void OnDrawGizmos()
        {
        }
#endif

        #endregion

        #region Variables

        protected ChemixReactionSystem system;

        [SerializeField]
        protected Mixture mixture;

        #endregion
    }
}