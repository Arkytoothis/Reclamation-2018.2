using Reclamation.World;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Gui.World
{
    public class BarracksListPanel : Panel
    {
        [SerializeField] GameObject pcListElementPrefab;
        [SerializeField] List<GameObject> pcListElements;
        [SerializeField] Transform pcListParent;

        public override void Initialize(GameScreen screen)
        {
            base.Initialize(screen);
            LoadCharacters();
        }

        public void LoadCharacters()
        {
            for (int i = 0; i < PlayerManager.instance.Pcs.Count; i++)
            {
                GameObject go = Instantiate(pcListElementPrefab, pcListParent);

                PcListElement listElement = go.GetComponent<PcListElement>();
                listElement.SetData(i);
                listElement.onSelectPc += ((BarracksScreen)screen).SelectPc;
            }
        }
    }
}