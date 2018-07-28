using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Reclamation.Abilities;
using Reclamation.Characters;
using Reclamation.Equipment;
using Reclamation.Name;
using Reclamation.World;

namespace Reclamation.Misc
{
    public static class Database
    {
        static bool initialized = false;

        //private static Dictionary<string, TransitionDefinition> transitions = new Dictionary<string, TransitionDefinition>();
        //public static Dictionary<string, TransitionDefinition> Transitions { get { return transitions; } }    

        //private static Dictionary<string, Encounter.TerrainData> terrainDefinitions = new Dictionary<string, Encounter.TerrainData>();
        //public static Dictionary<string, Encounter.TerrainData> TerrainDefinitions { get { return terrainDefinitions; } }

        //private static Dictionary<string, WorldSiteDefinition> worldSites = new Dictionary<string, WorldSiteDefinition>();
        //public static Dictionary<string, WorldSiteDefinition> WorldSites { get { return worldSites; } }

        private static List<AttributeDefinition> baseAttributeDefinitions = new List<AttributeDefinition>();
        public static List<AttributeDefinition> BaseAttributes { get { return baseAttributeDefinitions; } }

        private static List<AttributeDefinition> derivedAttributeDefinitions = new List<AttributeDefinition>();
        public static List<AttributeDefinition> DerivedAttributes { get { return derivedAttributeDefinitions; } }

        private static List<SkillDefinition> skillDefinitions = new List<SkillDefinition>();
        public static List<SkillDefinition> Skills { get { return skillDefinitions; } }

        private static List<AttributeDefinition> damageTypeDefinitions = new List<AttributeDefinition>();
        public static List<AttributeDefinition> DamageTypes { get { return damageTypeDefinitions; } }

        private static List<AttributeDefinition> partyAttributeDefinitions = new List<AttributeDefinition>();
        public static List<AttributeDefinition> PartyAttributes { get { return partyAttributeDefinitions; } }

        private static List<AttributeDefinition> weaponAttributes = new List<AttributeDefinition>();
        public static List<AttributeDefinition> WeaponAttributes { get { return weaponAttributes; } }

        private static List<AttributeDefinition> ammoAttributes = new List<AttributeDefinition>();
        public static List<AttributeDefinition> AmmoAttributes { get { return ammoAttributes; } }

        private static List<AttributeDefinition> wearableAttributes = new List<AttributeDefinition>();
        public static List<AttributeDefinition> WearableAttributes { get { return wearableAttributes; } }

        private static List<AttributeDefinition> accessoryAttributes = new List<AttributeDefinition>();
        public static List<AttributeDefinition> AccessoryAttributes { get { return accessoryAttributes; } }

        private static Dictionary<string, ResourceDefinition> resourceDefinitions = new Dictionary<string, ResourceDefinition>();
        public static Dictionary<string, ResourceDefinition> Resources { get { return resourceDefinitions; } }

        private static Dictionary<string, BuildingDefinition> buildingDefinitions = new Dictionary<string, BuildingDefinition>();
        public static Dictionary<string, BuildingDefinition> Buildings { get { return buildingDefinitions; } }

        private static Dictionary<string, Profession> professions = new Dictionary<string, Profession>();
        public static Dictionary<string, Profession> Professions { get { return professions; } }

        private static Dictionary<string, Race> races = new Dictionary<string, Race>();
        public static Dictionary<string, Race> Races { get { return races; } }

        private static Dictionary<string, ItemDefinition> itemDefinitions = new Dictionary<string, ItemDefinition>();
        public static Dictionary<string, ItemDefinition> Items { get { return itemDefinitions; } }

        private static Dictionary<string, ItemDefinition> artifactDefinitions = new Dictionary<string, ItemDefinition>();
        public static Dictionary<string, ItemDefinition> Artifacts { get { return artifactDefinitions; } }

        private static Dictionary<string, ItemModifier> itemModifiers = new Dictionary<String, ItemModifier>();
        public static Dictionary<string, ItemModifier> ItemModifiers { get { return itemModifiers; } }

        private static Dictionary<string, ResearchEntry> researchEntries = new Dictionary<string, ResearchEntry>();
        public static Dictionary<string, ResearchEntry> ResearchEntries { get { return researchEntries; } }

        private static Dictionary<string, Ability> abilities = new Dictionary<string, Ability>();
        public static Dictionary<string, Ability> Abilities { get { return abilities; } }

        private static Dictionary<string, AbilityModifier> runes = new Dictionary<string, AbilityModifier>();
        public static Dictionary<string, AbilityModifier> Runes { get { return runes; } }

        private static Dictionary<string, NPCDefinition> npcs = new Dictionary<string, NPCDefinition>();
        public static Dictionary<string, NPCDefinition> NPCs { get { return npcs; } }

        public static string DataPath;

        public static void Initialize()
        {
            if (initialized == false)
            {
                initialized = true;
                DataPath = Application.streamingAssetsPath + "/";

                //Debug.Log("Database Initialized");
                //LoadFiles();

                LoadData();
                Save();

                //AudioManager.Instance.Initialize();
            }
        }

        static void Save()
        {
            SaveAttributes();
            Save("resources", resourceDefinitions);
            Save("buildings", buildingDefinitions);
            Save("items", itemDefinitions);
            Save("item_modifiers", itemModifiers);
            Save("professions", professions);
            Save("races", races);
            Save("research_entries", researchEntries);
            Save("abilities", abilities);
            Save("runes", runes);
            Save("npcs", npcs);
        }

        static void LoadData()
        {
            LoadResources();
            LoadBuildings();
            LoadAttributes();
            LoadPartyAttributes();
            LoadSkills();
            LoadItems();
            LoadArtifacts();
            LoadItemMaterials();
            LoadPlusEnchants();
            LoadPreEnchants();
            LoadPostEnchants();
            LoadItemAttributes();
            LoadProfessions();
            LoadRaces();
            //LoadEncounterData();
            //LoadWorldSiteData();
            //LoadTransitionData();
            LoadResearchTree();
            LoadAbilities();
            LoadAbilityModifiers();
            LoadNPCs();
        }

        static void LoadFiles()
        {
            LoadResourceData();
            LoadBuildingData();
            LoadBaseAttributeData();
            LoadDerivedAttributeData();
            LoadDerivedDamageTypeData();
            LoadSkillData();
            LoadPartyAttributeData();
            LoadItemData();
            LoadArtifacts();
            LoadItemModifiers();
            LoadProfessionData();
            LoadRaceData();
            //LoadEncounterData();
            //LoadWorldSiteData();
            //LoadTransitionData();
            LoadAbilityData();
            LoadRuneData();
            LoadNpcData();
            LoadResearchData();
        }

        public static void SaveAttributes()
        {
            SaveAttributes("base_attributes", baseAttributeDefinitions);
            SaveAttributes("derived_attributes", derivedAttributeDefinitions);
            SaveAttributes("damage_types", damageTypeDefinitions);
            SaveAttributes("party_attibutes", partyAttributeDefinitions);
            SaveSkills("skills", skillDefinitions);
        }

        public static void Save<TKey, TValue>(string file, Dictionary<TKey, TValue> dictionary)
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string dataAsJson = JsonConvert.SerializeObject(dictionary, settings);
            string filePath = DataPath + file + ".json";
            File.WriteAllText(filePath, dataAsJson);
        }

        public static void SaveAttributes(string file, List<AttributeDefinition> list)
        {
            AttributeDefinitionList al = new AttributeDefinitionList();
            foreach (AttributeDefinition def in list)
            {
                al.Attributes.Add(def);
            }

            string dataAsJson = JsonUtility.ToJson(al);
            string filePath = DataPath + file + ".json";
            File.WriteAllText(filePath, dataAsJson);
        }

        public static void SaveSkills(string file, List<SkillDefinition> list)
        {
            SkillList sl = new SkillList();
            foreach (SkillDefinition def in list)
            {
                sl.Skills.Add(def);
            }

            string dataAsJson = JsonUtility.ToJson(sl);
            string filePath = DataPath + file + ".json";
            File.WriteAllText(filePath, dataAsJson);
        }

        private static void LoadNpcData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "npcs.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                npcs = JsonConvert.DeserializeObject<Dictionary<string, NPCDefinition>>(dataAsJson, settings);
            }
        }

        private static void LoadResourceData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "resources.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                resourceDefinitions = JsonConvert.DeserializeObject<Dictionary<string, ResourceDefinition>>(dataAsJson, settings);
            }
        }

        private static void LoadBuildingData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "buildings.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                buildingDefinitions = JsonConvert.DeserializeObject<Dictionary<string, BuildingDefinition>>(dataAsJson, settings);
            }
        }

        private static void LoadBaseAttributeData()
        {
            string filePath = DataPath + "base_attributes.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                AttributeDefinitionList list = JsonUtility.FromJson<AttributeDefinitionList>(dataAsJson);

                foreach (AttributeDefinition def in list.Attributes)
                {
                    baseAttributeDefinitions.Add(def);
                }
            }
        }

        private static void LoadDerivedAttributeData()
        {
            string filePath = DataPath + "derived_attributes.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                AttributeDefinitionList list = JsonUtility.FromJson<AttributeDefinitionList>(dataAsJson);

                foreach (AttributeDefinition def in list.Attributes)
                {
                    derivedAttributeDefinitions.Add(def);
                }
            }
        }

        private static void LoadDerivedDamageTypeData()
        {
            string filePath = DataPath + "damage_types.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                AttributeDefinitionList list = JsonUtility.FromJson<AttributeDefinitionList>(dataAsJson);

                foreach (AttributeDefinition def in list.Attributes)
                {
                    damageTypeDefinitions.Add(def);
                }
            }
        }

        private static void LoadPartyAttributeData()
        {
            string filePath = DataPath + "party_attributes.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                AttributeDefinitionList list = JsonUtility.FromJson<AttributeDefinitionList>(dataAsJson);

                foreach (AttributeDefinition def in list.Attributes)
                {
                    partyAttributeDefinitions.Add(def);
                }
            }
        }

        private static void LoadSkillData()
        {
            string filePath = DataPath + "skills.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                SkillList list = JsonUtility.FromJson<SkillList>(dataAsJson);

                foreach (SkillDefinition def in list.Skills)
                {
                    skillDefinitions.Add(def);
                }
            }
        }

        private static void LoadAbilityData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "abilities.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                abilities = JsonConvert.DeserializeObject<Dictionary<string, Ability>>(dataAsJson, settings);
            }
        }

        private static void LoadItemData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "items.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                itemDefinitions = JsonConvert.DeserializeObject<Dictionary<string, ItemDefinition>>(dataAsJson, settings);
            }
        }

        private static void LoadItemModifiers()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "item_modifiers.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                itemModifiers = JsonConvert.DeserializeObject<Dictionary<string, ItemModifier>>(dataAsJson, settings);
            }
        }

        private static void LoadProfessionData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "professions.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                professions = JsonConvert.DeserializeObject<Dictionary<string, Profession>>(dataAsJson, settings);
            }
        }

        private static void LoadRaceData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "races.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                races = JsonConvert.DeserializeObject<Dictionary<string, Race>>(dataAsJson, settings);
            }

            //foreach (KeyValuePair<string, Race> kvp in races)
            //{
            //    Debug.Log(kvp.Value.Name);
            //}
        }

        private static void LoadResearchData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "research_entries.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                researchEntries = JsonConvert.DeserializeObject<Dictionary<string, ResearchEntry>>(dataAsJson, settings);
            }
        }

        private static void LoadRuneData()
        {
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.TypeNameHandling = TypeNameHandling.Auto;
            string filePath = DataPath + "runes.json";

            if (File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath);
                runes = JsonConvert.DeserializeObject<Dictionary<string, AbilityModifier>>(dataAsJson, settings);
            }
        }

        //public static Encounter.TerrainData GetTerrainDefinition(string key)
        //{
        //    if (terrainDefinitions.ContainsKey(key) == false)
        //    {
        //        Debug.LogWarning("terrainDefinitions key: " + key + " does not exist");
        //        return null;
        //    }
        //    else
        //    {
        //        return terrainDefinitions[key];
        //    }
        //}

        //public static WorldSiteDefinition GetWorldSite(string key)
        //{
        //    if (worldSites.ContainsKey(key) == false)
        //    {
        //        Debug.LogWarning("worldSites key: " + key + " does not exist");
        //        return null;
        //    }
        //    else
        //    {
        //        return worldSites[key];
        //    }
        //}


        public static NPCDefinition GetNPC(string key)
        {
            if (npcs.ContainsKey(key) == false)
            {
                Debug.LogWarning("npcs key: " + key + " does not exist");
                return null;
            }
            else
            {
                return npcs[key];
            }
        }

        public static AttributeDefinition GetBaseAttribute(int index)
        {
            if (index >= 0 && index <= baseAttributeDefinitions.Count - 1)
            {
                return baseAttributeDefinitions[index];
            }
            else
            {
                Debug.Log("index out of range " + index);
                return null;
            }
        }

        public static AttributeDefinition GetDerivedAttribute(int index)
        {
            if (index >= 0 && index <= derivedAttributeDefinitions.Count - 1)
            {
                return derivedAttributeDefinitions[index];
            }
            else
            {
                Debug.Log("index out of range " + index);
                return null;
            }
        }

        public static AttributeDefinition GetDamageType(int index)
        {
            if (index >= 0 && index <= damageTypeDefinitions.Count - 1)
            {
                return damageTypeDefinitions[index];
            }
            else
            {
                Debug.Log("index out of range " + index);
                return null;
            }
        }

        public static SkillDefinition GetSkill(int index)
        {
            if (index >= 0 && index <= skillDefinitions.Count - 1)
            {
                return skillDefinitions[index];
            }
            else
            {
                Debug.Log("index out of range " + index);
                return null;
            }
        }

        public static AttributeDefinition GetPartyAttribute(int index)
        {
            if (index >= 0 && index <= partyAttributeDefinitions.Count - 1)
            {
                return partyAttributeDefinitions[index];
            }
            else
            {
                Debug.Log("index out of range " + index);
                return null;
            }
        }

        public static Profession GetProfession(string key)
        {
            if (professions.ContainsKey(key) == false)
            {
                Debug.LogWarning("Profession key: " + key + " does not exist");
                return null;
            }
            else
            {
                return professions[key];
            }
        }

        public static Race GetRace(string key)
        {
            if (races.ContainsKey(key) == false)
            {
                Debug.LogWarning("Race key: " + key + " does not exist");
                return null;
            }
            else
            {
                return races[key];
            }
        }

        public static string GetRaceAt(int index)
        {
            string race = Globals.EmptyString;
            int i = 0;

            foreach (KeyValuePair<string, Race> kvp in races)
            {
                i++;
                if (index == i)
                {
                    race = kvp.Key;
                    break;
                }
            }

            return race;
        }

        public static int GetRaceIndex(string race)
        {
            int index = 0;

            foreach (KeyValuePair<string, Race> kvp in races)
            {
                index++;
                if (race == kvp.Key)
                    break;
            }

            return index;
        }

        public static int GetProfessionIndex(string profession)
        {
            int index = 0;

            foreach (KeyValuePair<string, Profession> kvp in professions)
            {
                index++;
                if (profession == kvp.Key)
                    break;
            }

            return index;
        }

        public static Ability GetAbility(string key)
        {
            if (abilities.ContainsKey(key) == false)
            {
                Debug.LogWarning("traits key: " + key + " does not exist");
                return null;
            }
            else
            {
                return abilities[key];
            }
        }

        public static ResourceDefinition GetResource(string key)
        {
            if (resourceDefinitions.ContainsKey(key) == false)
            {
                Debug.LogWarning("resourceDefinitions key: " + key + " does not exist");
                return null;
            }
            else
            {
                return resourceDefinitions[key];
            }
        }

        public static ItemModifier GetItemModifier(string key)
        {
            if (itemModifiers.ContainsKey(key) == false)
            {
                Debug.LogWarning("itemModifiers key: " + key + " does not exist");
                return null;
            }
            else
            {
                return itemModifiers[key];
            }
        }

        public static ItemDefinition GetItem(string key, bool artifact)
        {
            if (artifact == false)
            {
                if (itemDefinitions.ContainsKey(key) == true)
                {
                    return itemDefinitions[key];
                }
                else
                {
                    Debug.LogWarning("Item key: " + key + " does not exist");
                    return null;
                }
            }
            else
            {
                if (artifactDefinitions.ContainsKey(key) == true)
                {
                    return artifactDefinitions[key];
                }
                else
                {
                    Debug.LogWarning("Artifact key: " + key + " does not exist");
                    return null;
                }
            }
        }

        public static ResearchEntry GetResearchEntry(string key)
        {
            if (researchEntries.ContainsKey(key) == false)
            {
                Debug.LogWarning("Research Entry key: " + key + " does not exist");
                return null;
            }
            else
            {
                return researchEntries[key];
            }
        }

        //static void LoadTransitionData()
        //{
        //    TransitionDefinition t = new TransitionDefinition("Stone Stairs Down", "stone_stairs_down", 223);
        //    transitions.Add(t.Key, t);
        //    t = new TransitionDefinition("Stone Stairs Up", "stone_stairs_up", 224);
        //    transitions.Add(t.Key, t);
        //    t = new TransitionDefinition("Wood Stairs Down", "wood_stairs_down", 225);
        //    transitions.Add(t.Key, t);
        //    t = new TransitionDefinition("Wood Stairs Up", "wood_stairs_up", 226);
        //    transitions.Add(t.Key, t);
        //    t = new TransitionDefinition("Cave Entrance", "cave_entrance", 185);
        //    transitions.Add(t.Key, t);
        //    t = new TransitionDefinition("Cave Exit", "cave_exit", 185);
        //    transitions.Add(t.Key, t);
        //}

        //static void LoadWorldSiteData()
        //{
        //    WorldSiteDefinition site = new WorldSiteDefinition("Stone Tower", "stone_tower", "grassland_standard", "hideout_standard", 2, 6, 415, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Sandstone Tower", "sandstone_tower", "grassland_standard", "hideout_standard", 2, 6, 416, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Stone Tower", "ruined_stone_tower", "grassland_standard", "hideout_standard", 2, 6, 417, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Sandstone Tower", "ruined_sandstone_tower", "grassland_standard", "hideout_standard", 2, 6, 418, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Round Stone Tower", "round_stone_tower", "grassland_standard", "hideout_standard", 2, 6, 419, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Round Sandstone Tower", "round_sandstone_tower", "grassland_standard", "hideout_standard", 2, 6, 420, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Round Stone Tower", "ruined_round_stone_tower", "grassland_standard", "hideout_standard", 2, 6, 421, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Round Sandstone Tower", "ruined_round_sandstone_tower", "grassland_standard", "hideout_standard", 2, 6, 422, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Building", "ruined_building", "grassland_standard", "hideout_standard", 2, 6, 423, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Sandstone Building", "ruined_sandstone_building", "grassland_standard", "hideout_standard", 2, 6, 424, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Ruined Building", "ancient_ruined_building", "grassland_standard", "hideout_standard", 2, 6, 425, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Ruined Sandstone Building", "ancient_ruined_sandstone_building", "grassland_standard", "hideout_standard", 2, 6, 426, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Graveyard", "graveyard", "hills_standard", "tomb_standard", 2, 6, 429, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Graveyard", "ancient_graveyard", "hills_standard", "tomb_standard", 2, 6, 430, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Cave Entrance", "cave_entrance", "grassland_standard", "cave_standard", 2, 6, 431, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Stone Cave Entrance", "stone_cave_entrance", "grassland_standard", "cave_standard", 2, 6, 434, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Dungeon Entrance", "dungeon_entrance", "grassland_standard", "dungeon_standard", 2, 6, 435, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Tower", "tower", "grassland_standard", "hideout_standard", 2, 6, 436, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Tower", "ruined_tower", "grassland_standard", "hideout_standard", 2, 6, 437, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Tomb", "tomb", "grassland_standard", "tomb_standard", 2, 6, 442, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Tomb", "acient_tomb", "grassland_standard", "tomb_standard", 2, 6, 443, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Temple", "temple", "grassland_standard", "tomb_standard", 2, 6, 469, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Temple", "ruined_temple", "grassland_standard", "tomb_standard", 2, 6, 470, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Sandstone Temple", "sandstone_temple", "grassland_standard", "tomb_standard", 2, 6, 442, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Sandstone Ruined Temple", "sandstone_ruined_temple", "grassland_standard", "tomb_standard", 2, 6, 443, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Ruined Temple", "ancient_ruined_temple", "grassland_standard", "tomb_standard", 2, 6, 446, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Oasis", "oasis", "swamp_standard", "", 1, 1, 447, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Dry Oasis", "dry_oasis", "swamp_standard", "", 1, 1, 448, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Standing Stones", "standing_stones", "grassland_standard", "", 1, 1, 449, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Manor", "manor", "grassland_standard", "dungeon_standard", 1, 2, 463, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Manor", "ruined_manor", "grassland_standard", "dungeon_standard", 1, 2, 464, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Ruined Manor", "ancient_ruined_manor", "grassland_standard", "dungeon_standard", 1, 2, 465, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Keep", "keep", "grassland_standard", "dungeon_standard", 2, 6, 466, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Keep", "ruined_keep", "grassland_standard", "dungeon_standard", 2, 6, 467, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Ruined Keep", "ancient_ruined_keep", "grassland_standard", "dungeon_standard", 2, 6, 468, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruins", "ruins", "grassland_standard", "dungeon_standard", 2, 6, 475, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ancient Ruins", "ancient_ruins", "grassland_standard", "dungeon_standard", 2, 6, 476, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Village", "village", "grassland_standard", "dungeon_standard", 1, 2, 477, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Ruined Village", "ruined_village", "grassland_standard", "dungeon_standard", 1, 2, 478, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Town", "town_1", "grassland_standard", "dungeon_standard", 1, 2, 474, "", 0);
        //    worldSites.Add(site.Key, site);

        //    site = new WorldSiteDefinition("Town", "town_2", "grassland_standard", "dungeon_standard", 1, 2, 481, "", 0);
        //    worldSites.Add(site.Key, site);
        //}

        //static void LoadEncounterData()
        //{
        //    Encounter.TerrainData def = new Encounter.TerrainData("Beach", "beach_standard");
        //    def.Floors.Add("beach_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("beach_1");
        //    def.Trees.Add("tree_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Cave", "cave_standard");
        //    def.Floors.Add("dirt_floor_1");
        //    def.Walls.Add("wall_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    def.Clutter.Add("stalagmite_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Dungeon", "dungeon_standard");
        //    def.Floors.Add("stone_brick_floor_1");
        //    def.Walls.Add("gray_stone_wall_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Forest", "forest_standard");
        //    def.Floors.Add("grass_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    def.Trees.Add("tree_2");
        //    def.Plants.Add("herbs_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Grassland", "grassland_standard");
        //    def.Floors.Add("grass_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    def.Trees.Add("tree_1");
        //    def.Plants.Add("herbs_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Hideout", "hideout_standard");
        //    def.Floors.Add("dirt_floor_1");
        //    def.Walls.Add("gray_stone_wall_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Hills", "hills_standard");
        //    def.Floors.Add("dirt_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    def.Trees.Add("tree_1");
        //    def.Clutter.Add("rocks_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Mountains", "mountains_standard");
        //    def.Floors.Add("dirt_1");
        //    def.Walls.Add("rough_stone_wall_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    def.Trees.Add("tree_1");
        //    def.Clutter.Add("rocks_1");
        //    def.Clutter.Add("stalagmites_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Swamp", "swamp_standard");
        //    def.Floors.Add("swamp_grass_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    def.Trees.Add("tree_4");
        //    def.Plants.Add("swamp_flower_1");
        //    terrainDefinitions.Add(def.Key, def);

        //    def = new Encounter.TerrainData("Tomb", "tomb_standard");
        //    def.Floors.Add("tomb_floor_1");
        //    def.Walls.Add("tomb_wall_1");
        //    def.Liquids.Add("water_1");
        //    def.LiquidFloors.Add("dirt_1");
        //    terrainDefinitions.Add(def.Key, def);
        //}

        static void LoadPlusEnchants()
        {
            ItemModifier enchant = new ItemModifier("+1", "Plus 1", 2, 50, 100, 0, ItemModifierType.Plus_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("+2", "Plus 2", 4, 200, 75, 0, ItemModifierType.Plus_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Rare);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("+3", "Plus 3", 6, 300, 100, 0, ItemModifierType.Plus_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Fabled);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("+4", "Plus 4", 8, 400, 125, 0, ItemModifierType.Plus_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("+5", "Plus 5", 10, 500, 150, 0, ItemModifierType.Plus_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Legendary);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Masterwork", "Masterwork", 2, 100, 20, 0, ItemModifierType.Quality, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Inferior", "Inferior", 2, 100, 20, 0, ItemModifierType.Quality, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Common);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Superior", "Superior", 2, 100, 20, 0, ItemModifierType.Quality, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Rare);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Magnificent", "Magnificent", 2, 100, 20, 0, ItemModifierType.Quality, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Fabled);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);
        }

        static void LoadPreEnchants()
        {
            ItemModifier enchant = new ItemModifier("Flaming", "Flaming", 2, 100, 20, 0, ItemModifierType.Pre_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Icey", "Icey", 2, 100, 20, 0, ItemModifierType.Pre_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Venomous", "Venomous", 2, 100, 20, 0, ItemModifierType.Pre_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Shocking", "Shocking", 2, 100, 20, 0, ItemModifierType.Pre_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Warriors", "Warriors", 4, 100, 100, 0, ItemModifierType.Pre_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Wizards", "Wizards", 4, 100, 25, 0, ItemModifierType.Pre_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("Priests", "Priests", 4, 100, 75, 0, ItemModifierType.Pre_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);
        }

        static void LoadPostEnchants()
        {
            ItemModifier enchant = new ItemModifier("of Strength", "Strength", 1, 100, 10, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };        //enchant.Effects.Add(new AlterCharacteristicEffect(TriggerType.Always_On, CharacteristicType.Base_Attribute, "strength", 1, 1));
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Endurance", "Endurance", 1, 100, 10, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Uncommon);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Might", "Might", 2, 100, 20, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Rare);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Insight", "Insight", 2, 100, 20, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Rare);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of the Wolf", "Wolf", 3, 100, 50, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of the Bear", "Bear", 3, 100, 75, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of the Snake", "Snake", 3, 100, 30, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Shadow", "Shadow", 3, 100, 30, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Light", "Light", 3, 100, 30, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Death", "Death", 3, 100, 30, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Arcana", "Arcana", 3, 100, 30, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Mythical);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);

            enchant = new ItemModifier("of Destiny", "Destiny", 3, 100, 30, 0, ItemModifierType.Post_Enchant, MaterialHardness.None, ItemTypeAllowed.Any, Rarity.Legendary);
            enchant.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemModifiers.Add(enchant.Key, enchant);
        }

        static void LoadItemMaterials()
        {
            ItemModifier material = new ItemModifier("Copper", "Copper", 0, 0, 0, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", -1), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Rusty", "Rusty", -1, -1, -1, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 1), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Iron", "Iron", 1, 1, 1, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 1), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Bronze", "Bronze", 1, 5, 5, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Uncommon);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Steel", "Steel", 2, 10, 12, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Rare);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 5), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Black Steel", "Black Steel", 3, 20, 25, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Fabled);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 25), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Mythril", "Mythril", 4, 100, 300, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Mythical);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 100), new ResourceData("Rare Materials", 10) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Arcanite", "Arcanite", 5, 150, 500, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Mythical);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Dragon Scale", "Dragon Scale", 6, 250, 1000, 0, ItemModifierType.Material, MaterialHardness.Metal, ItemTypeAllowed.Any, Rarity.Mythical);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Wood", "Wood", 0, 0, 0, 0, ItemModifierType.Material, MaterialHardness.Soft, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Bone", "Bone", -1, 1, -1, -1, ItemModifierType.Material, MaterialHardness.Soft, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 1), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Ironbark", "Ironbark", 1, 5, 15, 0, ItemModifierType.Material, MaterialHardness.Soft, ItemTypeAllowed.Any, Rarity.Uncommon);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 50), new ResourceData("Materials", 5) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Dragon Bone", "Dragon Bone", 6, 100, 1000, 0, ItemModifierType.Material, MaterialHardness.Soft, ItemTypeAllowed.Any, Rarity.Legendary);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Stone", "Stone", 0, 0, -1, 0, ItemModifierType.Material, MaterialHardness.Stone, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", -1), new ResourceData("Materials", -1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Flint", "Flint", 1, 1, 0, 0, ItemModifierType.Material, MaterialHardness.Stone, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", -1), new ResourceData("Materials", -1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Obsidian", "Obsidian", 1, 5, 3, 0, ItemModifierType.Material, MaterialHardness.Stone, ItemTypeAllowed.Any, Rarity.Uncommon);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Sun Stone", "Sun Stone", 3, 30, 75, 0, ItemModifierType.Material, MaterialHardness.Stone, ItemTypeAllowed.Any, Rarity.Rare);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Moon Stone", "Moon Stone", 3, 30, 75, 0, ItemModifierType.Material, MaterialHardness.Stone, ItemTypeAllowed.Any, Rarity.Rare);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Elder Clay", "Elder Clay", 5, 45, 120, 0, ItemModifierType.Material, MaterialHardness.Stone, ItemTypeAllowed.Any, Rarity.Fabled);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Linen", "Linen", 0, 0, 0, 0, ItemModifierType.Material, MaterialHardness.Cloth, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", -1), new ResourceData("Materials", -1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Silk", "Silk", 1, 10, 0, 0, ItemModifierType.Material, MaterialHardness.Cloth, ItemTypeAllowed.Any, Rarity.Uncommon);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Steelweave", "Steelweave", 2, 25, 20, 0, ItemModifierType.Material, MaterialHardness.Cloth, ItemTypeAllowed.Any, Rarity.Rare);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Spellcloth", "Spellcloth", 3, 35, 25, 0, ItemModifierType.Material, MaterialHardness.Cloth, ItemTypeAllowed.Any, Rarity.Fabled);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1), new ResourceData("Essence", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Rockleaf", "Rockleaf", 4, 20, 50, 0, ItemModifierType.Material, MaterialHardness.Cloth, ItemTypeAllowed.Any, Rarity.Mythical);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1), new ResourceData("Essence", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Fur", "Fur", -1, 1, 0, 0, ItemModifierType.Material, MaterialHardness.Leather, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", -1), new ResourceData("Materials", -1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Hide", "Hide", 0, 3, 1, 0, ItemModifierType.Material, MaterialHardness.Leather, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Hardened Leather", "Hardened Leather", 1, 10, 5, 0, ItemModifierType.Material, MaterialHardness.Leather, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Reinforced Leather", "Reinforced Leather", 2, 15, 12, 0, ItemModifierType.Material, MaterialHardness.Leather, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Troll Skin", "Troll Skin", 3, 20, 150, 0, ItemModifierType.Material, MaterialHardness.Leather, ItemTypeAllowed.Any, Rarity.Rare);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Dragon Hide", "Dragon Hide", 10, 100, 750, 0, ItemModifierType.Material, MaterialHardness.Leather, ItemTypeAllowed.Any, Rarity.Legendary);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Thick", "Thick", 1, 10, 0, 0, ItemModifierType.Material, MaterialHardness.Liquid, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 5), new ResourceData("Materials", 2), new ResourceData("Essence", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Bubbling", "Bubbling", 2, 10, 0, 0, ItemModifierType.Material, MaterialHardness.Liquid, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 25), new ResourceData("Materials", 2), new ResourceData("Essence", 2) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Paper", "Paper", 0, 10, 0, 0, ItemModifierType.Material, MaterialHardness.Paper, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Parchment", "Parchment", 1, 15, 0, 0, ItemModifierType.Material, MaterialHardness.Paper, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 5), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Plain", "Plain", 0, 0, 0, 0, ItemModifierType.Material, MaterialHardness.Food, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 1), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);

            material = new ItemModifier("Cooked", "Cooked", 1, 1, 0, 0, ItemModifierType.Material, MaterialHardness.Food, ItemTypeAllowed.Any, Rarity.Common);
            material.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 2), new ResourceData("Materials", 1) };
            itemModifiers.Add(material.Key, material);
        }

        static void LoadArtifacts()
        {
            ItemDefinition artifact = new ItemDefinition("Dragon Slayer", "Dragon Slayer", "items_large_322", "items_small_254", EquipmentSlot.Right_Hand, 10, 1, 10, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Artifact,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, 15, 1, 5, 5,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            artifact.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 100000), new ResourceData("Materials", 500), new ResourceData("Tools", 25), new ResourceData("Rare Materials", 25), new ResourceData("Essence", 25) };
            artifactDefinitions.Add(artifact.Key, artifact);

            artifact = new ItemDefinition("Champions Armor", "Champions Armor", "items_large_99", "items_small_474", EquipmentSlot.Body, 18, 4, 28, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Artifact, null,
                null, new WearableData(WearableType.Armor, 3, -4, 0, 0, new List<ResistanceData> { new ResistanceData(DamageType.Physical, 1) }), null, null);
            artifact.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 100000), new ResourceData("Materials", 500), new ResourceData("Tools", 25), new ResourceData("Rare Materials", 25), new ResourceData("Essence", 25) };
            artifact.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Medium_Armor, 1) };
            artifactDefinitions.Add(artifact.Key, artifact);

            artifact = new ItemDefinition("Holy Bulwark", "Holy Bulwark", "items_large_83", "items_small_559", EquipmentSlot.Left_Hand, 15, 3, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Soft, ItemNameFormat.Artifact, null,
                null, new WearableData(WearableType.Armor, 2, -5, 20, 0, new List<ResistanceData> { new ResistanceData(DamageType.Physical, 1) }), null, null);
            artifact.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 100000), new ResourceData("Materials", 500), new ResourceData("Tools", 25), new ResourceData("Rare Materials", 25), new ResourceData("Essence", 25) };
            artifact.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Shields, 1) };
            artifactDefinitions.Add(artifact.Key, artifact);
        }

        static void LoadItems()
        {
            ItemDefinition item = new ItemDefinition("Knife", "Knife", "items_large_347", "items_small_692", EquipmentSlot.Right_Hand, 10, 1, 10, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Finesse, 15, 1, 5, 5,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 10), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Dagger", "Dagger", "items_large_371", "items_small_691", EquipmentSlot.Right_Hand, 12, 2, 12, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Finesse, 15, 1, 5, 5,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 1), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 12), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Arrow", "Arrow", "items_large_676", "blank", EquipmentSlot.Left_Hand, 0, 1, 2, 5,
                ItemType.Ammo, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                null, new AmmoData(AmmoType.Arrow, 0, 1, 1,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 2), GameValue.Zero, 0, 0) }),
                null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 1), new ResourceData("Materials", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Hatchet", "Hatchet", "items_large_336", "items_small_648", EquipmentSlot.Right_Hand, 8, 1, 5, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, 15, 1, 6, 3,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 2), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 10), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Axe", "Axe", "items_large_282", "items_small_649", EquipmentSlot.Right_Hand, 10, 2, 10, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, -10, 1, 7, 5,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 4), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 15), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Battle Axe", "Battle Axe", "items_large_482", "items_small_288", EquipmentSlot.Right_Hand, 14, 3, 15, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, -15, 1, 8, 8,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 6), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 20), new ResourceData("Materials", 6), new ResourceData("Tools", 2) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Great Axe", "Great Axe", "items_large_412", "items_small_711", EquipmentSlot.Right_Hand, 20, 5, 20, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, -20, 1, 12, 10,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 5), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 30), new ResourceData("Materials", 8), new ResourceData("Tools", 3) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Short Sword", "Short Sword", "items_large_377", "items_small_687", EquipmentSlot.Right_Hand, 10, 1, 11, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Finesse, 0, 1, 5, 6,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 4), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 12), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 2) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Long Sword", "Long Sword", "items_large_350", "items_small_662", EquipmentSlot.Right_Hand, 14, 2, 15, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, 0, 1, 6, 8,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 6), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 15), new ResourceData("Materials", 6), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Broad Sword", "Broad Sword", "items_large_543", "items_small_665", EquipmentSlot.Right_Hand, 16, 3, 16, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, 0, 1, 7, 8,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 6), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 20), new ResourceData("Materials", 7), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Great Sword", "Great Sword", "items_large_498", "items_small_254", EquipmentSlot.Right_Hand, 20, 4, 20, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Two_Handed_Melee, AmmoType.None, AttackType.Might, 0, 1, 10, 10,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 10), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 30), new ResourceData("Materials", 8), new ResourceData("Tools", 2) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Club", "Club", "items_large_503", "items_small_235", EquipmentSlot.Right_Hand, 8, 1, 2, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, 0, 1, 6, 5,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 2), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 5), new ResourceData("Materials", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Mace", "Mace", "items_large_550", "items_small_668", EquipmentSlot.Right_Hand, 10, 2, 10, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.One_Handed_Melee, AmmoType.None, AttackType.Might, 0, 1, 7, 5,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 4), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 10), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Spear", "Spear", "items_large_380", "items_small_696", EquipmentSlot.Right_Hand, 7, 1, 4, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Polearm, AmmoType.None, AttackType.Might, 0, 2, 6, 7,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 2), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 8), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 2) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("War Spear", "War Spear", "items_large_578", "items_small_695", EquipmentSlot.Right_Hand, 9, 2, 8, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Polearm, AmmoType.None, AttackType.Might, 0, 1, 7, 9,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 5), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 15), new ResourceData("Materials", 6), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Halberd", "Halberd", "items_large_329", "items_small_641", EquipmentSlot.Right_Hand, 12, 3, 16, 5,
                ItemType.Weapon, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Polearm, AmmoType.None, AttackType.Might, 0, 1, 10, 12,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 10), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 28), new ResourceData("Materials", 8), new ResourceData("Tools", 2) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.One_Hand_Melee, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Throwing Knife", "Throwing Knife", "items_large_293", "items_small_306", EquipmentSlot.Right_Hand, 5, 1, 6, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Thrown, AmmoType.None, AttackType.Finesse, 0, 1, 3, 0,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 4, 2), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 1), new ResourceData("Materials", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Thrown, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Short Bow", "Short Bow", "items_large_646", "items_small_218", EquipmentSlot.Right_Hand, 12, 2, 12, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Bow, AmmoType.Arrow, AttackType.Finesse, 0, 10, 7, 0,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 6), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 15), new ResourceData("Materials", 6), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Archery, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Long Bow", "Long Bow", "items_large_447", "items_small_220", EquipmentSlot.Right_Hand, 14, 4, 16, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Bow, AmmoType.Arrow, AttackType.Finesse, 0, 15, 10, 0,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 10), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 20), new ResourceData("Materials", 7), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Archery, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Staff", "Staff", "items_large_354", "items_small_265", EquipmentSlot.Right_Hand, 6, 1, 4, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Polearm, AmmoType.None, AttackType.Might, 0, 2, 6, 8,
                new List<DamageData> { new DamageData(DamageType.Fire, (int)DerivedAttribute.Health, new GameValue(1, 6, 1), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 8), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Polearms, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Wand", "Wand", "items_large_264", "items_small_675", EquipmentSlot.Right_Hand, 100, 1, 12, 5,
                ItemType.Weapon, ItemHardnessAllowed.Soft_or_Hard, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Firearm, AmmoType.None, AttackType.Spell, 0, 10, 4, 0,
                new List<DamageData> { new DamageData(DamageType.Fire, (int)DerivedAttribute.Health, new GameValue(1, 6, 4), GameValue.Zero, 0, 0) }),
                null, null, null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 30), new ResourceData("Materials", 6), new ResourceData("Essence", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Amulet", "Amulet", "items_large_1", "blank", EquipmentSlot.Neck, 5, 1, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Clothing, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 20), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Belt", "Belt", "items_large_176", "blank", EquipmentSlot.Waist, 6, 1, 5, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Clothing, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 8), new ResourceData("Materials", 2) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Bracelet", "Bracelet", "items_large_785", "blank", EquipmentSlot.Arms, 5, 1, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Clothing, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 25), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Climbing Boot", "Climbing Boot", "items_large_194", "items_small_505", EquipmentSlot.Feet, 8, 1, 15, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Clothing, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 16), new ResourceData("Materials", 6), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Shirt", "Cloth Shirt", "items_large_96", "items_small_417", EquipmentSlot.Body, 4, 1, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Clothing, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 6), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Jacket", "Leather Jacket", "items_large_111", "items_small_44", EquipmentSlot.Body, 6, 2, 12, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 1, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 10), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Armor", "Leather Armor", "items_large_149", "items_small_450", EquipmentSlot.Body, 8, 2, 15, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 1, -1, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 14), new ResourceData("Materials", 6), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 2) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Cuirass", "Leather Curaiss", "items_large_130", "items_small_62", EquipmentSlot.Body, 12, 3, 16, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 2, -2, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 18), new ResourceData("Materials", 7), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 5) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Ringmail", "Ringmail", "items_large_148", "items_small_455", EquipmentSlot.Body, 15, 4, 24, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 3, -5, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 25), new ResourceData("Materials", 8), new ResourceData("Tools", 2) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Chainmail", "Chainmail", "items_large_97", "items_small_418", EquipmentSlot.Body, 18, 4, 28, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 3, -4, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 30), new ResourceData("Materials", 9), new ResourceData("Tools", 2) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Breastplate", "Breastplate", "items_large_136", "items_small_469", EquipmentSlot.Body, 22, 5, 32, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 4, -5, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 35), new ResourceData("Materials", 10), new ResourceData("Tools", 3) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Platemail", "Platemail", "items_large_144", "items_small_452", EquipmentSlot.Body, 26, 6, 36, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 5, -7, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 40), new ResourceData("Materials", 12), new ResourceData("Tools", 3) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Half Plate", "Half Plate", "items_large_142", "items_small_470", EquipmentSlot.Body, 30, 7, 34, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 7, -10, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 50), new ResourceData("Materials", 15), new ResourceData("Tools", 3) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Full Plate", "Full Plate", "items_large_106", "items_small_438", EquipmentSlot.Body, 50, 8, 40, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 8, -20, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 100), new ResourceData("Materials", 25), new ResourceData("Tools", 5) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Pants", "Cloth Pants", "items_large_54", "items_small_815", EquipmentSlot.Legs, 4, 1, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Cloth, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Clothing, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 6), new ResourceData("Materials", 3), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Pants", "Leather Pants", "items_large_56", "items_small_810", EquipmentSlot.Legs, 5, 2, 12, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 8), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Shoes", "Leather Shoes", "items_large_192", "items_small_99", EquipmentSlot.Feet, 5, 1, 8, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 6), new ResourceData("Materials", 3) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Boots", "Leather Boots", "items_large_194", "items_small_96", EquipmentSlot.Feet, 6, 1, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 8), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Cloak", "Cloak", "items_large_186", "items_small_113", EquipmentSlot.Back, 5, 1, 6, 5,
                ItemType.Wearable, ItemHardnessAllowed.Cloth, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Clothing, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 5), new ResourceData("Materials", 2) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Hunting Cloak", "Hunting Cloak", "items_large_188", "items_small_107", EquipmentSlot.Back, 12, 2, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Cloth, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 15), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Cap", "Cap", "items_large_244", "items_small_388", EquipmentSlot.Head, 7, 1, 4, 5,
                ItemType.Wearable, ItemHardnessAllowed.Cloth, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 6), new ResourceData("Materials", 2), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Leather Helm", "Leather Helm", "items_large_250", "items_small_157", EquipmentSlot.Head, 8, 2, 6, 5,
                ItemType.Wearable, ItemHardnessAllowed.Leather, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 8), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Metal Helm", "Metal Helm", "items_large_210", "items_small_796", EquipmentSlot.Head, 12, 4, 12, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 12), new ResourceData("Materials", 6), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Full Helm", "Full Helm", "items_large_218", "items_small_769", EquipmentSlot.Head, 16, 6, 16, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 16), new ResourceData("Materials", 8), new ResourceData("Tools", 2) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Wood Buckler", "Wood Buckler", "items_large_69", "items_small_554", EquipmentSlot.Left_Hand, 10, 1, 5, 5,
                ItemType.Wearable, ItemHardnessAllowed.Soft, ItemNameFormat.Material_Middle, null,
                null, new WearableData(WearableType.Armor, 0, -1, 10, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 10), new ResourceData("Materials", 4), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Metal Buckler", "Metal Buckler", "items_large_75", "items_small_183", EquipmentSlot.Left_Hand, 14, 2, 8, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_Middle, null,
                null, new WearableData(WearableType.Armor, 1, -2, 15, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 12), new ResourceData("Materials", 6), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Wood Shield", "Wood Shield", "items_large_95", "items_small_168", EquipmentSlot.Left_Hand, 15, 3, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Soft, ItemNameFormat.Material_Middle, null,
                null, new WearableData(WearableType.Armor, 2, -5, 20, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 15), new ResourceData("Materials", 7), new ResourceData("Tools", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Metal Shield", "Metal Shield", "items_large_93", "items_small_176", EquipmentSlot.Left_Hand, 20, 4, 14, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_Middle, null,
                null, new WearableData(WearableType.Armor, 3, -7, 25, 0, new List<ResistanceData> { new ResistanceData((int)DamageType.Physical, 1) }), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 20), new ResourceData("Materials", 8), new ResourceData("Tools", 2) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Cloth Glove", "Cloth Glove", "items_large_232", "items_small_128", EquipmentSlot.Hands, 4, 1, 8, 5,
                ItemType.Wearable, ItemHardnessAllowed.Cloth, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 3), new ResourceData("Materials", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Gauntlet", "Gauntlet", "items_large_233", "items_small_533", EquipmentSlot.Hands, 10, 5, 8, 5,
                ItemType.Wearable, ItemHardnessAllowed.Metal, ItemNameFormat.Material_First,
                new WeaponData(WeaponType.Unarmed, AmmoType.None, AttackType.Might, 0, 1, 3, 10,
                new List<DamageData> { new DamageData(DamageType.Physical, (int)DerivedAttribute.Health, new GameValue(1, 6, 1), GameValue.Zero, 0, 0) }),
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 3), new ResourceData("Materials", 1) };
            item.SkillRequirements = new List<SkillRequirement> { new SkillRequirement((int)Skill.Unarmed, 1), new SkillRequirement((int)Skill.Light_Armor, 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Cloth Hat", "Cloth Hat", "items_large_244", "items_small_401", EquipmentSlot.Head, 5, 1, 10, 5,
                ItemType.Wearable, ItemHardnessAllowed.Cloth, ItemNameFormat.Material_First, null,
                null, new WearableData(WearableType.Armor, 0, 0, 0, 0, null), null, null);
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 10), new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Small Healing Potion", "Small Healing Potion", "items_large_753", "blank", EquipmentSlot.None, 1, 2, 20, 5,
                ItemType.Accessory, ItemHardnessAllowed.Potion, ItemNameFormat.Material_First, null, null, null,
                new AccessoryData(AccessoryType.Consumable, 5), new UsableData(TimeType.None, 0, new List<AbilityEffect> {
        }));
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 100), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Energy Potion", "Energy Potion", "items_large_750", "blank", EquipmentSlot.None, 1, 2, 20, 5,
                ItemType.Accessory, ItemHardnessAllowed.Potion, ItemNameFormat.Material_First, null, null, null,
                new AccessoryData(AccessoryType.Consumable, 5),
                new UsableData(TimeType.None, 0, new List<AbilityEffect> {
                }));
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 100), new ResourceData("Materials", 10), new ResourceData("Essence", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Spell Scroll", "Spell Scroll", "items_large_904", "blank", EquipmentSlot.None, 1, 1, 30, 5,
                ItemType.Accessory, ItemHardnessAllowed.Scroll, ItemNameFormat.Material_First, null, null, null,
                new AccessoryData(AccessoryType.Consumable, 10),
                new UsableData(TimeType.None, 0, new List<AbilityEffect> { }));
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Coin", 150), new ResourceData("Materials", 5), new ResourceData("Essence", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Orange", "Orange", "items_large_833", "blank", EquipmentSlot.None, 1, 1, 1, 5,
                ItemType.Accessory, ItemHardnessAllowed.Food, ItemNameFormat.Material_First, null, null, null,
                new AccessoryData(AccessoryType.Consumable, 2),
                new UsableData(TimeType.None, 0, new List<AbilityEffect> {
                }));
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Supplies", 1) };
            itemDefinitions.Add(item.Key, item);

            item = new ItemDefinition("Meat", "Meat", "items_large_839", "blank", EquipmentSlot.None, 1, 1, 1, 5,
                ItemType.Accessory, ItemHardnessAllowed.Food, ItemNameFormat.Material_First, null, null, null,
                new AccessoryData(AccessoryType.Consumable, 2),
                new UsableData(TimeType.None, 0, new List<AbilityEffect> {
                }));
            item.ResourcesRequired = new List<ResourceData> { new ResourceData("Supplies", 1) };
            itemDefinitions.Add(item.Key, item);
        }

        static void LoadProfessions()
        {
            Profession temp = new Profession("Citizen", "Citizen", "", 25, new UpkeepData(1, 0, 0, 0), new GameValue(10, 20));
            temp.HealthPerLevel = new GameValue(1); temp.StaminaPerLevel = new GameValue(1); temp.EssencePerLevel = new GameValue(1);
            temp.StartingItems.Add(new ItemShort("Knife", "Copper", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloth Shirt", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloth Pants", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Shoes", "Hide", "", "", ""));
            temp.SkillProficiencies.Add(SkillProficiency.Randomize(new GameValue(1, 4)));
            temp.SkillProficiencies.Add(SkillProficiency.Randomize(new GameValue(1, 4)));
            temp.SkillProficiencies.Add(SkillProficiency.Randomize(new GameValue(1, 4)));
            temp.SkillProficiencies.Add(SkillProficiency.Randomize(new GameValue(1, 4)));
            temp.MinimumAttributes[(int)BaseAttribute.Strength] = 15;
            professions.Add(temp.Key, temp);

            temp = new Profession("Soldier", "Soldier", "", 100, new UpkeepData(2, 1, 1, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(10); temp.StaminaPerLevel = new GameValue(5); temp.EssencePerLevel = new GameValue(1);
            temp.StartingItems.Add(new ItemShort("Short Sword", "Copper", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Metal Buckler", "Copper", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Helm", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Armor", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Pants", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Shoes", "Hide", "", "", ""));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.One_Hand_Melee, 3, 6));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Shields, 3, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Heavy_Armor, 3, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Leadership, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Polearms, 2, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Two_hand_Melee, 2, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Survival, 1, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Training, 1, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Archery, 1, 0));
            temp.AttributePriorities.Add(BaseAttribute.Strength);
            temp.AttributePriorities.Add(BaseAttribute.Endurance);
            temp.AttributePriorities.Add(BaseAttribute.Dexterity);
            temp.AttributePriorities.Add(BaseAttribute.Agility);
            temp.AttributePriorities.Add(BaseAttribute.Senses);
            temp.AttributePriorities.Add(BaseAttribute.Willpower);
            temp.AttributePriorities.Add(BaseAttribute.Memory);
            temp.AttributePriorities.Add(BaseAttribute.Intellect);
            temp.AttributePriorities.Add(BaseAttribute.Wisdom);
            temp.AttributePriorities.Add(BaseAttribute.Charisma);
            temp.MinimumAttributes[(int)BaseAttribute.Strength] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Endurance] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Agility] = 15;
            temp.Powers.Add(new AbilityUnlock(AbilityType.Power, "Taunt", 1));
            professions.Add(temp.Key, temp);

            temp = new Profession("Scout", "Scout", "", 90, new UpkeepData(2, 0, 1, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(5); temp.StaminaPerLevel = new GameValue(5); temp.EssencePerLevel = new GameValue(3);
            temp.StartingItems.Add(new ItemShort("Short Bow", "Wood", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cap", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Jacket", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Pants", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloak", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Shoes", "Hide", "", "", ""));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Archery, 3, 6));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Navigation, 3, 6));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Scouting, 3, 3));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.One_Hand_Melee, 2, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Light_Armor, 2, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Survival, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Air_Magic, 1, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Bucklers, 1, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Precision, 1, 0));
            temp.Powers.Add(new AbilityUnlock(AbilityType.Power, "Eagle Eye", 1));
            temp.AttributePriorities.Add(BaseAttribute.Senses);
            temp.AttributePriorities.Add(BaseAttribute.Agility);
            temp.AttributePriorities.Add(BaseAttribute.Dexterity);
            temp.AttributePriorities.Add(BaseAttribute.Willpower);
            temp.AttributePriorities.Add(BaseAttribute.Strength);
            temp.AttributePriorities.Add(BaseAttribute.Endurance);
            temp.AttributePriorities.Add(BaseAttribute.Intellect);
            temp.AttributePriorities.Add(BaseAttribute.Wisdom);
            temp.AttributePriorities.Add(BaseAttribute.Charisma);
            temp.AttributePriorities.Add(BaseAttribute.Memory);
            temp.MinimumAttributes[(int)BaseAttribute.Senses] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Agility] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 15;
            professions.Add(temp.Key, temp);

            temp = new Profession("Rogue", "Rogue", "", 120, new UpkeepData(3, 0, 1, 0), new GameValue(40, 50));
            temp.HealthPerLevel = new GameValue(7); temp.StaminaPerLevel = new GameValue(5); temp.EssencePerLevel = new GameValue(1);
            temp.StartingItems.Add(new ItemShort("Knife", "Copper", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cap", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Jacket", "Hide", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloth Pants", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloak", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Shoes", "Hide", "", "", ""));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.One_Hand_Melee, 3, 6));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Tricks, 3, 6));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Devices, 3, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Scouting, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Persuasion, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Survival, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Shadow_Magic, 1, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Light_Armor, 1, 0));
            temp.Powers.Add(new AbilityUnlock(AbilityType.Power, "Pickpocket", 1));
            temp.AttributePriorities.Add(BaseAttribute.Dexterity);
            temp.AttributePriorities.Add(BaseAttribute.Agility);
            temp.AttributePriorities.Add(BaseAttribute.Senses);
            temp.AttributePriorities.Add(BaseAttribute.Memory);
            temp.AttributePriorities.Add(BaseAttribute.Willpower);
            temp.AttributePriorities.Add(BaseAttribute.Intellect);
            temp.AttributePriorities.Add(BaseAttribute.Strength);
            temp.AttributePriorities.Add(BaseAttribute.Endurance);
            temp.AttributePriorities.Add(BaseAttribute.Wisdom);
            temp.AttributePriorities.Add(BaseAttribute.Charisma);
            temp.MinimumAttributes[(int)BaseAttribute.Agility] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Senses] = 15;
            professions.Add(temp.Key, temp);

            temp = new Profession("Priest", "Priest", "", 80, new UpkeepData(2, 0, 1, 1), new GameValue(5, 10));
            temp.HealthPerLevel = new GameValue(5); temp.StaminaPerLevel = new GameValue(3); temp.EssencePerLevel = new GameValue(6);
            temp.StartingItems.Add(new ItemShort("Club", "Wood", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Wood Shield", "Wood", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloth Shirt", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloth Pants", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Shoes", "Hide", "", "", ""));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Persuasion, 3, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Life_Magic, 3, 6));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Medium_Armor, 3, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Leadership, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Shields, 2, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Water_Magic, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.One_Hand_Melee, 1, 0));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Channeling, 1, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Polearms, 1, 0));
            temp.Spells.Add(new AbilityUnlock(AbilityType.Spell, "Bless", 1));
            temp.AttributePriorities.Add(BaseAttribute.Wisdom);
            temp.AttributePriorities.Add(BaseAttribute.Charisma);
            temp.AttributePriorities.Add(BaseAttribute.Willpower);
            temp.AttributePriorities.Add(BaseAttribute.Memory);
            temp.AttributePriorities.Add(BaseAttribute.Dexterity);
            temp.AttributePriorities.Add(BaseAttribute.Senses);
            temp.AttributePriorities.Add(BaseAttribute.Strength);
            temp.AttributePriorities.Add(BaseAttribute.Endurance);
            temp.AttributePriorities.Add(BaseAttribute.Agility);
            temp.AttributePriorities.Add(BaseAttribute.Intellect);
            temp.MinimumAttributes[(int)BaseAttribute.Wisdom] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Willpower] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Charisma] = 15;
            professions.Add(temp.Key, temp);

            temp = new Profession("Apprentice", "Apprentice", "", 110, new UpkeepData(1, 0, 1, 1), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(1); temp.StaminaPerLevel = new GameValue(1); temp.EssencePerLevel = new GameValue(10);
            temp.StartingItems.Add(new ItemShort("Staff", "Wood", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloth Shirt", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloth Pants", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Cloak", "Linen", "", "", ""));
            temp.StartingItems.Add(new ItemShort("Leather Shoes", "Hide", "", "", ""));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Channeling, 3, 6));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Fire_Magic, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Air_Magic, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Water_Magic, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Earth_Magic, 2, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Lore, 1, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Arcane_Magic, 1, 1));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Alchemy, 1, 0));
            temp.Powers.Add(new AbilityUnlock(AbilityType.Power, "Metamagic - Empower", 1));
            temp.AttributePriorities.Add(BaseAttribute.Intellect);
            temp.AttributePriorities.Add(BaseAttribute.Memory);
            temp.AttributePriorities.Add(BaseAttribute.Dexterity);
            temp.AttributePriorities.Add(BaseAttribute.Willpower);
            temp.AttributePriorities.Add(BaseAttribute.Senses);
            temp.AttributePriorities.Add(BaseAttribute.Agility);
            temp.AttributePriorities.Add(BaseAttribute.Wisdom);
            temp.AttributePriorities.Add(BaseAttribute.Charisma);
            temp.AttributePriorities.Add(BaseAttribute.Strength);
            temp.AttributePriorities.Add(BaseAttribute.Endurance);
            temp.MinimumAttributes[(int)BaseAttribute.Intellect] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Memory] = 18;
            temp.MinimumAttributes[(int)BaseAttribute.Willpower] = 15;
            professions.Add(temp.Key, temp);

            temp = new Profession("Veteran", "Veteran", "", 150, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(12); temp.StaminaPerLevel = new GameValue(6); temp.EssencePerLevel = new GameValue(1);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.One_Hand_Melee, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Heavy_Armor, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Shields, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Strength] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Endurance] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Skirmisher", "Skirmisher", "", 125, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(8); temp.StaminaPerLevel = new GameValue(5); temp.EssencePerLevel = new GameValue(1);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.One_Hand_Melee, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Two_hand_Melee, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Medium_Armor, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Strength] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Endurance] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Archer", "Archer", "", 160, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(6); temp.StaminaPerLevel = new GameValue(6); temp.EssencePerLevel = new GameValue(1);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Archery, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Medium_Armor, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Scouting, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Agility] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Senses] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Explorer", "Explorer", "", 140, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(3); temp.StaminaPerLevel = new GameValue(5); temp.EssencePerLevel = new GameValue(1);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Navigation, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Scouting, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Survival, 3, 10));
            professions.Add(temp.Key, temp);

            temp = new Profession("Burglar", "Burglar", "", 165, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(3); temp.StaminaPerLevel = new GameValue(5); temp.EssencePerLevel = new GameValue(2);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Stealth, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Devices, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Tricks, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Agility] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Senses] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Mercenary", "Mercenary", "", 150, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(6); temp.StaminaPerLevel = new GameValue(7); temp.EssencePerLevel = new GameValue(1);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.One_Hand_Melee, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Medium_Armor, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Stealth, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Strength] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Agility] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Cleric", "Cleric", "", 100, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(8); temp.StaminaPerLevel = new GameValue(5); temp.EssencePerLevel = new GameValue(5);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Life_Magic, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Persuasion, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Medium_Armor, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Wisdom] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Charisma] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Willpower] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Druid", "Druid", "", 85, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(6); temp.StaminaPerLevel = new GameValue(4); temp.EssencePerLevel = new GameValue(6);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Water_Magic, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Survival, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Scouting, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Wisdom] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Willpower] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Senses] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Wizard", "Wizard", "", 175, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(2); temp.StaminaPerLevel = new GameValue(2); temp.EssencePerLevel = new GameValue(10);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Channeling, 3, 12));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Arcane_Magic, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Lore, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Intellect] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Memory] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Dexterity] = 20;
            professions.Add(temp.Key, temp);

            temp = new Profession("Elementalist", "Elementalist", "", 175, new UpkeepData(5, 0, 0, 0), new GameValue(20, 30));
            temp.HealthPerLevel = new GameValue(2); temp.StaminaPerLevel = new GameValue(2); temp.EssencePerLevel = new GameValue(10);
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Fire_Magic, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Air_Magic, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Water_Magic, 3, 10));
            temp.SkillProficiencies.Add(new SkillProficiency(Skill.Earth_Magic, 3, 10));
            temp.MinimumAttributes[(int)BaseAttribute.Intellect] = 25;
            temp.MinimumAttributes[(int)BaseAttribute.Memory] = 20;
            temp.MinimumAttributes[(int)BaseAttribute.Willpower] = 20;
            professions.Add(temp.Key, temp);
        }

        static void LoadRaces()
        {
            Race temp = new Race("Beastman", "Beastman", "race_69", "race_68", false, "", "", -1, -1, false, false, false,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 1.0f, new UpkeepData(0, 9, 0, 0), new GameValue(1, 5));
            temp.Powers.Add(new AbilityUnlock(AbilityType.Power, "Bestial Rage", 1));
            temp.StartingAttributes[(int)BaseAttribute.Strength].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Endurance].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Intellect].Number = -5;
            temp.StartingAttributes[(int)BaseAttribute.Willpower].Number = 5;
            races.Add(temp.Key, temp);

            temp = new Race("Deep Dwarf", "Deep Dwarf", "race_11", "race_10", true, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 8, 1.0f, new UpkeepData(0, 9, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Dark Vision", 1));
            temp.StartingAttributes[(int)BaseAttribute.Endurance].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Agility].Number = -2;
            temp.StartingAttributes[(int)BaseAttribute.Intellect].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Willpower].Number = 2;
            races.Add(temp.Key, temp);

            temp = new Race("Spriggan", "Spriggan", "race_97", "race_96", false, "", "", -1, -1, false, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 6, 0.5f, new UpkeepData(0, 4, 0, 0), new GameValue(1, 5));
            temp.StartingAttributes[(int)BaseAttribute.Agility].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Dexterity].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Senses].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Intellect].Number = -3;
            races.Add(temp.Key, temp);

            temp = new Race("Forest Elf", "Forest Elf", "race_43", "race_42", false, "", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 11, 0.75f, new UpkeepData(0, 5, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Camoflage", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Goblin", "Goblin", "race_61", "race_60", true, "", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 9, 0.8f, new UpkeepData(0, 8, 0, 0), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Godkin", "Godkin", "race_51", "race_50", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 12, 0.1f, new UpkeepData(0, 0, 0, 5), new GameValue(1, 5));
            temp.Resistances.Add(new ResistanceData(DamageType.Holy, 95));
            races.Add(temp.Key, temp);

            temp = new Race("Half Orc", "Half Orc", "race_9", "race_8", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 1.0f, new UpkeepData(0, 8, 0, 0), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Halfling", "Halfling", "race_43", "race_42", true, "", "", 0, -1, true, true, false,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 1.0f, new UpkeepData(0, 12, 0, 0), new GameValue(1, 5));
            temp.StartingAttributes[(int)BaseAttribute.Endurance].Number = 2;
            temp.StartingAttributes[(int)BaseAttribute.Agility].Number = 4;
            temp.StartingAttributes[(int)BaseAttribute.Charisma].Number = 4;
            temp.StartingAttributes[(int)BaseAttribute.Memory].Number = 2;
            races.Add(temp.Key, temp);

            temp = new Race("High Elf", "High Elf", "race_13", "race_12", false, "", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 0.5f, new UpkeepData(0, 5, 0, 0), new GameValue(1, 5));
            temp.StartingAttributes[(int)BaseAttribute.Endurance].Number = -2;
            temp.StartingAttributes[(int)BaseAttribute.Agility].Number = 4;
            temp.StartingAttributes[(int)BaseAttribute.Intellect].Number = 4;
            temp.StartingAttributes[(int)BaseAttribute.Memory].Number = 2;
            races.Add(temp.Key, temp);

            temp = new Race("Mountain Dwarf", "Mountain Dwarf", "race_15", "race_14", true, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 9, 1.0f, new UpkeepData(0, 8, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Dark Vision", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Imperial", "Imperial", "race_51", "race_50", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 1.1f, new UpkeepData(0, 7, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Fast Learner", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Southerner", "Southerner", "race_89", "race_88", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 1.1f, new UpkeepData(0, 7, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Fast Learner", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Ogrin", "Ogrin", "race_51", "race_50", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 1.1f, new UpkeepData(0, 7, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Fast Learner", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Orc", "Orc", "race_9", "race_8", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 1.0f, new UpkeepData(0, 9, 0, 0), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Plains Dwarf", "Plains Dwarf", "race_55", "race_54", true, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 9, 1.1f, new UpkeepData(0, 9, 0, 0), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Sidhe", "Sidhe", "race_13", "race_12", false, "race_323", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 16, 0.2f, new UpkeepData(0, 2, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Flight", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Revenant", "Revenant", "race_73", "race_72", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 7, 0.2f, new UpkeepData(0, 0, 0, 4), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Undead", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Shade", "Shade", "race_16", "race_15", false, "race_321", "", -1, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 11, 0.5f, new UpkeepData(0, 0, 0, 6), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Undead", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Shadow Elf", "Shadow Elf", "race_49", "race_48", false, "", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 0.5f, new UpkeepData(0, 4, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Dark Vision", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Trollkin", "Trollkin", "race_47", "race_46", false, "", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 0.75f, new UpkeepData(0, 11, 0, 0), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Regeneration", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Valarian", "Valarian", "race_51", "race_50", false, "", "", -1, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 10, 0.75f, new UpkeepData(0, 5, 0, 1), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Vampire", "Vampire", "race_83", "race_82", false, "race_320", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 14, 0.1f, new UpkeepData(0, 0, 0, 6), new GameValue(1, 5));
            temp.Traits.Add(new AbilityUnlock(AbilityType.Trait, "Undead", 1));
            races.Add(temp.Key, temp);

            temp = new Race("Wolfen", "Wolfen", "race_81", "race_80", false, "", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 12, 1.0f, new UpkeepData(0, 9, 0, 1), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Infernal", "Infernal", "race_19", "race_18", false, "", "", 0, -1, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 12, 1.0f, new UpkeepData(0, 7, 0, 1), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Gnome", "Gnome", "race_15", "race_14", false, "", "", 0, 0, true, true, true,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 12, 1.0f, new UpkeepData(0, 5, 1, 1), new GameValue(1, 5));
            races.Add(temp.Key, temp);

            temp = new Race("Half Ogre", "Half Ogre", "race_63", "race_62", false, "", "", 0, 0, false, true, false,
                new GameValue(1, 10), new GameValue(1, 10), new GameValue(1, 10), 12, 1.0f, new UpkeepData(0, 11, 0, 0), new GameValue(1, 5));
            races.Add(temp.Key, temp);
        }

        static void LoadTraits()
        {
            Ability ability = new Ability("Regeneration", "Regeneration", "abilities_77", AbilityClass.None, AbilityType.Trait);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Fast Learner", "Fast Learner", "abilities_232", AbilityClass.None, AbilityType.Trait);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Undead", "Undead", "abilities_60", AbilityClass.None, AbilityType.Trait);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Camoflage", "Camoflage", "trait", AbilityClass.None, AbilityType.Trait);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Dark Vision", "Dark Vision", "trait", AbilityClass.None, AbilityType.Trait);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Flight", "Flight", "trait", AbilityClass.None, AbilityType.Trait, 0);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Strong", "Strong", "trait", AbilityClass.None, AbilityType.Trait, 0);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Scaly Hide", "Scaly Hide", "trait", AbilityClass.None, AbilityType.Trait, 0, Skill.None);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Weak", "Weak", "trait", AbilityClass.None, AbilityType.Trait);
            ability.Components.Add(new TraitTypeComponent(TraitType.Misc));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Crippled", "Crippled", "trait", AbilityClass.None, AbilityType.Trait);
            ability.Components.Add(new TraitTypeComponent(TraitType.Wound));
            ability.Components.Add(new DurationComponent(DurationType.Permanent));
            abilities.Add(ability.Key, ability);
        }

        static void LoadPowers()
        {
            Ability ability = new Ability("Strike", "Strike", "abilities_178", AbilityClass.Encounter, AbilityType.Power, 0, Skill.One_Hand_Melee, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Power Strike", "Power Strike", "abilities_178", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.One_Hand_Melee, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("2H Strike", "2H Strike", "abilities_178", AbilityClass.Encounter, AbilityType.Power, 0, Skill.Two_hand_Melee, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Reckless Strike", "Reckless Strike", "abilities_439", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Two_hand_Melee, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Whirlwind", "Whirlwind", "abilities_439", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Two_hand_Melee, 10);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Sphere));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Long Strike", "Long Strike", "abilities_343", AbilityClass.Encounter, AbilityType.Power, 0, Skill.Polearms, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 3));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Beam));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Piercing Strike", "Piercing Strike", "abilities_343", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Polearms, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 3));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Beam));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Stunning Blow", "Stunning Blow", "abilities_345", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.One_Hand_Melee, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);


            ability = new Ability("Taunt", "Taunt", "abilities_419", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Improved Taunt", "Improved Taunt", "abilities_419", AbilityClass.Encounter, AbilityType.Power, 1000);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 2));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Sphere));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Pickpocket", "Pickpocket", "abilities_429", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Metamagic - Empower", "Metamagic - Empower", "abilities_404", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Metamagic - Overcharge", "Metamagic - Overcharge", "abilities_404", AbilityClass.Encounter, AbilityType.Power, 1000);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Metamagic - Quicken", "Metamagic - Quicken", "abilities_404", AbilityClass.Encounter, AbilityType.Power, 1000);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Eagle Eye", "Eagle Eye", "abilities_310", AbilityClass.Encounter, AbilityType.Power, 1000);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Repair", "Repair", "power", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Crafting, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Rebuild", "Rebuild", "power", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Engineering, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Careful Strike", "Careful Strike", "abilities_4", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.One_Hand_Melee, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Shoot", "Shoot", "abilities_118", AbilityClass.Encounter, AbilityType.Power, 0, Skill.Archery, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Accurate Shot", "Accurate Shot", "abilities_118", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Archery, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Fire", "Fire", "abilities_118", AbilityClass.Encounter, AbilityType.Power, 0, Skill.Firearms, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Throw", "Throw", "abilities_118", AbilityClass.Encounter, AbilityType.Power, 0, Skill.Thrown, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Trick Throw", "Trick Throw", "abilities_291", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Tricks, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Point Blank Shot", "Point Blank Shot", "abilities_421", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Firearms, 5);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Weapon));
            ability.Components.Add(new TargetComponent(TargetType.Enemy));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Bestial Rage", "Bestial Rage", "abilities_424", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Self));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Buckler Block", "Buckler Block", "abilities_257", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Bucklers, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Self));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Shield Block", "Shield Block", "abilities_257", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Shields, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Self));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Rally", "Rally", "abilities_445", AbilityClass.Encounter, AbilityType.Power, 1000, Skill.Leadership, 1);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.Turn, 5));
            ability.Components.Add(new RangeComponent(RangeType.Self));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            ability.Components.Add(new AreaComponent(AreaType.Single));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Fire Breath", "Fire Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Lava Breath", "Lava Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Lightning Breath", "Lightning Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Ice Breath", "Ice Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Poison Breath", "Poison Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Acid Breath", "Acid Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Holy Breath", "Holy Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Unholy Breath", "Unholy Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Shadow Breath", "Shadow Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Arcane Breath", "Arcane Breath", "abilities_380", AbilityClass.Encounter, AbilityType.Power);
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Stamina, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new AreaComponent(AreaType.Cone));
            abilities.Add(ability.Key, ability);
        }

        static void LoadSpells()
        {
            Ability ability = new Ability("Torchlight", "Torchlight", "abilities_24", AbilityClass.Encounter, AbilityType.Spell, 0, Skill.Fire_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Fire, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new TargetComponent(TargetType.Self, 1));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Firebolt", "Firebolt", "abilities_24", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Fire_Magic, 5);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Fire, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new TargetComponent(TargetType.Enemy, 1));
            ability.Effects.Add(new DamageEffect(DamageType.Fire, (int)DerivedAttribute.Health, new GameValue(1, 6), GameValue.Zero, 0, 0, 0f, 3));
            ability.Effects.Add(new DamageEffect(DamageType.Fire, (int)DerivedAttribute.Health, new GameValue(1), new GameValue(1, 3), 0, 0, 0f, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Fireball", "Fireball", "abilities_19", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Fire_Magic, 10);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Fire, 3));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 20));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 10));
            ability.Components.Add(new TargetComponent(TargetType.Any));
            ability.Components.Add(new AreaComponent(AreaType.Sphere, 3));
            ability.Effects.Add(new DamageEffect(DamageType.Fire, (int)DerivedAttribute.Health, new GameValue(2, 4), GameValue.Zero, 0, 0, 0f, 3));
            ability.Effects.Add(new DamageEffect(DamageType.Fire, (int)DerivedAttribute.Health, new GameValue(1, 2), new GameValue(1, 4), 0, 0, 0f, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Hasten", "Hasten", "abilities_24", AbilityClass.Encounter, AbilityType.Spell, 0, Skill.Air_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Fire, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new TargetComponent(TargetType.Self, 1));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Actions, new GameValue(2, 6), GameValue.Zero, true, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Shockbolt", "Shockbolt", "abilities_293", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Air_Magic, 5);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Air, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new TargetComponent(TargetType.Enemy, 1));
            ability.Effects.Add(new DamageEffect(DamageType.Shock, (int)DerivedAttribute.Health, new GameValue(1, 4), GameValue.Zero, 0, 0, 0f, 3));
            ability.Effects.Add(new DamageEffect(DamageType.Shock, (int)DerivedAttribute.Stamina, new GameValue(1, 6), GameValue.Zero, 0, 0, 0f, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Lightning Bolt", "Lightning Bolt", "abilities_375", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Air_Magic, 10);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Air, 3));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 25));
            ability.Components.Add(new DurationComponent(DurationType.Instant, TimeType.None));
            ability.Components.Add(new CooldownComponent(TimeType.None));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 5));
            ability.Components.Add(new TargetComponent(TargetType.Any));
            ability.Components.Add(new AreaComponent(AreaType.Beam, 10));
            ability.Effects.Add(new DamageEffect(DamageType.Shock, (int)DerivedAttribute.Health, new GameValue(1, 6), GameValue.Zero, 0, 0, 0f, 3));
            ability.Effects.Add(new DamageEffect(DamageType.Shock, (int)DerivedAttribute.Stamina, new GameValue(1, 6), GameValue.Zero, 0, 0, 0f, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Armor!", "Armor!", "abilities_420", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Earth_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Earth, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Touch));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Armor, new GameValue(1, 4), GameValue.Zero, true, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Stone Skin", "Stone Skin", "abilities_420", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Earth_Magic, 5);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Earth, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Touch));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            ability.Effects.Add(new AlterCharacteristicEffect(AttributeType.Derived, (int)DerivedAttribute.Armor, new GameValue(1, 4), new GameValue(2, 6)));
            ability.Effects.Add(new AlterCharacteristicEffect(AttributeType.Resistance, (int)DamageType.Physical, new GameValue(10), new GameValue(2, 6)));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Lesser Regen", "Lesser Regen", "abilities_82", AbilityClass.Encounter, AbilityType.Spell, 0, Skill.Water_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Water, 2));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 15));
            ability.Components.Add(new DurationComponent(DurationType.Duration));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Touch));
            ability.Components.Add(new TargetComponent(TargetType.Friend, 1));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Health, new GameValue(1, 4), new GameValue(2, 6), false, 5));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Lesser Drain", "Lesser Drain", "abilities_163", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Death_Magic, 5);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Death, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Duration));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 10));
            ability.Components.Add(new TargetComponent(TargetType.Friend, 1));
            ability.Effects.Add(new DamageEffect(DamageType.Unholy, (int)DerivedAttribute.Health, new GameValue(1, 6), GameValue.Zero, 0, 0, 0f, 3));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Health, new GameValue(1, 6), GameValue.Zero, true, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Curse", "Curse", "abilities_171", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Death_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Death, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            ability.Components.Add(new AreaComponent(AreaType.Sphere, 10));
            ability.Effects.Add(new AlterCharacteristicEffect(AttributeType.Derived, (int)DerivedAttribute.Might_Attack, new GameValue(-1, -6), new GameValue(2, 6)));
            ability.Effects.Add(new AlterCharacteristicEffect(AttributeType.Derived, (int)DerivedAttribute.Finesse_Attack, new GameValue(-1, -6), new GameValue(2, 6)));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Morale, new GameValue(-1, -4), GameValue.Zero, false, 2));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Lesser Heal", "Lesser Heal", "abilities_188", AbilityClass.Encounter, AbilityType.Spell, 0, Skill.Life_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Life, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Duration));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 10));
            ability.Components.Add(new TargetComponent(TargetType.Friend, 1));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Health, new GameValue(1, 6), GameValue.Zero, false, 2));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Lesser Courage", "Lesser Courage", "abilities_315", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Life_Magic, 5);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Life, 2));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Duration));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 10));
            ability.Components.Add(new TargetComponent(TargetType.Friend, 1));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Morale, new GameValue(1, 6), GameValue.Zero, false, 2));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Bless", "Bless", "abilities_189", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Life_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Life, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 10));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new TargetComponent(TargetType.Friend));
            ability.Components.Add(new AreaComponent(AreaType.Sphere, 10));
            ability.Effects.Add(new AlterCharacteristicEffect(AttributeType.Derived, (int)DerivedAttribute.Might_Attack, new GameValue(1, 4), new GameValue(2, 6)));
            ability.Effects.Add(new AlterCharacteristicEffect(AttributeType.Derived, (int)DerivedAttribute.Finesse_Attack, new GameValue(1, 4), new GameValue(2, 6)));
            //ability.Effects.Add(new RestoreEffect(RestoreType.Morale, new GameValue(1, 4), GameValue.Zero, false, 2));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Mirror Image", "Mirror Image", "abilities_191", AbilityClass.World, AbilityType.Spell, 1000, Skill.Shadow_Magic, 5);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Shadow, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 20));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Minute, 1));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Detect Illusion", "Detect Illusion", "abilities_468", AbilityClass.World, AbilityType.Spell, 1000, Skill.Shadow_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Shadow, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 20));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Minute, 1));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Arcane Missile", "Arcane Missile", "abilities_406", AbilityClass.Encounter, AbilityType.Spell, 1000, Skill.Arcane_Magic, 5);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Arcane, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 20));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Hour, 1));
            ability.Components.Add(new RangeComponent(RangeType.Distance, 10));
            ability.Components.Add(new TargetComponent(TargetType.Any));
            ability.Components.Add(new AreaComponent(AreaType.Sphere, 3));
            ability.Effects.Add(new DamageEffect(DamageType.Arcane, (int)DerivedAttribute.Health, new GameValue(1, 4), GameValue.Zero, 0, 0, 0f, 3));
            ability.Effects.Add(new DamageEffect(DamageType.Arcane, (int)DerivedAttribute.Essence, new GameValue(1, 4), GameValue.Zero, 0, 0, 0f, 3));
            abilities.Add(ability.Key, ability);

            ability = new Ability("Identify", "Identify", "abilities_442", AbilityClass.World, AbilityType.Spell, 1000, Skill.Arcane_Magic, 1);
            ability.Components.Add(new SpellLevelComponent(SpellSchoolType.Arcane, 1));
            ability.Components.Add(new ResourceComponent(DerivedAttribute.Essence, 20));
            ability.Components.Add(new DurationComponent(DurationType.Instant));
            ability.Components.Add(new CooldownComponent(TimeType.Minute, 1));
            abilities.Add(ability.Key, ability);
        }

        static void LoadAbilities()
        {
            Ability ability = new Ability("Blank", "Blank", "blank", AbilityClass.None, AbilityType.None);
            abilities.Add(ability.Key, ability);

            LoadQuirks();
            LoadTraits();
            LoadPowers();
            LoadSpells();
        }

        public static void LoadAbilityModifiers()
        {
            AbilityModifier rune = new AbilityModifier("Weak", "Weak");
            rune.Modifiers.Add(new ResourceModifier(-10));
            rune.Modifiers.Add(new DurationModifier(-2));
            rune.Modifiers.Add(new CooldownModifier(-2));
            rune.Modifiers.Add(new RangeModifier(-2));
            rune.Modifiers.Add(new AreaModifier(-2, -15));

            runes.Add(rune.Key, rune);

            rune = new AbilityModifier("Dim", "Dim");
            rune.Modifiers.Add(new ResourceModifier(-5));
            rune.Modifiers.Add(new DurationModifier(-1));
            rune.Modifiers.Add(new CooldownModifier(-1));
            rune.Modifiers.Add(new RangeModifier(-1));
            rune.Modifiers.Add(new AreaModifier(-1, -10));

            runes.Add(rune.Key, rune);

            rune = new AbilityModifier("Empowered", "Empowered");
            rune.Modifiers.Add(new ResourceModifier(10));
            rune.Modifiers.Add(new DurationModifier(2));
            rune.Modifiers.Add(new CooldownModifier(2));
            rune.Modifiers.Add(new RangeModifier(2));
            rune.Modifiers.Add(new AreaModifier(2, 15));

            runes.Add(rune.Key, rune);

            rune = new AbilityModifier("Devastating", "Devastating");
            rune.Modifiers.Add(new ResourceModifier(100));
            rune.Modifiers.Add(new DurationModifier(5));
            rune.Modifiers.Add(new CooldownModifier(5));
            rune.Modifiers.Add(new RangeModifier(5));
            rune.Modifiers.Add(new AreaModifier(5, 15));

            runes.Add(rune.Key, rune);
        }

        public static void LoadResearchTree()
        {
            // COMBAT RESEARCH
            ResearchEntry entry = new ResearchEntry("Combat Basics", "Combat Basics", "Combat", 1, 300, ResearchCategory.Combat, "",
                new List<ResourceData> { new ResourceData("Knowledge", 50), null, null }, new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Intermediate Combat", "Intermediate Combat", "Combat", 3, 1000, ResearchCategory.Combat,
                 "Combat Basics", new List<ResourceData> { new ResourceData("Knowledge", 2000), new ResourceData("Coin", 4000), null },
                 new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Advanced Combat", "Advanced Combat", "Combat", 6, 1800, ResearchCategory.Combat,
                 "Intermediate Combat", new List<ResourceData> { new ResourceData("Knowledge", 5000), new ResourceData("Coin", 10000), null },
                 new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Melee 1", "Melee 1", "Melee Combat", 2, 500, ResearchCategory.Combat,
                "Combat Basics", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Coin", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Melee 2", "Melee 2", "Melee Combat", 3, 900, ResearchCategory.Combat,
                "Melee 1", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Coin", 1000), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Melee 3", "Melee 3", "Melee Combat", 4, 1300, ResearchCategory.Combat,
                "Melee 2", new List<ResourceData> { new ResourceData("Knowledge", 2000), new ResourceData("Coin", 5000), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Dual Wielding 1", "Dual Wielding 1", "Dual Wielding", 3, 1000, ResearchCategory.Combat,
                "Melee 1", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Coin", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Dual Wielding 2", "Dual Wielding 2", "Dual Wielding", 4, 1500, ResearchCategory.Combat,
                "Dual Wielding 1", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Coin", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("One Handed Combat 1", "One Handed Combat 1", "One Handed Combat", 3, 800, ResearchCategory.Combat,
                "Melee 1", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Coin", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("One Handed Combat 2", "One Handed Combat 2", "One Handed Combat", 4, 1100, ResearchCategory.Combat,
                "One Handed Combat 1", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Coin", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Ranged 1", "Ranged 1", "Ranged Combat", 2, 500, ResearchCategory.Combat,
                "Combat Basics", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Coin", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Ranged 2", "Ranged 2", "Ranged Combat", 3, 800, ResearchCategory.Combat,
                "Ranged 1", new List<ResourceData> { new ResourceData("Knowledge", 450), new ResourceData("Coin", 900), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Ranged 3", "Ranged 3", "Ranged Combat", 4, 1000, ResearchCategory.Combat,
                "Ranged 2", new List<ResourceData> { new ResourceData("Knowledge", 1500), new ResourceData("Coin", 4000), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            // MAGIC RESEARCH
            entry = new ResearchEntry("Simple Magic", "Simple Magic", "Magic", 1, 500, ResearchCategory.Magic,
                "", new List<ResourceData> { new ResourceData("Knowledge", 50), null, null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Intermediate Magic", "Intermediate Magic", "Magic", 3, 1500, ResearchCategory.Magic,
                 "Simple Magic", new List<ResourceData> { new ResourceData("Knowledge", 2000), new ResourceData("Essence", 6000), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Advanced Magic", "Advanced Magic", "Magic", 6, 3000, ResearchCategory.Magic,
                 "Intermediate Magic", new List<ResourceData> { new ResourceData("Knowledge", 5000), new ResourceData("Essence", 12000), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Enchanting", "Enchanting", "Enchanting", 2, 1000, ResearchCategory.Magic,
                "Simple Magic", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Essence", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Advanced Enchanting", "Advanced Enchanting", "Enchanting", 4, 2000, ResearchCategory.Magic,
                "Enchanting", new List<ResourceData> { new ResourceData("Knowledge", 600), new ResourceData("Essence", 2500), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Fire Magic 1", "Fire Magic 1", "Fire Magic", 2, 1000, ResearchCategory.Magic,
                "Simple Magic", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Essence", 75), null },
                new List<string>(),
                new List<ResearchEntryUnlock>
                {
                new ResearchEntryUnlock(EntryUnlockType.Spell, "Fireball"), new ResearchEntryUnlock(EntryUnlockType.Spell, "Firebolt")
                });
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Fire Magic 2", "Fire Magic 2", "Fire Magic", 4, 2000, ResearchCategory.Magic,
                "Fire Magic 1", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Essence", 1800), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Fire Magic 3", "Fire Magic 3", "Fire Magic", 6, 3000, ResearchCategory.Magic,
                "Fire Magic 2", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Essence", 1800), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Water Magic 1", "Water Magic 1", "Water Magic", 2, 1000, ResearchCategory.Magic,
                "Simple Magic", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Essence", 75), null },
                new List<string>(),
                new List<ResearchEntryUnlock>
                {
                new ResearchEntryUnlock(EntryUnlockType.Spell, "Lesser Heal")
                });
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Water Magic 2", "Water Magic 2", "Water Magic", 4, 2000, ResearchCategory.Magic,
                "Water Magic 1", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Essence", 1800), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Water Magic 3", "Water Magic 3", "Water Magic", 6, 3000, ResearchCategory.Magic,
                "Water Magic 2", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Essence", 1800), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            // STRONGHOLD RESEARCH
            entry = new ResearchEntry("Construction 1", "Construction 1", "Stronghold", 1, 250, ResearchCategory.Stronghold,
                "", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Materials", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Construction 2", "Construction 2", "Stronghold", 3, 750, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Materials", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Construction 3", "Construction 3", "Stronghold", 5, 1500, ResearchCategory.Stronghold,
                "Construction 2", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Materials", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Simple Buildings", "Simple Buildings", "Stronghold Buildings", 2, 500, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 150), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Military Buildings 1", "Military Buildings 1", "Military Buildings", 3, 750, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 150), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Production Buildings 1", "Production Buildings 1", "Production Buildings", 3, 750, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 150), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Resource Buildings 1", "Resource Buildings 1", "Resource Buildings", 3, 750, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 150), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Simple Defenses", "Simple Defenses", "Stronghold Defenses", 2, 500, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 150), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Simple Resources", "Simple Resources", "Stronghold Resources", 2, 500, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 150), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Steel", "Steel", "Resource", 2, 500, ResearchCategory.Stronghold,
                "Construction 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 150), null },
                new List<string>(),
                new List<ResearchEntryUnlock>
                {
                new ResearchEntryUnlock(EntryUnlockType.Item_Material, "Steel"),
                });
            researchEntries.Add(entry.Key, entry);

            // LIBRARY RESEARCH
            entry = new ResearchEntry("Sage Unlocks 1", "Sage Unlocks 1", "Library", 1, 500, ResearchCategory.Library,
                "", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Materials", 50), new ResourceData("Essence", 1000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Research 1", "Research 1", "Library Research", 2, 1000, ResearchCategory.Library,
                "Sage Unlocks 1", new List<ResourceData> { new ResourceData("Knowledge", 150), new ResourceData("Materials", 150), new ResourceData("Essence", 3000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Research 2", "Research 2", "Library Research", 3, 1500, ResearchCategory.Library,
                "Research 1", new List<ResourceData> { new ResourceData("Knowledge", 550), new ResourceData("Materials", 750), new ResourceData("Essence", 6000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Research 3", "Research 3", "Library Research", 4, 2000, ResearchCategory.Library,
                "Research 2", new List<ResourceData> { new ResourceData("Knowledge", 1000), new ResourceData("Materials", 1500), new ResourceData("Essence", 9000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Research 4", "Research 4", "Library Research", 5, 2500, ResearchCategory.Library,
                "Research 3", new List<ResourceData> { new ResourceData("Knowledge", 2500), new ResourceData("Materials", 5000), new ResourceData("Essence", 20000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Enchanter Unlocks 1", "Enchanter Unlocks 1", "Library", 1, 500, ResearchCategory.Library,
                "", new List<ResourceData> { new ResourceData("Knowledge", 200), new ResourceData("Materials", 300), new ResourceData("Essence", 2000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Enchantments 1", "Enchantments 1", "Enchantments", 2, 1000, ResearchCategory.Library,
                "Enchanter Unlocks 1", new List<ResourceData> { new ResourceData("Knowledge", 250), new ResourceData("Materials", 250), new ResourceData("Essence", 3000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Enchantments 2", "Enchantments 2", "Enchantments", 3, 1500, ResearchCategory.Library,
                "Enchantments 1", new List<ResourceData> { new ResourceData("Knowledge", 600), new ResourceData("Materials", 800), new ResourceData("Essence", 4500) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Mystic Unlocks 1", "Mystic Unlocks 1", "Library", 1, 500, ResearchCategory.Library,
                "", new List<ResourceData> { new ResourceData("Knowledge", 250), new ResourceData("Materials", 250), new ResourceData("Essence", 3000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Runes 1", "Runes 1", "Runes", 2, 1000, ResearchCategory.Library,
                "Mystic Unlocks 1", new List<ResourceData> { new ResourceData("Knowledge", 250), new ResourceData("Materials", 250), new ResourceData("Essence", 5000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Runes 2", "Runes 2", "Runes", 3, 1500, ResearchCategory.Library,
                "Runes 1", new List<ResourceData> { new ResourceData("Knowledge", 700), new ResourceData("Materials", 1000), new ResourceData("Essence", 10000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            // STOCKPILE RESEARCH
            entry = new ResearchEntry("Stockpile Unlocks 1", "Stockpile Unlocks 1", "Stockpile", 1, 200, ResearchCategory.Stockpile,
                "", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Materials", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Storage Upgrades 1", "Storage Upgrades 1", "Storage Upgrades", 2, 400, ResearchCategory.Stockpile,
                "Stockpile Unlocks 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 100), new ResourceData("Coin", 250) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Storage Upgrades 2", "Storage Upgrades 2", "Storage Upgrades", 3, 600, ResearchCategory.Stockpile,
                "Storage Upgrades 1", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Materials", 650), new ResourceData("Coin", 1000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Storage Upgrades 3", "Storage Upgrades 3", "Storage Upgrades", 4, 800, ResearchCategory.Stockpile,
                "Storage Upgrades 2", new List<ResourceData> { new ResourceData("Knowledge", 1000), new ResourceData("Materials", 1500), new ResourceData("Coin", 5000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Inventory Slots 1", "Inventory Slots 1", "Inventory Slots", 2, 400, ResearchCategory.Stockpile,
                "Stockpile Unlocks 1", new List<ResourceData> { new ResourceData("Knowledge", 150), new ResourceData("Materials", 100), new ResourceData("Coin", 300) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Inventory Slots 2", "Inventory Slots 2", "Inventory Slots", 3, 600, ResearchCategory.Stockpile,
                "Inventory Slots 1", new List<ResourceData> { new ResourceData("Knowledge", 450), new ResourceData("Materials", 300), new ResourceData("Coin", 500) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Inventory Slots 3", "Inventory Slots 3", "Inventory Slots", 4, 800, ResearchCategory.Stockpile,
                "Inventory Slots 2", new List<ResourceData> { new ResourceData("Knowledge", 750), new ResourceData("Materials", 500), new ResourceData("Coin", 1000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            // BARRACKS RESEARCH
            entry = new ResearchEntry("Barracks Unlocks 1", "Barracks Unlocks 1", "Barracks", 1, 200, ResearchCategory.Barracks,
                "", new List<ResourceData> { new ResourceData("Knowledge", 75), new ResourceData("Supplies", 75), null },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Accessory Slots 1", "Accessory Slots 1", "Accessory Slots", 2, 400, ResearchCategory.Barracks,
                "Barracks Unlocks 1", new List<ResourceData> { new ResourceData("Knowledge", 100), new ResourceData("Materials", 100), new ResourceData("Supplies", 100) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Accessory Slots 2", "Accessory Slots 2", "Accessory Slots", 3, 600, ResearchCategory.Barracks,
                "Accessory Slots 1", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Materials", 500), new ResourceData("Supplies", 500) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Accessory Slots 3", "Accessory Slots 3", "Accessory Slots", 4, 800, ResearchCategory.Barracks,
                "Accessory Slots 2", new List<ResourceData> { new ResourceData("Knowledge", 1000), new ResourceData("Materials", 1000), new ResourceData("Supplies", 1000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Exp Bonuses 1", "Exp Bonuses 1", "Exp Bonuses", 2, 400, ResearchCategory.Barracks,
                "Barracks Unlocks 1", new List<ResourceData> { new ResourceData("Knowledge", 500), new ResourceData("Materials", 500), new ResourceData("Supplies", 500) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Exp Bonuses 2", "Exp Bonuses 2", "Exp Bonuses", 3, 600, ResearchCategory.Barracks,
                "Exp Bonuses 1", new List<ResourceData> { new ResourceData("Knowledge", 1500), new ResourceData("Materials", 1500), new ResourceData("Supplies", 1500) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);

            entry = new ResearchEntry("Exp Bonuses 3", "Exp Bonuses 3", "Exp Bonuses", 4, 800, ResearchCategory.Barracks,
                "Exp Bonuses 2", new List<ResourceData> { new ResourceData("Knowledge", 5000), new ResourceData("Materials", 5000), new ResourceData("Supplies", 5000) },
                new List<string>(), new List<ResearchEntryUnlock>());
            researchEntries.Add(entry.Key, entry);
        }

        public static void LoadAttributes()
        {
            AttributeDefinition bA = new AttributeDefinition("Strength", "Strength", "Str", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Endurance", "Endurance", "End", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Agility", "Agility", "Agi", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Dexterity", "Dexterity", "Dex", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Senses", "Senses", "Sns", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Intellect", "Intellect", "Int", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Wisdom", "Wisdom", "Wis", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Willpower", "Willpower", "Wil", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Charisma", "Charisma", "Cha", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);

            bA = new AttributeDefinition("Memory", "Memory", "Mem", "", 0, 999, AttributeDefinitionType.Base, null);
            baseAttributeDefinitions.Add(bA);


            AttributeDefinition dA = new AttributeDefinition("Armor", "Armor", "Arm", "", 0, 999, AttributeDefinitionType.Derived_Points,
                null);
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Health", "Health", "Hp", "", 0, 999, AttributeDefinitionType.Derived_Points, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Strength),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Endurance), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Stamina", "Stamina", "Sta", "", 0, 999, AttributeDefinitionType.Derived_Points, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Endurance),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Willpower), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Essence", "Essence", "Ess", "", 0, 999, AttributeDefinitionType.Derived_Points, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Intellect),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Memory), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Morale", "Morale", "Mor", "", 0, 999, AttributeDefinitionType.Derived_Points, null);
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Might Attack", "Might Att", "Matt", "", 0, 999, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Strength),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Agility), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Might Damage", "Might Dmg", "Mdmg", "", -1000, 1000, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Strength),
                    new AttributeModifier(AttributeModifierType.Value, 12), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Finesse Attack", "Finesse Att", "Fatt", "", 0, 999, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Agility),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Senses), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Finesse Damage", "Finesse Dmg", "FDmg", "", -1000, 1000, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Agility),
                    new AttributeModifier(AttributeModifierType.Value, 12), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Spell Attack", "Spell Att", "Satt", "", 1, 100, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Intellect),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Wisdom), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Spell Damage", "Spell Dmg", "Sdmg", "", -1000, 1000, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Intellect),
                    new AttributeModifier(AttributeModifierType.Value, 12), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Spell Modifier", "Spell Mod", "Mmod", "", -1000, 1000, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Memory),
                    new AttributeModifier(AttributeModifierType.Value, 12), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Block", "Block", "Blk", "", 0, 999, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Endurance),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Agility), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Dodge", "Dodge", "Ddg", "", 0, 999, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Agility),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Senses), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Parry", "Parry", "Par", "", 0, 999, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Agility),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Dexterity), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Resistance", "Resistance", "Res", "", 0, 99, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Endurance),
                    new AttributeModifier(AttributeModifierType.Value, 20), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Speed", "Speed", "Spd", "", 0, 99, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Race, 0), null, null,
                    AttributeCalculationOpperator.None,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Perception", "Perception", "Per", "", 0, 99, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Senses), null, null,
                    AttributeCalculationOpperator.None,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Concentration", "Concentration", "Con", "", 0, 999, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Willpower),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Memory), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);


            dA = new AttributeDefinition("Action Modifier", "Action Mod", "Amod", "", -1000, 1000, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Dexterity),
                    new AttributeModifier(AttributeModifierType.Value, 20),
                    new AttributeModifier(AttributeModifierType.Value, 1),
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.Multiply_Neg));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Duration Modifier", "Duration Mod", "Dur", "", -100, 100, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Wisdom),
                    new AttributeModifier(AttributeModifierType.Value, 20), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Range Modifier", "Range Mod", "Rng", "", -100, 100, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Senses),
                    new AttributeModifier(AttributeModifierType.Value, 20), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Magic Find", "Magic Find", "Mag", "", -100, 100, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Value, 0), null, null,
                    AttributeCalculationOpperator.None,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);



            dA = new AttributeDefinition("Fumble", "Fumble", "Fum", "", 1, 20, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Value, 5), null, null,
                    AttributeCalculationOpperator.None,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Graze", "Graze", "Grz", "", 1, 30, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Value, 10), null, null,
                    AttributeCalculationOpperator.None,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Critical Hit", "Crit", "CH", "", 75, 99, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Value, 65), null, null,
                    AttributeCalculationOpperator.None,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Perfect Hit", "Perfect", "PH", "", 75, 99, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Value, 65), null, null,
                    AttributeCalculationOpperator.None,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Critical Dmg", "Crit Dmg", "CD", "", 0, 1000, AttributeDefinitionType.Derived_Percent, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Dexterity),
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Senses), null,
                    AttributeCalculationOpperator.Add,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);


            dA = new AttributeDefinition("Health Regen", "Health Reg", "HReg", "", 0, 1000, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Endurance),
                    new AttributeModifier(AttributeModifierType.Value, 20), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Stamina Regen", "Stamina Reg", "SReg", "", 0, 1000, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Willpower),
                    new AttributeModifier(AttributeModifierType.Value, 20), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);

            dA = new AttributeDefinition("Essence Regen", "Essence Reg", "EReg", "", 0, 1000, AttributeDefinitionType.Derived_Score, new AttributeCalculation(
                    new AttributeModifier(AttributeModifierType.Base_Attribute, (int)BaseAttribute.Willpower),
                    new AttributeModifier(AttributeModifierType.Value, 20), null,
                    AttributeCalculationOpperator.Subtract,
                    AttributeCalculationOpperator.None));
            derivedAttributeDefinitions.Add(dA);



            AttributeDefinition res = new AttributeDefinition("Physical", "Physical", "Phy", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Fire", "Fire", "Fir", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Cold", "Cold", "cld", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Shock", "Shock", "Shk", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Poison", "Poison", "Poi", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Acid", "Acid", "Acd", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Unholy", "Unholy", "Unh", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Holy", "Holy", "Hly", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Psychic", "Psychic", "Psy", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);

            res = new AttributeDefinition("Arcane", "Arcane", "Arc", "", 0, 9999, AttributeDefinitionType.Resistance, null);
            damageTypeDefinitions.Add(res);
        }

        public static void LoadSkills()
        {
            SkillDefinition skill = new SkillDefinition(SkillCategory.Combat, Skill.One_Hand_Melee, "One Handed Melee", "1h Melee", "1HM", "", "strength", 0, 999,
                new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Two_hand_Melee, "Two Handed Melee", "2h Melee", "2HM", "", "strength", 0, 999,
                new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Polearms, "Polearms", "Polearm", "Pol", "", "strength", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Unarmed, "Unarmed", "Unarmed", "Una", "", "strength", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Thrown, "Thrown", "Thrown", "Thr", "", "dexterity", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Archery, "Archery", "Archery", "Arc", "", "dexterity", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Firearms, "Firearms", "Firarm", "Fir", "", "dexterity", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Explosives, "Explosives", "Explosive", "Exp", "", "dexterity", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Light_Armor, "Light Armor", "L Armor", "LAr", "", "agility", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Medicine, "Medium Armor", "M Armor", "MAr", "", "endurance", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Heavy_Armor, "Heavy Armor", "H Armor", "HAr", "", "endurance", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Bucklers, "Bucklers", "Buckler", "Buc", "", "agility", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Shields, "Shields", "Shield", "Shi", "", "endurance", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Leadership, "Leadership", "Leader", "Lea", "", "charisma", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Combat, Skill.Tactics, "Tactics", "Tactics", "Tac", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Firearms, "Fire Magic", "Fire", "FMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Air_Magic, "Air Magic", "Air", "AMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Water_Magic, "Water Magic", "Water", "WMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Earth_Magic, "Earth Magic", "Earth", "EMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Death_Magic, "Death Magic", "Death", "DMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Life_Magic, "Life Magic", "Life", "LMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Shadow_Magic, "Shadow Magic", "Shadow", "SMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Arcane_Magic, "Arcane Magic", "Arcane", "AMa", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Alchemy, "Alchemy", "Alchemy", "Alc", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Enchanting, "Enchanting", "Enchant", "Enc", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Lore, "Lore", "Lore", "Lor", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Magic, Skill.Research, "Researching", "Research", "Res", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Channeling, "Channeling", "Channel", "Cha", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Stealth, "Stealth", "Stealth", "Ste", "", "agility", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Scouting, "Scounting", "Scout", "Sct", "", "senses", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Tricks, "Tricks", "Tricks", "Tri", "", "dexterity", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Evasion, "Evasion", "Evasion", "Eva", "", "agility", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Precision, "Precision", "Precise", "Pre", "", "senses", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Devices, "Devices", "Device", "Dev", "", "dexterity", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Persuasion, "Persuasion", "Persuade", "Pes", "", "charisma", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Poison_Crafting, "Poison Craft", "Poison", "Poi", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Mining, "Mining", "Mining", "Min", "", "endurance", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Gathering, "Gathering", "Gather", "Gat", "", "senses", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Crafting, "Crafting", "Craft", "cra", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Engineering, "Engineering", "Engine", "Eng", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Logistics, "Logistics", "Logistics", "Log", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Steamcraft, "Steamcraft", "Steam", "Ste", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Survival, "Survival", "Survival", "Sur", "", "endurance", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Navigation, "Navigation", "Navigate", "Nav", "", "senses", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Training, "Training", "Train", "Tra", "", "charisma", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);

            skill = new SkillDefinition(SkillCategory.Misc, Skill.Medicine, "Medicine", "Medic", "Med", "", "intellect", 0, 999,
                 new List<AbilityUnlock> { });
            skillDefinitions.Add(skill);
        }

        public static void LoadResources()
        {
            ResourceDefinition resource = new ResourceDefinition("Coin", "Coin", "Coin", "Coin description", 0);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Supplies", "Supplies", "Supplies", "Supplies description", 1);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Materials", "Materials", "Materials", "Materials description", 2);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Tools", "Tools", "Tools", "Tools description", 3);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Rations", "Rations", "Rations", "Rations description", 4);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Knowledge", "Knowledge", "Knowledge", "Knowledge description", 5);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Essence", "Essence", "Essence", "Essence description", 6);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Influence", "Influence", "Influence", "Influence description", 7);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Luxuries", "Luxuries", "Luxuries", "Luxuries description", 8);
            resourceDefinitions.Add(resource.Key, resource);

            resource = new ResourceDefinition("Rare Materials", "Rare Materials", "Rare Materials", "Rare Materials description", 9);
            resourceDefinitions.Add(resource.Key, resource);
        }

        public static void LoadBuildings()
        {
            BuildingDefinition building = new BuildingDefinition("Town Hall", "Town Hall", "Town Hall", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 5000), new ResourceData("Materials", 1000), new ResourceData("Tools", 20) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 0), new ResourceData("Tools", 0) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Coin", 100), new ResourceData("Rations", 100), new ResourceData("Materials", 10),
            new ResourceData("Essence", 10),new ResourceData("Influence", 1) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Hovel", "Hovel", "Hovel", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 100), new ResourceData("Materials", 50), new ResourceData("Tools", 2) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 1) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Small House", "Small House", "Small House", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 200), new ResourceData("Materials", 100), new ResourceData("Tools", 5) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 3) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Medium House", "Medium House", "Medium House", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 300), new ResourceData("Materials", 150), new ResourceData("Tools", 10) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Large House", "Large House", "Large House", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 250), new ResourceData("Tools", 25) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 10), new ResourceData("Tools", 3) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Mansion", "Mansion", "Mansion", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 10000), new ResourceData("Materials", 1000), new ResourceData("Tools", 25) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 20), new ResourceData("Tools", 15) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Influence", 1) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("General Store", "General Store", "General Store", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 200), new ResourceData("Materials", 100), new ResourceData("Tools", 5) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 3), new ResourceData("Supplies", 3), new ResourceData("Tools", 1) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Coin", 1) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Market", "Market", "Market", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 200), new ResourceData("Tools", 15) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 5), new ResourceData("Supplies", 5), new ResourceData("Tools", 2) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Coin", 10) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Tavern", "Tavern", "Tavern", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 200), new ResourceData("Materials", 100), new ResourceData("Tools", 5) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 1), new ResourceData("Supplies", 3), new ResourceData("Tools", 1) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Coin", 5) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Inn", "Inn", "Inn", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 2000), new ResourceData("Materials", 250), new ResourceData("Tools", 10) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 5), new ResourceData("Supplies", 5), new ResourceData("Tools", 2) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Coin", 25) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Blacksmith", "Blacksmith", "Blacksmith", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 150), new ResourceData("Materials", 50), new ResourceData("Tools", 10) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Tools", 1) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Materials", 5), new ResourceData("Tools", 1) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Weaponsmith", "Weaponsmith", "Weaponsmith", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 200), new ResourceData("Materials", 75), new ResourceData("Tools", 15) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Tools", 2) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Materials", 10) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Armorsmith", "Armorsmith", "Armorsmith", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 200), new ResourceData("Materials", 75), new ResourceData("Tools", 15) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Tools", 2) };
            building.ResourcesGenerated = new List<ResourceData> { new ResourceData("Materials", 10) };
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Training Grounds", "Training Grounds", "Training Grounds", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 250), new ResourceData("Tools", 25) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 10), new ResourceData("Tools", 3) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Stables", "Stables", "Stables", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 250), new ResourceData("Tools", 25) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 10), new ResourceData("Tools", 3) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Apothocary", "Apothocary", "Apothocary", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 250), new ResourceData("Tools", 25) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 10), new ResourceData("Tools", 3) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);

            building = new BuildingDefinition("Herbalist", "Herbalist", "Herbalist", 200, 100, true, "Building Description");
            building.ResourcesToBuild = new List<ResourceData> { new ResourceData("Coin", 500), new ResourceData("Materials", 250), new ResourceData("Tools", 25) };
            building.ResourcesUsed = new List<ResourceData> { new ResourceData("Materials", 10), new ResourceData("Tools", 3) };
            building.ResourcesGenerated = new List<ResourceData>();
            buildingDefinitions.Add(building.Key, building);
        }

        public static void LoadPartyAttributes()
        {
            AttributeDefinition attDef = new AttributeDefinition("March Speed", "March", "Spd", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);

            attDef = new AttributeDefinition("Scouting Range", "Scout", "Sct", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);

            attDef = new AttributeDefinition("Supplies", "Supplies", "Sup", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);

            attDef = new AttributeDefinition("Supplies Used", "Supply U", "SupU", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);

            attDef = new AttributeDefinition("Max Supplies", "Supply M", "MSup", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);

            attDef = new AttributeDefinition("Rations", "Rations", "Rat", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);

            attDef = new AttributeDefinition("Max Rations", "Rations U", "MRat", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);

            attDef = new AttributeDefinition("Rations Used", "Rations M", "RatU", "", 0, 9999, AttributeDefinitionType.Party, null);
            partyAttributeDefinitions.Add(attDef);
        }

        static List<string> npcKeys = new List<string>();

        public static void LoadNPCs()
        {
            NPCDefinition npc = new NPCDefinition(new FantasyName("Giant Rat", "", ""), CharacterType.Animal, EntitySize.Small, Gender.Either, "giant_rat", "", "", NPCImageType.Sprite, "npc_6",
                -1, -1, 1, 2, 10, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);

            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Giant Black Rat", "", ""), CharacterType.Animal, EntitySize.Small, Gender.Either, "giant_black_rat", "", "", NPCImageType.Sprite, "npc_70",
                -1, -1, 1, 2, 10, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Giant Rat Queen", "", ""), CharacterType.Animal, EntitySize.Small, Gender.Either, "giant_rat_queen", "", "", NPCImageType.Sprite, "npc_102",
                -1, -1, 2, 4, 20, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Spitting Grub", "", ""), CharacterType.Insect, EntitySize.Small, Gender.Either, "spitting_grub", "", "", NPCImageType.Sprite, "npc_487",
                -1, -1, 1, 3, 10, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Biting Grub", "", ""), CharacterType.Insect, EntitySize.Small, Gender.Either, "biting_grub", "", "", NPCImageType.Sprite, "npc_519",
                -1, -1, 1, 3, 10, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Giant Bat", "", ""), CharacterType.Animal, EntitySize.Small, Gender.Either, "giant_bat", "", "", NPCImageType.Sprite, "npc_361",
                -1, -1, 3, 6, 30, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Walking Corpse", "", ""), CharacterType.Undead, EntitySize.Small, Gender.Either, "walking_corpse", "", "", NPCImageType.Sprite, "npc_967",
                -1, -1, 2, 10, 20, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Spirit", "", ""), CharacterType.Undead, EntitySize.Small, Gender.Either, "spirit", "", "", NPCImageType.Sprite, "npc_303",
                -1, -1, 5, 12, 50, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Goblin", "", ""), CharacterType.Humanoid, EntitySize.Small, Gender.Either, "goblin", "", "", NPCImageType.Sprite, "npc_776",
                -1, -1, 1, 5, 10, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Orc Savage", "", ""), CharacterType.Humanoid, EntitySize.Small, Gender.Either, "orc_savage", "", "", NPCImageType.Sprite, "npc_101",
                -1, -1, 4, 40, 40, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Baby Flame Dragon", "", ""), CharacterType.Draconic, EntitySize.Small, Gender.Either, "baby_flame_dragon", "", "", NPCImageType.Sprite, "npc_138",
                -1, -1, 5, 8, 50, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Baby Stone Dragon", "", ""), CharacterType.Draconic, EntitySize.Small, Gender.Either, "baby_stone_dragon", "", "", NPCImageType.Sprite, "npc_106",
                -1, -1, 5, 8, 50, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Human Guard", "", ""), CharacterType.Humanoid, EntitySize.Small, Gender.Either, "human_guard", "", "", NPCImageType.Sprite, "npc_690",
                -1, -1, 4, 12, 40, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Human Citizen", "", ""), CharacterType.Humanoid, EntitySize.Small, Gender.Either, "human_citizen", "", "", NPCImageType.Sprite, "npc_695",
                -1, -1, 1, 6, 5, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);

            npc = new NPCDefinition(new FantasyName("Fire Elemental", "", ""), CharacterType.Elemental, EntitySize.Small, Gender.Either, "fire_elemental", "", "", NPCImageType.Sprite, "npc_874",
                -1, -1, 15, 20, 150, 10);
            npc.BaseStart[(int)BaseAttribute.Strength] = 10; npc.BaseStart[(int)BaseAttribute.Intellect] = 10;
            npc.BaseStart[(int)BaseAttribute.Endurance] = 10; npc.BaseStart[(int)BaseAttribute.Wisdom] = 10;
            npc.BaseStart[(int)BaseAttribute.Agility] = 10; npc.BaseStart[(int)BaseAttribute.Willpower] = 10;
            npc.BaseStart[(int)BaseAttribute.Dexterity] = 10; npc.BaseStart[(int)BaseAttribute.Charisma] = 10;
            npc.BaseStart[(int)BaseAttribute.Senses] = 10; npc.BaseStart[(int)BaseAttribute.Memory] = 10;
            npc.BasePerLevel[(int)BaseAttribute.Strength] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Intellect] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Endurance] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Wisdom] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Agility] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Willpower] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Dexterity] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Charisma] = new GameValue(0, 0);
            npc.BasePerLevel[(int)BaseAttribute.Senses] = new GameValue(0, 0); npc.BasePerLevel[(int)BaseAttribute.Memory] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Armor] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Health] = new GameValue(1, 2); npc.DerivedPerLevel[(int)DerivedAttribute.Stamina] = new GameValue(1, 1);
            npc.DerivedPerLevel[(int)DerivedAttribute.Essence] = new GameValue(0, 1); npc.DerivedPerLevel[(int)DerivedAttribute.Morale] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Might_Attack] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Finesse_Attack] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Block] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Dodge] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Parry] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Speed] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Strike] = new GameValue(0, 0); npc.DerivedPerLevel[(int)DerivedAttribute.Critical_Damage] = new GameValue(0, 0);
            npc.DerivedPerLevel[(int)DerivedAttribute.Perception] = new GameValue(0, 0);
            npcs.Add(npc.Key, npc); npcKeys.Add(npc.Key);
        }

        public static void LoadQuirks()
        {
            Ability quirk = new Ability("Adventurous", "adventurous", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Attractive", "attractive", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("ugly"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Alert", "alert", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Athletic", "athletic", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("lazy"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Articulate", "articulate", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Benevolent", "benevolanet", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("cruel"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Calm", "calm", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Charming", "charming", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Cheerful", "Cheerful", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Clever", "clever", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Courageous", "courageous", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("cowardly"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Dignified", "dignified", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Disciplined", "disciplined", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Educated", "educated", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("uneducated"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Eloquent", "eloquent", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Friendly", "friendly", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Generous", "generous", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("greedy"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Honorable", "Honorable", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Humorous", "humorous", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Idealistic", "idealistic", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Intuitive", "Intuitive", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Kind", "kind", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Logical", "logical", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Loyal", "loyal", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Observant", "observant", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Optimistic", "optimistic", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("pesimistic"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Peaceful", "peaceful", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Persuasive", "persuasive", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Protective", "protective", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Resourceful", "resourceful", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Romantic", "romantic", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Selfless", "selfless", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Sexy", "sexy", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Simple", "simple", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Social", "social", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Sweet", "sweet", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Wity", "wity", "trait", AbilityClass.Either, AbilityType.Positive_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);


            quirk = new Ability("Frugal", "frugal", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Moralistic", "moralistic", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Outspoken", "outspoken", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Political", "political", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Quiet", "quiet", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Religious", "religious", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Proud", "proud", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Reserved", "reserved", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Sarcastic", "sarcastic", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Skeptical", "skeptical", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Strict", "strict", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Whimsical", "whimsical", "trait", AbilityClass.Either, AbilityType.Neutral_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);


            quirk = new Ability("Abrasive", "abrasive", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Apathetic", "Apathetic", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Arrogant", "arrogant", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Antisocial", "antisocial", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("social"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Blunt", "blunt", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Careless", "careless", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Childish", "childish", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Compulsive", "compulsive", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Cowardly", "cowardly", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("courageous"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Crazy", "crazy", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Crude", "crude", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Cruel", "cruel", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("benevolent"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Deceptive", "deceptive", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Destructive", "destructive", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Disobedient", "disobedient", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Egocentric", "egocentric", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("selfless"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Fanatical", "fanatical", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Fearful", "fearful", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Forgetful", "forgetful", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Hateful", "hateful", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Insecure", "insecure", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Lazy", "lazy", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("athletic"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Moody", "moody", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Obsessive", "obsessive", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Paranoid", "paranoid", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Pesimitic", "pesimitic", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("optimistic"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Secretive", "secretive", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Shy", "shy", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Superstitious", "superstitious", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Timid", "timid", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent(""));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Ugly", "ugly", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("attractive"));
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Uneducated", "uneducated", "trait", AbilityClass.Either, AbilityType.Negative_Quirk);
            quirk.Components.Add(new OpposingTraitComponent("educated"));
            abilities.Add(quirk.Key, quirk);


            quirk = new Ability("Minor Limp", "minor_limp", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Major Limp", "major_limp", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Lisp", "lisp", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Missing Finger", "missing_finger", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Missing Toe", "missing_toe", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Missing Foot", "missing_foot", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Missing Hand", "missing_hand", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Deep Scar", "deep_scar", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Missing Ear", "missing_ear", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Missing Eye", "missing_eye", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);

            quirk = new Ability("Head Wound", "head_wound", "trait", AbilityClass.Either, AbilityType.Wound);
            abilities.Add(quirk.Key, quirk);
        }

        public static void LoadItemAttributes()
        {
            AttributeDefinition att = new AttributeDefinition("Attack", "Attack", "Acc", "", 0, 9999, AttributeDefinitionType.Weapon, null);
            weaponAttributes.Add(att);
            att = new AttributeDefinition("Range", "Range", "Ran", "", 0, 9999, AttributeDefinitionType.Weapon, null);
            weaponAttributes.Add(att);
            att = new AttributeDefinition("Actions", "Actions", "Act", "", 0, 9999, AttributeDefinitionType.Weapon, null);
            weaponAttributes.Add(att);
            att = new AttributeDefinition("Parry", "Parry", "Pry", "", 0, 9999, AttributeDefinitionType.Weapon, null);
            weaponAttributes.Add(att);

            att = new AttributeDefinition("Armor", "Armor", "Arm", "", 0, 9999, AttributeDefinitionType.Wearable, null);
            wearableAttributes.Add(att);
            att = new AttributeDefinition("Dodge", "Dodge", "Ddg", "", 0, 9999, AttributeDefinitionType.Wearable, null);
            wearableAttributes.Add(att);
            att = new AttributeDefinition("Block", "Block", "Blk", "", 0, 9999, AttributeDefinitionType.Wearable, null);
            wearableAttributes.Add(att);
            att = new AttributeDefinition("Actions", "Actions", "Act", "", 0, 9999, AttributeDefinitionType.Wearable, null);
            wearableAttributes.Add(att);

            att = new AttributeDefinition("Attack", "Attack", "Attack", "", 0, 9999, AttributeDefinitionType.Ammo, null);
            ammoAttributes.Add(att);
            att = new AttributeDefinition("Range", "Range", "Ran", "", 0, 9999, AttributeDefinitionType.Ammo, null);
            ammoAttributes.Add(att);
            att = new AttributeDefinition("Actions", "Actions", "Act", "", 0, 9999, AttributeDefinitionType.Ammo, null);
            ammoAttributes.Add(att);

            att = new AttributeDefinition("Actions", "Actions", "Act", "", 0, 9999, AttributeDefinitionType.Accessory, null);
            accessoryAttributes.Add(att);
            att = new AttributeDefinition("Cooldown", "Cooldown", "Cd", "", 0, 9999, AttributeDefinitionType.Accessory, null);
            accessoryAttributes.Add(att);
        }
    }
}