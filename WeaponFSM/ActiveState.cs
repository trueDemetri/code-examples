using JetBrains.Collections.Viewable;
using JetBrains.Core;
using UnityEngine;

namespace Game.EnemyFSM
{
    public class ActiveState : WeaponState
    {
        private readonly ViewableProperty<int> _currentAmmo;
        private readonly IReadonlyProperty<float> _rateOfFire;
        private readonly IReadonlyProperty<int> _magazineSize;
        private readonly Signal<Unit> _shotSignal;
        private float _nextShotTime;

        public ActiveState(
            ViewableProperty<int> currentAmmo,
            IReadonlyProperty<float> rateOfFire,
            IReadonlyProperty<int> magazineSize,
            Signal<Unit> shotSignal)
        {
            _currentAmmo = currentAmmo;
            _rateOfFire = rateOfFire;
            _shotSignal = shotSignal;
            _magazineSize = magazineSize;
        }

        public override void Fire()
        {
            if (Time.time < _nextShotTime) return;
            
            // тут может быть код выстрела, но может быть пустым,
            // а сам выстрел производиться какой-то системой снаружи по этому сигналу
            _shotSignal.Fire();

            if (_currentAmmo.Value == 0)
            {
                StateMachine.SetState<ReloadingState>();
                return;
            }
            
            _nextShotTime = Time.time + 1f / _rateOfFire.Value;
        }

        public override void Reload()
        {
            if (_currentAmmo.Value < _magazineSize.Value)
            {
                StateMachine.SetState<ReloadingState>();
            }
        }
    }
}