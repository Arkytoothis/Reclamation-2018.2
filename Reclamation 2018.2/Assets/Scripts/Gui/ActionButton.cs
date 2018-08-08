using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Reclamation.Abilities;
using Reclamation.Audio;
using Reclamation.Misc;
using System;

namespace Reclamation.Gui
{
    public class ActionButton : GuiElement
    {
        [SerializeField] ActionButtonData skinData;
        [SerializeField] Image icon;
        [SerializeField] Image cooldownImage;
        [SerializeField] TMP_Text cooldownLabel;
        [SerializeField] TMP_Text hotkeyLabel;
        [SerializeField] UnityEngine.UI.Button button;
        [SerializeField] float cooldown;


        [SerializeField] bool isCooldown = false;
        [SerializeField] int index = -1;
        public int Index { get { return index; } }

        protected override void OnSkinGui()
        {
            base.OnSkinGui();

            button.transition = Selectable.Transition.ColorTint;
            button.colors = skinData.buttonColors;

            cooldownImage.color = skinData.cooldownColor;
            cooldownImage.fillAmount = 0;

            cooldownLabel.color = skinData.cooldownTextColor;
            cooldownLabel.text = "";

            cooldown = 5f;
        }

        public void OnPointerClick()
        {
            if (isCooldown == false)
            {
                //Debug.Log("Action Triggered");
                isCooldown = true;
                cooldownImage.fillAmount = 1;
                AudioManager.instance.PlaySound("button 02", false);
            }
        }

        void Update()
        {
            if (isCooldown == true)
            {
                cooldownImage.fillAmount -= (1 / cooldown) * Time.deltaTime;

                float time = cooldown * cooldownImage.fillAmount;
                string s = String.Format("{0:0.0}", time);
                cooldownLabel.text = s;

                if (cooldownImage.fillAmount <= 0)
                {
                    //cooldownImage.fillAmount = 0;
                    isCooldown = false;
                    cooldownLabel.text = "";
                }
            }
        }
        public void SetData(int index, Ability ability)
        {
            this.index = index;
            SetData(ability);
        }

        public void SetData(Ability ability)
        {
            if (ability != null)
            {
                hotkeyLabel.text = (index + 1).ToString();
                cooldown = ability.cooldown;

                if (ability.SpriteKey == "") Debug.Log(ability.Name);
                icon.sprite = SpriteManager.instance.GetAbilitySprite(ability.SpriteKey);
                icon.color = Color.white;
                //this.pc = pc;
                //GameObject go = null;
                //AttributeBar bar = null;
                //nameLabel.text = pc.name.FirstName;

                //for (int i = 0; i < (int)Vital.Number; i++)
                //{
                //    go = Instantiate(attributeBarPrefab, attributeBarsParent);
                //    bar = go.GetComponent<AttributeBar>();
                //    bar.SetData(pc, i);

                //    attributeBars.Add(go);
                //}

                //go = Instantiate(attributeBarPrefab, attributeBarsParent);
                //bar = go.GetComponent<AttributeBar>();
                //bar.SetExpData(pc);
                //expBar = go;

                //GameObject model = ModelManager.instance.CharacterPrefabs[pc.raceKey + " " + pc.gender];

                //PortraitRoom.instance.AddModel(model, pc.encounterIndex);
                //portrait.texture = PortraitRoom.instance.characterMounts[pc.encounterIndex].rtCamera.targetTexture;
            }
            else
            {
                cooldown = 0;
                cooldownLabel.text = "";
                cooldownImage.fillAmount = 0;
                hotkeyLabel.text = (index + 1).ToString();
                icon.color = Color.clear;
                icon.sprite = null;
            }
        }
    }
}