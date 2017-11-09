using HutongGames.PlayMaker;
using MSCLoader;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace VirtualMirror
{
    public class VirtualMirror : Mod
    {
        public override string ID => "VirtualMirror";
        public override string Name => "VirtualMirror";
        public override string Author => "PigeonBB";
        public override string Version => "0.1";

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        public Keybind virtualMirrorKey = new Keybind("virtualMirror", "Virtual Mirror", KeyCode.F10);
        public Keybind toggleMenuKey = new Keybind("toggleMenu", "Toggle Menu", KeyCode.F12, KeyCode.LeftControl);
        private GameObject _fpsfpsCamera;
        private GameObject _player;
        private GameObject _CAM_VERTICAL;
        private GameObject _GUI;
        private FsmBool _playerInMenu;
        private bool _guiActive;
#if DEBUG
        private GameObject test;
#endif
        Rect rect = new Rect((Screen.width / 2) - (500 / 2), (Screen.height / 2) - (500 / 2), 500, 500);

        private GameObject _fpsCamera;
        private GameObject RIGHTSIDE_Mirror;
        private GameObject REARVIEW_Mirror;
        private GameObject LEFTSIDE_Mirror;
        private GameObject RIGHTSIDE_Cam;
        public GameObject REARVIEW_Cam;
        private GameObject LEFTSIDE_Cam;
        private RenderTexture _rightMirrorTargetTexture;
        private RenderTexture _rearviewTargetTexture;
        private RenderTexture _leftMirrorTargetTexture;
        private const String SETTINGS_FILE_NAME = "settings.xml";
        // car names using with GlobalVariables.FindFsmString("PlayerCurrentVehicle") returned value
        private const String GIFU = "Gifu";
        private const String VAN = "Hayosiko";
        private const String SATSUMA = "Satsuma";
        private const String MUSCLE = "Ferndale";
        private const String KEKMET = "Kekmet";
        private const String BOAT = "Boat";
        private const String MOPED = "Jonnez ES";
        private const String RUSCKO = "Ruscko";
        // car object names using with GameObject.Find
        //private const String OBJ_GIFU = "GIFU(750/450psi)";
        //private const String OBJ_VAN = "HAYOSIKO(1500kg, 250)";
        //private const String OBJ_SATSUMA = "SATSUMA(557kg, 248)";
        //private const String OBJ_MUSCLE = "FERNDALE(1630kg)";
        //private const String OBJ_KEKMET = "KEKMET(350-400psi)";
        //private const String OBJ_BOAT = "BOAT";
        //private const String OBJ_MOPED = "JONNEZ ES(Clone)";
        //private const String OBJ_RUSCKO = "RCO_RUSCKO12(270)";

        private bool _isInit = true;
        private bool _isPlayerBoarded = true;
        //private int _switchMirrorsValue = 0;
        //private int _farClipPlane = 100;
        private Settings _settings;
        private GameObject _optionMenu;
        private Vector3 _newPos = Vector3.zero;
        private bool _buttonPressed;
        private RenderTexture _rightRenderTexture;
        private RenderTexture _leftRenderTexture;
        private bool _rearviewMirrorActive;
        private bool _rightMirrorActive;
        private bool _leftMirrorActive;
        private Cars _currentCar;
        private GameObject _clockData;
        private GameObject _TEXT;

        private enum SwitchMirrorsNum
        {
            None,
            Right,
            RightCenter,
            All,
            Center,
            RightLeft
        }

        //Called when mod is loading
        public override void OnLoad()
        {
            Keybind.Add(this, virtualMirrorKey);
            Keybind.Add(this, toggleMenuKey);

            _settings = CreateData();
            // load settings from file.
            if (File.Exists(Path.Combine(ModLoader.GetModConfigFolder(this), SETTINGS_FILE_NAME)))
            {
                using (FileStream fs = new FileStream(Path.Combine(ModLoader.GetModConfigFolder(this), SETTINGS_FILE_NAME), FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    _settings = (Settings)serializer.Deserialize(fs);
                }
            }
        }

        //----
        // OnGui
        //----
        public override void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 200, 200), "angles: "+ _GUI.transform.localEulerAngles);
            GUI.Label(new Rect(0, 20, 200, 200), "scale: " + _GUI.transform.localScale);
            GUI.Label(new Rect(0, 40, 200, 200), "local pos: " + _GUI.transform.localPosition);
            //rect = new Rect((Screen.width / 2) - (500 / 2), (Screen.height / 2) - (500 / 2), 500, 500);
            if (_rightMirrorActive)
            {
                float height = Screen.height * 1 / 7;
                float width = height * 1.5f;
                float padding = 10;
                //GUI.backgroundColor = Color.clear;
                GUILayout.BeginArea(new Rect(Screen.width - (width + padding), 0, width + padding, height + padding));
                GUILayout.BeginVertical("box");
                GUILayout.FlexibleSpace();
                GUI.DrawTextureWithTexCoords(
                    new Rect(padding / 2, padding / 2, width, height),
                    _rightRenderTexture,
                    new Rect(1, 0, -1, 1)
                    );
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            if (_rearviewMirrorActive)
            {
                float height = Screen.height * 1 / 7;
                float width = height * 4f;
                float padding = 10;
                //GUI.backgroundColor = Color.clear;
                GUILayout.BeginArea(new Rect((Screen.width / 2) - (width / 2), 0, width + padding, height + padding));
                GUILayout.BeginVertical("box");
                GUILayout.FlexibleSpace();
                GUI.DrawTextureWithTexCoords(
                    new Rect(padding / 2, padding / 2, width, height),
                    _rearviewTargetTexture,
                    new Rect(1, 0, -1, 1)
                    );
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            if (_leftMirrorActive)
            {
                float height = Screen.height * 1 / 7;
                float width = height * 1.5f;
                float padding = 10;
                //GUI.backgroundColor = Color.clear; 
                GUILayout.BeginArea(new Rect(0, 0, width + padding, height + padding));
                GUILayout.BeginVertical("box");
                GUILayout.FlexibleSpace();
                GUI.DrawTextureWithTexCoords(
                    new Rect(padding / 2, padding / 2, width, height),
                    _leftRenderTexture,
                    new Rect(1, 0, -1, 1)
                    );
                GUILayout.EndVertical();
                GUILayout.EndArea();
            }
            if (_guiActive)
            {
                rect = GUI.Window(0, rect, DoWindow, "Mirror Mod Option Menu");
            }
            else
            {
                rect = new Rect((Screen.width / 2) - (500 / 2), (Screen.height / 2) - (500 / 2), 500, 500);
            }
        }

        void DoWindow(int id)
        {
            float buttom_width = 40;
            float area_width = 160;
            float area_height = 100;
            float padding = 10;
            #region Translate
            #region Left side mirror
            GUILayout.BeginArea(new Rect(padding, 20, area_width, area_height));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("|", GUILayout.MaxWidth(buttom_width))) // backword
                        {
                            //LEFTSIDE_Cam.transform.Translate(0.025f, 0, 0, LEFTSIDE_Cam.transform.parent);
                            //_currentCar.LeftCam.LocalPosition = LEFTSIDE_Cam.transform.localPosition;
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, -0.025f, 0, 0);
                        }
                        if (GUILayout.Button("^", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Translate(0, 0.025f, 0, Space.World);
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, 0.025f, 0, Space.World);
                        }
                        if (GUILayout.Button("|", GUILayout.MaxWidth(buttom_width))) // forword
                        {
                            //LEFTSIDE_Cam.transform.Translate(-0.025f, 0, 0, LEFTSIDE_Cam.transform.parent);
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0.025f, 0, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if ( GUILayout.Button("<", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Translate(0, -0.025f, 0, LEFTSIDE_Cam.transform.parent);
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -0.025f, 0);
                        }
                        GUILayout.Button("o", GUILayout.MaxWidth(buttom_width));
                        if (GUILayout.Button(">", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Translate(0, 0.025f, 0, LEFTSIDE_Cam.transform.parent);
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, 0.025f, 0);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("v", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Translate(0, -0.025f, 0, Space.World);
                            _currentCar.LeftCam.LocalPosition = Translate(LEFTSIDE_Cam, 0, -0.025f, 0, Space.World);
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
            GUILayout.BeginArea(new Rect(area_width + padding, 20, area_width, area_height));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Button("^", GUILayout.MaxWidth(buttom_width));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Button("<", GUILayout.MaxWidth(buttom_width));
                        GUILayout.Button("O", GUILayout.MaxWidth(buttom_width));
                        GUILayout.Button(">", GUILayout.MaxWidth(buttom_width));
                        GUILayout.FlexibleSpace();

                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Button("v", GUILayout.MaxWidth(buttom_width));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            #endregion
            #region Right side mirror
            GUILayout.BeginArea(new Rect(area_width * 2 + padding, 20, area_width, area_height));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Button("^", GUILayout.MaxWidth(buttom_width));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Button("<", GUILayout.MaxWidth(buttom_width));
                        GUILayout.Button("O", GUILayout.MaxWidth(buttom_width));
                        GUILayout.Button(">", GUILayout.MaxWidth(buttom_width));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        GUILayout.Button("v", GUILayout.MaxWidth(buttom_width));
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            #endregion
            #endregion
            #region Rotation
            #region Left side mirror
            GUILayout.BeginArea(new Rect(padding, area_height + padding, area_width, area_height));
            {
                GUILayout.BeginVertical("box");
                {
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("(", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Rotate(GameObject.Find(_currentCar.Name).transform.forward, -1);
                            //_currentCar.LeftCam.LocalEulerAngles = LEFTSIDE_Cam.transform.localEulerAngles;
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.forward, -1);
                        }
                        if (GUILayout.Button("^", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Rotate(GameObject.Find(_currentCar.Name).transform.right, -1, Space.World);
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.right, -1/*, Space.World*/);
                        }
                        if (GUILayout.Button(")", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Rotate(GameObject.Find(_currentCar.Name).transform.forward, 1);
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.forward, 1);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("<", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Rotate(GameObject.Find(_currentCar.Name).transform.up, 1, Space.World);
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.up, 1, Space.World);
                        }
                        if (GUILayout.Button("O", GUILayout.MaxWidth(buttom_width)))
                        {
                            var revert = CreateData();
                            ModConsole.Print("currentCar: "+_currentCar.LeftCam.LocalEulerAngles);
                            ModConsole.Print("revert: "+ revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalEulerAngles);
                            LEFTSIDE_Cam.transform.localEulerAngles = revert.Cars.Find(x => x.Name == _currentCar.Name).LeftCam.LocalEulerAngles;
                            _currentCar.LeftCam.LocalEulerAngles = LEFTSIDE_Cam.transform.localEulerAngles;
                        }
                        if (GUILayout.Button(">", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Rotate(GameObject.Find(_currentCar.Name).transform.up, -1, Space.World);
                            _currentCar.LeftCam.LocalEulerAngles = Rotate(LEFTSIDE_Cam, GameObject.Find(_currentCar.Name).transform.up, -1, Space.World);
                        }
                        GUILayout.FlexibleSpace();
                    }
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    {
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("v", GUILayout.MaxWidth(buttom_width)))
                        {
                            //LEFTSIDE_Cam.transform.Rotate(GameObject.Find(_currentCar.Name).transform.right, 1, Space.World);
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

        //----
        // Update is called once per frame
        //----
        public override void Update()
        {
            if (Application.loadedLevelName != "GAME") return;
            if (_isInit)
            {
                if (!Initialize()) return;
                _isInit = false;
            }

            // TODO: 車に乗ってる間アクティブにするようにすること
            #region keydown
            if (toggleMenuKey.IsDown())
            {
                if (_guiActive)
                {
                    _guiActive = false;
                    _playerInMenu.Value = false;
                }
                else
                {
                    _playerInMenu.Value = true;
                    _guiActive = true;
                }
            }
            #endregion

            var cars = FsmVariables.GlobalVariables.FindFsmString("PlayerCurrentVehicle").Value;
            // activate once if player is boarded a car
            if (cars.Length != 0 && _isPlayerBoarded ||
                cars.Length != 0 && virtualMirrorKey.IsDown() 
                //||
                //_buttonPressed
                )
            {
                //Cars car_list;
                
                string car_obj = GameObject.Find("PLAYER").transform.root.name; // If Player is driving mode, we can take car object via "PLAYER" root object.
                switch (cars)
                {
                    case GIFU:
                        _currentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(_currentCar);
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: _currentCar.RearviewCam
                            );
                        _rightRenderTexture = GameObject.Find("RightSideMirrorCam").GetComponent<Camera>().targetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(_currentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam);
                        break;
                    case VAN:
                        _currentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(_currentCar);
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: _currentCar.RearviewCam
                            );
                        _rightRenderTexture = GameObject.Find("RightSideMirrorCam").GetComponent<Camera>().targetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(_currentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam);
                        break;
                    case SATSUMA:
                        _currentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(_currentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/Body/pivot_door_right/door right(Clone)").transform,
                            cam_settings: _currentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj + "/CarRearMirrorPivot").transform,
                            cam_settings: _currentCar.RearviewCam
                            );
                        _rightRenderTexture = _rightMirrorTargetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(_currentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam);
                        break;
                    case KEKMET:
                        _currentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(_currentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors 1/doorr/door(right)").transform,
                            cam_settings: _currentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: _currentCar.RearviewCam
                            );
                        SetCameraPositon(
                            camera: LEFTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors 1/doorl/door(leftx)").transform,
                            cam_settings: _currentCar.LeftCam
                            );
                        _rightRenderTexture = _rightMirrorTargetTexture;
                        _leftRenderTexture = _leftMirrorTargetTexture;
                        SetActive(_currentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam, LEFTSIDE_Cam);
                        break;
                    case MUSCLE:
                        _currentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(_currentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors/door(right)").transform,
                            cam_settings: _currentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: _currentCar.RearviewCam
                            );
                        _rightRenderTexture = _rightMirrorTargetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        //RIGHTSIDE_Mirror.GetComponent<Renderer>().material.mainTexture = _rightMirrorTargetTexture;
                        //LEFTSIDE_Mirror.GetComponent<Renderer>().material.mainTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(_currentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam);
                        break;
                    case BOAT:
                        break;
                    case MOPED:
                        break;
                    case RUSCKO:
                        _currentCar = _settings.Cars.Find(x => x.Name == cars);
                        //if (_buttonPressed)
                        //{
                        //    car_list.LeftCam.LocalPosition += _newPos;
                        //    ModConsole.Print("in ruscko: " + car_list.LeftCam.LocalPosition);
                        //    _buttonPressed = false;
                        //}
                        SwitchMirrors(_currentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors/doorr").transform,
                            cam_settings: _currentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: _currentCar.RearviewCam
                            );
                        SetCameraPositon(
                            camera: LEFTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors/doorl").transform,
                            cam_settings: _currentCar.LeftCam
                            );
                        //LEFTSIDE_Mirror.GetComponent<Renderer>().material.mainTexture = _leftMirrorTargetTexture;
                        //LEFTSIDE_Mirror.SetActive(true); 
                        _rightRenderTexture = _rightMirrorTargetTexture; // <- これいる？
                        _leftRenderTexture = _leftMirrorTargetTexture;
                        SetActive(_currentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam, LEFTSIDE_Cam);
                        break;
                    default:
#if DEBUG
                        foreach (var fsm in GameObject.Find("PLAYER").GetComponents<PlayMakerFSM>())
                        {
                            if (fsm.name == "PLAYER" && fsm.FsmVariables.FindFsmBool("PlayerInCar") != null)
                            {
                                //ModConsole.Print(fsm.FsmVariables.FindFsmBool("PlayerInCar").Value);
                                break;
                            }
                        }
#endif
                        break;
                }
                _isPlayerBoarded = false;
            }
            // save settings if player unboard.
            else if (cars.Length == 0 && !_isPlayerBoarded)
            {
#if DEBUG
                //SetDeactiveAll();
#else
                SetDeactiveAll();
#endif

                using (FileStream fs = new FileStream(Path.Combine(ModLoader.GetModConfigFolder(this), SETTINGS_FILE_NAME), FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                    ns.Add(String.Empty, String.Empty);
                    serializer.Serialize(fs, _settings, ns);
                }

                _isPlayerBoarded = true;
            }
        }

        private void SwitchMirrors(Cars car_list)
        {
            if (virtualMirrorKey.IsDown())
            {
                car_list.SwitchMirrorsNum++;
                if (car_list.SwitchMirrorsNum > (int)SwitchMirrorsNum.RightLeft)
                {
                    car_list.SwitchMirrorsNum = (int)SwitchMirrorsNum.None;
                }
            }
        }

        private bool Initialize()
        {
            _player = GameObject.Find("PLAYER");
            //if (!_fpsCamera) return false;
            if (!_player) return false;
            _fpsCamera = GameObject.Find("FPSCamera");
            _fpsfpsCamera = GameObject.Find("FPSCamera/FPSCamera");
            _CAM_VERTICAL = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera");
            _playerInMenu = FsmVariables.GlobalVariables.FindFsmBool("PlayerInMenu");
            foreach (var r in Resources.FindObjectsOfTypeAll<GameObject>())
            {
                if (r.name == "OptionsMenu")
                {
                    _optionMenu = r;
                    break;
                }
            }

            //_CAM_HORIZONTAL = GameObject.Find("PLAYER");
            //_CAM_VERTICAL = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera");

#if DEBUG
            test = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            test.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            test.transform.localEulerAngles = Vector3.zero;
            test.GetComponent<Collider>().enabled = false;
#endif
            Vector3 sidemirror_scale = new Vector3(0.012f * 1.5f, 0.012f, 0.0001f);
            Vector3 rearview_scale = new Vector3(0.01f * 4f, 0.01f, 0.0001f);
            Vector3 local_scale = new Vector3(0.012f * 1.5f, 0.012f, 0.0001f);
            Vector3 local_position = new Vector3(0f, 0.034f, 0.1f);
            // right-side mirror for Truck & Van
            float res = (float)Screen.width / (float)Screen.height;
            if (Math.Abs(res - (16f / 9f)) < 0.1f)
            {
                //ModConsole.Print("16:9");
                local_position += new Vector3(0.063f, 0);
            }
            else if (Math.Abs(res - (16f / 10f)) < 0.1f)
            {
                //ModConsole.Print("16:10");
                local_position += new Vector3(0.056f, 0);
            }
            else if (Math.Abs(res - (4f / 3f)) < 0.1f)
            {
                //ModConsole.Print("4:3");
                local_position += new Vector3(0.046f, 0);
            }
            else if (Math.Abs(res - (3f / 2f)) < 0.1f)
            {
                //ModConsole.Print("3:2");
                local_position += new Vector3(0.052f, 0);
            }
            else
            {
                //ModConsole.Print("other");
                /*RIGHTSIDE_Mirror.transform.localPosition = new Vector3(0.1f, 0.034f, 0.1f);
                var view_point =  Camera.main.WorldToViewportPoint(RIGHTSIDE_Mirror.transform.position);
                while (view_point.x > 0.8f)
                {
                    RIGHTSIDE_Mirror.transform.localPosition -= new Vector3(0.05f, 0);
                    view_point = Camera.main.WorldToViewportPoint(RIGHTSIDE_Mirror.transform.position);
                }*/
                local_position += new Vector3(0.04f, 0);
            }

            //RIGHTSIDE_Mirror = CreateVirtualMirror("RIGHTSIDE_Mirror", local_position, local_scale);
            //LEFTSIDE_Mirror = CreateVirtualMirror("LEFTSIDE_Mirror", local_position, local_scale, true);

            _rightMirrorTargetTexture = new RenderTexture(
                _settings.SideMirrorsRenderTextureWidth,
                _settings.SideMirrorsRenderTextureHeight,
                _settings.RenderTextureDepth
                );
            RIGHTSIDE_Cam = CreateVitualMirrorCam("RIGHTSIDE_Cam", _rightMirrorTargetTexture);

            _leftMirrorTargetTexture = new RenderTexture(
                _settings.SideMirrorsRenderTextureWidth,
                _settings.SideMirrorsRenderTextureHeight,
                _settings.RenderTextureDepth
                );
            LEFTSIDE_Cam = CreateVitualMirrorCam("LEFTSIDE_Cam", _leftMirrorTargetTexture);

            _rearviewTargetTexture = new RenderTexture(
                _settings.RearviewMirrorsRenderTextureWidth,
                _settings.RearviewMirrorsRenderTextureHeight,
                _settings.RenderTextureDepth
                );
            REARVIEW_Cam = CreateVitualMirrorCam("REARVIEW_Cam", _rearviewTargetTexture);

            //local_position = new Vector3(0, 0.035f, 0.1f); // local position on screen
            //local_scale = new Vector3(0.01f * 4f, 0.01f, 0.0001f);
            //REARVIEW_Mirror = CreateVirtualMirror("REARVIEW_Mirror", local_position, local_scale);
            //REARVIEW_Mirror.GetComponent<Renderer>().material.mainTexture = _rearviewTargetTexture;  // Rearview has using always this renderer textrue.

            try
            {
                _GUI = GameObject.Instantiate<GameObject>(GameObject.Find("GUI/HUD/Day"));
                _clockData = _GUI.transform.Find("BarBG").gameObject;
                _TEXT = _GUI.transform.Find("Data").gameObject;
                GameObject.Destroy(_clockData.GetComponent("PlayMakerFSM"));
                _clockData.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Texture"));
                _clockData.GetComponent<MeshRenderer>().material.mainTexture = _leftMirrorTargetTexture;
                _TEXT.SetActive(false);
                _GUI.transform.parent = GameObject.Find("GUI").transform;
                _GUI.transform.name = "LeftVirtualMirror";
                _GUI.transform.localPosition = new Vector3(-14f, 8f, 0); // -12でGUIのはじっこ
                _GUI.transform.localScale = new Vector3(5, 5*2f);
            }
            catch (Exception e)
            {
                ModConsole.Error(e.ToString());
            }
            return true;
        }

        private GameObject CreateVitualMirrorCam(String name, RenderTexture targetTexture)
        {
            GameObject obj = new GameObject(name);
            obj.transform.localEulerAngles = Vector3.zero;
            obj.AddComponent<Camera>();
            obj.GetComponent<Camera>().targetTexture = targetTexture;
            obj.GetComponent<Camera>().fieldOfView = 20; // default fov
            obj.GetComponent<Camera>().cullingMask = -181598775; // magick number for don't draw player-layer object.
            obj.SetActive(false);
            return obj;
        }

        private GameObject CreateVirtualMirror(String name, Vector3 localPosition, Vector3 scale, bool is_leftside = false)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube);
            obj.transform.parent = Camera.main.transform;
            obj.transform.localEulerAngles = Vector3.zero;
            obj.GetComponent<Collider>().enabled = false;
            obj.GetComponent<Renderer>().material = new Material(Shader.Find("Unlit/Texture"));
            obj.name = name;
            obj.layer = _fpsCamera.layer;
            //obj.layer = _player.layer;
            //obj.layer = GameObject.Find("GUI").layer;
            obj.transform.localScale = new Vector3(scale.x, -scale.y, scale.z);
            if (!is_leftside)
            {
                obj.transform.localPosition = localPosition;
            }
            else
            {
                obj.transform.localPosition = new Vector3(-localPosition.x, localPosition.y, localPosition.z);
            }
            obj.SetActive(false);
            return obj;
        }

        private void SetCameraPositon(GameObject camera, Transform parent, Cam cam_settings/*, Vector3 local_position, Vector3 local_euler_angle, float near_clip_plane, float field_of_view = 20*/)
        {
#if DEBUG
            test.transform.parent = parent;
            test.transform.localPosition = cam_settings.LocalPosition;
            test.transform.localEulerAngles = cam_settings.LocalEulerAngles;
#endif
            camera.transform.parent = parent;
            camera.transform.localPosition = cam_settings.LocalPosition;
            //camera.transform.TransformPoint(cam_settings.LocalPosition);
            //camera.transform.localPosition = camera.transform.InverseTransformPoint(camera.transform.TransformPoint(cam_settings.LocalPosition));
            //camera.transform.localPosition = local_position;
            camera.transform.localEulerAngles = cam_settings.LocalEulerAngles;
            //camera.transform.localEulerAngles = local_euler_angle;
            camera.GetComponent<Camera>().nearClipPlane = cam_settings.NearClipPlane;
            //camera.GetComponent<Camera>().nearClipPlane = near_clip_plane;
            camera.GetComponent<Camera>().farClipPlane = _settings.FarClipPlane;
            camera.GetComponent<Camera>().fieldOfView = cam_settings.FieldOfView;
        }

        /**********************
            SwitchMirrors.None;
            SwitchMirrors.Right;
            SwitchMirrors.RightCenter;
            SwitchMirrors.All;
            SwitchMirrors.Center;
            SwitchMirrors.RightLeft;
        **********************/
        private void SetActive(int mirror_num, out bool rearview_mirror_active, out bool right_mirror_active, out bool left_mirror_active, GameObject rearview_cam, GameObject right_cam = null, GameObject left_cam = null)
        {
            // rearview
            if (mirror_num == (int)SwitchMirrorsNum.RightCenter ||
                mirror_num == (int)SwitchMirrorsNum.All ||
                mirror_num == (int)SwitchMirrorsNum.Center)
            {
                rearview_mirror_active = true;
                //rearview_mirror.SetActive(true);
                rearview_cam.SetActive(true);
            }
            else
            {
                rearview_mirror_active = false;
                //rearview_mirror.SetActive(false);
                rearview_cam.SetActive(false);
            }
            // rightside
            if (mirror_num == (int)SwitchMirrorsNum.Right ||
                mirror_num == (int)SwitchMirrorsNum.RightCenter ||
                mirror_num == (int)SwitchMirrorsNum.RightLeft ||
                mirror_num == (int)SwitchMirrorsNum.All)
            {
                right_mirror_active= true;
                //right_mirror.SetActive(true);
                if (right_cam != null)
                    right_cam.SetActive(true);
            }
            else
            {
                right_mirror_active = false;
                //right_mirror.SetActive(false);
                if (right_cam != null)
                    right_cam.SetActive(false);
            }
            // leftside
            if (mirror_num == (int)SwitchMirrorsNum.All ||
                mirror_num == (int)SwitchMirrorsNum.RightLeft)
            {
                left_mirror_active = true;
                if (left_cam != null)
                    left_cam.SetActive(true);
            }
            else
            {
                left_mirror_active = false;
                if (left_cam != null)
                    left_cam.SetActive(false);
            }
        }

        private void SetDeactiveAll()
        {
            _rightMirrorActive = false;
            _leftMirrorActive = false;
            _rearviewMirrorActive = false;
            RIGHTSIDE_Cam.SetActive(false);
            REARVIEW_Cam.SetActive(false);
            LEFTSIDE_Cam.SetActive(false);
            //RIGHTSIDE_Mirror.SetActive(false);
            //REARVIEW_Mirror.SetActive(false);
            //LEFTSIDE_Mirror.SetActive(false);
        }

        private Settings CreateData()
        {
            Settings settings = new Settings { Cars = new List<Cars>() };

            settings.Version = Version;
            settings.FarClipPlane = 100;
            settings.RenderTextureDepth = 16;
            settings.SideMirrorsRenderTextureWidth = 256;
            settings.SideMirrorsRenderTextureHeight = 256+16;
            settings.RearviewMirrorsRenderTextureWidth = 1024;
            settings.RearviewMirrorsRenderTextureHeight = 256;

            Cars this_car = new Cars
            {
                Name = GIFU,
                SwitchMirrorsNum = (int)SwitchMirrorsNum.Right,
                RearviewCam = new Cam
                {
                    LocalPosition = new Vector3(0, 2.2f, 0f),
                    LocalEulerAngles = new Vector3(0, 180),
                    NearClipPlane = 3.3f,
                    FieldOfView = 20
                }
            };
            settings.Cars.Add(this_car);

            this_car = new Cars
            {
                Name = VAN,
                SwitchMirrorsNum = (int)SwitchMirrorsNum.Right,
                RearviewCam = new Cam
                {
                    LocalPosition = new Vector3(0, 1.3f, 1.4f),
                    LocalEulerAngles = new Vector3(0, 180),
                    NearClipPlane = 3.6f,
                    FieldOfView = 20
                }
            };
            settings.Cars.Add(this_car);

            this_car = new Cars
            {
                Name = SATSUMA,
                SwitchMirrorsNum = (int)SwitchMirrorsNum.Right,
                RightCam = new Cam
                {
                    LocalPosition = new Vector3(-0.1f, 0.0f, 0.2f),  // -x,z,y
                    LocalEulerAngles = new Vector3(90, 80, 90), // y,z,
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                },
                RearviewCam = new Cam
                {
                    LocalPosition = Vector3.zero,
                    LocalEulerAngles = Vector3.zero,
                    NearClipPlane = 1.9f,
                    FieldOfView = 20
                }
            };
            settings.Cars.Add(this_car);

            this_car = new Cars
            {
                Name = KEKMET,
                SwitchMirrorsNum = (int)SwitchMirrorsNum.Right,
                RightCam = new Cam
                {
                    LocalPosition = new Vector3(0.1f, 0.6f, 0.2f), // z,x,y
                    LocalEulerAngles = new Vector3(180, 75, 90), // y,x,z
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                },
                RearviewCam = new Cam
                {
                    LocalPosition = new Vector3(0f, 1.7f, 0), // ,y,
                    LocalEulerAngles = new Vector3(5, 180, 0), // ,y,
                    NearClipPlane = 0.6f,
                    FieldOfView = 20
                },
                LeftCam = new Cam
                {
                    LocalPosition = new Vector3(0.1f, -0.6f, 0.2f), // z,-x,y
                    LocalEulerAngles = new Vector3(180, 75, 90), // y,x,z
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                }
            };
            settings.Cars.Add(this_car);

            this_car = new Cars
            {
                Name = MUSCLE,
                SwitchMirrorsNum = (int)SwitchMirrorsNum.Right,
                RightCam = new Cam
                {
                    LocalPosition = new Vector3(0.2f, 0.1f, 0.4f),
                    LocalEulerAngles = new Vector3(182, 90, 90),
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                },
                RearviewCam = new Cam
                {
                    LocalPosition = new Vector3(0, 1f, 0.3f),
                    LocalEulerAngles = new Vector3(0, 180),
                    NearClipPlane = 2.5f,
                    FieldOfView = 10
                }
            };
            settings.Cars.Add(this_car);

            this_car = new Cars
            {
                Name = RUSCKO,
                SwitchMirrorsNum = (int)SwitchMirrorsNum.Right,
                RightCam = new Cam
                {
                    LocalPosition = new Vector3(0, 0.3f, 0),
                    LocalEulerAngles = new Vector3(0, 0, 0),
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                },
                RearviewCam = new Cam
                {
                    LocalPosition = new Vector3(0, 0, 0),
                    LocalEulerAngles = new Vector3(0, 0, 0),
                    NearClipPlane = 1.9f,
                    FieldOfView = 20
                },
                LeftCam = new Cam
                {
                    LocalPosition = new Vector3(0.8f, 0, 0.4f), // z,x,y
                    LocalEulerAngles = new Vector3(6, 262, 270), // ,,y
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                }
            };
            settings.Cars.Add(this_car);
            return settings;
        }
    }

}
