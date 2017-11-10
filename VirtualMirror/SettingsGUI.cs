using MSCLoader;
using UnityEngine;

namespace VirtualMirror
{
    class SettingsGUI
    {
        private Cars _currentCar = VirtualMirror.CurrentCar;
        private GameObject LEFTSIDE_Cam = VirtualMirror.LEFTSIDE_Cam;
        private GameObject RIGHTSIDE_Cam = VirtualMirror.RIGHTSIDE_Cam;
        private GameObject REARVIEW_Cam = VirtualMirror.REARVIEW_Cam;
        private float _buttomWidth = 40;
        private float _areaWidth = 160;
        private float _areaHeight = 80;
        private float _padding = 10;
        private float _top = 80;
        private float _translateFactor = 0.05f;
        private float _rotationFactor = 1.5f;

        public void DoWindow()
        {
            string car_obj_name = GameObject.Find("PLAYER").transform.root.name;
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.gray;

            GUILayout.Label("Virtual Mirrors", labelStyle);

            #region TRANLATE
            #region Left side mirror
            GUILayout.BeginArea(new Rect(_padding, _top, _areaWidth, _areaHeight));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // backword
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, -_translateFactor, 0, 0);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, _translateFactor, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // forword
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, _translateFactor, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -_translateFactor, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            LEFTSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalPosition;
                            _currentCar.LeftCam.LocalPosition = LEFTSIDE_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, _translateFactor, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            #endregion
            #region Rearview mirror
            GUILayout.BeginArea(new Rect(_areaWidth + _padding, _top, _areaWidth, _areaHeight));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // backword
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, 0, -_translateFactor);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, _translateFactor, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // forword
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, 0, _translateFactor);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, -_translateFactor, 0, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            REARVIEW_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RearviewCam.LocalPosition;
                            _currentCar.RearviewCam.LocalPosition = REARVIEW_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, _translateFactor, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, -_translateFactor, 0, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            #endregion
            #region Right side mirror
            GUILayout.BeginArea(new Rect(_areaWidth * 2 + _padding, _top, _areaWidth, _areaHeight));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // backword
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, -_translateFactor, 0, 0);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, _translateFactor, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // forword
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, _translateFactor, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, -_translateFactor, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            RIGHTSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RightCam.LocalPosition;
                            _currentCar.RightCam.LocalPosition = RIGHTSIDE_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, _translateFactor, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            #endregion
            #endregion


            #region ROTATION
            #region Left side mirror
            GUILayout.BeginArea(new Rect(_padding, _top + _areaHeight + _padding, _areaWidth, _areaHeight));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.right, -_rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.up, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.right, _rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.forward, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            LEFTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalEulerAngles;
                            _currentCar.LeftCam.LocalEulerAngles = LEFTSIDE_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            #endregion
            #region Rearview Mirror
            GUILayout.BeginArea(new Rect(_padding + _areaWidth, _top + _areaHeight + _padding, _areaWidth, _areaHeight));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.right, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.forward, _rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.up, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            REARVIEW_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).RearviewCam.LocalEulerAngles;
                            _currentCar.RearviewCam.LocalEulerAngles = REARVIEW_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.up, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.right, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();

            #endregion
            #region Right side mirror
            GUILayout.BeginArea(new Rect(_padding + _areaWidth * 2, _top + _areaHeight + _padding, _areaWidth, _areaHeight));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.right, -_rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.up, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.right, _rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.forward, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            RIGHTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).RightCam.LocalEulerAngles;
                            _currentCar.RightCam.LocalEulerAngles = RIGHTSIDE_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            #endregion
            #endregion


            GUI.DragWindow();

        }

        private static Vector3 Rotate(GameObject target, Vector3 axis, float angle, Space relativeTo = Space.Self)
        {
            target.transform.Rotate(axis, angle, relativeTo);
            return target.transform.localEulerAngles;
        }

        private static Vector3 Translate(GameObject target, float v1, float v2, float v3, Space relativeTo)
        {
            target.transform.Translate(v1, v2, v3, relativeTo);
            return target.transform.localPosition;
        }

        private static Vector3 Translate(GameObject target, float v1, float v2, float v3)
        {
            target.transform.Translate(v1, v2, v3, target.transform.parent);
            return target.transform.localPosition;
        }
    }
}
