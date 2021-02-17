namespace Base.Game.GameObject
{
    using UnityEngine;
    public class ShockAbsorber : MonoBehaviour
    {
        [SerializeField] private Transform _target = null;
        [SerializeField] private bool _isHorizontalPart = false;

        private void FixedUpdate()
        {
            float distance = (_isHorizontalPart ? Mathf.Abs(transform.position.x - _target.position.x) : Mathf.Abs(transform.position.y - _target.position.y)) / 2;
            transform.localScale = new Vector3(transform.localScale.x, distance < 0.1f ? 0.1f : distance, transform.localScale.z);
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }
    }
}
