﻿using Reclamation.Characters;
using Reclamation.Misc;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Gui.World
{
    public class BarracksDerivedAttributesPanel : Panel
    {
        [SerializeField] GameObject attributeElementPrefab;
        [SerializeField] List<GameObject> attributeElements;
        [SerializeField] Transform attributeElementsParent;

        public override void Initialize(GameScreen screen)
        {
            base.Initialize(screen);
            SetupAttributeElements();
        }

        public void SetupAttributeElements()
        {
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                GameObject go = Instantiate(attributeElementPrefab, attributeElementsParent);

                AttributeElement element = go.GetComponent<AttributeElement>();
                element.Initialize(((DerivedAttribute)i).ToString());
                attributeElements.Add(go);
            }
        }

        public void SetPcData(PcData pcData)
        {
            for (int i = 0; i < (int)DerivedAttribute.Number; i++)
            {
                Attribute attribute = pcData.Attributes.GetAttribute(AttributeListType.Derived, i);
                attributeElements[i].GetComponent<AttributeElement>().SetData(attribute);
            }
        }
    }
}