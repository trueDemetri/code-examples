using DTools.FSM;

namespace Game.WeaponFSM
{
    public abstract class WeaponState : State<WeaponState>, ITickable
    {
        public virtual void Reload()
        {}
        
        public virtual void Fire()
        {}

        public virtual void Tick()
        {}
    }
}