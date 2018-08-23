using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour {

    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Vector2 curosHotspot = new Vector2(96f,96f);

    //solve const and serialize fields
    [SerializeField] const int walkableLayerNumber = 8; 
    [SerializeField] const int enemyLayerNumber = 9; 

    CameraRaycaster cameraRaycaster;
    
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.notifyLayerChangeObservers += OnLayerChanged; //register to delegate 
    }

    void OnLayerChanged(int newlayer) {
        switch (newlayer)
        {
            case walkableLayerNumber:
                Cursor.SetCursor(walkCursor, curosHotspot, CursorMode.Auto);
                break;
            case enemyLayerNumber:
                Cursor.SetCursor(attackCursor, curosHotspot, CursorMode.Auto);
                break;
            default:
                Cursor.SetCursor(unknownCursor, curosHotspot, CursorMode.Auto);
                break;
        }
        
        // Consider the regesration of events OnLayerChange on leaving game scene 
    }
}
