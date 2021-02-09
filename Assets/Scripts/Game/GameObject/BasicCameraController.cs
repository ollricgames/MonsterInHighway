namespace Base.Game.GameObject
{
    using Base.Game.GameObject.Interactional;
    using Base.Game.Signal;
    using Cinemachine;
    using System;
    using UnityEngine;

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class BasicCameraController : MonoBehaviour
    {
        private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            Registration();
        }

        private void OnDestroy()
        {
            UnRegistration();
        }
        
        private void Registration()
        {
            SignalBus<SignalPlayerCarSpawn, PlayerCar>.Instance.Register(OnPlayerCarSpawned);
        }

        private void UnRegistration()
        {
            SignalBus<SignalPlayerCarSpawn, PlayerCar>.Instance.UnRegister(OnPlayerCarSpawned);
        }

        private void OnPlayerCarSpawned(PlayerCar obj)
        {
            _camera.Follow = obj.Transform;
            _camera.LookAt = obj.Transform;
        }
    }
}