namespace Base.Game.GameObject.Interactional
{
    using Base.Game.GameObject.Interactable;
    using UnityEngine;
    public class Wheel : MonoBehaviour, IInteractableObject, IInteractionalObject
    {
        [SerializeField] private bool _isRightWheel = false;
        [SerializeField] private SuspensionDownPart _suspensionDownPart = null;
        [SerializeField] private WheelCollider _collider = null;

        [SerializeField] private Transform _defaultPos = null;


        public Transform Transform 
        {
            get
            {
                return transform;
            }
        }
        
        public void Active()
        {
            gameObject.SetActive(true);
        }

        public void DeActive()
        {
            gameObject.SetActive(false);
        }

        public void EndInteract(IInteractableObject obj)
        {

        }

        public void Interact(IInteractionalObject obj)
        {
            if (!(obj is PlayerCar))
                return;
            if((obj as PlayerCar).ChassisUp)
            {
                float target = _defaultPos.position.x + (_isRightWheel ? 1f : -1f);
                _collider.transform.position = Vector3.MoveTowards(_collider.transform.position, new Vector3(target, _collider.transform.position.y, _collider.transform.position.z), Time.deltaTime * 2);
            }
            else
            {
                if((_collider.transform.position.x != _defaultPos.position.x))
                    _collider.transform.position = Vector3.MoveTowards(_collider.transform.position, new Vector3(_defaultPos.position.x, _defaultPos.transform.position.y, _defaultPos.position.z), Time.deltaTime * 2);
            }
            _suspensionDownPart.Interact(this);
        }

        public void EndInteract(IInteractionalObject obj)
        {
        }

        public void Interact(IInteractableObject obj)
        {
        }
    }
}