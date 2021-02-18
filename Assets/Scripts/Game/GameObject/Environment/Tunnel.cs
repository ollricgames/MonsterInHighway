namespace Base.Game.GameObject.Environment
{
    using Base.Game.GameObject.Interactable;
    using Base.Game.GameObject.Interactional;
    using Base.Game.Signal;
    using UnityEngine;
    [RequireComponent(typeof(Collider))]
    public class Tunnel : MonoBehaviour, IInteractableObject
    {
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
            if(obj is PlayerCar car)
            {
                car.CarExitTunnel();
            }
        }

        public void Interact(IInteractionalObject obj)
        {
            if(obj is PlayerCar car)
            {
                car.CarEnterTunnel();
            }
        }
    }
}