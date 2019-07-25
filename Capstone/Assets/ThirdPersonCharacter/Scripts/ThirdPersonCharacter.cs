using System.Collections.Generic;
using UnityEngine;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(CapsuleCollider))]
	[RequireComponent(typeof(Animator))]
	public class ThirdPersonCharacter : MonoBehaviour
	{
        public bool IsSelected = false;
        Renderer r;
        private Color originalColor;
        public Piece pieceType;
        public string boardPosition;
        public List<string> attackPositions = new List<string>();
        public List<string> futureMovePositions = new List<string>();

        public void SetSelection(bool isselected)
        {
            IsSelected = isselected;
        }

        void Start()
		{
            r = transform.Find("Layer_1").GetComponent<Renderer>();
            originalColor = r.material.color;

            //Physics.IgnoreLayerCollision(11, 10);
            //Physics.IgnoreLayerCollision(12, 10);
        }

        private void Update()
        {
            SelectedColor();
        }

        private void SelectedColor()
        {
            if (IsSelected)
                r.material.SetColor("_Color", Color.yellow);
            else
                r.material.SetColor("_Color", originalColor);
        }
    }
}
