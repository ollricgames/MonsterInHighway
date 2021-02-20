﻿namespace Base.Game.Manager
{
    using Base.Game.Factory;
    using Base.Game.GameObject.Environment;
    using Base.Game.GameObject.Interactable;
    using Base.Game.GameObject.Interactional;
    using Base.Game.Signal;
    using Base.Util;
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GameManager : MonoBehaviour
    {
        private IFactory<PlayerCar> _playerCarFactory;
        private IFactory<NPCCar> _npcCarFactory;
        private IFactory<BasePlatform> _platformFactory;

        private List<IInteractionalObject> _interactionalObjectInGame;
        private List<IInteractableObject> _interactableObjectInGame;


        private int _totalActivatePlatformCount;
        private BasePlatform _lastPlatform;
        private List<BasePlatform> _playerCarPassedOverPlatforms;

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            _interactionalObjectInGame = new List<IInteractionalObject>();
            _interactableObjectInGame = new List<IInteractableObject>();
            _playerCarPassedOverPlatforms = new List<BasePlatform>();
            Registration();
            SetFactories();
            InitialMap();
        }

        private void OnDestroy()
        {
            UnRegistration();
        }

        private void Registration()
        {
            SignalBus<SignalPlayerCarPassedOver, BasePlatform>.Instance.Register(OnPlayerCarPassedOver);
            SignalBus<SignalPlatformDeActive, BasePlatform>.Instance.Register(OnPlatformDeActive);
        }

        private void UnRegistration()
        {
            SignalBus<SignalPlatformDeActive, BasePlatform>.Instance.UnRegister(OnPlatformDeActive);
            SignalBus<SignalPlayerCarPassedOver, BasePlatform>.Instance.UnRegister(OnPlayerCarPassedOver);
        }

        private void OnPlatformDeActive(BasePlatform obj)
        {
            _playerCarPassedOverPlatforms.Remove(obj);
        }

        private int _lastSpawnedOfPlatformCount = 0;
        private int _count = 0;
        private void OnPlayerCarPassedOver(BasePlatform obj)
        {
            if (_playerCarPassedOverPlatforms.Contains(obj))
                return;
            _playerCarPassedOverPlatforms.Add(obj);
            if(_playerCarPassedOverPlatforms.Count > 5)
            {
                BasePlatform newPlatform = _platformFactory.GetObject();
                newPlatform.SetPosition(_lastPlatform.EndPoint);
                newPlatform.Active();
                _lastPlatform = newPlatform;
                NPCCar npcCar = null;
                if(UnityEngine.Random.Range(0, 2) == 0)
                {
                    npcCar = _npcCarFactory.GetObject();
                    npcCar.Active();
                }
                _lastPlatform.SpawnInteractionalObjectOnPlatform(npcCar);
                BasePlatform newPlat = _playerCarPassedOverPlatforms[0];
                newPlat.DeActive();
                _lastSpawnedOfPlatformCount = 0;
            }
            _lastSpawnedOfPlatformCount++;
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
            _interactableObjectInGame.Add(prevPlatform);

            for(int i = 0; i < Constant.defaultPlatformInPool; i++)
            {
                BasePlatform nextPlatform = _platformFactory.GetObject();
                nextPlatform.SetPosition(prevPlatform.EndPoint);
                prevPlatform = nextPlatform;
                prevPlatform.Active();
                _interactableObjectInGame.Add(prevPlatform);
                _lastPlatform = prevPlatform;
            }
            _totalActivatePlatformCount += Constant.defaultPlatformInPool;
            PlayerCar playerCar = _playerCarFactory.GetObject();
            (_interactableObjectInGame[1] as BasePlatform).SpawnInteractionalObjectOnPlatform(playerCar);
            playerCar.Active();
            _interactionalObjectInGame.Add(playerCar);
            for(int i = 2; i < _interactableObjectInGame.Count; i++)
            {
                NPCCar npcCar = null;
                if (UnityEngine.Random.Range(0, 2) == 0)
                {
                    npcCar = _npcCarFactory.GetObject();
                    npcCar.Active();
                    _interactionalObjectInGame.Add(npcCar);
                }
                (_interactableObjectInGame[i] as BasePlatform).SpawnInteractionalObjectOnPlatform(npcCar);
            }
        }


    }
}