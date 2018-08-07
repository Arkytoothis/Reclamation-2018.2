using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Reclamation.Characters;
using Reclamation.Props;

namespace Reclamation.Misc
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] Texture2D moveCursor;
        [SerializeField] Texture2D interactCursor;
        [SerializeField] Texture2D transitionCursor;
        [SerializeField] Texture2D meleeCursor;
        [SerializeField] Texture2D rangedCursor;
        [SerializeField] Texture2D powerCursor;
        [SerializeField] Texture2D spellCursor;
        [SerializeField] Texture2D guiCursor;
        [SerializeField] Texture2D errorCursor;

        [SerializeField] Vector2 hotspot;

        public delegate bool OnMouseOverTerrain(RaycastHit hit);
        public event OnMouseOverTerrain onMouseOverWalkable;

        public delegate bool OnMouseOverEnemy(GameObject go);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate bool OnMouseOverInteractable(GameObject go);
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
                NpcController npc = hit.collider.gameObject.GetComponent<NpcController>();

                if (npc != null && npc.CheckIsAlive() == true)
                {
                    Cursor.SetCursor(meleeCursor, hotspot, CursorMode.Auto);

                    if(onMouseOverEnemy != null)
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
                    if(interactable is Transition == true)
                        Cursor.SetCursor(transitionCursor, hotspot, CursorMode.Auto);
                    else
                        Cursor.SetCursor(interactCursor, hotspot, CursorMode.Auto);

                    if(onMouseOverInteractable != null)
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

                    if(onMouseOverWalkable != null)
                        onMouseOverWalkable(hit);

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