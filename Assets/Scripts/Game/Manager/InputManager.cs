namespace Base.Game.Manager
{
    using Base.Game.Signal;
    using UnityEngine;
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick = null;
        private bool _gameStarted = false;
        private void Update()
        {
            if (!_joystick)
                return;
            if(_joystick.Horizontal <= -.25f)
                SignalBus<SignalLineChange, bool>.Instance.Fire(true);
            else
                SignalBus<SignalLineChange, bool>.Instance.Fire(false);

            if(_joystick.Vertical >= .25f)
                SignalBus<SignalCarChassisUp, bool>.Instance.Fire(true);
            else if(!(_joystick.Vertical <= -.25f))
                SignalBus<SignalCarChassisUp, bool>.Instance.Fire(false);
            if (_joystick.Vertical <= -.25f)
                SignalBus<SignalCarBrake, bool>.Instance.Fire(true);
            else
                SignalBus<SignalCarBrake, bool>.Instance.Fire(false);
            if (Input.GetMouseButtonDown(0) && !_gameStarted)
            {
                _gameStarted = true;
                SignalBus<SignalStartGame>.Instance.Fire();
            }

        }

    }
}
