using MSCLoader;
using UnityEngine;

namespace VirtualMirror
{
    class SettingsGUI
    {
        private Cars _currentCar = VirtualMirror.CurrentCar;
        private GameObject LEFTSIDE_Cam = VirtualMirror.LEFTSIDE_Cam;
        private GameObject RIGHSIDE_Cam = VirtualMirror.RIGHTSIDE_Cam;
        private GameObject REARVIEW_Cam = VirtualMirror.REARVIEW_Cam;
        private float _buttomWidth = 40;
        private float _areaWidth = 160;
        private float _areaHeight = 80;
        private float _padding = 10;
        private float _top = 80;

        public void DoWindow()
        {
            //GUILayout.BeginArea(new Rect(padding, 0, 500 - padding * 2, 20));

            //GUILayout.EndArea();
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.gray;

            GUILayout.Label("Virtual Mirrors", labelStyle);

            #region Translate
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
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, -0.01f, 0, 0);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, 0.01f, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // forword
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0.01f, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -0.01f, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            LEFTSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalPosition;
                            _currentCar.LeftCam.LocalPosition = LEFTSIDE_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, 0.01f, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -0.01f, 0, Space.World);
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
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, -0.01f, 0, 0);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, 0.01f, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // forword
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0.01f, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, -0.01f, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            REARVIEW_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RearviewCam.LocalPosition;
                            _currentCar.RearviewCam.LocalPosition = REARVIEW_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, 0.01f, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, -0.01f, 0, Space.World);
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
                            _currentCar.RightCam.LocalPosition = Translate(RIGHSIDE_Cam, -0.01f, 0, 0);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHSIDE_Cam, 0, 0.01f, 0, Space.World);
                        }
                        if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttomWidth))) // forword
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHSIDE_Cam, 0.01f, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHSIDE_Cam, 0, -0.01f, 0);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            RIGHSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RightCam.LocalPosition;
                            _currentCar.RightCam.LocalPosition = RIGHSIDE_Cam.transform.localPosition;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHSIDE_Cam, 0, 0.01f, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.RightCam.LocalPosition = Translate(RIGHSIDE_Cam, 0, -0.01f, 0, Space.World);
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
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.forward, -1);
                        }
                        if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.right, -1/*, Space.World*/);
                        }
                        if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.forward, 1);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.up, 1, Space.World);
                        }
                        if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                            ModConsole.Print("currentCar: " + _currentCar.LeftCam.LocalEulerAngles);
                            ModConsole.Print("revert: " + revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalEulerAngles);
                            LEFTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalEulerAngles;
                            _currentCar.LeftCam.LocalEulerAngles = LEFTSIDE_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.up, -1, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttomWidth)))
                        {
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.right, 1/*, Space.World*/);
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

        private static Vector3 Rotate(GameObject obj, Vector3 axis, float angle, Space relativeTo = Space.Self)
        {
            obj.transform.Rotate(axis, angle, relativeTo);
            return obj.transform.localEulerAngles;
        }

        private static Vector3 Translate(GameObject obj, float v1, float v2, float v3, Space relativeTo)
        {
            obj.transform.Translate(v1, v2, v3, relativeTo);
            return obj.transform.localPosition;
        }

        private static Vector3 Translate(GameObject obj, float v1, float v2, float v3)
        {
            obj.transform.Translate(v1, v2, v3, obj.transform.parent);
            return obj.transform.localPosition;
        }
    }
}
