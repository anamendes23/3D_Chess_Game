using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
    private CameraRaycaster cameraRaycaster;
    private BoardController boardController;
    private GameObject selectedPiece;
    private List<GameObject> pieces;

    // Start is called before the first frame update
    void Start()
    {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        boardController = FindObjectOfType<BoardController>();
        pieces = boardController.activePieces;
    }

    void FixedUpdate()
    {
        ProcessGameAction();
    }

    private void ProcessGameAction()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hitInfo = cameraRaycaster.hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.Log(cameraRaycaster.currentLayerHit);

            switch (cameraRaycaster.currentLayerHit)
            {
                case Layer.Board:
                    if (Physics.Raycast(ray, out hitInfo))
                    {
                        Debug.Log(hitInfo.collider.gameObject.name);
                    }
                    break;
                case Layer.BlueTeam:

                    break;
                case Layer.RedTeam:
                    break;
                case Layer.MoveTiles:
                    break;
                case Layer.RaycastEndStop:
                    break;
                default:
                    break;
            }
        }
    }

}
