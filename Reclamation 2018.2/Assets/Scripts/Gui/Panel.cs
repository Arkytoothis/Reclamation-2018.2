using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Reclamation.Gui
{
    public class Panel : GuiElement
    {
        [SerializeField] TMP_Text headingLabel;
        [SerializeField] Image headingBackground;
        [SerializeField] Image background;
        [SerializeField] Image border;
        [SerializeField] PanelData panelData;

        protected GameScreen screen;

        protected override void OnSkinGui()
        {
            Refresh();
        }

        public virtual void Initialize(GameScreen screen)
        {
            this.screen = screen;
        }

        public void Refresh()
        {
            base.OnSkinGui();

            headingLabel.color = panelData.headingLabelColor;
            headingBackground.color = panelData.headingBackgroundColor;
            background.color = panelData.backgroundColor;
            border.color = panelData.borderColor;
        }
    }
}