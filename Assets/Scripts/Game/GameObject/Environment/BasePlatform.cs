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
        [SerializeField] private Transform _endPoint;
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

        public void Active()
        {
            gameObject.SetActive(true);
        }

        public void DeActive()
        {
            foreach (IInteractionalObject obj in _onObjects)
                obj.DeActive();
            _onObjects.Clear();
            gameObject.SetActive(false);
        }

        public void Interact(IInteractionalObject obj)
        {
            if (_onObjects.Contains(obj))
                return;
            _onObjects.Add(obj);
        }

        public void EndInteract(IInteractionalObject obj)
        {
            _onObjects.Remove(obj);
        }
    }
}
