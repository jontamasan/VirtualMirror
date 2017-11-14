using MSCLoader;
using UnityEngine;

namespace VirtualMirror
{
    class SettingsGUI
    {
        private Cars _currentCar = VirtualMirror.CurrentCar;
        private static int _tabInt = 0;
        private float _buttonWidth = 40;
        private float _rotationFactor = 0.25f;
        private float _translateFactor = 0.02f;
        private Vector2 _scaleVerticalFactor = new Vector2(0, 0.05f);
        private Vector2 _scaleHorizontalFactor = new Vector2(0.05f, 0);
        private Vector2 _positionVerticalFactor = new Vector2(0, 0.05f);
        private Vector2 _positionHorizontalFactor = new Vector2(0.05f, 0);
        private static int _prevSelectedSide = VirtualMirror.Settings.SideMirrorsSelectionGrid;
        private static int _prevSelectedRear = VirtualMirror.Settings.RearviewMirrorSelectionGrid;
        private string _textRelativePosition = "Linking Left-Right";
        private string _textPreserveAspectRatio = "fixed aspect ratio";
        private string[] _sideResolutionText = { "64", "128", "256", "512" };
        private string[] _rearviewResolutionText = { "64*256", "128*512", "256*1024", "512*2048" };
        private string[] _tabName = {"Virtual Mirror Settings", "Camera Settings", "Ovarall Settings" };
        private bool _guiToggleUpdate;
        private static bool _prevLeftMirrorEnabled = VirtualMirror.IsGuiLeftMirrorEnabled;
        private static bool _prevRightMirrorEnabled = VirtualMirror.IsGuiRightMirrorEnabled;
        private static bool _prevRearviewMirrorEnabled = VirtualMirror.IsGuiRearviewMirrorEnabled;
        private static bool _toggleRelativeScale = true;
        private static bool _toggleRelativePosistion = true;
        private static bool _toggleLeftPreserveAspectRatio;
        private static bool _toggleRearPreserveAspectRatio;
        private static bool _toggleRightPreserveAspectRatio;

        public void DoWindow()
        {
            string car_obj_name = GameObject.Find("PLAYER").transform.root.name;
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.normal.textColor = Color.gray;

            #region mirror toggle switch
            using (new GUILayout.HorizontalScope("box"))
            {
                GUILayout.FlexibleSpace();
                VirtualMirror.IsGuiLeftMirrorEnabled = GUILayout.Toggle(VirtualMirror.IsGuiLeftMirrorEnabled, "Left mirror");
                if (_prevLeftMirrorEnabled != VirtualMirror.IsGuiLeftMirrorEnabled)
                {
                    _prevLeftMirrorEnabled = VirtualMirror.IsGuiLeftMirrorEnabled;
                    _guiToggleUpdate = true;
                    VirtualMirror.LEFTSIDE_Mirror.SetActive(VirtualMirror.IsGuiLeftMirrorEnabled);
                }
                GUILayout.FlexibleSpace();
                VirtualMirror.IsGuiRearviewMirrorEnabled = GUILayout.Toggle(VirtualMirror.IsGuiRearviewMirrorEnabled, "Rearview mirror");
                if (_prevRearviewMirrorEnabled != VirtualMirror.IsGuiRearviewMirrorEnabled)
                {
                    _prevRearviewMirrorEnabled = VirtualMirror.IsGuiRearviewMirrorEnabled;
                    _guiToggleUpdate = true;
                    VirtualMirror.REARVIEW_Mirror.SetActive(VirtualMirror.IsGuiRearviewMirrorEnabled);
                }
                GUILayout.FlexibleSpace();
                VirtualMirror.IsGuiRightMirrorEnabled = GUILayout.Toggle(VirtualMirror.IsGuiRightMirrorEnabled, "Right mirror");
                if (_prevRightMirrorEnabled != VirtualMirror.IsGuiRightMirrorEnabled)
                {
                    _prevRightMirrorEnabled = VirtualMirror.IsGuiRightMirrorEnabled;
                    _guiToggleUpdate = true;
                    VirtualMirror.RIGHTSIDE_Mirror.SetActive(VirtualMirror.IsGuiRightMirrorEnabled);
                }
                if (_guiToggleUpdate)
                {
                    if (VirtualMirror.LEFTSIDE_Mirror.activeSelf && VirtualMirror.RIGHTSIDE_Mirror.activeSelf && VirtualMirror.REARVIEW_Mirror.activeSelf)
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.Left + (int)VirtualMirror.MirrorsNum.Right + (int)VirtualMirror.MirrorsNum.Center;
                    }
                    else if (VirtualMirror.LEFTSIDE_Mirror.activeSelf && VirtualMirror.RIGHTSIDE_Mirror.activeSelf)
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.Left + (int)VirtualMirror.MirrorsNum.Right;
                    }
                    else if (VirtualMirror.LEFTSIDE_Mirror.activeSelf && VirtualMirror.REARVIEW_Mirror.activeSelf)
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.Left + (int)VirtualMirror.MirrorsNum.Center;
                    }
                    else if (VirtualMirror.RIGHTSIDE_Mirror.activeSelf && VirtualMirror.REARVIEW_Mirror.activeSelf)
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.Right + (int)VirtualMirror.MirrorsNum.Center;
                    }
                    else if (VirtualMirror.LEFTSIDE_Mirror.activeSelf)
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.Left;
                    }
                    else if (VirtualMirror.REARVIEW_Mirror.activeSelf)
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.Center;
                    }
                    else if (VirtualMirror.RIGHTSIDE_Mirror.activeSelf)
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.Right;
                    }
                    else
                    {
                        _currentCar.SwitchMirrorsNum = (int)VirtualMirror.MirrorsNum.None;
                    }

                    VirtualMirror.SetActive(_currentCar, 
                        VirtualMirror.REARVIEW_Mirror, 
                        VirtualMirror.RIGHTSIDE_Mirror,
                        VirtualMirror.LEFTSIDE_Mirror,
                        VirtualMirror.REARVIEW_Cam,
                        VirtualMirror.RIGHTSIDE_Cam,
                        VirtualMirror.LEFTSIDE_Cam);
                    _guiToggleUpdate = false;
                }
                GUILayout.FlexibleSpace();
            }
            #endregion

            GUILayout.Space(10);
            _tabInt = GUILayout.Toolbar(_tabInt, _tabName);
            if (_tabInt == 0)
            #region TAB Virtual Mirror Settings
            {
                GUILayout.Label("Virtual mirror position", labelStyle);
                #region Virtual mirroros position
                using (new GUILayout.HorizontalScope())
                {
                    #region Left side mirror
                    GUI.enabled = VirtualMirror.IsGuiLeftMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        _toggleRelativePosistion = GUILayout.Toggle(_toggleRelativePosistion, _textRelativePosition);
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateLeftVirtualMirror(_positionVerticalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateRightVirtualMirror(_positionVerticalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateLeftVirtualMirror(-_positionHorizontalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateRightVirtualMirror(_positionHorizontalFactor);
                                }
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                VirtualMirror.Settings.LeftVirtualMirrorPosition = VirtualMirror.DefaultLeftVirtualMirrorPosition;
                                VirtualMirror.LEFTSIDE_Mirror.transform.localPosition = VirtualMirror.Settings.LeftVirtualMirrorPosition;
                                if (_toggleRelativePosistion)
                                {
                                    VirtualMirror.Settings.RightVirtualMirrorPosition = VirtualMirror.DefaultRightVirtualMirrorPosition;
                                    VirtualMirror.RIGHTSIDE_Mirror.transform.localPosition = VirtualMirror.Settings.RightVirtualMirrorPosition;
                                }
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateLeftVirtualMirror(_positionHorizontalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateRightVirtualMirror(-_positionHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateLeftVirtualMirror(-_positionVerticalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateRightVirtualMirror(-_positionVerticalFactor);
                                }
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
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRearviewVirtualMirror(_positionVerticalFactor);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRearviewVirtualMirror(-_positionHorizontalFactor);
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                VirtualMirror.Settings.RearVirtualMirrorPosition = VirtualMirror.DefaultRearVirtualMirrorPosition;
                                VirtualMirror.REARVIEW_Mirror.transform.localPosition = VirtualMirror.Settings.RearVirtualMirrorPosition;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRearviewVirtualMirror(_positionHorizontalFactor);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRearviewVirtualMirror(-_positionVerticalFactor);
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
                        _toggleRelativePosistion = GUILayout.Toggle(_toggleRelativePosistion, _textRelativePosition);
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRightVirtualMirror(_positionVerticalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateLeftVirtualMirror(_positionVerticalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRightVirtualMirror(-_positionHorizontalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateLeftVirtualMirror(_positionHorizontalFactor);
                                }
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                VirtualMirror.Settings.RightVirtualMirrorPosition = VirtualMirror.DefaultRightVirtualMirrorPosition;
                                VirtualMirror.RIGHTSIDE_Mirror.transform.localPosition = VirtualMirror.Settings.RightVirtualMirrorPosition;
                                if (_toggleRelativePosistion)
                                {
                                    VirtualMirror.Settings.LeftVirtualMirrorPosition = VirtualMirror.DefaultLeftVirtualMirrorPosition;
                                    VirtualMirror.LEFTSIDE_Mirror.transform.localPosition = VirtualMirror.Settings.LeftVirtualMirrorPosition;
                                }
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRightVirtualMirror(_positionHorizontalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateLeftVirtualMirror(-_positionHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                TranslateRightVirtualMirror(-_positionVerticalFactor);
                                if (_toggleRelativePosistion)
                                {
                                    TranslateLeftVirtualMirror(-_positionVerticalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                }
                #endregion
                GUI.enabled = true;
                GUILayout.Label("Virtual mirror scalse", labelStyle);
                #region Virtual mirrors scale
                using (new GUILayout.HorizontalScope())
                {
                    #region Left side mirror
                    GUI.enabled = VirtualMirror.IsGuiLeftMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        _toggleRelativeScale = GUILayout.Toggle(_toggleRelativeScale, _textRelativePosition);
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleLeftVirtualMirror(_scaleVerticalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleRightVirtualMirror(_scaleVerticalFactor);
                                    if (_toggleRightPreserveAspectRatio)
                                    {
                                        ScaleRightVirtualMirror(_scaleHorizontalFactor);
                                    }
                                }
                                if (_toggleLeftPreserveAspectRatio)
                                {
                                    ScaleLeftVirtualMirror(_scaleHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleLeftVirtualMirror(-_scaleHorizontalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleRightVirtualMirror(-_scaleHorizontalFactor);
                                    if (_toggleRightPreserveAspectRatio)
                                    {
                                        ScaleRightVirtualMirror(-_scaleVerticalFactor);
                                    }
                                }
                                if (_toggleLeftPreserveAspectRatio)
                                {
                                    ScaleLeftVirtualMirror(-_scaleVerticalFactor);
                                }
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                VirtualMirror.Settings.LeftVirtualMirrorScale = VirtualMirror.DefaultSideVirtualMirrorsScale;
                                VirtualMirror.LEFTSIDE_Mirror.transform.localScale = VirtualMirror.Settings.LeftVirtualMirrorScale;
                                if (_toggleRelativeScale)
                                {
                                    VirtualMirror.Settings.RightVirtualMirrorScale = VirtualMirror.DefaultSideVirtualMirrorsScale;
                                    VirtualMirror.RIGHTSIDE_Mirror.transform.localScale = VirtualMirror.Settings.RightVirtualMirrorScale;
                                }
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleLeftVirtualMirror(_scaleHorizontalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleRightVirtualMirror(_scaleHorizontalFactor);
                                    if (_toggleRightPreserveAspectRatio)
                                    {
                                        ScaleRightVirtualMirror(_scaleVerticalFactor);
                                    }
                                }
                                if (_toggleLeftPreserveAspectRatio)
                                {
                                    ScaleLeftVirtualMirror(_scaleVerticalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleLeftVirtualMirror(-_scaleVerticalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleRightVirtualMirror(-_scaleVerticalFactor);
                                    if (_toggleRightPreserveAspectRatio)
                                    {
                                        ScaleRightVirtualMirror(-_scaleHorizontalFactor);
                                    }
                                }
                                if (_toggleLeftPreserveAspectRatio)
                                {
                                    ScaleLeftVirtualMirror(-_scaleHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            _toggleLeftPreserveAspectRatio = GUILayout.Toggle(_toggleLeftPreserveAspectRatio, _textPreserveAspectRatio);
                        }
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
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRearviewVirtualMirror(_scaleVerticalFactor);
                                if (_toggleRearPreserveAspectRatio)
                                {
                                    ScaleRearviewVirtualMirror(_scaleHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRearviewVirtualMirror(-_scaleHorizontalFactor);
                                if (_toggleRearPreserveAspectRatio)
                                {
                                    ScaleRearviewVirtualMirror(-_scaleVerticalFactor);
                                }
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                VirtualMirror.Settings.RearVirtualMirrorScale = VirtualMirror.DefaultRearVirtualMirrorScale;
                                VirtualMirror.REARVIEW_Mirror.transform.localScale = VirtualMirror.Settings.RearVirtualMirrorScale;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRearviewVirtualMirror(_scaleHorizontalFactor);
                                if (_toggleRearPreserveAspectRatio)
                                {
                                    ScaleRearviewVirtualMirror(_scaleVerticalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRearviewVirtualMirror(-_scaleVerticalFactor);
                                if (_toggleRearPreserveAspectRatio)
                                {
                                    ScaleRearviewVirtualMirror(-_scaleHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            _toggleRearPreserveAspectRatio = GUILayout.Toggle(_toggleRearPreserveAspectRatio, _textPreserveAspectRatio);
                        }
                    }
                    #endregion
                    #region Right side mirror
                    GUI.enabled = VirtualMirror.IsGuiRightMirrorEnabled;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        _toggleRelativeScale = GUILayout.Toggle(_toggleRelativeScale, _textRelativePosition);
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRightVirtualMirror(_scaleVerticalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleLeftVirtualMirror(_scaleVerticalFactor);
                                    if (_toggleLeftPreserveAspectRatio)
                                    {
                                        ScaleLeftVirtualMirror(_scaleHorizontalFactor);
                                    }
                                }
                                if (_toggleRightPreserveAspectRatio)
                                {
                                    ScaleRightVirtualMirror(_scaleHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRightVirtualMirror(-_scaleHorizontalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleLeftVirtualMirror(-_scaleHorizontalFactor);
                                    if (_toggleLeftPreserveAspectRatio)
                                    {
                                        ScaleLeftVirtualMirror(-_scaleVerticalFactor);
                                    }
                                }
                                if (_toggleRightPreserveAspectRatio)
                                {
                                    ScaleRightVirtualMirror(-_scaleVerticalFactor);
                                }
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                VirtualMirror.Settings.RightVirtualMirrorScale = VirtualMirror.DefaultSideVirtualMirrorsScale;
                                VirtualMirror.RIGHTSIDE_Mirror.transform.localScale = VirtualMirror.Settings.RightVirtualMirrorScale;
                                if (_toggleRelativeScale)
                                {
                                    VirtualMirror.Settings.LeftVirtualMirrorScale = VirtualMirror.DefaultSideVirtualMirrorsScale;
                                    VirtualMirror.LEFTSIDE_Mirror.transform.localScale = VirtualMirror.Settings.LeftVirtualMirrorScale;
                                }
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRightVirtualMirror(_scaleHorizontalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleLeftVirtualMirror(_scaleHorizontalFactor);
                                    if (_toggleLeftPreserveAspectRatio)
                                    {
                                        ScaleLeftVirtualMirror(_scaleVerticalFactor);
                                    }
                                }
                                if (_toggleRightPreserveAspectRatio)
                                {
                                    ScaleRightVirtualMirror(_scaleVerticalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                ScaleRightVirtualMirror(-_scaleVerticalFactor);
                                if (_toggleRelativeScale)
                                {
                                    ScaleLeftVirtualMirror(-_scaleVerticalFactor);
                                    if (_toggleLeftPreserveAspectRatio)
                                    {
                                        ScaleLeftVirtualMirror(-_scaleHorizontalFactor);
                                    }
                                }
                                if (_toggleRightPreserveAspectRatio)
                                {
                                    ScaleRightVirtualMirror(-_scaleHorizontalFactor);
                                }
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            _toggleRightPreserveAspectRatio = GUILayout.Toggle(_toggleRightPreserveAspectRatio, _textPreserveAspectRatio);
                        }
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            else if (_tabInt == 1)
            #region TAB Camera Settings
            {
                GUI.enabled = true;
                GUILayout.Label("Translate", labelStyle);

                #region TRANLATE
                using (new GUILayout.HorizontalScope())
                {
                    #region Left side mirror
                    GUI.enabled = VirtualMirror.LEFTSIDE_Cam.activeSelf;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // backword
                            {
                                _currentCar.LeftCam.LocalPosition = Translate(VirtualMirror.LEFTSIDE_Cam, -_translateFactor, 0, 0);
                            }
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                 _currentCar.LeftCam.LocalPosition = Translate(VirtualMirror.LEFTSIDE_Cam, 0, _translateFactor, 0, Space.World);
                            }
                            if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // forword
                            {
                                _currentCar.LeftCam.LocalPosition = Translate(VirtualMirror.LEFTSIDE_Cam, _translateFactor, 0, 0);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalPosition = Translate(VirtualMirror.LEFTSIDE_Cam, 0, -_translateFactor, 0);
                            }
                            if (GUILayout.Button("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                                VirtualMirror.LEFTSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalPosition;
                                _currentCar.LeftCam.LocalPosition = VirtualMirror.LEFTSIDE_Cam.transform.localPosition;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalPosition = Translate(VirtualMirror.LEFTSIDE_Cam, 0, _translateFactor, 0);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalPosition = Translate(VirtualMirror.LEFTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Rearview mirror
                    GUI.enabled = VirtualMirror.REARVIEW_Cam.activeSelf;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // backword
                            {
                                _currentCar.RearviewCam.LocalPosition = Translate(VirtualMirror.REARVIEW_Cam, 0, 0, -_translateFactor);
                            }
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalPosition = Translate(VirtualMirror.REARVIEW_Cam, 0, _translateFactor, 0, Space.World);
                            }
                            if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // forword
                            {
                                _currentCar.RearviewCam.LocalPosition = Translate(VirtualMirror.REARVIEW_Cam, 0, 0, _translateFactor);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalPosition = Translate(VirtualMirror.REARVIEW_Cam, -_translateFactor, 0, 0);
                            }
                            if (GUILayout.Button("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                                VirtualMirror.REARVIEW_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RearviewCam.LocalPosition;
                                _currentCar.RearviewCam.LocalPosition = VirtualMirror.REARVIEW_Cam.transform.localPosition;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalPosition = Translate(VirtualMirror.REARVIEW_Cam, _translateFactor, 0, 0);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalPosition = Translate(VirtualMirror.REARVIEW_Cam, 0, -_translateFactor, 0, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Right side mirror
                    GUI.enabled = VirtualMirror.RIGHTSIDE_Cam.activeSelf;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // backword
                            {
                                _currentCar.RightCam.LocalPosition = Translate(VirtualMirror.RIGHTSIDE_Cam, -_translateFactor, 0, 0);
                            }
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalPosition = Translate(VirtualMirror.RIGHTSIDE_Cam, 0, _translateFactor, 0, Space.World);
                            }
                            if (GUILayout.RepeatButton("|", GUILayout.MaxWidth(_buttonWidth))) // forword
                            {
                                _currentCar.RightCam.LocalPosition = Translate(VirtualMirror.RIGHTSIDE_Cam, _translateFactor, 0, 0);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalPosition = Translate(VirtualMirror.RIGHTSIDE_Cam, 0, -_translateFactor, 0);
                            }
                            if (GUILayout.Button("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                                VirtualMirror.RIGHTSIDE_Cam.transform.localPosition = revert.Cars.Find(x => x.Name == _currentCar.Name).RightCam.LocalPosition;
                                _currentCar.RightCam.LocalPosition = VirtualMirror.RIGHTSIDE_Cam.transform.localPosition;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalPosition = Translate(VirtualMirror.RIGHTSIDE_Cam, 0, _translateFactor, 0);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalPosition = Translate(VirtualMirror.RIGHTSIDE_Cam, 0, -_translateFactor, 0, Space.World);
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
                    GUI.enabled = VirtualMirror.LEFTSIDE_Cam.activeSelf;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalEulerAngles = Rotate(VirtualMirror.LEFTSIDE_Cam, VirtualMirror.LEFTSIDE_Cam.transform.parent.right, -_rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalEulerAngles = Rotate(VirtualMirror.LEFTSIDE_Cam, VirtualMirror.LEFTSIDE_Cam.transform.parent.up, _rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalEulerAngles = Rotate(VirtualMirror.LEFTSIDE_Cam, VirtualMirror.LEFTSIDE_Cam.transform.parent.right, _rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalEulerAngles = Rotate(VirtualMirror.LEFTSIDE_Cam, VirtualMirror.LEFTSIDE_Cam.transform.parent.forward, _rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                                VirtualMirror.LEFTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalEulerAngles;
                                _currentCar.LeftCam.LocalEulerAngles = VirtualMirror.LEFTSIDE_Cam.transform.localEulerAngles;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalEulerAngles = Rotate(VirtualMirror.LEFTSIDE_Cam, VirtualMirror.LEFTSIDE_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.LeftCam.LocalEulerAngles = Rotate(VirtualMirror.LEFTSIDE_Cam, VirtualMirror.LEFTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Rearview Mirror
                    GUI.enabled = VirtualMirror.REARVIEW_Cam.activeSelf;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalEulerAngles = Rotate(VirtualMirror.REARVIEW_Cam, VirtualMirror.REARVIEW_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalEulerAngles = Rotate(VirtualMirror.REARVIEW_Cam, VirtualMirror.REARVIEW_Cam.transform.parent.right, _rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalEulerAngles = Rotate(VirtualMirror.REARVIEW_Cam, VirtualMirror.REARVIEW_Cam.transform.parent.forward, _rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalEulerAngles = Rotate(VirtualMirror.REARVIEW_Cam, VirtualMirror.REARVIEW_Cam.transform.parent.up, _rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                                VirtualMirror.REARVIEW_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).RearviewCam.LocalEulerAngles;
                                _currentCar.RearviewCam.LocalEulerAngles = VirtualMirror.REARVIEW_Cam.transform.localEulerAngles;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalEulerAngles = Rotate(VirtualMirror.REARVIEW_Cam, VirtualMirror.REARVIEW_Cam.transform.parent.up, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RearviewCam.LocalEulerAngles = Rotate(VirtualMirror.REARVIEW_Cam, VirtualMirror.REARVIEW_Cam.transform.parent.right, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        GUILayout.FlexibleSpace();
                    }
                    #endregion
                    #region Right side mirror
                    GUI.enabled = VirtualMirror.RIGHTSIDE_Cam.activeSelf;
                    using (new GUILayout.VerticalScope("box"))
                    {
                        GUILayout.FlexibleSpace();
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("(", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalEulerAngles = Rotate(VirtualMirror.RIGHTSIDE_Cam, VirtualMirror.RIGHTSIDE_Cam.transform.parent.right, -_rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton("^", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalEulerAngles = Rotate(VirtualMirror.RIGHTSIDE_Cam, VirtualMirror.RIGHTSIDE_Cam.transform.parent.up, _rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton(")", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalEulerAngles = Rotate(VirtualMirror.RIGHTSIDE_Cam, VirtualMirror.RIGHTSIDE_Cam.transform.parent.right, _rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("<", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalEulerAngles = Rotate(VirtualMirror.RIGHTSIDE_Cam, VirtualMirror.RIGHTSIDE_Cam.transform.parent.forward, _rotationFactor, Space.World);
                            }
                            if (GUILayout.RepeatButton("O", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                var revert = VirtualMirror.CreateData(VirtualMirror.VERSION);
                                VirtualMirror.RIGHTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).RightCam.LocalEulerAngles;
                                _currentCar.RightCam.LocalEulerAngles = VirtualMirror.RIGHTSIDE_Cam.transform.localEulerAngles;
                            }
                            if (GUILayout.RepeatButton(">", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalEulerAngles = Rotate(VirtualMirror.RIGHTSIDE_Cam, VirtualMirror.RIGHTSIDE_Cam.transform.parent.forward, -_rotationFactor, Space.World);
                            }
                            GUILayout.FlexibleSpace();
                        }
                        using (new GUILayout.HorizontalScope())
                        {
                            GUILayout.FlexibleSpace();
                            if (GUILayout.RepeatButton("v", GUILayout.MaxWidth(_buttonWidth)))
                            {
                                _currentCar.RightCam.LocalEulerAngles = Rotate(VirtualMirror.RIGHTSIDE_Cam, VirtualMirror.RIGHTSIDE_Cam.transform.parent.up, -_rotationFactor, Space.World);
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
                    GUI.enabled = VirtualMirror.LEFTSIDE_Cam.activeSelf;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.LEFTSIDE_Cam.activeSelf)
                        {
                            VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane
                                =  float.Parse(GUILayout.TextField(VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane
                                    = GUILayout.HorizontalSlider(VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane, 0.1f, 10);
                            }
                            _currentCar.LeftCam.NearClipPlane = VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().nearClipPlane;
                        }
                        else
                        {
                            if (VirtualMirror.LEFTSIDE_Mirror.activeSelf)
                            {
                                GUILayout.TextField("stock", GUILayout.MaxWidth(40));
                            }
                            else
                            {
                                GUILayout.TextField("off", GUILayout.MaxWidth(40));
                            }
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                GUILayout.HorizontalSlider(1, 1, 100);
                            }
                        }
                    }
                    GUI.enabled = VirtualMirror.REARVIEW_Cam.activeSelf;
                    using (new GUILayout.HorizontalScope())
                    {
                        VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().nearClipPlane
                            = float.Parse(GUILayout.TextField(VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                        using (new GUILayout.VerticalScope())
                        {
                            GUILayout.Space(5);
                            VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().nearClipPlane
                                = GUILayout.HorizontalSlider(VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().nearClipPlane, 0.1f, 10);
                        }
                        _currentCar.RearviewCam.NearClipPlane = VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().nearClipPlane;
                    }
                    GUI.enabled = VirtualMirror.RIGHTSIDE_Cam.activeSelf;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.RIGHTSIDE_Cam.activeSelf)
                        {
                            VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane
                                = float.Parse(GUILayout.TextField(VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane
                                    = GUILayout.HorizontalSlider(VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane, 0.1f, 10);
                            }
                            _currentCar.RightCam.NearClipPlane = VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().nearClipPlane;
                        }
                        else
                        {
                            if (VirtualMirror.RIGHTSIDE_Mirror.activeSelf)
                            {
                                GUILayout.TextField("stock", GUILayout.MaxWidth(40));
                            }
                            else
                            {
                                GUILayout.TextField("off", GUILayout.MaxWidth(40));
                            }
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                GUILayout.HorizontalSlider(1, 1, 100);
                            }
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
                    GUI.enabled = VirtualMirror.LEFTSIDE_Cam.activeSelf;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.LEFTSIDE_Cam.activeSelf)
                        {
                            VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView
                                = float.Parse(GUILayout.TextField(VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView
                                    = GUILayout.HorizontalSlider(VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                            }
                            _currentCar.LeftCam.FieldOfView = VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().fieldOfView;
                        }
                        else
                        {
                            if (VirtualMirror.LEFTSIDE_Mirror.activeSelf)
                            {
                                GUILayout.TextField("stock", GUILayout.MaxWidth(40));
                            }
                            else
                            {
                                GUILayout.TextField("off", GUILayout.MaxWidth(40));
                            }
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                GUILayout.HorizontalSlider(1, 1, 100);
                            }
                        }
                    }
                    GUI.enabled = VirtualMirror.REARVIEW_Cam.activeSelf;
                    using (new GUILayout.HorizontalScope())
                    {
                        VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().fieldOfView
                            = float.Parse(GUILayout.TextField(VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                        using (new GUILayout.VerticalScope())
                        {
                            GUILayout.Space(5);
                            VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().fieldOfView
                                = GUILayout.HorizontalSlider(VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                        }
                        _currentCar.RearviewCam.FieldOfView = VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().fieldOfView;
                    }
                    GUI.enabled = VirtualMirror.RIGHTSIDE_Cam.activeSelf;
                    using (new GUILayout.HorizontalScope())
                    {
                        if (VirtualMirror.RIGHTSIDE_Cam.activeSelf)
                        {
                            VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView
                                = float.Parse(GUILayout.TextField(VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView.ToString(), 4, GUILayout.MaxWidth(40)));
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView
                                    = GUILayout.HorizontalSlider(VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView, 1, 100);
                            }
                            _currentCar.RightCam.FieldOfView = VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().fieldOfView;
                        }
                        else
                        {
                            if (VirtualMirror.RIGHTSIDE_Mirror.activeSelf)
                            {
                                GUILayout.TextField("stock", GUILayout.MaxWidth(40));
                            }
                            else
                            {
                                GUILayout.TextField("off", GUILayout.MaxWidth(40));
                            }
                            using (new GUILayout.VerticalScope())
                            {
                                GUILayout.Space(5);
                                GUILayout.HorizontalSlider(1, 1, 100);
                            }
                        }
                    }
                }
                #endregion
            }
            #endregion
            else if (_tabInt == 2)
            #region TAB Overall Settings
            {
                GUI.enabled = true;
                GUILayout.Label("Max draw distance", labelStyle);
                using (new GUILayout.HorizontalScope())
                {
                    if (VirtualMirror.LEFTSIDE_Cam.activeSelf ||
                        VirtualMirror.RIGHTSIDE_Cam.activeSelf ||
                        VirtualMirror.REARVIEW_Cam.activeSelf)
                    {
                        GUI.enabled = true;
                        VirtualMirror.Settings.FarClipPlane = int.Parse(GUILayout.TextField(VirtualMirror.Settings.FarClipPlane.ToString(), 4, GUILayout.MaxWidth(40)));
                        using (new GUILayout.VerticalScope())
                        {
                            GUILayout.Space(5);
                            VirtualMirror.Settings.FarClipPlane = (int)GUILayout.HorizontalSlider(VirtualMirror.Settings.FarClipPlane, 1, 2000);
                        }
                    }
                    else
                    {
                        GUI.enabled = false;
                        if (VirtualMirror.LEFTSIDE_Mirror.activeSelf && !VirtualMirror.LEFTSIDE_Cam.activeSelf ||
                            VirtualMirror.RIGHTSIDE_Mirror.activeSelf && !VirtualMirror.RIGHTSIDE_Cam.activeSelf)
                        {
                            GUILayout.TextField("stock", GUILayout.MaxWidth(40));
                        }
                        else
                        {
                            GUILayout.TextField("off", GUILayout.MaxWidth(40));
                        }
                        using (new GUILayout.VerticalScope())
                        {
                            GUILayout.Space(5);
                            GUILayout.HorizontalSlider(1, 1, 10);
                        }
                    }
                        VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().farClipPlane = VirtualMirror.Settings.FarClipPlane;
                        VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().farClipPlane = VirtualMirror.Settings.FarClipPlane;
                        VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().farClipPlane = VirtualMirror.Settings.FarClipPlane;
                }

                GUI.enabled = true;
                GUILayout.Label("Side mirror resolution", labelStyle);
                using (new GUILayout.HorizontalScope("box"))
                {
                    if (VirtualMirror.LEFTSIDE_Cam.activeSelf ||
                        VirtualMirror.RIGHTSIDE_Cam.activeSelf)
                    {
                        GUI.enabled = true;
                        VirtualMirror.Settings.SideMirrorsSelectionGrid = GUILayout.SelectionGrid(VirtualMirror.Settings.SideMirrorsSelectionGrid, _sideResolutionText, 4);
                        if (_prevSelectedSide != VirtualMirror.Settings.SideMirrorsSelectionGrid)
                        {
                            _prevSelectedSide = VirtualMirror.Settings.SideMirrorsSelectionGrid;
                            int width = 0;
                            if (VirtualMirror.Settings.SideMirrorsSelectionGrid == 0)
                            {
                                width = 64;
                            }
                            else if (VirtualMirror.Settings.SideMirrorsSelectionGrid == 1)
                            {
                                width = 128;
                            }
                            else if (VirtualMirror.Settings.SideMirrorsSelectionGrid == 2)
                            {
                                width = 256;
                            }
                            else
                            {
                                width = 512;
                            }

                            VirtualMirror.Settings.SideMirrorsRenderTextureWidth = VirtualMirror.Settings.SideMirrorsRenderTextureHeight = width;
                            if (VirtualMirror.IsLeftCamUsed)
                            {
                                VirtualMirror.LeftRenderTexture =
                                    new RenderTexture(VirtualMirror.Settings.SideMirrorsRenderTextureWidth, VirtualMirror.Settings.SideMirrorsRenderTextureHeight, VirtualMirror.Settings.RenderTextureDepth);
                                VirtualMirror.LEFTSIDE_Cam.GetComponent<Camera>().targetTexture = VirtualMirror.LeftRenderTexture;
                                VirtualMirror.LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = VirtualMirror.LeftRenderTexture;
                            }
                            if (VirtualMirror.IsRightCamUsed)
                            {
                                VirtualMirror.RightRenderTexture =
                                    new RenderTexture(VirtualMirror.Settings.SideMirrorsRenderTextureWidth, VirtualMirror.Settings.SideMirrorsRenderTextureHeight, VirtualMirror.Settings.RenderTextureDepth);
                                VirtualMirror.RIGHTSIDE_Cam.GetComponent<Camera>().targetTexture = VirtualMirror.RightRenderTexture;
                                VirtualMirror.RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = VirtualMirror.RightRenderTexture;
                            }
                        }
                    }
                    else                                                                                                                                                                                                                                                                            
                    {
                        GUI.enabled = false;
                        if (VirtualMirror.LEFTSIDE_Mirror.activeSelf && !VirtualMirror.LEFTSIDE_Cam.activeSelf ||
                            VirtualMirror.RIGHTSIDE_Mirror.activeSelf && !VirtualMirror.RIGHTSIDE_Cam.activeSelf)
                        {
                            using (new GUILayout.HorizontalScope())
                            {
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("stock");
                                GUILayout.FlexibleSpace();
                            }
                        }
                        else
                        {
                            GUILayout.SelectionGrid(VirtualMirror.Settings.SideMirrorsSelectionGrid, _sideResolutionText, 4);
                        }
                    }
                }

                GUI.enabled = true;
                GUILayout.Label("Rearview mirror resolution", labelStyle);
                using (new GUILayout.HorizontalScope("box"))
                {
                    if (VirtualMirror.REARVIEW_Cam.activeSelf)
                    {
                        GUI.enabled = true;
                        VirtualMirror.Settings.RearviewMirrorSelectionGrid = GUILayout.SelectionGrid(VirtualMirror.Settings.RearviewMirrorSelectionGrid, _rearviewResolutionText, 4);
                        if (_prevSelectedRear != VirtualMirror.Settings.RearviewMirrorSelectionGrid)
                        {
                            _prevSelectedRear = VirtualMirror.Settings.RearviewMirrorSelectionGrid;
                            int height = 0;
                            if (VirtualMirror.Settings.RearviewMirrorSelectionGrid == 0)
                            {
                                height = 64;
                            }
                            else if (VirtualMirror.Settings.RearviewMirrorSelectionGrid == 1)
                            {
                                height = 128;
                            }
                            else if (VirtualMirror.Settings.RearviewMirrorSelectionGrid == 2)
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
                            VirtualMirror.REARVIEW_Cam.GetComponent<Camera>().targetTexture = VirtualMirror.RearviewRenderTexture;
                            VirtualMirror.REARVIEW_Mirror.GetComponent<MeshRenderer>().material.mainTexture = VirtualMirror.RearviewRenderTexture;
                        }
                    }
                    else
                    {
                        GUI.enabled = false;
                        GUILayout.SelectionGrid(VirtualMirror.Settings.RearviewMirrorSelectionGrid, _rearviewResolutionText, 4);
                    }
                }
            }
            #endregion

            GUI.enabled = true;
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Close"))
            {
                VirtualMirror.IsGuiActive = false;
                VirtualMirror.IsPlayerInMenu.Value = false;
            }

            GUI.DragWindow();
        }

        private void TranslateLeftVirtualMirror( Vector2 position)
        {
            VirtualMirror.Settings.LeftVirtualMirrorPosition += position;
            VirtualMirror.LEFTSIDE_Mirror.transform.localPosition = VirtualMirror.Settings.LeftVirtualMirrorPosition;
        }
        
        private void TranslateRightVirtualMirror(Vector2 position)
        {
            VirtualMirror.Settings.RightVirtualMirrorPosition += position;
            VirtualMirror.RIGHTSIDE_Mirror.transform.localPosition = VirtualMirror.Settings.RightVirtualMirrorPosition;
        }

        private void TranslateRearviewVirtualMirror(Vector2 position)
        {
            VirtualMirror.Settings.RearVirtualMirrorPosition += position;
            VirtualMirror.REARVIEW_Mirror.transform.localPosition = VirtualMirror.Settings.RearVirtualMirrorPosition;
        }

        private void ScaleLeftVirtualMirror(Vector2 scale)
        {
            VirtualMirror.Settings.LeftVirtualMirrorScale += scale;
            VirtualMirror.LEFTSIDE_Mirror.transform.localScale = VirtualMirror.Settings.LeftVirtualMirrorScale;
        }

        private void ScaleRightVirtualMirror(Vector2 scale)
        {
            VirtualMirror.Settings.RightVirtualMirrorScale += scale;
            VirtualMirror.RIGHTSIDE_Mirror.transform.localScale = VirtualMirror.Settings.RightVirtualMirrorScale;
        }

        private void ScaleRearviewVirtualMirror(Vector2 scale)
        {
            VirtualMirror.Settings.RearVirtualMirrorScale += scale;
            VirtualMirror.REARVIEW_Mirror.transform.localScale = VirtualMirror.Settings.RearVirtualMirrorScale;
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
