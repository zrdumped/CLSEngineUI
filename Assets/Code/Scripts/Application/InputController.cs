using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    public class InputController : Singleton<InputController>
    {
        public GameObject currentPawn
        {
            get;
            private set;
        }

        public int expectedLayer
        {
            get { return m_ExpectedLayer; }
        }

        public BaseClickHandler handler
        {
            get;
            set;
        }

        private Camera m_Camera = null;
        private float m_Distance = 0;
        private Vector3 m_Offset = new Vector3();

        private void Start()
        {
            if (!Chemix.Config.enableMouseControl)
            {
                enabled = false;
                return;
            }

            m_Camera = ChemixEngine.Instance.mainCamera;
        }

        private void Update()
        {
            HandleClick();
            UpdatePawn();
        }

        private void HandleClick()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (currentPawn)
                {
                    if (handler)
                    {
                        if (handler.OnClick(currentPawn))
                        {
                            UnPossess();
                        }
                    }
                    else
                    {
                        UnPossess();
                    }
                }
                else if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << m_ExpectedLayer))
                {
                    UnPossess();
                    Possess(hit.collider.gameObject);
                }
            }
        }

        private Vector3 ScreenToWorldPoint(float distance)
        {
            var screenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            return m_Camera.ScreenToWorldPoint(screenPoint);
        }

        private void UpdatePawn()
        {
            if (currentPawn)
            {
                var newPosition = ScreenToWorldPoint(m_Distance) + m_Offset;
                newPosition.y = Mathf.Max(newPosition.y, m_MinimumY);
                currentPawn.transform.position = newPosition;
            }
        }

        private void Possess(GameObject go)
        {
            //Debug.LogFormat("Input: ({0}) clicked", go.name);
            currentPawn = go;
            m_Distance = m_Camera.WorldToScreenPoint(currentPawn.transform.position).z;
            m_Offset = currentPawn.transform.position - ScreenToWorldPoint(m_Distance);

            Chemix.SetLayerRecursively(currentPawn.transform, 2);// ignore raycast
            
            //var inputLogger = currentPawn.GetComponent<InputLogger>();
            //if (inputLogger.previousParent)
            //{
            //    currentPawn.transform.parent = inputLogger.previousParent;
            //    inputLogger.previousParent = null;
            //}
        }

        private void UnPossess()
        {
            if (currentPawn)
            {
                Chemix.RestoreLayerRecursively(currentPawn.transform);

                currentPawn = null;
            }
        }

        [SerializeField]
        private int m_ExpectedLayer = 10;

        [SerializeField]
        private float m_MinimumY = 1;
    }
}