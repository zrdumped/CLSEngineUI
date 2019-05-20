using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Chemix
{
    public class EnvironmentController : Singleton<EnvironmentController>
    {
        public Camera camera;
        public Light light;
        public GameObject room;
        // Use this for initialization

        void Start()
        {
            if (Chemix.CustomMode && GM.GM_Core.instance)
            {
                var envInfo = GameManager.Instance.GetExperimentalSetup().envInfo;

                if (envInfo != null)
                {
                    Debug.Log("Chemix: Setup environment");
                    Vector3 cameraRotation = new Vector3(envInfo.cameraAngle, 0, 0);
                    camera.transform.rotation = Quaternion.Euler(cameraRotation);

                    Vector3 cameraPosition = camera.transform.position;
                    cameraPosition.y = envInfo.cameraHeight;
                    camera.transform.position = cameraPosition;

                    room.SetActive(envInfo.useRoom);

                    light.color = envInfo.lightColor;
                    light.intensity = envInfo.lightIntensity;
                }
            }
        }
    }
}