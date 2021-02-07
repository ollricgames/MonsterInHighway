using System;
using UnityEngine;

namespace UnityStandardAssets.Vehicles.Car
{
    public class BrakeLight : MonoBehaviour
    {
        private Renderer m_Renderer;


        private void Start()
        {
            m_Renderer = GetComponent<Renderer>();
        }

        public void Active()
        {
            m_Renderer.enabled = true;
        }

        public void DeActive()
        {
            m_Renderer.enabled = false;
        }

    }
}
