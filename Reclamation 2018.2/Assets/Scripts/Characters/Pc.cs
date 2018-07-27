using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Reclamation.Misc;
using Reclamation.Name;
using Reclamation.Equipment;

namespace Reclamation.Characters
{
    public enum PcStatus
    {
        Idle, Adventuring, Resting, Camping, Working, Training,
        Number, None
    }

    [System.Serializable]
    public class Pc : BaseCharacter
    {
        public UpkeepData Upkeep;
        public int Wealth;
        public PcStatus Status;
        public int EncounterIndex;
        public int WorldIndex;
        public int PartyIndex;
        public int PartySlot;

        public int Level;

        private int experience;
        public int Experience { get { return experience; } }
        private int expToLevel;
        public int ExpToLevel { get { return expToLevel; } }
        private int maxExp;
        public int MaxExp { get { return maxExp; } }
        private float expBonus;
        public float ExpBonus { get { return expBonus; } }

        public CharacterAbilities Abilities;

        public int MaxAccessories;

        public Pc()
        {
            Wealth = 0;
            Upkeep = new UpkeepData();
            Name = new FantasyName();
            Gender = Gender.None;
            Background = null;
            Status = PcStatus.Idle;

            RaceKey = "";
            ProfessionKey = "";
            Description = "";

            EncounterIndex = -1;
            WorldIndex = -1;
            PartyIndex = -1;
            Hair = -1;
            Beard = -1;
            MaxAccessories = 1;

            Level = 0;
            experience = 0;
            expToLevel = 0;
            maxExp = 0;
            expBonus = 0;

            BaseAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                BaseAttributes.Add(new Characteristic(CharacteristicType.Base_Attribute, i, GameSettings.AttributeExpCost));
            }

            DerivedAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                DerivedAttributes.Add(new Characteristic(CharacteristicType.Derived_Attribute, i, 0));
            }

            Skills = new List<Characteristic>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                Skills.Add(new Characteristic(CharacteristicType.Skill, i, GameSettings.SkillExpCost));
            }

            Resistances = new List<Characteristic>();
            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                Resistances.Add(new Characteristic(CharacteristicType.Resistance, i, 0));
            }

            Abilities = new CharacterAbilities();
            Inventory = new CharacterInventory();
        }

        public Pc(FantasyName name, Gender gender, Background background, string race, string profession, int hair, int beard, int index, int enc_index,
            int power_slots, int spell_slots)
        {
            Wealth = 0;
            Upkeep = new UpkeepData();
            Name = name;
            Background = new Background(background);

            Gender = gender;
            RaceKey = race;
            ProfessionKey = profession;
            WorldIndex = index;
            EncounterIndex = enc_index;
            PartyIndex = -1;
            Hair = hair;
            Beard = beard;

            MaxAccessories = Random.Range(1, 4);

            Level = 0;
            experience = 0;
            expToLevel = 0;
            maxExp = 0;
            expBonus = 0.0f;

            BaseAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                BaseAttributes.Add(new Characteristic(CharacteristicType.Base_Attribute, i, GameSettings.AttributeExpCost));
            }

            DerivedAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                DerivedAttributes.Add(new Characteristic(CharacteristicType.Derived_Attribute, i, 0));
            }

            Skills = new List<Characteristic>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                Skills.Add(new Characteristic(CharacteristicType.Skill, i, GameSettings.SkillExpCost));
            }

            Resistances = new List<Characteristic>();
            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                Resistances.Add(new Characteristic(CharacteristicType.Resistance, i, 0));
            }

            Abilities = new CharacterAbilities(this, power_slots, spell_slots);
            Inventory = new CharacterInventory();
        }

        public Pc(Pc pc)
        {
            Name = pc.Name;
            Gender = pc.Gender;

            Wealth = pc.Wealth;
            Upkeep = new UpkeepData(pc.Upkeep);
            Background = new Background(pc.Background);
            RaceKey = pc.RaceKey;
            ProfessionKey = pc.ProfessionKey;
            WorldIndex = pc.WorldIndex;
            EncounterIndex = pc.EncounterIndex;
            PartyIndex = pc.PartyIndex;
            PartySlot = pc.PartySlot;
            Hair = pc.Hair;
            Beard = pc.Beard;

            Description = pc.Description;

            Level = pc.Level;
            experience = pc.Experience;
            expToLevel = pc.ExpToLevel;
            maxExp = pc.MaxExp;
            expBonus = pc.ExpBonus;

            BaseAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                BaseAttributes.Add(new Characteristic(pc.BaseAttributes[i]));
            }

            DerivedAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                DerivedAttributes.Add(new Characteristic(pc.DerivedAttributes[i]));
            }

            Skills = new List<Characteristic>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                Skills.Add(new Characteristic(pc.Skills[i]));
            }

            Resistances = new List<Characteristic>();
            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                Resistances.Add(new Characteristic(pc.Resistances[i]));
            }

            Abilities = new CharacterAbilities(pc);
            Inventory = new CharacterInventory(pc.Inventory);
        }

        public void CalculateStartingAttributes()
        {
            List<int> rolls = new List<int>((int)BaseAttribute.Number);

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                rolls.Add(Random.Range(5, 21));
            }

            if (Database.Professions[ProfessionKey].AttributePriorities.Count > 0)
            {
                rolls.Sort();
                rolls.Reverse();
                int total = 0;

                for (int i = 0; i < (int)BaseAttribute.Number; i++)
                {
                    total = rolls[(int)Database.Professions[ProfessionKey].AttributePriorities[i]];
                    total += Database.GetRace(RaceKey).StartingAttributes[i].Number;

                    if (Database.Professions[ProfessionKey].MinimumAttributes[i] > 0 &&
                        total < Database.Professions[ProfessionKey].MinimumAttributes[i])
                        total = Database.Professions[ProfessionKey].MinimumAttributes[i];

                    BaseAttributes[i].SetStart(total, 0, total);
                }
            }
            else
            {
                int total = 0;

                for (int i = 0; i < (int)BaseAttribute.Number; i++)
                {
                    total = rolls[i];
                    total += Database.GetRace(RaceKey).StartingAttributes[i].Number;

                    if (Database.Professions[ProfessionKey].MinimumAttributes[i] > 0 &&
                        total < Database.Professions[ProfessionKey].MinimumAttributes[i])
                        total = Database.Professions[ProfessionKey].MinimumAttributes[i];

                    BaseAttributes[i].SetStart(total, 0, total);
                }
            }

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                BaseAttributes[i].SetMax(BaseAttributes[i].Start + BaseAttributes[i].Modifier, true);
            }
        }

        public void CalculateDerivedAttributes()
        {
            DerivedAttributes[(int)DerivedAttribute.Armor].SetStart(0);
            DerivedAttributes[(int)DerivedAttribute.Health].SetStart(BaseAttributes[(int)BaseAttribute.Strength].Current + BaseAttributes[(int)BaseAttribute.Endurance].Current + Database.GetRace(RaceKey).HealthPerLevel.Roll(false));

            if (Database.GetRace(RaceKey).StaminaPerLevel != null)
            {
                DerivedAttributes[(int)DerivedAttribute.Stamina].SetStart(BaseAttributes[(int)BaseAttribute.Endurance].Current + BaseAttributes[(int)BaseAttribute.Willpower].Current + Database.GetRace(RaceKey).StaminaPerLevel.Roll(false));
            }

            if (Database.GetRace(RaceKey).EssencePerLevel != null)
            {
                DerivedAttributes[(int)DerivedAttribute.Essence].SetStart(BaseAttributes[(int)BaseAttribute.Intellect].Current + BaseAttributes[(int)BaseAttribute.Wisdom].Current + Database.GetRace(RaceKey).EssencePerLevel.Roll(false));
            }

            DerivedAttributes[(int)DerivedAttribute.Morale].SetStart(100);

            DerivedAttributes[(int)DerivedAttribute.Might_Attack].SetStart(BaseAttributes[(int)BaseAttribute.Strength].Current + BaseAttributes[(int)BaseAttribute.Agility].Current);
            DerivedAttributes[(int)DerivedAttribute.Finesse_Attack].SetStart(BaseAttributes[(int)BaseAttribute.Agility].Current + BaseAttributes[(int)BaseAttribute.Senses].Current);
            DerivedAttributes[(int)DerivedAttribute.Block].SetStart(BaseAttributes[(int)BaseAttribute.Endurance].Current + BaseAttributes[(int)BaseAttribute.Agility].Current);
            DerivedAttributes[(int)DerivedAttribute.Dodge].SetStart(BaseAttributes[(int)BaseAttribute.Agility].Current + BaseAttributes[(int)BaseAttribute.Senses].Current);
            DerivedAttributes[(int)DerivedAttribute.Parry].SetStart(BaseAttributes[(int)BaseAttribute.Strength].Current + BaseAttributes[(int)BaseAttribute.Agility].Current);
            DerivedAttributes[(int)DerivedAttribute.Speed].SetStart(Database.GetRace(RaceKey).BaseSpeed);
            DerivedAttributes[(int)DerivedAttribute.Perception].SetStart(BaseAttributes[(int)BaseAttribute.Senses].Current);
            DerivedAttributes[(int)DerivedAttribute.Concentration].SetStart(BaseAttributes[(int)BaseAttribute.Memory].Current);
            DerivedAttributes[(int)DerivedAttribute.Might_Damage].SetStart((BaseAttributes[(int)BaseAttribute.Strength].Current - 12));
            DerivedAttributes[(int)DerivedAttribute.Resistance].SetStart((BaseAttributes[(int)BaseAttribute.Endurance].Current - 20));
            DerivedAttributes[(int)DerivedAttribute.Finesse_Damage].SetStart((BaseAttributes[(int)BaseAttribute.Agility].Current - 12));
            DerivedAttributes[(int)DerivedAttribute.Action_Modifier].SetStart((BaseAttributes[(int)BaseAttribute.Dexterity].Current - 20) * -1);
            DerivedAttributes[(int)DerivedAttribute.Range_Modifier].SetStart((BaseAttributes[(int)BaseAttribute.Senses].Current - 20));
            DerivedAttributes[(int)DerivedAttribute.Spell_Damage].SetStart((BaseAttributes[(int)BaseAttribute.Intellect].Current - 12));
            DerivedAttributes[(int)DerivedAttribute.Duration_Modifier].SetStart((BaseAttributes[(int)BaseAttribute.Wisdom].Current - 20));
            DerivedAttributes[(int)DerivedAttribute.Spell_Attack].SetStart(BaseAttributes[(int)BaseAttribute.Intellect].Current + BaseAttributes[(int)BaseAttribute.Willpower].Current);
            DerivedAttributes[(int)DerivedAttribute.Spell_Modifier].SetStart((BaseAttributes[(int)BaseAttribute.Charisma].Current - 12));
            DerivedAttributes[(int)DerivedAttribute.Magic_Find].SetStart((BaseAttributes[(int)BaseAttribute.Memory].Current - 20));

            DerivedAttributes[(int)DerivedAttribute.Fumble].SetStart(5);
            DerivedAttributes[(int)DerivedAttribute.Graze].SetStart(10);
            DerivedAttributes[(int)DerivedAttribute.Critical_Strike].SetStart(95);
            DerivedAttributes[(int)DerivedAttribute.Perfect_Strike].SetStart(100);
            DerivedAttributes[(int)DerivedAttribute.Critical_Damage].SetStart(BaseAttributes[(int)BaseAttribute.Dexterity].Current + BaseAttributes[(int)BaseAttribute.Senses].Current);

            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                DerivedAttributes[i].SetMax(DerivedAttributes[i].Start + DerivedAttributes[i].Modifier, true);
            }
        }

        public void CalculateStartingSkills()
        {
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                Skills[i].SetStart(0, 0, 100);
            }

            for (int j = 0; j < Database.GetProfession(ProfessionKey).SkillProficiencies.Count; j++)
            {
                int skill = (int)Database.GetProfession(ProfessionKey).SkillProficiencies[j].Skill;
                int value = Database.GetProfession(ProfessionKey).SkillProficiencies[j].Value;
                int result = GameValue.Roll(new GameValue(1, 2), false) * value;

                Skills[skill].SetStart(result, 0, 100);
            }

            for (int i = 0; i < Database.GetRace(RaceKey).SkillProficiencies.Count; i++)
            {
                int skill = (int)Database.GetRace(RaceKey).SkillProficiencies[i].Skill;
                int value = Database.GetRace(RaceKey).SkillProficiencies[i].Value;

                Skills[skill].SetStart(Skills[skill].Start + value, 0, 100);
            }

            for (int i = 0; i < (int)Skill.Number; i++)
            {
                Skills[i].SetMax(Skills[i].Start + Skills[i].Modifier, true);
            }

            CalculateExpCosts();
        }

        public void CalculateResistances()
        {
            for (int i = 0; i < Database.Races[RaceKey].Resistances.Count; i++)
            {
                int resistance = (int)Database.Races[RaceKey].Resistances[i].DamageType;
                int value = Database.Races[RaceKey].Resistances[i].Value;
                Resistances[resistance].SetStart(value, 0, 100);
            }

            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                Resistances[i].SetMax(Resistances[i].Start + Resistances[i].Modifier, true);
            }
        }

        public bool CanEquip(Item item, EquipmentSlot slot)
        {
            bool canEquip = false;

            if (item.Slot != slot)
                canEquip = true;

            return canEquip;
        }

        //public GameObject CreateEncounterObject(Transform parent, int index)
        //{
        //    GameObject go = new GameObject();
        //    go.name = Name.ShortName;
        //    go.transform.SetParent(parent);
        //    go.transform.position = new Vector3(EncounterX + 0.5f, EncounterY + 0.5f, 0.8f);
        //    //go.transform.position = new Vector3(EncounterX, EncounterY, 0.8f);
        //    go.transform.Rotate(0, 0, 0);

        //    PcObject pcObject = go.AddComponent<PcObject>();
        //    pcObject.Name = Name.ShortName;
        //    pcObject.Index = index;
        //    pcObject.Type = CharacterObjectType.PC;

        //    GameObject go2 = new GameObject();
        //    go2.transform.position = new Vector3(EncounterX, EncounterY, -1);
        //    go2.transform.Rotate(0, 0, 0);
        //    go2.transform.SetParent(go.transform);

        //    SpriteRenderer sr = go2.AddComponent<SpriteRenderer>();       
        //    sr.color = Color.green;
        //    sr.sprite = SpriteManager.Instance.GetIconSprite("Small Border");
        //    sr.sortingLayerName = "Selection Indicator";

        //    SelectionIndicator indicator = go2.AddComponent<SelectionIndicator>();
        //    indicator.SpriteRenderer = sr;
        //    indicator.Deactivate();

        //    pcObject.UnitRenderer = pcObject.gameObject.AddComponent<CharacterRenderer>();
        //    pcObject.UnitRenderer.Initialize("PC", new Vector3(-0.5f, -0.5f, -1), Vector3.one, new Vector3(0, 0, 0));
        //    //pcObject.UnitRenderer.Initialize("PlayerUnit", new Vector3(0, 0, -1), Vector3.one, new Vector3(0, 0, 0));
        //    pcObject.SetupRenderer(this);

        //    Seeker seeker = go.AddComponent<Seeker>();
        //    seeker.startEndModifier.exactEndPoint = StartEndModifier.Exactness.SnapToNode;
        //    seeker.startEndModifier.exactStartPoint = StartEndModifier.Exactness.SnapToNode;
        //    seeker.CharacterObject = pcObject;

        //    CharacterPathfinder pathfinder = go.AddComponent<CharacterPathfinder>();
        //    pathfinder.canMove = true;
        //    pathfinder.canSearch = false;
        //    pathfinder.enableRotation = false;

        //    CharacterDestinationSetter dest = go.AddComponent<CharacterDestinationSetter>();

        //    pcObject.Destination = dest;
        //    pcObject.SelectionIndicator = indicator;
        //    pcObject.Pathfinder = pathfinder;
        //    pcObject.DisablePathLine();

        //    return go;
        //}

        public void CalculateAttributeModifiers()
        {
            for (int slot = 0; slot < (int)EquipmentSlot.Number; slot++)
            {
                if (Inventory.EquippedItems[slot] != null)
                {
                    Item item = Inventory.EquippedItems[slot];

                    if (item.WeaponData != null)
                    {
                        if (item.WeaponData.AttackType == AttackType.Might)
                            DerivedAttributes[(int)DerivedAttribute.Might_Attack].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Attack].Value);
                        else if (item.WeaponData.AttackType == AttackType.Finesse)
                            DerivedAttributes[(int)DerivedAttribute.Finesse_Attack].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Attack].Value);
                        else if (item.WeaponData.AttackType == AttackType.Spell)
                            DerivedAttributes[(int)DerivedAttribute.Spell_Attack].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Attack].Value);

                        DerivedAttributes[(int)DerivedAttribute.Parry].AddToModifier(item.WeaponData.Attributes[(int)WeaponAttributes.Parry].Value);
                    }

                    if (item.WearableData != null)
                    {
                        DerivedAttributes[(int)DerivedAttribute.Armor].AddToModifier(item.WearableData.Attributes[(int)WearableAttributes.Armor].Value);
                        DerivedAttributes[(int)DerivedAttribute.Block].AddToModifier(item.WearableData.Attributes[(int)WearableAttributes.Block].Value);
                        DerivedAttributes[(int)DerivedAttribute.Dodge].AddToModifier(item.WearableData.Attributes[(int)WearableAttributes.Dodge].Value);

                        for (int r = 0; r < item.WearableData.Resistances.Count; r++)
                        {
                            Resistances[(int)item.WearableData.Resistances[r].DamageType].AddToModifier(item.WearableData.Resistances[r].Value);
                        }
                    }

                    //for (int m = 0; m < item.Modifiers.Count; m++)
                    //{
                    //    for (int e = 0; e < item.Modifiers[m].Effects.Count; e++)
                    //    {
                    //        if (item.Modifiers[m].Effects[e].GetType() == typeof(AlterCharacteristicEffect))
                    //        {
                    //            AlterCharacteristicEffect effect = (AlterCharacteristicEffect)item.Modifiers[m].Effects[e];

                    //            if (effect.Type == CharacteristicType.Base_Attribute)
                    //                BaseAttributes[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //            else if (effect.Type == CharacteristicType.Derived_Attribute)
                    //                DerivedAttributes[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //            else if (effect.Type == CharacteristicType.Skill)
                    //                Skills[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //            else if (effect.Type == CharacteristicType.Resistance)
                    //                Resistances[effect.Characteristic].AddToModifier(effect.MaxValue);
                    //        }
                    //    }
                    //}
                }
            }
        }

        public void CalculateExp()
        {
            expToLevel = Level * 1000;
            maxExp = Level * 10000;
        }

        public void CalculateExpCosts()
        {
            for (int i = 0; i < BaseAttributes.Count; i++)
            {
                BaseAttributes[i].CalculateExpCost();
            }

            for (int i = 0; i < Skills.Count; i++)
            {
                Skills[i].CalculateExpCost();
            }
        }

        public void AddExperience(int amount, bool adjusted)
        {
            int expToAdd = 0;

            if (adjusted == true)
            {
                expToAdd = (int)((float)amount * Database.GetRace(RaceKey).ExpModifier);
                expToAdd += (int)((float)amount * ExpBonus);
            }
            else
            {
                expToAdd = amount;
            }

            experience += expToAdd;
        }

        public void SpendExperience(int amount)
        {
            experience -= amount;
        }

        public void LevelUp()
        {
            SpendExperience(expToLevel);
            Level++;
            CalculateExp();
            CalculateExpCosts();
            CalculateDerivedAttributes();
        }

        public void CalculateUpkeep()
        {
            Upkeep = new UpkeepData();

            Race race = Database.GetRace(RaceKey);
            Upkeep.Coin = race.Upkeep.Coin;
            Upkeep.Essence = race.Upkeep.Essence;
            Upkeep.Materials = race.Upkeep.Materials;
            Upkeep.Rations = race.Upkeep.Rations;

            Profession profession = Database.GetProfession(ProfessionKey);
            Upkeep.Coin += profession.Upkeep.Coin;
            Upkeep.Essence += profession.Upkeep.Essence;
            Upkeep.Materials += profession.Upkeep.Materials;
            Upkeep.Rations += profession.Upkeep.Rations;

            Wealth = Database.Races[RaceKey].StartingWealth.Roll(false) + Database.Professions[ProfessionKey].StartingWealth.Roll(false);
        }
    }
}