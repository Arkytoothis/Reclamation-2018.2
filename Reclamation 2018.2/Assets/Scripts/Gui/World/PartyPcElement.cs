using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Reclamation.Characters;

namespace Reclamation.Gui
{
    public class PartyPcElement : MonoBehaviour
    {
        [SerializeField] TMP_Text nameLabel;
        [SerializeField] TMP_Text detailsLabel;
        [SerializeField] TMP_Text hpLabel;
        [SerializeField] UnityEngine.UI.Button button;

        public void SetData(PcData pc)
        {
            if (pc != null)
            {
                nameLabel.text = pc.Name.FirstName + "\n" + pc.Name.LastName;
                detailsLabel.text = "Lvl " + pc.Level + " " + pc.RaceKey + " " + pc.ProfessionKey;
                //detailsLabel.text += "Health " + pc.DerivedAttributes[(int)DerivedAttribute.Health].Current + "/" + pc.DerivedAttributes[(int)DerivedAttribute.Health].Maximum;
                //detailsLabel.text += "Morale " + pc.DerivedAttributes[(int)DerivedAttribute.Morale].Current + "/" + pc.DerivedAttributes[(int)DerivedAttribute.Morale].Maximum;
                detailsLabel.text += "Exp " + pc.Experience + "/" + pc.ExpToLevel;
                button.interactable = true;
            }
            else
            {
                nameLabel.text = "";
                detailsLabel.text = "";
                button.interactable = false;
            }
        }
    }
}