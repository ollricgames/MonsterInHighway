namespace Base.Game.GameObject.Environment
{
    using Base.Game.GameObject.Interactable;
    using Base.Game.GameObject.Interactional;
    using Base.Game.Signal;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class BasePlatform : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private Transform _endPoint = null;
        public Vector3 EndPoint { get => _endPoint.position; }

        public Transform Transform => transform;

        private List<IInteractionalObject> _onObjects;

        private void Awake()
        {
            _onObjects = new List<IInteractionalObject>();
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public void SpawnInteractionalObjectOnPlatform(IInteractionalObject obj)
        {
            if (obj == null)
                return;
            if(obj is NPCCar car)
            {
                car.Transform.position = transform.position;
                car.PlaceOnLine(Random.Range(0, 3) != 0);
            }
            else if(obj is PlayerCar)
            {
                obj.Transform.position = transform.position;
            }
        }

        public void Active()
        {
            transform.rotation = Quaternion.Euler(Vector3.up * -90);
            gameObject.SetActive(true);
        }

        public void DeActive()
        {
            foreach (IInteractionalObject obj in _onObjects)
                obj.DeActive();
            _onObjects.Clear();
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            SignalBus<SignalPlatformDeActive, BasePlatform>.Instance.Fire(this);
        }

        public void Interact(IInteractionalObject obj)
        {
        }

        public void EndInteract(IInteractionalObject obj)
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            BaseCar obj = other.GetComponent<BaseCar>();
            if (obj)
            {
                if (obj is PlayerCar)
                {
                    SignalBus<SignalPlayerCarPassedOver, BasePlatform>.Instance.Fire(this);
                }
                if (_onObjects.Contains(obj) || obj is PlayerCar)
                    return;
                _onObjects.Add(obj);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            BaseCar obj = other.GetComponent<BaseCar>();
            if (obj)
            {
                _onObjects.Remove(obj);
            }
        }

    }
}
