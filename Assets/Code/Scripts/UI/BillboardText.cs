using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.UI
{
    public class BillboardText : MonoBehaviour
    {
        public ChemixObject owner
        {
            get;
            set;
        }

        Transform m_cameraTransform;

        void Awake()
        {
            m_cameraTransform = Camera.main.transform;
        }

        //Orient the camera after all movement is completed this frame to avoid jittering
        void LateUpdate()
        {
            // rotates the object relative to the camera
            Vector3 targetPos = transform.position + m_cameraTransform.rotation * Vector3.forward;
            Vector3 targetOrientation = m_cameraTransform.rotation * Vector3.up;
            transform.LookAt(targetPos, targetOrientation);
        }
    }
}