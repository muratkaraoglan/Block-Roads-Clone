using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TestScene
{
    public class Shape : MonoBehaviour
    {
        private bool _isSelected;
        private bool _canSelectable;
        private Vector3 _movementStartPosition;
        [SerializeField] private float moveSpeed = 2f;
        [SerializeField] private Transform[] children;

        private void Start()
        {
            int childCount = transform.childCount;
            _canSelectable = true;
            _movementStartPosition =
                new Vector3(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width * .5f, Screen.height * .5f)).x, 1,
                    -2);
        }

        private void Update()
        {
            if (!_isSelected) return;
            Vector2 moseDelta = InputHelper.Instance.MouseDelta;
            transform.position += Time.deltaTime * moveSpeed * new Vector3(moseDelta.x, 0, moseDelta.y);
            GridCreator.Instance.Highlight(children);
            if (Mouse.current.leftButton.wasReleasedThisFrame)
            {
                var canPlace = GridCreator.Instance.CanPlaceShape(children, out Vector3 pos);
                //GridCreator.Instance.ClearHighlights();
                if (!canPlace) return;
                transform.position = pos;
                _isSelected = false;
                _canSelectable = false;
            }
        }

        private void OnGUI()
        {
            if (_canSelectable)
                GUI.TextArea(new Rect(300, 300, 200, 200),
                    transform.position.ToString() + " " + GridCreator.Instance.WorldToGridPosition(transform.position));
        }

        public void OnMouseDown()
        {
            if (!_canSelectable) return;
            transform.position = _movementStartPosition;
            _isSelected = true;
        }


        [ContextMenu("Set Pivot")]
        public void SetPivot()
        {
            if (transform.childCount == 0) return;

            int childCount = transform.childCount;

            children = new Transform[childCount];
            for (int i = 0; i < childCount; i++)
                children[i] = transform.GetChild(i);

            Vector3 center = Vector3.zero;
            for (int i = 0; i < childCount; i++)
            {
                center += children[i].localPosition;
            }

            center /= childCount;

            Debug.DrawLine(transform.position, transform.position + Vector3.up * 3f, Color.green, 10f);

            for (int i = 0; i < childCount; i++)
            {
                children[i].localPosition -= center;
            }
        }
    }
}