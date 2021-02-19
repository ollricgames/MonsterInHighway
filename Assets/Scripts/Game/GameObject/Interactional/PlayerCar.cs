namespace Base.Game.GameObject.Interactional
{
    using Base.Game.GameObject.Environment;
    using Base.Game.GameObject.Interactable;
    using Base.Game.Signal;
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class PlayerCar : BaseCar
    {
        [SerializeField] private GameObject _carBody = null;
        [SerializeField] private float _carBodyUpDistance = 5f;
        [SerializeField] private float _bodyUpSpeed = 1f;

        private Vector3 _defaultBodyPos;
        private Vector3 _bodyPosTarget;

        private float _timer = 0;

        private BasePlatform _onPlatform;
        private bool _onRoad = false;

        private bool _chassisUp = false;
        public bool ChassisUp { get => _chassisUp; }

        private List<Wheel> _wheels;

        protected override void Initialize()
        {
            base.Initialize();
            Registration();
            _defaultBodyPos = _carBody.transform.localPosition;
            _bodyPosTarget = _defaultBodyPos;
            _bodyUpSpeed *= Time.fixedDeltaTime;
            _wheels = new List<Wheel>();
            foreach (GameObject wheel in _controller.WheelMeshes)
            {
                _wheels.Add(wheel.GetComponent<Wheel>());
            }
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
            SceneManager.LoadScene(0);
        }

        protected override void KeepInLine()
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.x, transform.position.y, transform.position.z), (_controller.CurrentSpeed / (_controller.MaxSpeed * 4 * 6)) + .025f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), (_controller.CurrentSpeed / (_controller.MaxSpeed / 30 * 6)) + .04f);
            if (transform.rotation.y > .3f || transform.rotation.y < -.3f)
            {
                Brake();
            }
            else
            {
                BrakeOff();
            }
            foreach (Wheel wheel in _wheels)
            {
                wheel.Interact(this);
            }
            base.KeepInLine();
        }

        private void FixedUpdate()
        {
            Move();
            KeepInLine();
            SetCarBodyPosition();
            ControlOnRoad();
        }

        private void ControlOnRoad()
        {
            if(!_onRoad)
                _timer += Time.fixedDeltaTime;
            if(_timer > 8f)
            {
                transform.position = _onPlatform.transform.position;
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if(other.GetComponent<BasePlatform>())
            {
                _onPlatform = other.GetComponent<BasePlatform>();
                _onRoad = true;
                _timer = 0;
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (other.GetComponent<BasePlatform>())
            {
                _onRoad = false;
                _timer = 0;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<BasePlatform>())
            {
                _onRoad = true;
                _timer = 0;
            }
        }
        private void SetCarBodyPosition()
        {
            _carBody.transform.localPosition = Vector3.MoveTowards(_carBody.transform.localPosition, _bodyPosTarget, _bodyUpSpeed);
        }

        private void Registration()
        {
            SignalBus<SignalLineChange, bool>.Instance.Register(OnLineChanged);
            SignalBus<SignalCarChassisUp, bool>.Instance.Register(OnChassisUp);
            SignalBus<SignalCarBrake, bool>.Instance.Register(OnHandleBrake);
        }

        private void UnRegistration()
        {
            SignalBus<SignalLineChange, bool>.Instance.UnRegister(OnLineChanged);
            SignalBus<SignalCarChassisUp, bool>.Instance.UnRegister(OnChassisUp);
            SignalBus<SignalCarBrake, bool>.Instance.UnRegister(OnHandleBrake);
        }

        private void OnHandleBrake(bool obj)
        {
            if (obj)
            {
                Brake();
            }
            else
            {
                BrakeOff();
            }
        }

        private void OnChassisUp(bool obj)
        {
            _bodyPosTarget = obj ? _defaultBodyPos + (Vector3.up) * _carBodyUpDistance : _defaultBodyPos;
            //_collider.enabled = !obj;
            _chassisUp = obj;
        }

        private void OnLineChanged(bool isLeftLine)
        {
            _target = isLeftLine ? _leftLine : _rightLine;
        }

        public void CarEnterTunnel()
        {
            SignalBus<SignalCarOnTunnel, bool>.Instance.Fire(true);
        }

        public void CarExitTunnel()
        {
            SignalBus<SignalCarOnTunnel, bool>.Instance.Fire(false);
        }

    }
}