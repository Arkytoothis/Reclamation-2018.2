using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Reclamation.Misc;

namespace Reclamation.Abilities
{
    [System.Serializable]
    public class TriggerComponent : AbilityComponent
    {
        public TriggerType Type;

        public TriggerComponent()
        {
            ComponentType = AbilityComponentType.Trigger;
            Type = TriggerType.None;
            Setup();
        }

        public TriggerComponent(TriggerType type)
        {
            ComponentType = AbilityComponentType.Trigger;
            Type = type;
            Setup();
        }

        public override void Setup()
        {
            Widgets = new List<AbilityPartWidgetType>();
            Widgets.Add(AbilityPartWidgetType.Dropdown);
        }

        public override string GetTooltipString()
        {
            string s = "";

            s = "Trait Type" + Type.ToString();

            return s;
        }
    }
}
