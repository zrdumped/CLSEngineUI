using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix.Utils
{
    public class FirstPersonController : MonoBehaviour
    {
        #region variables
        public float moveSpeed = 10f;
        public float rotateSpeed = 5.0f;
        public float rotateSmoothing = 2.0f;

        bool shouldRotateCamera = true;
        Vector2 rotation;
        Vector2 deltaRotation;
        Camera mainCamera;
        #endregion

        private void Start()
        {
            mainCamera = Camera.main;
        }

        void FixedUpdate()
        {
            // Deal with movement
            var moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.Translate(moveDirection * Time.fixedDeltaTime * moveSpeed);

            // Deal with rotation
            if (shouldRotateCamera)
            {
                var mouseCoords = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                var targetRotation = mouseCoords * rotateSpeed * rotateSmoothing;
                deltaRotation = Vector2.Lerp(deltaRotation, targetRotation, 1f / rotateSmoothing);
                rotation += deltaRotation;
                rotation.y = Mathf.Clamp(rotation.y, -45, 45);

                mainCamera.transform.localRotation = Quaternion.AngleAxis(-rotation.y, Vector3.right);
                transform.localRotation = Quaternion.AngleAxis(rotation.x, transform.up);
            }

            // Deal with other input
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log("FPC: Escape key detected");
                shouldRotateCamera = !shouldRotateCamera;
            }
        }
    }
}