using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.Characters
{
    public static class NpcGenerator
    {
        public static List<string> Any = new List<string>();
        public static List<string> Animals = new List<string>();
        public static List<string> Beasts = new List<string>();
        public static List<string> Undead = new List<string>();
        public static List<string> Humanoids = new List<string>();
        public static List<string> Elementals = new List<string>();
        public static List<string> Draconic = new List<string>();
        public static List<string> Insects = new List<string>();

        static bool initialized = false;

        public static void Initialize()
        {
            if (initialized == false)
            {
                initialized = true;

                foreach (KeyValuePair<string, NPCDefinition> kvp in Database.NPCs)
                {
                    Any.Add(kvp.Key);

                    switch (kvp.Value.species)
                    {
                        case Species.Animal:
                            Animals.Add(kvp.Key);
                            break;
                        case Species.Beast:
                            Beasts.Add(kvp.Key);
                            break;
                        case Species.Undead:
                            Undead.Add(kvp.Key);
                            break;
                        case Species.Humanoid:
                            Humanoids.Add(kvp.Key);
                            break;
                        case Species.Elemental:
                            Elementals.Add(kvp.Key);
                            break;
                        case Species.Draconic:
                            Draconic.Add(kvp.Key);
                            break;
                        case Species.Insect:
                            Insects.Add(kvp.Key);
                            break;
                        case Species.Number:
                            break;
                        case Species.None:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static NpcData Generate(NpcType data_type, Species character_type, int level)
        {
            NpcData npc = null;
            string def = GetKey(character_type);

            if (Database.NPCs.ContainsKey(def))
            {
                npc = new NpcData(Database.NPCs[def].ConvertToCharacter());
                npc.level = Random.Range(Database.GetNPC(npc.key).minLevel, Database.GetNPC(npc.key).maxLevel + 1);
                npc.expValue = npc.level * Database.GetNPC(npc.key).expPerLevel;

                CalculateAttributes(npc);

                if (data_type == NpcType.Boss)
                    GenerateBoss(npc);
                else if (data_type == NpcType.Boss_Guard)
                    GenerateBossGuard(npc);
                else if (data_type == NpcType.Objective_Enemy)
                    GenerateObjectiveEnemy(npc);
                else if (data_type == NpcType.Enemy)
                    GenerateEnemy(npc);
                else if (data_type == NpcType.Rescue_Target)
                    GenerateRescueTarget(npc);
                else if (data_type == NpcType.Citizen)
                    GenerateCitizen(npc);
            }
            else
            {
                Debug.Log(def + " does not exist");
            }

            return npc;
        }

        public static void CalculateAttributes(NpcData npc)
        {
            int value = 0;
            int mod = 0;

            for (int i = 0; i < (int)BaseAttribute.Number; i++)
            {
                value = Database.NPCs[npc.key].baseStart[i];
                mod = 0;

                for (int j = 0; j < npc.level; j++)
                {
                    mod = Random.Range(Database.NPCs[npc.key].BasePerLevel[i].Number, Database.NPCs[npc.key].BasePerLevel[i].Die + 1);
                    value += mod;
                }

                //npc.BaseAttributes[i].SetStart(value + mod, 0, value + mod);
            }

            //value = npc.BaseAttributes[(int)BaseAttribute.Strength].Current + npc.BaseAttributes[(int)BaseAttribute.Dexterity].Current;
            //npc.DerivedAttributes[(int)DerivedAttribute.Might_Attack].SetStart(value, 0, value);

            //value = npc.BaseAttributes[(int)BaseAttribute.Dexterity].Current + npc.BaseAttributes[(int)BaseAttribute.Senses].Current;
            //npc.DerivedAttributes[(int)DerivedAttribute.Finesse_Attack].SetStart(value, 0, value);

            //value = npc.BaseAttributes[(int)BaseAttribute.Endurance].Current + npc.BaseAttributes[(int)BaseAttribute.Willpower].Current;
            //npc.DerivedAttributes[(int)DerivedAttribute.Health].SetStart(value, 0, value);

            //value = npc.BaseAttributes[(int)BaseAttribute.Intellect].Current + npc.BaseAttributes[(int)BaseAttribute.Willpower].Current;
            //npc.DerivedAttributes[(int)DerivedAttribute.Stamina].SetStart(value, 0, value);

            //value = npc.BaseAttributes[(int)BaseAttribute.Intellect].Current + npc.BaseAttributes[(int)BaseAttribute.Wisdom].Current;
            //npc.DerivedAttributes[(int)DerivedAttribute.Essence].SetStart(value, 0, value);


            for (int a = 0; a < (int)DerivedAttribute.Number; a++)
            {
                value = 0;

                for (int i = 0; i < npc.level; i++)
                {
                    value += Random.Range(Database.NPCs[npc.key].derivedPerLevel[a].Number, Database.NPCs[npc.key].derivedPerLevel[a].Die + 1);
                }

                value *= 10;

                npc.SetStart(AttributeType.Derived, (int)DerivedAttribute.Health, value, 0, value);
            }

            //npc.DerivedAttributes[(int)DerivedAttribute.Speed].SetStart(Database.NPCs[npc.Key].BaseSpeed, 0, Database.NPCs[npc.Key].BaseSpeed);
        }

        public static void GenerateBoss(NpcData npc)
        {
            npc.name.LastName = "Boss";
            //npc.DerivedAttributes[(int)DerivedAttribute.Health].ModifyStart(Random.Range(200, 1000));
            //npc.DerivedAttributes[(int)DerivedAttribute.Block].ModifyStart(Random.Range(1, 100));
            //npc.DerivedAttributes[(int)DerivedAttribute.Dodge].ModifyStart(Random.Range(1, 100));
            //npc.DerivedAttributes[(int)DerivedAttribute.Parry].ModifyStart(Random.Range(1, 100));
            npc.level += 3;
            npc.expValue *= 3;
        }

        public static void GenerateBossGuard(NpcData npc)
        {
            npc.name.LastName = "Boss Guard";
            //npc.DerivedAttributes[(int)DerivedAttribute.Health].ModifyStart(Random.Range(100, 250));
            //npc.DerivedAttributes[(int)DerivedAttribute.Block].ModifyStart(Random.Range(1, 50));
            //npc.DerivedAttributes[(int)DerivedAttribute.Dodge].ModifyStart(Random.Range(1, 50));
            //npc.DerivedAttributes[(int)DerivedAttribute.Parry].ModifyStart(Random.Range(1, 50));
            npc.level += 1;
            npc.expValue *= 2;
        }

        public static void GenerateObjectiveEnemy(NpcData npc)
        {
            npc.name.LastName = "Objective Enemy";
            //npc.DerivedAttributes[(int)DerivedAttribute.Health].ModifyStart(Random.Range(5, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Block].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Dodge].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Parry].ModifyStart(Random.Range(1, 20));
        }

        public static void GenerateEnemy(NpcData npc)
        {
            npc.name.LastName = "Enemy";
            //npc.DerivedAttributes[(int)DerivedAttribute.Block].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Dodge].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Parry].ModifyStart(Random.Range(1, 20));
        }

        public static void GenerateRescueTarget(NpcData npc)
        {
            npc.name.LastName = "Rescue Target";
            //npc.DerivedAttributes[(int)DerivedAttribute.Block].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Dodge].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Parry].ModifyStart(Random.Range(1, 20));
        }

        public static void GenerateCitizen(NpcData npc)
        {
            npc.name.LastName = "Citizen";
            //npc.DerivedAttributes[(int)DerivedAttribute.Block].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Dodge].ModifyStart(Random.Range(1, 20));
            //npc.DerivedAttributes[(int)DerivedAttribute.Parry].ModifyStart(Random.Range(1, 20));
        }

        public static string GetKey(Species character_type)
        {
            string key = "";

            switch (character_type)
            {
                case Species.Animal:
                    key = Animals[Random.Range(0, Animals.Count)];
                    break;
                case Species.Beast:
                    key = Beasts[Random.Range(0, Beasts.Count)];
                    break;
                case Species.Undead:
                    key = Undead[Random.Range(0, Undead.Count)];
                    break;
                case Species.Humanoid:
                    key = Humanoids[Random.Range(0, Humanoids.Count)];
                    break;
                case Species.Elemental:
                    key = Elementals[Random.Range(0, Elementals.Count)];
                    break;
                case Species.Draconic:
                    key = Draconic[Random.Range(0, Draconic.Count)];
                    break;
                case Species.Insect:
                    key = Insects[Random.Range(0, Insects.Count)];
                    break;
                case Species.Number:
                    break;
                case Species.None:
                    break;
                default:
                    break;
            }

            return key;
        }
    }
}