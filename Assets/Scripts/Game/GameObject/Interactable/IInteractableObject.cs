namespace Base.Game.GameObject.Interactable
{
    using Base.Game.GameObject.Interactional;
    using UnityEngine;
    public interface IInteractableObject
    {
        Transform Transform { get; }
        void Active();
        void DeActive();
        void Interact(IInteractionalObject obj);
        void EndInteract(IInteractionalObject obj);
    }
}
