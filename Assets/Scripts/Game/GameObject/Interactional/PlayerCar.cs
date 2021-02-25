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
        private Rigidbody _body;

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
            _body = GetComponent<Rigidbody>();
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
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.x, transform.position.y, transform.position.z + 5), (_controller.CurrentSpeed / (_controller.MaxSpeed * 4 * 6)) + .025f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(Vector3.zero), (_controller.CurrentSpeed / (_controller.MaxSpeed / 3f * 1)) + .04f);
            
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
                BrakeOff();
            }
        }
        protected override void OnTriggerExit(Collider other)
        {
            base.OnTriggerExit(other);
            if (other.GetComponent<BasePlatform>())
            {
                if(_timer > .5f)
                    Brake();
                _onRoad = false;
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
            SignalBus<SignalJoystickMultipier, float, float>.Instance.Register(OnJoystickMultipier);
        }

        private void UnRegistration()
        {
            SignalBus<SignalJoystickMultipier, float, float>.Instance.UnRegister(OnJoystickMultipier);
        }

        private void OnJoystickMultipier(float h, float v)
        {
            if(h < -.5f)
            {
                OnLineChanged(true);
            }else if(h > .5f)
            {
                OnLineChanged(false);
            }
            if(v < -.5f)
            {
                Brake();
            }
            else
            {
                BrakeOff();
            }
            if(v > .25f)
            {
                OnChassisUp(true);
            }
            else
            {
                OnChassisUp(false);
            }
        }

        private void OnChassisUp(bool obj)
        {
            _bodyPosTarget = obj ? _defaultBodyPos + (Vector3.up) * _carBodyUpDistance : _defaultBodyPos;
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