namespace Base.Game.GameObject.Interactable
{
    using Base.Game.GameObject.Interactional;
    using UnityEngine;

    public class SuspensionDownPart : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private SuspensionMiddlePart _suspensionMiddlePart = null;
        private Transform _rawTransform;

        private void Awake()
        {
            _rawTransform = GetComponentsInChildren<Transform>()[1];
        }

        public Transform Transform => _rawTransform;

        public void Active()
        {
            gameObject.SetActive(true);
        }

        public void DeActive()
        {
            gameObject.SetActive(false);
        }

        public void EndInteract(IInteractionalObject obj)
        {

        }

        public void Interact(IInteractionalObject obj)
        {
            transform.position = obj.Transform.position;
            _suspensionMiddlePart.Interact(this);
        }
    }
}
