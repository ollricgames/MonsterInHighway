namespace Base.Game.GameObject.Interactable
{
    using Base.Game.GameObject.Interactional;
    using System.Collections;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class ImpactableObject : MonoBehaviour, IInteractableObject
    {
        private Collider _collider;

        private Vector3 _startingPos;
        private Vector3 _startingRot;

        private Coroutine _routine;

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
        }

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

        public void Interact(IInteractionalObject obj)
        {
            if(obj is BaseCar car && _routine == null)
            {
                _routine = StartCoroutine(ImpactAction(car));
            }
        }

        private IEnumerator ImpactAction(BaseCar car)
        {
            Rigidbody body = gameObject.AddComponent<Rigidbody>();

            _collider.attachedRigidbody.AddExplosionForce(car.CurrentSpeed * Time.fixedDeltaTime, transform.position, 10F, 3f);
            yield return new WaitForSeconds(2f);
            Destroy(body);
            _routine = null;
        }
    }
}
