﻿namespace Base.Game.GameObject.Interactable
{
    using Base.Game.GameObject.Interactional;
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class ImpactableObject : MonoBehaviour
    {
        private Collider _collider;

        private Vector3 _startingPos;
        private Vector3 _startingRot;

        private Coroutine _routine;
        private Rigidbody _body;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _startingPos = transform.localPosition;
            _startingRot = transform.localRotation.eulerAngles;
        }

        private void OnEnable()
        {
            transform.localPosition = _startingPos;
            transform.localRotation = Quaternion.Euler(_startingRot);
            if (_body)
            {
                Destroy(_body);
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<BaseCar>() is BaseCar car && _body == null && car != GetComponentInParent<BaseCar>())
            {
                _body = gameObject.AddComponent<Rigidbody>();
                _collider.attachedRigidbody.AddExplosionForce(car.CurrentSpeed * Time.fixedDeltaTime, transform.position, 10F, 3f);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(collision.collider.GetComponent<BaseCar>() is BaseCar car && car != GetComponentInParent<BaseCar>() && _body)
            {
                _collider.attachedRigidbody.AddExplosionForce(car.CurrentSpeed * Time.fixedDeltaTime, transform.position, 10F, 3f);
            }
        }

    }
}
