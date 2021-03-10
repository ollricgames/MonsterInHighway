namespace Base.Game.GameObject.Interactional
{
    using Base.Game.Signal;
    using UnityEngine;
    public class NPCCar : BaseCar
    {
        [SerializeField] private GameObject _chassis = null;
        [SerializeField] private GameObject _crashedForm = null;
        private Vector3 _rotTarget;
        public int LineNumber { get; set; }

        public void PlaceOnLine(int lineNumber)
        {
            if (lineNumber > 1)
            {
                _rightLine = Vector3.right * 5.75f;
                _leftLine = Vector3.right * 2f;
                _target = lineNumber == 3 ? _rightLine : _leftLine;
                transform.rotation = Quaternion.Euler(Vector3.zero);
                _rotTarget = Vector3.zero;
                LineNumber = lineNumber;
            }
            else
            {
                _rightLine = Vector3.right * -5.5f;
                _leftLine = Vector3.right * -1.9f;
                _target = lineNumber == 1 ? _rightLine : _leftLine;
                transform.rotation = Quaternion.Euler(Vector3.up * 180);
                _rotTarget = Vector3.up * 180;
                LineNumber = lineNumber;
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
            _acceleration = _defaultAcceleration;
            _handbrake = 0f;
            if (_crashedForm)
            {
                _chassis.SetActive(true);
                _crashedForm.SetActive(false);
            }
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
            if (_crashedForm)
            {
                _chassis.SetActive(false);
                _crashedForm.SetActive(true);
            }
            _acceleration = _defaultAcceleration * -1;
            _handbrake = 1f;
        }
    }
}