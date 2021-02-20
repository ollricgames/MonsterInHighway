namespace Base.Game.UI.Button
{
    using Base.Game.Signal;
    using UnityEngine;
    using UnityEngine.EventSystems;

    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class BtnStartGame : MonoBehaviour, IPointerClickHandler
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            SignalBus<SignalStartGame>.Instance.Fire();
        }
    }
}