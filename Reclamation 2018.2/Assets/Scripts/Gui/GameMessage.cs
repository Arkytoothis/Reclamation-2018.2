using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reclamation.Gui
{
    public class GameMessage
    {
        public TMP_Text label;
        public string text;
        [SerializeField] Button background;

        public void SetData(string text)
        {
            label.text = text;
        }
    }
}