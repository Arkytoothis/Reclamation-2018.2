using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Reclamation.Characters;

namespace Reclamation.Gui
{
    public class FlexiblePcButton : FlexibleGui
    {
        public PcButtonData skinData;
        public Image background;
        public Button button;

        public TMP_Text nameLabel;
        public RawImage portrait;

        private Pc pc;

        protected override void OnSkinGui()
        {
            base.OnSkinGui();

            background.sprite = skinData.background;
            background.type = Image.Type.Sliced;

            button.transition = Selectable.Transition.ColorTint;
            button.colors = skinData.buttonColors;
        }

        public void SetData(Pc pc)
        {
            this.pc = pc;
        }

        public void UpdateData()
        {
            nameLabel.text = pc.Name.FirstName;// + " " + pc.DerivedAttributes[(int)DerivedAttribute.Health].Current + "/" + pc.DerivedAttributes[(int)DerivedAttribute.Health].Maximum;
        }
    }
}