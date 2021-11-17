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
            if (Input.GetButtonDown("Num1"))
                SelectDemoView(1);

            if (Input.GetButtonDown("Num2"))
                SelectDemoView(2);

            if (Input.GetButtonDown("Num3"))
                SelectDemoView(3);

            if (Input.GetButtonDown("Num4"))
                SelectDemoView(4);

            transitionTime += Time.deltaTime;

            if (!inputDiverted)
                LerpToTarget();
        }

    }

}
