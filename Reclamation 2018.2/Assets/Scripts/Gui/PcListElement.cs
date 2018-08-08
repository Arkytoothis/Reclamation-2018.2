using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Reclamation.Characters;
using Reclamation.World;

namespace Reclamation.Gui
{
    public class PcListElement : GuiElement, IPointerClickHandler
    {
        [SerializeField] PcListElementData skinData;
        [SerializeField] Image background;
        [SerializeField] UnityEngine.UI.Button button;
        [SerializeField] TMP_Text nameLabel;
        [SerializeField] TMP_Text detailsLabel;
        [SerializeField] int pcIndex;

        public delegate bool OnSelectPc(int index);
        public event OnSelectPc onSelectPc;


        protected override void OnSkinGui()
        {
            base.OnSkinGui();
        }

        public void SetData(int pcIndex)
        {
            this.pcIndex = pcIndex;
            PcData pcData = PlayerManager.instance.GetPcData(pcIndex);

            if (pcData != null)
            {
                nameLabel.text = pcData.Name.FullName;
                detailsLabel.text = pcData.Level + " " + pcData.RaceKey + " " + pcData.ProfessionKey;
            }
            else
            {
                nameLabel.text = "";
                detailsLabel.text = "";
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onSelectPc(pcIndex);
        }
    }
}