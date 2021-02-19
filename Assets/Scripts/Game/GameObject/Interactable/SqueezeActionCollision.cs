namespace Base.Game.GameObject.Interactable
{
    using Base.Game.GameObject.Interactional;
    using UnityEngine;

    public class SqueezeActionCollision : MonoBehaviour
    {
        [SerializeField] private float _maxSqueezeValue = .5f;
        private NPCCar _ownerCar;

        private void Awake()
        {
            _ownerCar = GetComponentInParent<NPCCar>();
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.GetComponentInParent<PlayerCar>())
            {
                _ownerCar?.Crashed(_maxSqueezeValue);
            }
        }

    }
}
