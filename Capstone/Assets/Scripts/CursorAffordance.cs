using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//if we attach this to the main camera and have icons for actions, the mouse cursor will change according to the action being taken
[RequireComponent(typeof(CameraRaycaster))]
public class CursorAffordance : MonoBehaviour
{
    [SerializeField] Texture2D walkCursor = null;
    [SerializeField] Texture2D attackCursor = null;
    [SerializeField] Texture2D unknownCursor = null;
    [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);

    CameraRaycaster cameraRaycaster;

    // Start is called before the first frame update
    void Start()
    {
        cameraRaycaster = GetComponent<CameraRaycaster>();
        cameraRaycaster.onLayerChange += OnLayerChange;
    }

    // Update is called once per frame
    void OnLayerChange(Layer newLayer) //only gets called when layer changes
    {
        switch (newLayer)
        {
            case Layer.MoveTiles:
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                break;
            //case Layer.Enemy:
            //    Cursor.SetCursor(attackCursor, cursorHotspot, CursorMode.Auto);
            //    break;
            case Layer.RaycastEndStop:
                Cursor.SetCursor(unknownCursor, cursorHotspot, CursorMode.Auto);
                break;
            default:
                Debug.LogError("Don't know what cursor to show");
                return;
        }
    }

    // TODO consider de-registering OnLayerChanged on leaving all game scenes
}
