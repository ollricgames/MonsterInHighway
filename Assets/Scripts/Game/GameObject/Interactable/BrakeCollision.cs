namespace Base.Game.GameObject.Interactable
{
    using Base.Game.GameObject.Interactional;
    using UnityEngine;
    public class BrakeCollision : MonoBehaviour
    {
        private BaseCar _ownCar;

        private void Awake()
        {
            _ownCar = GetComponentInParent<BaseCar>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (!_ownCar || other.CompareTag("Brake"))
                return;
            BaseCar obj = other.GetComponentInParent<BaseCar>();
            if (obj != null && obj is BaseCar car && car != _ownCar)
            {
                _ownCar.Brake();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_ownCar || other.CompareTag("Brake"))
                return;
            BaseCar obj = other.GetComponentInParent<BaseCar>();
            if (obj != null && obj is BaseCar car && car != _ownCar)
            {
                _ownCar.BrakeOff();
            }
        }
    }

}
