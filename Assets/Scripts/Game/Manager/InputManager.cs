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
            SignalBus<SignalJoystickMultipier, float, float>.Instance.Fire(_joystick.Horizontal, _joystick.Vertical);
        }

    }
}
