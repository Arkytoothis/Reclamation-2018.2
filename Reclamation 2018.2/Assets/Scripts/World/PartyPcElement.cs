using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PartyPcElement : MonoBehaviour
{
    public TMP_Text nameLabel;
    public TMP_Text detailsLabel;
    public TMP_Text hpLabel;

    public void SetData(Pc pc)
    {
        if (pc != null)
        {
            nameLabel.text = pc.Name.FirstName + "\n" + pc.Name.LastName;
            detailsLabel.text = "Lvl " + pc.Level + " " + pc.RaceKey + " " + pc.ProfessionKey;
            detailsLabel.text += "Health " + pc.DerivedAttributes[(int)DerivedAttribute.Health].Current + "/" + pc.DerivedAttributes[(int)DerivedAttribute.Health].Maximum;
            detailsLabel.text += "Morale " + pc.DerivedAttributes[(int)DerivedAttribute.Morale].Current + "/" + pc.DerivedAttributes[(int)DerivedAttribute.Morale].Maximum;
            detailsLabel.text += "Exp " + pc.Experience + "/" + pc.ExpToLevel;

        }
        else
        {
            nameLabel.text = "";
            detailsLabel.text = "";
        }
    }
}