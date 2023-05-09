using Microsoft.Xna.Framework;
using RpgLibrary.Characters;
using SharedProject.Moves;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;

namespace SharedProject.Mobs
{
    public class BasicAttack : Move
    {
        private BasicAttack(string name,
                           string description,
                           string attribute,
                           int mana,
                           int range,
                           Point spread,
                           MoveType moveType,
                           TargetType targetType,
                           Status status)
            : base(name, description, attribute, mana, range, spread, moveType, targetType, status)
        {

        }

        public override string Apply(ICharacter source, ICharacter target)
        {
            return base.Apply(source, target);
        }

        public override object Clone()
        {
            BasicAttack basicAttack = (BasicAttack)MemberwiseClone();
            return basicAttack;
        }

        public static Move CreateInstance()
        {
            Move move = new BasicAttack("basic attack",
                            "This move does basic attack without weapons.",
                            "Strength",
                            0,
                            1,
                            new(1, 5),
                            MoveType.Damage,
                            TargetType.Health,
                            Status.Normal);

            return move;
        }
    }
}
