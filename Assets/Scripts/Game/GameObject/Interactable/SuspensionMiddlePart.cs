namespace Base.Game.GameObject.Interactable
{
    using Base.Game.GameObject.Interactional;
    using UnityEngine;
    public class SuspensionMiddlePart : MonoBehaviour
    {
        [SerializeField] private Transform _target = null;
        public Transform Transform => transform;

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

        public void Interact(IInteractableObject obj)
        {
            transform.LookAt(_target);
            transform.position = obj.Transform.position;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 4 * Vector3.Distance(transform.position, _target.position));
        }
    }
}