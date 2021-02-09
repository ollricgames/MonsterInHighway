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

        private bool _chassisUp = false;

        protected override void Initialize()
        {
            base.Initialize();
            Registration();
            _defaultBodyPos = _carBody.transform.localPosition;
            _bodyPosTarget = _defaultBodyPos;
            _bodyUpSpeed *= Time.fixedDeltaTime;
            _collider = GetComponent<Collider>();
        }

        public override void Active()
        {
            base.Active();
            Registration();
            SignalBus<SignalPlayerCarSpawn, PlayerCar>.Instance.Fire(this);
        }

        public override void DeActive()
        {
            base.DeActive();
            UnRegistration();
        }

        public override void Brake()
        {
            if (_chassisUp)
                return;
            base.Brake();
        }

        protected override void Move()
        {
            base.Move();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.x, transform.position.y, transform.position.z), .1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), .5f);
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
            _chassisUp = obj;
        }

        private void OnLineChanged(bool isLeftLine)
        {
            _target = isLeftLine ? _leftLine : _rightLine;
        }

    }
}