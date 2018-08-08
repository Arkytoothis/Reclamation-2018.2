using UnityEngine;
using TMPro;
using Reclamation.Characters;
using Reclamation.Misc;

namespace Reclamation.Gui
{
    public class AttributeElement : GuiElement
    {
        public UnityEngine.UI.Button backgroundButton;
        public TMP_Text nameLabel;
        public TMP_Text valueLabel;
        public TMP_Text expLabel;
        public UnityEngine.UI.Button minusButton;
        public UnityEngine.UI.Button plusButton;

        public AttributeElementData skinData;

        protected override void OnSkinGui()
        {
            base.OnSkinGui();

            backgroundButton.colors = skinData.buttonColors;
            nameLabel.color = skinData.textColor;
            valueLabel.color = skinData.textColor;
            expLabel.color = skinData.textColor;
        }

        public void Initialize(string name)
        {
            nameLabel.text = name;
            valueLabel.text = "";
            expLabel.text = "";
            minusButton.gameObject.SetActive(false);
            plusButton.gameObject.SetActive(false);
        }

        public void SetData(Attribute attribute)
        {
            if (attribute != null)
            {
                valueLabel.text = attribute.Current + "/" + attribute.Maximum;
                expLabel.text = "";
                minusButton.gameObject.SetActive(false);
                plusButton.gameObject.SetActive(false);
            }
            else
            {
                valueLabel.text = "";
                expLabel.text = "";
                minusButton.gameObject.SetActive(false);
                plusButton.gameObject.SetActive(false);
            }
        }
    }
}