using Reclamation.World;
using Reclamation.Misc;
using UnityEngine;

namespace Reclamation.Party
{
    public class PartyCursor : Singleton<PartyCursor>
    {
        public bool PlaceMoveCursor(Vector3 position)
        {
            //Debug.Log("Moving Cursor " + position);
            transform.position = position;

            return true;
        }
    }
}