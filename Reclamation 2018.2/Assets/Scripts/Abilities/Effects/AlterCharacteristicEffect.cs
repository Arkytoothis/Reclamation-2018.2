using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.Abilities
{
    [System.Serializable]
    public class AlterCharacteristicEffect : AbilityEffect
    {
        public CharacteristicType CharacteristicType;
        public int Characteristic;
        public GameValue Amount;
        public GameValue Duration;

        public AlterCharacteristicEffect()
        {
            EffectType = AbilityEffectType.Alter_Characteristic;
            CharacteristicType = CharacteristicType.None;
            Characteristic = 0;
            Amount = new GameValue();
            Duration = new GameValue();
        }

        public AlterCharacteristicEffect(CharacteristicType type, int characteristic, GameValue amount, GameValue duration)
        {
            EffectType = AbilityEffectType.Alter_Characteristic;
            CharacteristicType = type;
            Characteristic = characteristic;

            if (amount != null) Amount = new GameValue(amount);
            if (duration != null) Duration = new GameValue(duration);
        }

        public AlterCharacteristicEffect(AlterCharacteristicEffect effect)
        {
            EffectType = AbilityEffectType.Alter_Characteristic;
            CharacteristicType = effect.CharacteristicType;
            Characteristic = effect.Characteristic;
            if (effect.Amount != null) Amount = new GameValue(effect.Amount);
            if (effect.Duration != null) Duration = new GameValue(effect.Duration);
        }

        public override string GetTooltipString()
        {
            string s = "";

            if (Amount.Number >= 0)
                s += "+" + Amount.ToString();
            else
                s += Amount.ToString();

            if (CharacteristicType == CharacteristicType.Base_Attribute)
            {
                s += " " + Database.BaseAttributes[Characteristic].Name;
            }
            else if (CharacteristicType == CharacteristicType.Derived_Attribute)
            {
                s += " " + Database.DerivedAttributes[Characteristic].Name;
            }
            else if (CharacteristicType == CharacteristicType.Skill)
            {
                s += " " + Database.Skills[Characteristic].Name;
            }
            else if (CharacteristicType == CharacteristicType.Resistance)
            {
                s += "% " + Database.DamageTypes[Characteristic].Name + " Resistance";
            }

            if (Duration.Number != 0)
            {
                s += " for " + Duration.ToString() + " turns";
            }

            return s;
        }
    }
}