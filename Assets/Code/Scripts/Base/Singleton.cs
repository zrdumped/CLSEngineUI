using UnityEngine;

namespace Chemix
{
    /// <summary>
    /// Inherit from this base class to create a singleton.
    /// e.g. public class MyClassName : Singleton<MyClassName> {}
    /// </summary>
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        // Check to see if we're about to be destroyed.
        private static bool m_ShuttingDown = false;
        //private static object m_Lock = new object();
        private static T m_Instance;

        /// <summary>
        /// Access singleton instance through this propriety.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (m_ShuttingDown)
                {
                    //Debug.LogWarning("[Singleton] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                    return null;
                }
                return m_Instance;
            }
        }

        protected virtual void Awake()
        {
            if (m_Instance)
            {
                Debug.LogError("Singleton: there are multiple " + typeof(T));
            }
            else
            {
                m_Instance = (T)FindObjectOfType(typeof(T));
                m_ShuttingDown = false;
            }
        }

        protected void OnApplicationQuit()
        {
            m_ShuttingDown = true;
        }

        protected void OnDestroy()
        {
            m_ShuttingDown = true;
        }
    }
}