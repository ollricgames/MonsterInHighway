namespace Base.Game.GameObject.Environment
{
    using Base.Game.GameObject.Interactable;
    using Base.Game.GameObject.Interactional;
    using Base.Game.Signal;
    using System.Collections.Generic;
    using UnityEngine;

    [RequireComponent(typeof(Collider))]
    public class BasePlatform : MonoBehaviour, IInteractableObject
    {
        [SerializeField] private Transform _endPoint = null;
        public Vector3 EndPoint { get => _endPoint.position; }

        public Transform Transform => transform;

        private List<NPCCar> _onObjects;

        public bool IsEmpty { get 
            {
                if (_playerIn)
                    return false;
                foreach (int i in _lineStatu)
                {
                    if (i == 0)
                        return true;
                }
                return false;
            } 
        }
        private bool _playerIn = false;

        private int[] _lineStatu = new int[4];

        private void Awake()
        {
            _onObjects = new List<NPCCar>();
            for(int i = 0; i < _lineStatu.Length-1; i++)
            {
                _lineStatu[i] = 0;
            }
        }

        public void SetPosition(Vector3 pos)
        {
            transform.position = pos;
        }

        public void SpawnInteractionalObjectOnPlatform(IInteractionalObject obj)
        {
            if (obj == null)
                return;
            if(obj is NPCCar car)
            {
                car.Transform.position = transform.position + (Vector3.right * (Random.Range(-3, 3))) + (Vector3.up * 3f);
                int rand = Random.Range(0, _lineStatu.Length);
                while (_lineStatu[rand] != 0)
                {
                    rand = Random.Range(0, _lineStatu.Length);
                }
                car.PlaceOnLine(rand);
                _onObjects.Add(car);
            }
            else if(obj is PlayerCar)
            {
                obj.Transform.position = transform.position;
                _playerIn = true;
            }
        }

        public void Active()
        {
            transform.rotation = Quaternion.Euler(Vector3.up * -90);
            _playerIn = false;
            gameObject.SetActive(true);
        }

        public void DeActive()
        {
            foreach (IInteractionalObject obj in _onObjects)
                obj.DeActive();
            _onObjects.Clear();
            gameObject.SetActive(false);
        }


        private void OnDisable()
        {
            SignalBus<SignalPlatformDeActive, BasePlatform>.Instance.Fire(this);
        }

        public void Interact(IInteractionalObject obj)
        {
        }

        public void EndInteract(IInteractionalObject obj)
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            BaseCar obj = other.GetComponent<BaseCar>();
            if (obj)
            {
                if (obj is PlayerCar)
                {
                    SignalBus<SignalPlayerCarPassedOver, BasePlatform>.Instance.Fire(this);
                    _playerIn = true;
                }else if(obj is NPCCar npcCar)
                {
                    if (_onObjects.Contains(npcCar))
                        return;
                    _lineStatu[(obj as NPCCar).LineNumber] = 1;
                    _onObjects.Add(npcCar);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            BaseCar obj = other.GetComponent<BaseCar>();
            if (obj)
            {
                if (obj is PlayerCar)
                    _playerIn = false;
                else if(obj is NPCCar npcCar)
                {
                    _onObjects.Remove(npcCar);
                    if(!_onObjects.Find(x => x.LineNumber == npcCar.LineNumber))
                    {
                        _lineStatu[npcCar.LineNumber] = 0;
                    }
                }
            }
        }

    }
}
