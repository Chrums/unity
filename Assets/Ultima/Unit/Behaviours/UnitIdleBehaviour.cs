using System;
using Fizz6.Actor;

namespace Ultima.Unit.Behaviours
{
    [Serializable]
    public class UnitIdleBehaviour : UnitBehaviour
    {
        public override bool Query() => true;
        public override bool Yield(IBehaviour behaviour) => true;
    }
}