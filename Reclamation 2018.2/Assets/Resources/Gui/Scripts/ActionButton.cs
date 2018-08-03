using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Reclamation.Abilities;
using System;

namespace Reclamation.Gui
{
    public class ActionButton : GuiElement
    {
        public ActionButtonData skinData;
        public Image cooldownImage;
        public TMP_Text cooldownLabel;
        public Button button;
        public float cooldown;
        bool isCooldown = false;

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
                Debug.Log("Action Triggered");
                isCooldown = true;
                cooldownImage.fillAmount = 1;
                AudioManager.instance.PlaySound("button 01");
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


        public void SetData(Ability ability)
        {
            if (ability != null)
            {
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
                //nameLabel.text = "";
                //portrait.texture = null;
            }
        }
    }
}