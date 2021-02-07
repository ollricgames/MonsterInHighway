namespace Base.Game.GameObject.Interactional
{
    using Base.Game.Signal;
    using System;
    using UnityEngine;
    public class PlayerCar : BaseCar
    {
        [SerializeField] private GameObject _carBody = null;
        [SerializeField] private float _carBodyUpDistance = 5f;
        [SerializeField] private float _bodyUpSpeed = 1f;

        private Vector3 _defaultBodyPos;
        private Vector3 _bodyPosTarget;

        private Collider _collider;

        protected override void Initialize()
        {
            base.Initialize();
            Registration();
            _acceleration = 1f;
            _defaultBodyPos = _carBody.transform.localPosition;
            _bodyPosTarget = _defaultBodyPos;
            _bodyUpSpeed *= Time.fixedDeltaTime;
            _collider = GetComponent<Collider>();
        }

        public override void DeActive()
        {
            base.DeActive();
            UnRegistration();
        }

        private void FixedUpdate()
        {
            Move();
            KeepInLine();
            SetCarBodyPosition();
        }

        private void SetCarBodyPosition()
        {
            _carBody.transform.localPosition = Vector3.MoveTowards(_carBody.transform.localPosition, _bodyPosTarget, _bodyUpSpeed);
        }

        private void Registration()
        {
            SignalBus<SignalLineChange, bool>.Instance.Register(OnLineChanged);
            SignalBus<SignalCarChassisUp, bool>.Instance.Register(OnChassisUp);
        }

        private void UnRegistration()
        {
            SignalBus<SignalLineChange, bool>.Instance.UnRegister(OnLineChanged);
            SignalBus<SignalCarChassisUp, bool>.Instance.UnRegister(OnChassisUp);
        }

        private void OnChassisUp(bool obj)
        {
            _bodyPosTarget = obj ? _defaultBodyPos + (Vector3.up) * _carBodyUpDistance : _defaultBodyPos;
            _collider.enabled = !obj;
        }

        private void OnLineChanged(bool isLeftLine)
        {
            _target = isLeftLine ? _leftLine : _rightLine;
        }

    }
}