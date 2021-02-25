namespace Base.Game.GameObject.Interactable
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


        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponentInParent<BaseCar>() is BaseCar car && _routine == null)
            {
                _routine = StartCoroutine(ImpactAction(car));
            }
        }

        private IEnumerator ImpactAction(BaseCar car)
        {
            Rigidbody body = gameObject.AddComponent<Rigidbody>();

            _collider.attachedRigidbody.AddExplosionForce(car.CurrentSpeed * Time.fixedDeltaTime, transform.position, 10F, 3f);
            yield return new WaitForSeconds(5f);
            Destroy(body);
            _routine = null;
        }
    }
}
