namespace Base.Game.Manager
{
    using Base.Game.Factory;
    using Base.Game.GameObject.Environment;
    using Base.Game.GameObject.Interactable;
    using Base.Game.GameObject.Interactional;
    using Base.Game.Signal;
    using Base.Util;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int _platformYogunlugu = 0;
        [SerializeField] private int _arabaYogunlugu = 0;

        private IFactory<PlayerCar> _playerCarFactory;
        private IFactory<NPCCar> _npcCarFactory;
        private IFactory<BasePlatform> _platformFactory;

        private List<BasePlatform> _activePlatforms;


        private int _totalActivatePlatformCount;
        private BasePlatform _lastPlatform;
        private List<BasePlatform> _playerCarPassedOverPlatforms;
        private List<NPCCar> _npcCarsInPool;

        private PlayerCar _playerCar;

        private Coroutine _npcCarSpawnRoutine;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Constant.defaultPlatformInPool = _platformYogunlugu;
            Constant.defaultNPCCarInPool = _arabaYogunlugu;
            Initialize();
        }

        private void Initialize()
        {
            _activePlatforms = new List<BasePlatform>();
            _playerCarPassedOverPlatforms = new List<BasePlatform>();
            _npcCarsInPool = new List<NPCCar>();
            Registration();
            SetFactories();
            InitialMap();
        }

        private void OnDestroy()
        {
            UnRegistration();
        }

        private void OnApplicationQuit()
        {
            UnRegistration();
        }

        private void Registration()
        {
            SignalBus<SignalPlayerCarPassedOver, BasePlatform>.Instance.Register(OnPlayerCarPassedOver);
            SignalBus<SignalPlatformDeActive, BasePlatform>.Instance.Register(OnPlatformDeActive);
            SignalBus<SignalStartGame>.Instance.Register(OnStartGame);
            SignalBus<SignalNPCCarDeActive, NPCCar>.Instance.Register(OnNPCCarDeActive);
        }

        private void UnRegistration()
        {
            SignalBus<SignalPlatformDeActive, BasePlatform>.Instance.UnRegister(OnPlatformDeActive);
            SignalBus<SignalPlayerCarPassedOver, BasePlatform>.Instance.UnRegister(OnPlayerCarPassedOver);
            SignalBus<SignalStartGame>.Instance.UnRegister(OnStartGame);
            SignalBus<SignalNPCCarDeActive, NPCCar>.Instance.UnRegister(OnNPCCarDeActive);
        }

        private IEnumerator NPCCarSpawnerAction()
        {
            var wait2f = new WaitForSeconds(1.5f);
            var waitFixed = new WaitForFixedUpdate();

            while (true)
            {
                if (_lastPlatform.IsEmpty && (_npcCarFactory.GetTotalObject() < 11 || _npcCarsInPool.Count > 0))
                {
                    NPCCar npcCar = _npcCarFactory.GetObject();
                    npcCar.Active();
                    _npcCarsInPool.Remove(npcCar);
                    _lastPlatform.SpawnInteractionalObjectOnPlatform(npcCar);
                }
                yield return wait2f;
            }
        }

        private void OnNPCCarDeActive(NPCCar obj)
        {
            if (!_npcCarsInPool.Contains(obj))
            {
                _npcCarsInPool.Add(obj);
            }
        }

        private void OnPlatformDeActive(BasePlatform obj)
        {
            _playerCarPassedOverPlatforms.Remove(obj);
        }

        private void OnPlayerCarPassedOver(BasePlatform obj)
        {
            if (_playerCarPassedOverPlatforms.Contains(obj))
                return;
            _playerCarPassedOverPlatforms.Add(obj);
            if(_playerCarPassedOverPlatforms.Count > 3)
            {
                BasePlatform newPlatform = _platformFactory.GetObject();
                newPlatform.SetPosition(_lastPlatform.EndPoint);
                newPlatform.Active();
                _lastPlatform = newPlatform;
                BasePlatform prevPlat = _playerCarPassedOverPlatforms[0];
                prevPlat.DeActive();
                if (!_activePlatforms.Contains(newPlatform))
                {
                    _activePlatforms.Add(newPlatform);
                }
            }
        }

        private void SetFactories()
        {
            _playerCarFactory = new Factory<PlayerCar, SignalPlayerCarDeActive>.Builder()
                                    .AddPrefab("PlayerCar")
                                    .SetHandle()
                                    .Build();
            _npcCarFactory = new Factory<NPCCar, SignalNPCCarDeActive>.Builder()
                                 .AddAllPrefabOnPath("NPCCars")
                                 .SetHandle()
                                 .Build();
            _platformFactory = new Factory<BasePlatform, SignalPlatformDeActive>.Builder()
                                   .AddAllPrefabOnPath("Platforms")
                                   .SetHandle()
                                   .Build();
        }

        private void InitialMap()
        {
            BasePlatform prevPlatform = _platformFactory.GetObject();
            prevPlatform.SetPosition(Vector3.zero);
            prevPlatform.Active();
            _activePlatforms.Add(prevPlatform);

            for(int i = 0; i < Constant.defaultPlatformInPool; i++)
            {
                BasePlatform nextPlatform = _platformFactory.GetObject();
                nextPlatform.SetPosition(prevPlatform.EndPoint);
                prevPlatform = nextPlatform;
                prevPlatform.Active();
                _activePlatforms.Add(prevPlatform);
                _lastPlatform = prevPlatform;
            }
            _totalActivatePlatformCount += Constant.defaultPlatformInPool;
            _npcCarSpawnRoutine = StartCoroutine(NPCCarSpawnerAction());
        }

        private void OnStartGame()
        {
            PlayerCar playerCar = _playerCarFactory.GetObject();
            (_activePlatforms[1] as BasePlatform).SpawnInteractionalObjectOnPlatform(playerCar);
            _playerCar = playerCar;
            playerCar.Active();
        }


    }
}