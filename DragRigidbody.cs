using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class DragRigidbody : MonoBehaviour
    {
        const float k_Spring = 350.0f;
        const float k_Damper = 5.0f;
        const float k_Drag = 5.0f;
        const float k_AngularDrag = 5.0f;
        const float k_Distance = 0.2f;
        const bool k_AttachToCenterOfMass = false;

        private SpringJoint m_SpringJoint;
        public Rigidbody RigidbodyJoint;
        public bool isKin;

        private void Update()
        {
            if (RigidbodyJoint != null)
            {
                if (RigidbodyJoint.isKinematic == true)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        RigidbodyJoint.isKinematic = false;
                        RigidbodyJoint = null;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (RigidbodyJoint.isKinematic == false)
                    {
                        RigidbodyJoint.isKinematic = true;
                        RigidbodyJoint = null;
                    }
                    else
                    {
                        RigidbodyJoint.isKinematic = false;
                    }
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    RigidbodyJoint.transform.Rotate(Vector3.right * 30 * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.E))
                {
                    RigidbodyJoint.transform.Rotate(Vector3.left * 30 * Time.deltaTime);
                }

            }

            // Make sure the user pressed the mouse down
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }

            var mainCamera = FindCamera();

            // We need to actually hit an object
            RaycastHit hit = new RaycastHit();
            if (
                !Physics.Raycast(mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f)).origin,
                                 mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f)).direction, out hit, 100,
                                 Physics.DefaultRaycastLayers))

            {
                return;
            }
            if (hit.collider.tag == "GravityObject")
            {
                if (hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic == true)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.collider.gameObject.GetComponent<Rigidbody>().isKinematic = false;
                        isKin = false;
                    }
                }
            }
            // We need to hit a rigidbody that is not kinematic
            if (!hit.rigidbody || hit.rigidbody.isKinematic)
            {
                return;
            }

            

            if (!m_SpringJoint)
            {
                var go = new GameObject("Rigidbody dragger");
                Rigidbody body = go.AddComponent<Rigidbody>();
                m_SpringJoint = go.AddComponent<SpringJoint>();
                body.isKinematic = true;
            }
            

            m_SpringJoint.transform.position = hit.point;
            m_SpringJoint.anchor = Vector3.zero;


            m_SpringJoint.spring = k_Spring;
            m_SpringJoint.damper = k_Damper;
            m_SpringJoint.maxDistance = k_Distance;
            m_SpringJoint.connectedBody = hit.rigidbody;

            
            StartCoroutine("DragObject", hit.distance);
            RigidbodyJoint = m_SpringJoint.connectedBody;
            
        }


        private IEnumerator DragObject(float distance)
        {
            if (isKin == false)
            {
                var oldDrag = m_SpringJoint.connectedBody.drag;
                var oldAngularDrag = m_SpringJoint.connectedBody.angularDrag;
                m_SpringJoint.connectedBody.drag = k_Drag;

                m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;
                var mainCamera = FindCamera();

                while (Input.GetMouseButton(0))
                {
                    var ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f));
                    m_SpringJoint.transform.position = ray.GetPoint(distance);
                    yield return null;
                }
                if (m_SpringJoint.connectedBody)
                {
                    m_SpringJoint.connectedBody.drag = oldDrag;
                    m_SpringJoint.connectedBody.angularDrag = oldAngularDrag;
                    m_SpringJoint.connectedBody = null;
                }
            }
        }



        private Camera FindCamera()
        {
            if (GetComponent<Camera>())
            {
                return GetComponent<Camera>();
            }

            return Camera.main;
        }
    }
}
