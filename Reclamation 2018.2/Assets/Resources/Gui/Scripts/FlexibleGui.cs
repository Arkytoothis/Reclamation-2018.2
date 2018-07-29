using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Reclamation.Gui
{
    [ExecuteInEditMode()]
    public class FlexibleGui : MonoBehaviour
    {

        protected virtual void OnSkinGui()
        {
        }

        public virtual void Awake()
        {
            OnSkinGui();
        }

        //public virtual void Update()
        //{
        //    if (Application.isEditor == true)
        //    {
        //        OnSkinGui();
        //    }
        //}
    }
}