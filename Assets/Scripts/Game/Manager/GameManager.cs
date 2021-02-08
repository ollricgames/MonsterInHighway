namespace Base.Game.Manager
{
    using Base.Game.Factory;
    using Base.Game.GameObject.Environment;
    using Base.Game.GameObject.Interactional;
    using Base.Game.Signal;
    using UnityEngine;
    public class GameManager : MonoBehaviour
    {
        private IFactory<PlayerCar> _playerCarFactory;
        private IFactory<NPCCar> _npcCarFactory;
        private IFactory<BasePlatform> _platformFactory;
        private void Initialize()
        {
            
        }

        private void SetFactories()
        {
            _playerCarFactory = new Factory<PlayerCar, SignalPlayerCarDeActive>.Builder()
                                    .SetPrefab("PlayerCar")
                                    .SetHandle()
                                    .Build();
            _npcCarFactory = new Factory<NPCCar, SignalNPCCarDeActive>.Builder()
                                 .SetPrefab("NPCCar")
                                 .SetHandle()
                                 .Build();

        }

    }
}