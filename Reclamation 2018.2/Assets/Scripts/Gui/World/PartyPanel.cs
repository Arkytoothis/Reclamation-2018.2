﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Reclamation.Party;
using Reclamation.World;
using TMPro;

namespace Reclamation.Gui.World
{
    public class PartyPanel : MonoBehaviour
    {
        [SerializeField] TMP_Text nameLabel;

        [SerializeField] GameObject[] pcElements = new GameObject[6];
        [SerializeField] GameObject pcElementPrefab;
        [SerializeField] Transform pcElementParent;

        //[SerializeField] ListData listData = new ListData();

        public void Initialize()
        {
            //listData.SetData(pcElementPrefab, pcElementParent);

            if (PlayerManager.instance.GetPartyData(0) != null)
            {
                for (int i = 0; i < PartyData.MaxPartySize; i++)
                {
                    pcElements[i].GetComponent<PartyPcElement>().SetData(null);
                }
            }

            UpdateData();
        }

        public void UpdateData()
        {
            nameLabel.text = PlayerManager.instance.GetPartyData(0).Name;

            for (int i = 0; i < pcElements.Length; i++)
            {
                if (i < PlayerManager.instance.Pcs.Count)
                {
                    pcElements[i].GetComponent<PartyPcElement>().SetData(PlayerManager.instance.GetPcData(i));
                }
                else
                {
                    pcElements[i].GetComponent<PartyPcElement>().SetData(null);
                }
            }
        }
    }
}