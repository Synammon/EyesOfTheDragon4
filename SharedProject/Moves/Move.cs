using Microsoft.Xna.Framework;
using RpgLibrary.Characters;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;

namespace SharedProject.Moves
{
    public enum MoveType { Damage, Heal, Status }
    public enum TargetType { Health, Mana, Attribute }
    public enum Status { Normal, Poison, Paralysis }

    public abstract class Move : ICloneable
    {
        public string Name { get; protected set; }
        public string Description { get; protected set; }
        public string Attribute { get; protected set; }
        public int Mana { get; protected set; }
        public int Range { get; protected set; }
        public Point Spread { get; protected set; }
        public MoveType MoveType { get; protected set; }
        public TargetType TargetType { get; protected set; }
        public Status Status { get; protected set; }

        protected Move(string name,
                       string description,
                       string attribute,
                       int mana,
                       int range,
                       Point spread,
                       MoveType moveType,
                       TargetType targetType,
                       Status status)
        {
            Name = name;
            Description = description;
            Attribute = attribute;
            Mana = mana;
            Range = range;
            Spread = spread;
            MoveType = moveType;
            TargetType = targetType;
            Status = status;
        }

        public virtual string Apply(ICharacter source, ICharacter target)
        {
            if (source.Mana.Current <= Mana)
            {
                return $"Not enough mana to use {Name.ToLower()}.";
            }

            source.Mana.Adjust(-Mana);

            int adjustment = GetAdjustment(source, Attribute);
            int spread = (Helper.Random.Next(Spread.X, Spread.Y) + adjustment);

            if (MoveType == MoveType.Damage)
            {

                if (spread <= 0)
                {
                    spread = 1;
                }

                target.Health.Adjust(-spread);
                return $"{source.Name} uses {Name.ToLower()}.";
            }
            else if (MoveType == MoveType.Heal)
            {
                target.Health.Adjust(spread);
                return $"{source.Name} uses {Name.ToLower()}.";
            }

            return $"Cannot use {Name} at this time.";
        }

        public static int GetAdjustment(ICharacter source, string attribute)
        {
            PropertyInfo info = source.GetType().GetProperty(attribute);

            if (info != null)
            {
                if (!int.TryParse(info.GetValue(source, null).ToString(), out int adjustment))
                {
                    return 0;
                }

                switch (adjustment)
                {
                    case 0:
                    case 1:
                    case 2:
                        return -1;
                    case 3:
                    case 4:
                    case 5:
                        return 0;
                    case 6:
                    case 7:
                        return 1;
                    case 8:
                    case 9:
                        return 2;
                    default: 
                        return 3;
                }
            }

            return 0;
        }

        public abstract object Clone();
    }
}
