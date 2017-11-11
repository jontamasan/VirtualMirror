using MSCLoader;
using System;
using UnityEngine;

namespace VirtualMirror
{
    class SettingsGUI
    {
        private Cars _currentCar = VirtualMirror.CurrentCar;
        private GameObject LEFTSIDE_Cam = VirtualMirror.LEFTSIDE_Cam;
        private GameObject RIGHTSIDE_Cam = VirtualMirror.RIGHTSIDE_Cam;
        private GameObject REARVIEW_Cam = VirtualMirror.REARVIEW_Cam;
        private float _buttonWidth = 40;
        private float _translateFactor = 0.02f;
        private float _rotationFactor = 0.25f;

        public void DoWindow()
        {
            string car_obj_name = GameObject.Find("PLAYER").transform.root.name;
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.gray;

            GUILayout.Label("Virtual Mirrors", labelStyle);

            labelStyle.fontStyle = FontStyle.Normal;
            GUILayout.Label("Translate", labelStyle);

            #region TRANLATE
            #region Left side mirror
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // backword
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, -_translateFactor, 0, 0);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, _translateFactor, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // forword
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, _translateFactor, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -_translateFactor, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            LEFTSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalPosition;
                            _currentCar.LeftCam.LocalPosition = LEFTSIDE_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, _translateFactor, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
            #endregion
            #region Rearview mirror
                GUILayout.BeginVertical("box");
                GUILayout.FlexibleSpace();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // backword
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, 0, -_translateFactor);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, _translateFactor, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // forword
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, 0, _translateFactor);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, -_translateFactor, 0, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            REARVIEW_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RearviewCam.LocalPosition;
                            _currentCar.RearviewCam.LocalPosition = REARVIEW_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, _translateFactor, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, -_translateFactor, 0, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                #endregion
                #region Right side mirror
                GUILayout.BeginVertical("box");
                GUILayout.FlexibleSpace();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // backword
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, -_translateFactor, 0, 0);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, _translateFactor, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // forword
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, _translateFactor, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, -_translateFactor, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            RIGHTSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RightCam.LocalPosition;
                            _currentCar.RightCam.LocalPosition = RIGHTSIDE_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, _translateFactor, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            #endregion
            #endregion

            GUILayout.Label("Rotate", labelStyle);

            #region ROTATION
            #region Left side mirror
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical("box");
                GUILayout.FlexibleSpace();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.right, -_rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.up, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.right, _rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.forward, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            LEFTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalEulerAngles;
                            _currentCar.LeftCam.LocalEulerAngles = LEFTSIDE_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                #endregion
                #region Rearview Mirror
                GUILayout.BeginVertical("box");
                GUILayout.FlexibleSpace();
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.right, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.forward, _rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.up, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            REARVIEW_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).RearviewCam.LocalEulerAngles;
                            _currentCar.RearviewCam.LocalEulerAngles = REARVIEW_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.up, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.right, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
                #endregion
                #region Right side mirror
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.right, -_rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.up, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.right, _rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.forward, _rotationFactor, Space.World);
                        }
                        if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            RIGHTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).RightCam.LocalEulerAngles;
                            _currentCar.RightCam.LocalEulerAngles = RIGHTSIDE_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                        {
                            _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.FlexibleSpace();
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
            #endregion
            #endregion

            #region Label Min draw distance
            GUILayout.BeginHorizontal();
            {
                for (int i = 0; i < 3; i++)
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label("Min draw distance", labelStyle);
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
            #endregion

            #region Slider Min draw distance
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginHorizontal();
                {
                    LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane = float.Parse(GUILayout.TextField(LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                    LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane = GUILayout.HorizontalSlider(LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane, 0, 10);
                    _currentCar.LeftCam.NearClipPlane = LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    REARVIEW_Cam.GetComponent<Camera>().nearClipPlane = float.Parse(GUILayout.TextField(REARVIEW_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                    REARVIEW_Cam.GetComponent<Camera>().nearClipPlane = GUILayout.HorizontalSlider(REARVIEW_Cam.GetComponent<Camera>().nearClipPlane, 0, 10);
                    _currentCar.RearviewCam.NearClipPlane = REARVIEW_Cam.GetComponent<Camera>().nearClipPlane;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane = float.Parse(GUILayout.TextField(RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                    RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane = GUILayout.HorizontalSlider(RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane, 0, 10);
                    _currentCar.RightCam.NearClipPlane = RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();
            #endregion

            #region Label FOV
            GUILayout.BeginHorizontal();
            {
                for (int i = 0; i < 3; i++)
                {
                    GUILayout.BeginVertical();
                    {
                        GUILayout.Label("FOV", labelStyle);
                    }
                    GUILayout.EndVertical();
                }
            }
            GUILayout.EndHorizontal();
            #endregion

            #region Slider FOV
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginHorizontal();
                {
                    LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView = float.Parse(GUILayout.TextField(LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                    LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView = GUILayout.HorizontalSlider(LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                    _currentCar.LeftCam.FieldOfView = LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    REARVIEW_Cam.GetComponent<Camera>().fieldOfView = float.Parse(GUILayout.TextField(REARVIEW_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                    REARVIEW_Cam.GetComponent<Camera>().fieldOfView = GUILayout.HorizontalSlider(REARVIEW_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                    _currentCar.RearviewCam.FieldOfView = REARVIEW_Cam.GetComponent<Camera>().fieldOfView;
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                {
                    RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView = float.Parse(GUILayout.TextField(RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                    RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView = GUILayout.HorizontalSlider(RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                    _currentCar.RightCam.FieldOfView = RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView;
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndHorizontal();
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
