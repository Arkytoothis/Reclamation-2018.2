using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Reclamation.Props;

namespace Reclamation.Misc
{
    public class CameraRaycaster : MonoBehaviour
    {
        public Texture2D moveCursor;
        public Texture2D interactCursor;
        public Texture2D meleeCursor;
        public Texture2D rangedCursor;
        public Texture2D powerCursor;
        public Texture2D spellCursor;
        public Texture2D guiCursor;
        public Texture2D errorCursor;

        public Vector2 hotspot;

        public delegate void OnMouseOverTerrain(Vector3 destination);
        public event OnMouseOverTerrain onMouseOverWalkable;

        public delegate void OnMouseOverEnemy(GameObject go);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate void OnMouseOverInteractable(GameObject go);
        public event OnMouseOverInteractable onMouseOverInteractable;

        const int WALKABLE_LAYER_NUMBER = 8;

        void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject() == true)
            {
                Cursor.SetCursor(guiCursor, hotspot, CursorMode.Auto);
                return;
            }
            else
            {
                PerformRaycasts();
            }
        }

        void PerformRaycasts()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (RaycastForEnemy(ray) == true) { return; }
            if (RaycastForInteraction(ray) == true) { return; }
            if (RaycastForWalkability(ray) == true) { return; }
        }

        bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                EncounterNpcController npc = hit.collider.gameObject.GetComponent<EncounterNpcController>();

                if (npc != null)
                {
                    Cursor.SetCursor(meleeCursor, hotspot, CursorMode.Auto);
                    onMouseOverEnemy(npc.gameObject);
                    return true;
                }
            }

            return false;
        }

        bool RaycastForInteraction(Ray ray)
        {
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                Interactable interactable = hit.collider.gameObject.GetComponent<Interactable>();

                if (interactable != null)
                {
                    Cursor.SetCursor(interactCursor, hotspot, CursorMode.Auto);
                    onMouseOverInteractable(interactable.gameObject);
                    return true;
                }
            }

            return false;
        }

        bool RaycastForWalkability(Ray ray)
        {
            RaycastHit hit;
            LayerMask walkableLayer = 1 << WALKABLE_LAYER_NUMBER;
            bool walkability = Physics.Raycast(ray, out hit, 1000, walkableLayer);

            if (walkability == true)
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    Cursor.SetCursor(moveCursor, hotspot, CursorMode.Auto);
                    onMouseOverWalkable(hit.point);
                    return true;
                }
            }
            else
            {
                Cursor.SetCursor(errorCursor, hotspot, CursorMode.Auto);
            }

            return false;
        }
    }
}