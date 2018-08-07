using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reclamation.Gui
{
    public class Panel : GuiElement
    {
        public TMP_Text headingLabel;
        public Image headingBackground;
        public Image background;
        public Image border;

        public PanelData panelData;

        protected override void OnSkinGui()
        {
            Refresh();
        }

        public void Refresh()
        {
            base.OnSkinGui();

            headingLabel.text = panelData.defaultHeadingText;
            headingLabel.color = panelData.headingLabelColor;
            headingBackground.color = panelData.headingBackgroundColor;
            background.color = panelData.backgroundColor;
            border.color = panelData.borderColor;
        }
    }
}