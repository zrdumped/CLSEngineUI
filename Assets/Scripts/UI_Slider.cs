using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace UI
{
    public class UI_Slider : MonoBehaviour
    {

        public GameObject sceneCamera;
        public GameObject lightSource;
        public GameObject showColor;

        public enum State { HEIGHT, ANGLE, R, G, B, Intensity };
        public State sliderType;


        // Use this for initialization
        void Start()
        {
            if (sliderType == State.HEIGHT)
            {
                gameObject.GetComponent<Slider>().value = sceneCamera.transform.position.y;
            }
            else if (sliderType == State.ANGLE)
            {
                gameObject.GetComponent<Slider>().value = sceneCamera.transform.eulerAngles.x;
            }
            else if (sliderType == State.Intensity)
            {
                gameObject.GetComponent<Slider>().value = lightSource.GetComponent<Light>().intensity;
            }
            else
            {
                Color srcLightColor = lightSource.GetComponent<Light>().color;
                showColor.GetComponent<Image>().color = srcLightColor;
                if (sliderType == State.R)
                {
                    gameObject.GetComponent<Slider>().value = srcLightColor.r * 255;
                }
                else if (sliderType == State.G)
                {
                    gameObject.GetComponent<Slider>().value = srcLightColor.g * 255;
                }
                else if (sliderType == State.B)
                {
                    gameObject.GetComponent<Slider>().value = srcLightColor.b * 255;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AdjustCameraHeight()
        {
            Vector3 oldPos = sceneCamera.transform.position;
            float input = gameObject.GetComponent<Slider>().value;
            oldPos.y = input;
            sceneCamera.transform.position = oldPos;
        }

        public void AdjustCameraAngle()
        {
            Vector3 oldRot = sceneCamera.transform.eulerAngles;
            float input = gameObject.GetComponent<Slider>().value;
            oldRot.x = input;
            sceneCamera.transform.eulerAngles = oldRot;
        }

        public void AdjustLightColor()
        {
            Color srcLightColor = lightSource.GetComponent<Light>().color;
            Color sliderColor = new Color(0, 0, 0);
            Color oldLightColor = lightSource.GetComponent<Light>().color;
            if (sliderType == State.R)
            {
                float value = gameObject.GetComponent<Slider>().value / 255;
                srcLightColor.r = oldLightColor.r = sliderColor.r = value;
            }
            else if (sliderType == State.G)
            {
                float value = gameObject.GetComponent<Slider>().value / 255;
                srcLightColor.g = oldLightColor.g = sliderColor.g = value;
            }
            else if (sliderType == State.B)
            {
                float value = gameObject.GetComponent<Slider>().value / 255;
                srcLightColor.b = oldLightColor.b = sliderColor.b = value;
            }
            ColorBlock cb = gameObject.GetComponent<Slider>().colors;
            cb.pressedColor = sliderColor;
            gameObject.GetComponent<Slider>().colors = cb;

            lightSource.GetComponent<Light>().color = oldLightColor;
            showColor.GetComponent<Image>().color = oldLightColor;
        }

        public void AdjustLightIntensity()
        {
            lightSource.GetComponent<Light>().intensity = gameObject.GetComponent<Slider>().value;
        }
    }
}
