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
        private CinemachineTransposer _camTransposer;

        private float _defaultOffset;

        private void Awake()
        {
            _camera = GetComponent<CinemachineVirtualCamera>();
            _camTransposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _defaultOffset = _camTransposer.m_FollowOffset.y;
            Registration();
        }

        private void OnDestroy()
        {
            UnRegistration();
        }
        
        private void Registration()
        {
            SignalBus<SignalPlayerCarSpawn, PlayerCar>.Instance.Register(OnPlayerCarSpawned);
            SignalBus<SignalCarOnTunnel, bool>.Instance.Register(OnCarOnTunnel);
        }

        private void UnRegistration()
        {
            SignalBus<SignalPlayerCarSpawn, PlayerCar>.Instance.UnRegister(OnPlayerCarSpawned);
            SignalBus<SignalCarOnTunnel, bool>.Instance.UnRegister(OnCarOnTunnel);
        }

        private void OnCarOnTunnel(bool obj)
        {
            _camTransposer.m_FollowOffset.y = obj ? 5 : _defaultOffset;
        }

        private void OnPlayerCarSpawned(PlayerCar obj)
        {
            _camera.Follow = obj.Transform;
            _camera.LookAt = obj.Transform;
        }
    }
}