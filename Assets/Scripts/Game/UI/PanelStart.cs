namespace Base.Game.UI.Panel
{
    using Base.Game.Signal;
    using System;
    using UnityEngine;

    public class PanelStart : MonoBehaviour
    {
        private void Awake()
        {
            Registration();
        }

        private void OnDestroy()
        {
            UnRegistration();
        }

        private void Registration()
        {
            SignalBus<SignalStartGame>.Instance.Register(OnStartGame);
        }

        private void UnRegistration()
        {
            SignalBus<SignalStartGame>.Instance.UnRegister(OnStartGame);
        }

        private void OnStartGame()
        {
            gameObject.SetActive(false);
        }
    }
}
