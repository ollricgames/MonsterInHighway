namespace Base.Game.GameObject.Interactional
{
    using Base.Game.GameObject.Interactable;
    using System;
    using UnityEngine;
    using UnityStandardAssets.Vehicles.Car;

    [RequireComponent(typeof(CarController))]
    public class BaseCar : MonoBehaviour, IInteractionalObject
    {
        [SerializeField] protected Vector3 _rightLine = Vector3.zero;
        [SerializeField] protected Vector3 _leftLine = Vector3.zero;

        protected CarController _controller;

        protected float _steering;
        protected float _acceleration;
        protected float _brake;
        protected float _handbrake;
        protected Vector3 _target;

        private void Awake()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            _controller = GetComponent<CarController>();
            _target = _rightLine;
        }

        protected virtual void Move()
        {
            _controller.Move(_steering, _acceleration, _brake, _handbrake);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.x, transform.position.y, transform.position.z), .1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), .5f);
        }

        protected virtual void KeepInLine()
        {
            if (Math.Abs(transform.position.x - _target.x) > .05f || Math.Abs(transform.rotation.eulerAngles.y) > 1f)
            {
                Vector3 t = new Vector3(_target.x, transform.position.y, transform.position.z + 1);
                _steering = (transform.position - t).normalized.x * -1;
            }
        }

        public Transform Transform => transform;

        public virtual void Active()
        {
            gameObject.SetActive(true);
        }

        public virtual void DeActive()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnCollisionEnter(Collision collision)
        {
            collision.collider.GetComponent<IInteractableObject>()?.Interact(this);
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            other.GetComponent<IInteractableObject>()?.Interact(this);
            if (other.GetComponent<BaseCar>())
            {
                if(transform.position.z < other.transform.position.z)
                {
                    _brake = _acceleration > 0 ? _acceleration * -1 : _acceleration;
                    _acceleration = _brake;
                }
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<BaseCar>())
            {
                if (transform.position.z < other.transform.position.z)
                {
                    _acceleration = _acceleration < 0 ? _acceleration * -1 : _acceleration;
                    _brake = 0;
                }
            }
        }

        public virtual void Interact(IInteractableObject obj)
        {

        }
    }
}