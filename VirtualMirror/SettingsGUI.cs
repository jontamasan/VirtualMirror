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
        private float _buttonWidth = 40;
        private float _rotationFactor = 0.25f;
        private float _translateFactor = 0.02f;
        private static int _tabInt = 0;
        private static int _prevSelectedSide = VirtualMirror.Settings.SideMirrorsSelectionGrid;
        private static int _prevSelectedRear = VirtualMirror.Settings.RearviewMirrorSelectionGrid;
        private static int _selectSideTextureResolution = VirtualMirror.Settings.SideMirrorsSelectionGrid;
        private static int _selectRearviewTextureResolution = VirtualMirror.Settings.RearviewMirrorSelectionGrid;
        private string[] _tabName = { "Camera Settings", "Ovarall Settings" };
        private string[] _sideResolutionText = { "64", "128", "256", "512" };
        private string[] _rearviewResolutionText = { "64*256", "128*512", "256*1024", "512*2048" };

        public void DoWindow()
        {
            string car_obj_name = GameObject.Find("PLAYER").transform.root.name;
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.gray;

            _tabInt = GUILayout.Toolbar(_tabInt, _tabName);

            if (_tabInt == 0)
            {
                GUILayout.Label("Translate", labelStyle);

                #region TRANLATE
                using (new GUILayout.HorizontalScope())
                {
                    #region Left side mirror
                    GUI.enabled = VirtualMirror.IsGuiLeftMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Rearview mirror
                    GUI.enabled = VirtualMirror.IsGuiRearviewMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalPosition = Translate(REARVIEW_Cam, 0, -_translateFactor, 0, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Right side mirror
                    GUI.enabled = VirtualMirror.IsGuiRightMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalPosition = Translate(RIGHTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                }
                #endregion

                GUI.enabled = true;
                GUILayout.Label("Rotate", labelStyle);

                #region ROTATION
                using (new GUILayout.HorizontalScope())
                {
                    #region Left side mirror
                    GUI.enabled = VirtualMirror.IsGuiLeftMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, LEFTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Rearview Mirror
                    GUI.enabled = VirtualMirror.IsGuiRearviewMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalEulerAngles = Rotate(REARVIEW_Cam, REARVIEW_Cam.transform.parent.right, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Right side mirror
                    GUI.enabled = VirtualMirror.IsGuiRightMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
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
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalEulerAngles = Rotate(RIGHTSIDE_Cam, RIGHTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                }
                #endregion

                #region Label Min draw distance
                GUI.enabled = true;
                using (new GUILayout.HorizontalScope())
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
                #endregion

                #region Slider Min draw distance
                using (new GUILayout.HorizontalScope())
                {
                    GUI.enabled = VirtualMirror.IsGuiLeftMirrorEnabled;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.IsGuiLeftMirrorEnabled)
                        {
                            LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane = float.Parse(GUILayout.TextField(LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                            LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane = GUILayout.HorizontalSlider(LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane, 0, 10);
                            _currentCar.LeftCam.NearClipPlane = LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane;
                        }
                        else
                        {
                            GUILayout.TextField("", GUILayout.MaxWidth(40));
                            GUILayout.HorizontalSlider(1, 1, 100);
                        }
                    }
                    GUI.enabled = VirtualMirror.IsGuiRearviewMirrorEnabled;
                    using (new GUILayout.HorizontalScope())
                    {
                        REARVIEW_Cam.GetComponent<Camera>().nearClipPlane = float.Parse(GUILayout.TextField(REARVIEW_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                        REARVIEW_Cam.GetComponent<Camera>().nearClipPlane = GUILayout.HorizontalSlider(REARVIEW_Cam.GetComponent<Camera>().nearClipPlane, 0, 10);
                        _currentCar.RearviewCam.NearClipPlane = REARVIEW_Cam.GetComponent<Camera>().nearClipPlane;
                    }
                    GUI.enabled = VirtualMirror.IsGuiRightMirrorEnabled;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.IsGuiRightMirrorEnabled)
                        {
                            RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane = float.Parse(GUILayout.TextField(RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                            RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane = GUILayout.HorizontalSlider(RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane, 0, 10);
                            _currentCar.RightCam.NearClipPlane = RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane;
                        }
                        else
                        {
                            GUILayout.TextField("", GUILayout.MaxWidth(40));
                            GUILayout.HorizontalSlider(1, 1, 100);
                        }
                    }
                }
                #endregion

                #region Label FOV
                GUI.enabled = true;
                using (new GUILayout.HorizontalScope())
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
                #endregion

                #region Slider FOV
                using (new GUILayout.HorizontalScope())
                {
                    GUI.enabled = VirtualMirror.IsGuiLeftMirrorEnabled;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.IsGuiLeftMirrorEnabled)
                        {
                            LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView = float.Parse(GUILayout.TextField(LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                            LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView = GUILayout.HorizontalSlider(LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                            _currentCar.LeftCam.FieldOfView = LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView;
                        }
                        else
                        {
                            GUILayout.TextField("", GUILayout.MaxWidth(40));
                            GUILayout.HorizontalSlider(1, 1, 100);
                        }
                    }
                    GUI.enabled = VirtualMirror.IsGuiRearviewMirrorEnabled;
                    using (new GUILayout.HorizontalScope())
                    {
                        REARVIEW_Cam.GetComponent<Camera>().fieldOfView = float.Parse(GUILayout.TextField(REARVIEW_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                        REARVIEW_Cam.GetComponent<Camera>().fieldOfView = GUILayout.HorizontalSlider(REARVIEW_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                        _currentCar.RearviewCam.FieldOfView = REARVIEW_Cam.GetComponent<Camera>().fieldOfView;
                    }
                    GUI.enabled = VirtualMirror.IsGuiRightMirrorEnabled;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.IsGuiRightMirrorEnabled)
                        {
                            RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView = float.Parse(GUILayout.TextField(RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                            RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView = GUILayout.HorizontalSlider(RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                            _currentCar.RightCam.FieldOfView = RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView;
                        }
                        else
                        {
                            GUILayout.TextField("", GUILayout.MaxWidth(40));
                            GUILayout.HorizontalSlider(1, 1, 100);
                        }
                    }
                }
                #endregion
            }
            else
            {
                GUI.enabled = true;
                GUILayout.Label("Max draw distance", labelStyle);
                using (new GUILayout.HorizontalScope())
                {
                    if (VirtualMirror.IsGuiRightMirrorEnabled ||
                        VirtualMirror.IsGuiRearviewMirrorEnabled ||
                        VirtualMirror.IsGuiLeftMirrorEnabled)
                    {
                        GUI.enabled = true;
                        VirtualMirror.Settings.FarClipPlane = int.Parse(GUILayout.TextField(VirtualMirror.Settings.FarClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                        VirtualMirror.Settings.FarClipPlane = (int)GUILayout.HorizontalSlider(VirtualMirror.Settings.FarClipPlane, 1, 2000);
                    }
                    else
                    {
                        GUI.enabled = false;
                        GUILayout.TextField("", GUILayout.MaxWidth(40));
                        GUILayout.HorizontalSlider(1, 1, 10);
                    }
                    if (VirtualMirror.IsGuiLeftMirrorEnabled)
                        VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().farClipPlane = VirtualMirror.Settings.FarClipPlane;
                    if (VirtualMirror.IsGuiRearviewMirrorEnabled)
                        VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().farClipPlane = VirtualMirror.Settings.FarClipPlane;
                    if (VirtualMirror.IsGuiRightMirrorEnabled)
                        VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().farClipPlane = VirtualMirror.Settings.FarClipPlane;
                }

                GUILayout.Label("Side mirror resolution", labelStyle);
                using (new GUILayout.HorizontalScope())
                {
                    _selectSideTextureResolution = GUILayout.SelectionGrid(_selectSideTextureResolution, _sideResolutionText, 4);
                    if (_prevSelectedSide != _selectSideTextureResolution)
                    {
                        ModConsole.Print("tama");
                        _prevSelectedSide = _selectSideTextureResolution;
                        int width = 0;
                        if (_selectSideTextureResolution == 0)
                        {
                            width = 64;
                        }
                        else if (_selectSideTextureResolution == 1)
                        {
                            width = 128;
                        }
                        else if (_selectSideTextureResolution == 2)
                        {
                            width = 256;
                        }
                        else
                        {
                            width = 512;
                        }
                        VirtualMirror.Settings.SideMirrorsRenderTextureWidth = VirtualMirror.Settings.SideMirrorsRenderTextureHeight = width;
                        VirtualMirror.LeftRenderTexture =
                            new RenderTexture(VirtualMirror.Settings.SideMirrorsRenderTextureWidth, VirtualMirror.Settings.SideMirrorsRenderTextureHeight, VirtualMirror.Settings.RenderTextureDepth);
                        VirtualMirror.RightRenderTexture =
                            new RenderTexture(VirtualMirror.Settings.SideMirrorsRenderTextureWidth, VirtualMirror.Settings.SideMirrorsRenderTextureHeight, VirtualMirror.Settings.RenderTextureDepth);
                        //if (VirtualMirror.IsGuiLeftMirrorEnabled)
                        //{
                        //    VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().targetTexture = VirtualMirror.LeftRenderTexture;
                        //    VirtualMirror.LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = VirtualMirror.LeftRenderTexture;
                        //}
                        //if (VirtualMirror.IsGuiRightMirrorEnabled)
                        //{
                        //    VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().targetTexture = VirtualMirror.RightRenderTexture;
                        //    VirtualMirror.RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = VirtualMirror.RightRenderTexture;
                        //}
                    }
                }

                GUILayout.Label("Rearview mirror resolution", labelStyle);
                using (var scope = new GUILayout.HorizontalScope())
                {
                    _selectRearviewTextureResolution = GUILayout.SelectionGrid(_selectRearviewTextureResolution, _rearviewResolutionText, 4);
                    if (_prevSelectedRear != _selectRearviewTextureResolution)
                    {
                        _prevSelectedRear = _selectRearviewTextureResolution;
                        int height = 0;
                        if (_selectRearviewTextureResolution == 0)
                        {
                            height = 64;
                        }
                        else if (_selectRearviewTextureResolution == 1)
                        {
                            height = 128;
                        }
                        else if (_selectRearviewTextureResolution == 2)
                        {
                            height = 256;
                        }
                        else
                        {
                            height = 512;
                        }
                        VirtualMirror.Settings.RearviewMirrorRenderTextureHeight = height;
                        VirtualMirror.Settings.RearviewMirrorRenderTextureWidth = height * 4;
                        VirtualMirror.RearviewRenderTexture =
                            new RenderTexture(VirtualMirror.Settings.RearviewMirrorRenderTextureWidth, VirtualMirror.Settings.RearviewMirrorRenderTextureHeight, VirtualMirror.Settings.RenderTextureDepth);
                        //if (VirtualMirror.IsGuiRearviewMirrorEnabled)
                        //{
                        //    VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().targetTexture = VirtualMirror.RearviewRenderTexture;
                        //    VirtualMirror.REARVIEW_Mirror.GetComponent<MeshRenderer>().material.mainTexture = VirtualMirror.RearviewRenderTexture;
                        //}
                    }
                }

            }

            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close"))
            {
                VirtualMirror.IsGuiActive = false;
                VirtualMirror.IsPlayerInMenu.Value = false;
            }

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
