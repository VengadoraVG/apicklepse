using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Laser {
    public class SimplerControl : MonoBehaviour {
        public Transform controlPoints;
        public Vector2 input;
        public bool lockPosition = false;

        private Transform _rig;
        private Transform _xRot;
        private Transform _yRot;

        private Transform _up;
        private Transform _down;
        private Transform _topRight;
        private Transform _topLeft;
        private Transform _bottomRight;
        private Transform _bottomLeft;

        private float _width;
        private float _height;
        private Vector2 _top; // screen point
        private Vector2 _bottom; // screen point

        void Start () {
            _rig = transform.Find("rig");
            _xRot = _rig.transform.Find("x rotation");
            _yRot = _xRot.transform.Find("y rotation");

            _up = controlPoints.Find("up");
            _down = controlPoints.Find("down");
            _topRight = controlPoints.Find("top right");
            _topLeft = controlPoints.Find("top left");
            _bottomRight = controlPoints.Find("bottom right");
            _bottomLeft = controlPoints.Find("bottom left");

            _top = new Vector2(Camera.main.WorldToScreenPoint(_topLeft.position).x,
                               Camera.main.WorldToScreenPoint(_up.position).y);
            _bottom = new Vector2(Camera.main.WorldToScreenPoint(_topRight.position).x,
                                  Camera.main.WorldToScreenPoint(_down.position).y);

            _width = Mathf.Abs(_top.x - _bottom.x);
            _height = Mathf.Abs(_top.y - _bottom.y);
            input = new Vector2(0.5f, 0.5f);
        }

        void Update () {
            if (Input.GetMouseButton(0)) {
                float xProportion = (_bottom.x - Input.mousePosition.x) / _width;
                float yProportion = (Input.mousePosition.y - _bottom.y) / _height;
                input = new Vector2(Mathf.Lerp(1, 0, xProportion),
                                    Mathf.Lerp(1, 0, yProportion));
            }

            if (!lockPosition) {
                _rig.position = new Vector3(Mathf.Lerp(_topLeft.position.x,
                                                       _topRight.position.x, input.x),
                                            _rig.position.y, _rig.position.z);
            }

            Quaternion leftRot =
                Quaternion.Lerp(_topLeft.rotation, _bottomLeft.rotation, input.y);
            Quaternion rightRot =
                Quaternion.Lerp(_topRight.rotation, _bottomRight.rotation, input.y);

            _xRot.localRotation =
                Quaternion.Lerp(_up.rotation, _down.rotation, input.y);
            _yRot.localRotation =
                Quaternion.Lerp(leftRot, rightRot, input.x);
        }
    }
}
