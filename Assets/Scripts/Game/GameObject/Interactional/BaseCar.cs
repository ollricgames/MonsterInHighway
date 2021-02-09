namespace Base.Game.GameObject.Interactional
{
    using Base.Game.GameObject.Interactable;
    using Base.Game.Signal;
    using System;
    using UnityEngine;
    using UnityStandardAssets.Vehicles.Car;

    [RequireComponent(typeof(CarController))]
    public class BaseCar : MonoBehaviour, IInteractionalObject
    {
        [SerializeField] protected Vector3 _rightLine = Vector3.zero;
        [SerializeField] protected Vector3 _leftLine = Vector3.zero;
        [Range(0, 1)] [SerializeField] protected float _defaultAcceleration = .3f;

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
            _acceleration = _defaultAcceleration;
        }

        protected virtual void Move()
        {
            _controller.Move(_steering, _acceleration, _acceleration, _handbrake);
        }

        public virtual void Brake()
        {
            _acceleration = -1 * _defaultAcceleration;
        }

        public virtual void BrakeOff()
        {
            _acceleration = _defaultAcceleration;
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

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

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
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            other.GetComponent<IInteractableObject>()?.EndInteract(this);
        }

        public virtual void Interact(IInteractableObject obj)
        {

        }

        public virtual void EndInteract(IInteractableObject obj)
        {

        }
    }
}