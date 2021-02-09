namespace Base.Game.GameObject.Interactional
{
    using Base.Game.Signal;
    using UnityEngine;
    public class NPCCar : BaseCar
    {

        protected override void Initialize()
        {
            base.Initialize();
        }

        public void PlaceOnLine(bool isRightLine)
        {
            _target = isRightLine ? _rightLine : _leftLine;
            transform.rotation = isRightLine ? Quaternion.Euler(Vector3.zero):Quaternion.Euler(Vector3.up * 180);
        }

        private void OnDisable()
        {
            SignalBus<SignalNPCCarDeActive, NPCCar>.Instance.Fire(this);
        }

        public override void Active()
        {
            base.Active();
            _acceleration = Random.Range(0f, .5f);
        }

        protected override void Move()
        {
            base.Move();
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.x, transform.position.y, transform.position.z), .1f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(_target.Equals(_rightLine) ? Vector3.zero : Vector3.up * 180), .5f);
        }

        private void FixedUpdate()
        {
            KeepInLine();
            Move();
        }
    }
}