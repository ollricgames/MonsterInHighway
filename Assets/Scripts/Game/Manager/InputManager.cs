namespace Base.Game.Manager
{
    using Base.Game.Signal;
    using UnityEngine;
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick = null;

        private void Update()
        {
            if (!_joystick)
                return;
            if(_joystick.Horizontal <= -.5f)
                SignalBus<SignalLineChange, bool>.Instance.Fire(true);
            else
                SignalBus<SignalLineChange, bool>.Instance.Fire(false);

            if(_joystick.Vertical >= .5f)
                SignalBus<SignalCarChassisUp, bool>.Instance.Fire(true);
            else
                SignalBus<SignalCarChassisUp, bool>.Instance.Fire(false);

        }

    }
}
