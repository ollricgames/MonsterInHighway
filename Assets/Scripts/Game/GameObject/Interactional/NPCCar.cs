namespace Base.Game.GameObject.Interactional
{
    using Base.Game.Signal;
    using UnityEngine;
    public class NPCCar : BaseCar
    {
        [Range(0,1)][SerializeField] private float _defaultAcceleration = .3f;

        protected override void Initialize()
        {
            base.Initialize();
            _acceleration = _defaultAcceleration;
        }

        public void PlaceOnLine(bool isRightLine)
        {
            _target = isRightLine ? _rightLine : _leftLine;
            transform.rotation = Quaternion.Euler(Vector3.up * 180);
        }

        public override void DeActive()
        {
            base.DeActive();
            SignalBus<SignalNPCCarDeActive, NPCCar>.Instance.Fire(this);
        }

        private void FixedUpdate()
        {
            KeepInLine();
            Move();
        }
    }
}