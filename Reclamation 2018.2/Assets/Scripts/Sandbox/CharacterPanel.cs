using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Misc;
using TMPro;
using Reclamation.Characters;

namespace Reclamation.Sandbox
{
    public class CharacterPanel : MonoBehaviour
    {
        [SerializeField] string currentRace;
        //[SerializeField] string currentProfession;
        [SerializeField] Gender currentGender;
        //[SerializeField] BodyType currentBodyType;
        [SerializeField] string currentHair;
        [SerializeField] string currentBeard;
        [SerializeField] string currentSkin;

        [SerializeField] TMP_Dropdown raceDropdown;
        [SerializeField] TMP_Dropdown professionDropdown;
        [SerializeField] TMP_Dropdown hairDropdown;
        [SerializeField] TMP_Dropdown beardDropdown;
        [SerializeField] TMP_Dropdown skinDropdown;

        [SerializeField] Toggle maleToggle;
        [SerializeField] Toggle femaleToggle;

        public void Initialize()
        {
            raceDropdown.ClearOptions();
            raceDropdown.options.Add(new TMP_Dropdown.OptionData("Imperial"));
            raceDropdown.options.Add(new TMP_Dropdown.OptionData("Halfling"));
            raceDropdown.options.Add(new TMP_Dropdown.OptionData("High Elf"));
            raceDropdown.options.Add(new TMP_Dropdown.OptionData("Mountain Dwarf"));
            raceDropdown.value = 0;
            raceDropdown.RefreshShownValue();
            

            professionDropdown.ClearOptions();
            professionDropdown.options.Add(new TMP_Dropdown.OptionData("Citizen"));
            professionDropdown.options.Add(new TMP_Dropdown.OptionData("Soldier"));
            professionDropdown.options.Add(new TMP_Dropdown.OptionData("Scout"));
            professionDropdown.options.Add(new TMP_Dropdown.OptionData("Rogue"));
            professionDropdown.options.Add(new TMP_Dropdown.OptionData("Priest"));
            professionDropdown.options.Add(new TMP_Dropdown.OptionData("Apprentice"));
            professionDropdown.value = 0;
            professionDropdown.RefreshShownValue();

            hairDropdown.ClearOptions();
            foreach (KeyValuePair<string, GameObject> kvp in ModelManager.instance.HairPrefabs)
            {
                hairDropdown.options.Add(new TMP_Dropdown.OptionData(kvp.Key));
            }
            hairDropdown.value = 0;
            hairDropdown.RefreshShownValue();

            beardDropdown.ClearOptions();
            foreach (KeyValuePair<string, GameObject> kvp in ModelManager.instance.BeardPrefabs)
            {
                beardDropdown.options.Add(new TMP_Dropdown.OptionData(kvp.Key));
            }
            beardDropdown.value = 0;
            beardDropdown.RefreshShownValue();

            UpdateBody();
            UpdateHair();
            //foreach (KeyValuePair<string, Race> kvp in Database.Races)
            //{
            //    raceDropdown.options.Add(new TMP_Dropdown.OptionData(kvp.Key));
            //}
            //foreach (KeyValuePair<string, Profession> kvp in Database.Professions)
            //{
            //    professionDropdown.options.Add(new TMP_Dropdown.OptionData(kvp.Key));
            //}
        }

        public void UpdateGender()
        {
            if (maleToggle.isOn == true)
            {
                currentGender = Gender.Male;
                UpdateBody();
            }
            else if (femaleToggle.isOn == true)
            {
                currentGender = Gender.Female;
                UpdateBody();
            }
        }

        public void UpdateBody()
        {
            string race = raceDropdown.options[raceDropdown.value].text;

            if (Database.GetRace(race) != null)
            {
                currentRace = race;
                SandboxManager.instance.SetBodyModel(currentRace + " " + currentGender);
            }
        }

        public void UpdateProfession()
        {
            string profession = professionDropdown.options[professionDropdown.value].text;

            if (Database.GetProfession(profession) != null)
            {
                //currentProfession = profession;
                //Generate();
            }
        }

        public void UpdateBodyType()
        {

            //currentBodyType = BodyType.Normal;
            LoadBody(Database.GetRace(currentRace).scale, currentRace);
        }

        public void UpdateHair()
        {
            string hair = hairDropdown.options[hairDropdown.value].text;
            currentHair = hair;
            SandboxManager.instance.SetHair(LoadHair(currentHair));
        }

        public void UpdateBeard()
        {
            string beard = beardDropdown.options[beardDropdown.value].text;
            currentBeard = beard;
            SandboxManager.instance.SetBeard(LoadBeard(currentBeard));
        }

        public void UpdateSkin()
        {
        }

        public GameObject LoadBody(Vector3 scale, string body)
        {
            return ModelManager.instance.GetCharacterPrefab(scale, body);
        }

        public GameObject LoadHair(string hair)
        {
            //Debug.Log("Loading hair " + hair);
            return ModelManager.instance.GetHairModel(hair);
        }

        public GameObject LoadBeard(string beard)
        {
            //Debug.Log("Loading beard " + beard);
            return ModelManager.instance.GetBeardModel(beard);
        }
    }
}