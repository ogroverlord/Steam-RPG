using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D errorCursor = null;
    [SerializeField] Vector2 curosHotspot = new Vector2(96f,96f);

    CameraRaycaster cameraRaycaster;
    

   
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += OnLayerChanged; //register to delegate 
    }

    void OnLayerChanged(Layer newlayer) {
        switch (newlayer)
        {
              case Layer.Walkable:
                Cursor.SetCursor(walkCursor, curosHotspot, CursorMode.Auto);
                break;
            case Layer.Enemy:
                Cursor.SetCursor(attackCursor, curosHotspot, CursorMode.Auto);
                break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(errorCursor, curosHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("No cursor for this case!");
                break;
        }
        
        // Consider the regesration of events OnLayerChange on leaving game scene 
    }
}
