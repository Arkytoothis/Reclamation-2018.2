using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Reclamation.Characters;

public class PcElement : MonoBehaviour
{
    public TMP_Text nameLabel;
    public TMP_Text detailsLabel;
    public RawImage portraitImage;

    public void SetData(PcData pc)
    {
        if (pc != null)
        {
            nameLabel.text = pc.name.FirstName;
            detailsLabel.text = "Lvl " + pc.level + " " + pc.raceKey + " " + pc.professionKey;
        }
        else
        {
            nameLabel.text = "";
            detailsLabel.text = "";
            portraitImage.texture = null;
        }
    }
}
