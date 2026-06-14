using DTools.FSM;
using JetBrains.Collections.Viewable;
using JetBrains.Core;

namespace Game.EnemyFSM
{
    public interface IWeapon
    {
        IReadonlyProperty<int> MagazineSize { get; }
        IReadonlyProperty<int> Ammo { get; }
        IReadonlyProperty<float> RateOfFire { get; }
        IReadonlyProperty<float> ReloadingDuration { get; }
        IReadonlyProperty<bool> ReloadingStatus { get; }
        
        ISource<Unit> ShotSignal { get; }
        
        void Fire();
        void Reload();
    }

    public class Weapon : ITickable, IWeapon
    {
        public IReadonlyProperty<int> MagazineSize => _magazineSize;
        public IReadonlyProperty<int> Ammo => _ammo;
        public IReadonlyProperty<float> RateOfFire => _rateOfFire;
        public IReadonlyProperty<float> ReloadingDuration => _reloadingDuration;
        public IReadonlyProperty<bool> ReloadingStatus => _reloadingStatus;

        public ISource<Unit> ShotSignal => _shotSignal;

        private readonly ViewableProperty<int> _magazineSize;
        private readonly ViewableProperty<int> _ammo;
        private readonly ViewableProperty<float> _rateOfFire;
        private readonly ViewableProperty<float> _reloadingDuration;
        private readonly ViewableProperty<bool> _reloadingStatus;
        
        private readonly Signal<Unit> _shotSignal = new();
        
        private readonly StateMachine<WeaponState> _stateMachine = new();

        public Weapon(int magazineSize, int ammo, float rateOfFire, float reloadingDuration)
        {
            _magazineSize = new(magazineSize);
            _ammo = new(ammo);
            _rateOfFire = new(rateOfFire);
            _reloadingDuration = new(reloadingDuration);
            _reloadingStatus = new(false);
            
            _stateMachine.SetStates(new WeaponState[]
            {
                new ActiveState(_ammo, _rateOfFire, _magazineSize, _shotSignal),
                new ReloadingState(_ammo, _magazineSize, _reloadingDuration, _reloadingStatus)
            });
        }

        public void Fire()
        {
            _stateMachine.CurrentState.Fire();
        }
        
        public void Reload()
        {
            _stateMachine.CurrentState.Reload();
        }

        public void Tick()
        {
            _stateMachine.CurrentState.Tick();
        }
    }
}