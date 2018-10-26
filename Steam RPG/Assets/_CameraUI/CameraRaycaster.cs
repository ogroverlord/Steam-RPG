using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using RPG.Characters;
using System.Collections.Generic;
using System;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {
        [SerializeField] int[] layerPriorities = null;
        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D attackCursor = null;
        [SerializeField] Vector2 cursorHotspot = new Vector2(0f, 0f);


        const int WALKABLE_LAYER = 8;

        float maxRaycastDepth = 100f; // Hard coded value

        Rect screenRectAtStartPlay = new Rect(0, 0, Screen.width, Screen.height);

        public delegate void OnMouseOverEnemy(Enemy enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;

        public delegate void OnMoseOverWalkable(Vector3 destination);
        public event OnMoseOverWalkable onMouseOverWalkable;


        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //UI Interaction

            }
            else
            {
                PerformRaycast();
            }
        }

        void PerformRaycast()
        {
            if (screenRectAtStartPlay.Contains(Input.mousePosition))
            {
                // Raycast to max depth, every frame as things can move under mouse
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (RaycastForEnemy(ray)) { return; }
                if (RaycastForWaklable(ray)) { return; }
            }

        }

        private bool RaycastForWaklable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask walkableLayer = 1 << WALKABLE_LAYER;
            bool walkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, walkableLayer);

            if (walkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverWalkable(hitInfo.point);
                return true;
            }
            return false;
        }

        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;
            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);
            var gameObjectHit = hitInfo.collider.gameObject;
            var enemyHit = gameObjectHit.GetComponent<Enemy>();

            if (enemyHit)
            {
                Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
                onMouseOverEnemy(enemyHit);
                return true;
            }
            return false;
        }



    }
}