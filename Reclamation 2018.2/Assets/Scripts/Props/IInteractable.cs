﻿using UnityEngine;

namespace Reclamation.Props
{
    /// <summary>
    /// Interface for objects that character can interact with.
    /// </summary>
    public interface IInteractable
    {
        /// <summary>
        /// Interact with the object.
        /// </summary>
        bool Interact(GameObject other);
    }
}