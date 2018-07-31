using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Sandbox
{
    public class SandboxGuiManager : MonoBehaviour
    {
        public CharacterPanel characterPanel;
        public EquipmentPanel equipmentPanel;
        public AnimationsPanel animationsPanel;
        public CameraPanel cameraPanel;

        public void Initialize()
        {
            characterPanel.Initialize();
            equipmentPanel.Initialize();
            animationsPanel.Initialize();
            cameraPanel.Initialize();
        }
    }
}