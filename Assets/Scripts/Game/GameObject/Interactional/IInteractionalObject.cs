namespace Base.Game.GameObject.Interactional
{
    using Base.Game.GameObject.Interactable;
    using UnityEngine;
    public interface IInteractionalObject
    {
        Transform Transform { get; }
        void Active();
        void DeActive();
        void Interact(IInteractableObject obj);
    }
}
