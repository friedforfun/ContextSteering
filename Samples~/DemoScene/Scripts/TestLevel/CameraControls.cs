using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Friedforfun.ContextSteering.Demo
{
    public class CameraControls : MonoBehaviour
    {

        [SerializeField]
        private Transform[] DemoTransforms;

        private Transform targetTransform;
        private float transitionTime = 0f;
        private Vector3 StartPosition;
        private bool inputDiverted = true;

        private string[] buttonBindings = { "Num1", "Num2", "Num3", "Num4" };


        private void SelectDemoView(int numKeyPress)
        {
            inputDiverted = false;
            transitionTime = 0f;
            StartPosition = transform.position;

            if (DemoTransforms == null)
                return;

            if (numKeyPress > DemoTransforms.Length || numKeyPress == 0)
                return;

            targetTransform = DemoTransforms[numKeyPress - 1];
        }

        private void LerpToTarget()
        {
            transform.position = Vector3.Lerp(StartPosition, targetTransform.position, transitionTime);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(transform.rotation.eulerAngles, targetTransform.rotation.eulerAngles, transitionTime));
        }

        private void Update()
        {
            checkButtons();

            transitionTime += Time.deltaTime;

            if (!inputDiverted)
                LerpToTarget();
        }

        private void checkButtons()
        {
            for (int i = 0; i < buttonBindings.Length; i++)
            {
                if (buttonExists(buttonBindings[i]))
                    if (Input.GetButtonDown(buttonBindings[i]))
                        SelectDemoView(i + 1);
            }
        }

        private bool buttonExists(string name)
        {
            try
            {
                Input.GetButton(name);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }

}
