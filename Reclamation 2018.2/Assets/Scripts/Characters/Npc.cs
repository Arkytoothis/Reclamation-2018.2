using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Reclamation.Abilities;
using Reclamation.Name;
using Reclamation.Characters;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    public enum CombatStatus { Awake, Unconcisous, Dead, Number, None }

    public enum NpcType
    {
        Boss, Mini_Boss, Boss_Guard, Objective_Enemy, Powerful_Enenmy, Enemy, Weak_Enemy,
        Citizen, Rescue_Target, Survivor, Neutral, Hireling, Story, Trader,
        Number, None
    }

    public class NPC : BaseCharacter
    {
        public NPCImageType ImageType;
        public string Sprite;
        public string Key;
        public int NPCIndex;
        public int PartyIndex;
        public int PartySlot;

        public int Level;
        public int ExpValue;

        public List<Ability> Abilities;

        //StatBarWidgetWorld healthBar;
        //StatBarWidgetWorld actionsBar;

        CombatStatus combatStatus;
        public CombatStatus CombatStatus { get { return combatStatus; } }

        SpriteRenderer statusSprite;
        public SpriteRenderer StatusSprite { get { return statusSprite; } }

        public NPC()
        {
            Name = new FantasyName();
            Gender = Gender.None;
            Background = null;

            Key = "";
            ImageType = NPCImageType.None;
            Sprite = "";
            combatStatus = CombatStatus.None;
            RaceKey = "";
            ProfessionKey = "";
            NPCIndex = -1;
            PartyIndex = -1;
            Hair = -1;
            Beard = -1;

            Level = 0;
            ExpValue = 0;

            Description = "";

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

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory();

            //healthBar = null;
            //actionsBar = null;
            statusSprite = null;
        }

        public NPC(FantasyName name, string key, Gender gender, NPCImageType image_type, string sprite, string race, string profession, int hair, int beard, int index, int map_x, int map_y, int enc_x, int enc_y)
        {
            Name = new FantasyName(name);
            Background = new Background();

            Key = key;
            Gender = gender;
            RaceKey = race;
            ProfessionKey = profession;
            NPCIndex = index;
            PartyIndex = -1;
            Hair = hair;
            Beard = beard;
            combatStatus = CombatStatus.Awake;
            ImageType = image_type;
            Sprite = sprite;

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

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory();

            //healthBar = null;
            //actionsBar = null;
            statusSprite = null;
        }

        public NPC(NPC npc)
        {
            Name = new FantasyName(npc.Name);
            Background = new Background(npc.Background);

            Key = npc.Key;
            RaceKey = npc.RaceKey;
            ProfessionKey = npc.ProfessionKey;
            NPCIndex = npc.NPCIndex;
            PartyIndex = npc.PartyIndex;
            PartySlot = npc.PartySlot;

            ImageType = npc.ImageType;
            Sprite = npc.Sprite;
            Hair = npc.Hair;
            Beard = npc.Beard;

            Description = npc.Description;
            Background = npc.Background;

            Level = npc.Level;
            ExpValue = npc.ExpValue;

            combatStatus = npc.combatStatus;
            statusSprite = npc.statusSprite;

            BaseAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                BaseAttributes.Add(new Characteristic(npc.BaseAttributes[i]));
            }

            DerivedAttributes = new List<Characteristic>();
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                DerivedAttributes.Add(new Characteristic(npc.DerivedAttributes[i]));
            }

            Skills = new List<Characteristic>();
            for (int i = 0; i < (int)Skill.Number; i++)
            {
                Skills.Add(new Characteristic(npc.Skills[i]));
            }

            Resistances = new List<Characteristic>();
            for (int i = 0; i < (int)DamageType.Number; i++)
            {
                Resistances.Add(new Characteristic(npc.Resistances[i]));
            }

            Abilities = new List<Ability>();
            Inventory = new CharacterInventory(npc.Inventory);

            //healthBar = npc.healthBar;
            //actionsBar = npc.actionsBar;
        }

        public void CalculateStartingAttributes()
        {
            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                int roll = GameValue.Roll(new GameValue(5, 4), false);
                int total = roll + Database.GetRace(RaceKey).StartingAttributes[i].Roll(false);

                BaseAttributes[i].SetStart(total, 0, total);
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
            DerivedAttributes[(int)DerivedAttribute.Stamina].SetStart(BaseAttributes[(int)BaseAttribute.Endurance].Current + BaseAttributes[(int)BaseAttribute.Willpower].Current + Database.GetRace(RaceKey).StaminaPerLevel.Roll(false));
            DerivedAttributes[(int)DerivedAttribute.Essence].SetStart(BaseAttributes[(int)BaseAttribute.Intellect].Current + BaseAttributes[(int)BaseAttribute.Wisdom].Current + Database.GetRace(RaceKey).EssencePerLevel.Roll(false));
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
                int result = GameValue.Roll(new GameValue(1, 4), false) * value;

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

        //public GameObject CreateEncounterObject(Transform parent, int index, string layer)
        //{
        //    GameObject go = new GameObject();
        //    go.name = Name.ShortName;
        //    go.transform.SetParent(parent);
        //    go.transform.position = new Vector3(EncounterX + 0, EncounterY - 0, 0.8f);
        //    go.transform.Rotate(0, 0, 0);

        //    //BoxCollider2D col = go.AddComponent<BoxCollider2D>();

        //    NPCObject npcObject = go.AddComponent<NPCObject>();
        //    npcObject.Name = Name.ShortName;
        //    npcObject.Index = index;
        //    npcObject.Type = CharacterObjectType.NPC;

        //    GameObject go2 = new GameObject();
        //    go2.name = "Selection Indicator";
        //    go2.transform.position = new Vector3(EncounterX, EncounterY, -1);
        //    go2.transform.Rotate(0, 0, 0);
        //    go2.transform.SetParent(go.transform);

        //    SpriteRenderer sr = go2.AddComponent<SpriteRenderer>();
        //    sr.color = Color.green;
        //    sr.sprite = SpriteManager.Instance.GetIconSprite("Small Border");
        //    sr.sortingLayerName = "Selection Indicator";

        //    GameObject go3 = new GameObject();
        //    go3.name = "Status Indicator";
        //    go3.transform.position = new Vector3(EncounterX, EncounterY + 1, -1);
        //    go3.transform.Rotate(0, 0, 0);
        //    go3.transform.SetParent(go.transform);

        //    statusSprite = go3.AddComponent<SpriteRenderer>();
        //    statusSprite.sprite = SpriteManager.Instance.GetNPCSprite("npc_951");
        //    statusSprite.sortingLayerName = "NPC Status";

        //    npcObject.UnitRenderer = npcObject.gameObject.AddComponent<CharacterRenderer>();
        //    npcObject.UnitRenderer.Initialize(layer, new Vector3(-0, 1, -1), Vector3.one, new Vector3(0, 0, 0));
        //    npcObject.SetupRenderer(this);

        //    SelectionIndicator indicator = go2.AddComponent<SelectionIndicator>();
        //    indicator.SpriteRenderer = sr;
        //    indicator.Deactivate();

        //    npcObject.SelectionIndicator = indicator;
        //    npcObject.DisablePathLine();

        //    GameObject hpBarObject = GameObject.Instantiate(EncounterManager.Instance.StatBarPrefab);
        //    hpBarObject.transform.SetParent(go.transform);
        //    hpBarObject.transform.localPosition = new Vector3(-0, -0, -1);

        //    healthBar = hpBarObject.GetComponent<StatBarWidgetWorld>();
        //    healthBar.SetData(DerivedAttributes[(int)DerivedAttribute.Health].Current, DerivedAttributes[(int)DerivedAttribute.Health].Maximum, Color.red, Color.black);

        //    GameObject apBarObject = GameObject.Instantiate(EncounterManager.Instance.StatBarPrefab);
        //    apBarObject.transform.SetParent(go.transform);
        //    apBarObject.transform.localPosition = new Vector3(-0, 0.1f, -1);

        //    actionsBar = apBarObject.GetComponent<StatBarWidgetWorld>();
        //    actionsBar.SetData(DerivedAttributes[(int)DerivedAttribute.Actions].Current, DerivedAttributes[(int)DerivedAttribute.Actions].Maximum, Color.yellow, Color.black);

        //    return go;
        //}

        //public void UpdateHealthBar()
        //{
        //    if (healthBar != null)
        //    {
        //        healthBar.UpdateData(DerivedAttributes[(int)DerivedAttribute.Health].Current, DerivedAttributes[(int)DerivedAttribute.Health].Maximum);
        //    }
        //}

        //public void UpdateAPBar()
        //{
        //    if (actionsBar != null)
        //    {
        //        actionsBar.UpdateData(DerivedAttributes[(int)DerivedAttribute.Actions].Current, DerivedAttributes[(int)DerivedAttribute.Actions].Maximum);
        //    }
        //}

        //public void ActivateBars()
        //{
        //    healthBar.gameObject.SetActive(true);
        //    actionsBar.gameObject.SetActive(true);
        //}

        //public void DeactivateBars()
        //{
        //    healthBar.gameObject.SetActive(false);
        //    actionsBar.gameObject.SetActive(false);
        //}

        //public void SetCombatStatus(CombatStatus status)
        //{
        //    combatStatus = status;

        //    switch (combatStatus)
        //    {
        //        case CombatStatus.Awake:
        //            statusSprite.sprite = SpriteManager.Instance.GetNPCSprite("npc_32");
        //            ActivateBars();
        //            break;
        //        case CombatStatus.Unconcisous:
        //            statusSprite.sprite = SpriteManager.Instance.GetNPCSprite("npc_951");
        //            DeactivateBars();
        //            break;
        //        case CombatStatus.Dead:
        //            statusSprite.sprite = SpriteManager.Instance.GetNPCSprite("npc_983");
        //            DeactivateBars();
        //            break;
        //        case CombatStatus.Number:
        //            DeactivateBars();
        //            break;
        //        case CombatStatus.None:
        //            DeactivateBars();
        //            break;
        //        default:
        //            break;
        //    }
        //}

        //public void CombatUpdate()
        //{
        //    if (DerivedAttributes[(int)DerivedAttribute.Health].Current > 0)
        //    {
        //        SetCombatStatus(CombatStatus.Awake);
        //    }
        //    else if (DerivedAttributes[(int)DerivedAttribute.Health].Current < 1)
        //    {
        //        SetCombatStatus(CombatStatus.Unconcisous);
        //    }
        //    else if (DerivedAttributes[(int)DerivedAttribute.Health].Current < -DerivedAttributes[(int)DerivedAttribute.Health].Maximum)
        //    {
        //        SetCombatStatus(CombatStatus.Dead);
        //    }
        //}
    }
}