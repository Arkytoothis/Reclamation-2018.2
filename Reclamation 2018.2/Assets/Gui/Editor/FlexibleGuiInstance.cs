using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Reclamation.Gui
{
    public class FlexibleGuiInstance : Editor
    {
        static GameObject clickedObject;

        [MenuItem("GameObject/Flexible Gui/Button", priority = 0)]
        public static void AddButton()
        {
            Create("Flexible Button");
        }

        [MenuItem("GameObject/Flexible Gui/Pc Button", priority = 0)]
        public static void AddPcButton()
        {
            Create("Flexible Pc Button");
        }

        private static GameObject Create(string objectName)
        {
            Debug.Log("Gui/" + objectName);
            GameObject instance = Instantiate(Resources.Load<GameObject>("Gui/" + objectName));
            instance.name = objectName;
            clickedObject = UnityEditor.Selection.activeObject as GameObject;

            if (clickedObject != null)
            {
                instance.transform.SetParent(clickedObject.transform, false);
            }

            return instance;
        }
    }
}