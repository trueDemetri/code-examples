using JetBrains.Collections.Viewable;
using UnityEngine;

namespace Game.WeaponFSM
{
    public class ReloadingState : WeaponState
    {
        private readonly ViewableProperty<int> _currentAmmo;
        private readonly IReadonlyProperty<int> _magazineSize;
        private readonly IReadonlyProperty<float> _reloadingDuration;
        private readonly ViewableProperty<bool> _reloadingStatus;

        private float _reloadingCompletedTime;

        public ReloadingState(
            ViewableProperty<int> currentAmmo,
            IReadonlyProperty<int> magazineSize, 
            IReadonlyProperty<float> reloadingDuration,
            ViewableProperty<bool> reloadingStatus)
        {
            _currentAmmo = currentAmmo;
            _magazineSize = magazineSize;
            _reloadingDuration = reloadingDuration;
            _reloadingStatus = reloadingStatus;
        }

        public override void Tick()
        {
            if (Time.time >= _reloadingCompletedTime)
            {
                _currentAmmo.Value = _magazineSize.Value;
                _reloadingStatus.Value = false;
                StateMachine.SetState<ActiveState>();
            }
        }

        protected override void OnEnter(WeaponState prevState, object arg)
        {
            _reloadingStatus.Value = true;
            _reloadingCompletedTime = Time.time + _reloadingDuration.Value;
        }
    }
}