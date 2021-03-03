namespace Base.Game.GameObject.Interactional
{
    using Base.Game.Signal;
    using UnityEngine;
    public class NPCCar : BaseCar
    {
        [SerializeField] private Transform _chassisTransform = null;
        private Vector3 _rotTarget;
        protected override void Initialize()
        {
            base.Initialize();
        }

        public void PlaceOnLine(bool isForwardLine)
        {
            if (isForwardLine)
            {
                _rightLine = Vector3.right * 5.75f;
                _leftLine = Vector3.right * 2f;
                _target = Random.Range(0,2) == 0 ? _rightLine : _leftLine;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                _rotTarget = Vector3.zero;
            }
            else
            {
                _rightLine = Vector3.right * -5.5f;
                _leftLine = Vector3.right * -1.9f;
                _target = Random.Range(0, 2) == 0 ? _rightLine : _leftLine;
                transform.rotation = Quaternion.Euler(Vector3.up * 180);
                _rotTarget = Vector3.up * 180;
            }
        }

        private void OnEnable()
        {
            _defaultAcceleration = Random.Range(0.75f, 1f);
            _acceleration = _defaultAcceleration;
        }

        private void OnDisable()
        {
            SignalBus<SignalNPCCarDeActive, NPCCar>.Instance.Fire(this);
        }

        public override void Active()
        {
            base.Active();
            _acceleration = .5f;
            _handbrake = 0f;
            _chassisTransform.localScale = Vector3.one;
        }

        protected override void Move()
        {
            base.Move();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.x, transform.position.y, transform.position.z), .1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_rotTarget), .5f);
        }

        private void FixedUpdate()
        {
            //KeepInLine();
            Move();
        }

        public void Crashed(float _squeezeValue)
        {
            _chassisTransform.localScale = new Vector3(1f, _squeezeValue, 1f);
            _acceleration = _defaultAcceleration * -1;
            _handbrake = 1f;
        }
    }
}