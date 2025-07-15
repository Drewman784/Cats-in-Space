using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Example1
{
    //this buff effects a unit's action points for a given amount of time
    [CreateAssetMenu]
    public class ActionBuff : Buff
    {
        public int Factor;

        public override void Apply(Unit unit)
        {
            unit.ActionPoints += Factor;
        }

        public override void Undo(Unit unit)
        {
            unit.ActionPoints -= Factor;
        }
    }
}

