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
        public const string VERSION = "0.1";

        public override string ID => "VirtualMirror";
        public override string Name => "VirtualMirror";
        public override string Author => "PigeonBB";
        public override string Version => VERSION;

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        public Keybind virtualMirrorKey = new Keybind("virtualMirror", "Virtual Mirror", KeyCode.F10);
        public Keybind toggleMenuKey = new Keybind("toggleMenu", "Toggle Menu", KeyCode.F12, KeyCode.LeftControl);

        private GameObject _player;
        private FsmBool _playerInMenu;
        private bool _guiActive;
#if DEBUG
        private GameObject test;
#endif
        Rect rect = new Rect((Screen.width / 2) - (500 / 2), (Screen.height / 2) - (500 / 2), 500, 500);

        public static GameObject RIGHTSIDE_Cam;
        public static GameObject REARVIEW_Cam;
        public static GameObject LEFTSIDE_Cam;

        private  GameObject RIGHTSIDE_Mirror;
        private  GameObject REARVIEW_Mirror;
        private GameObject LEFTSIDE_Mirror;
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
        private Vector3 _newPos = Vector3.zero;
        private RenderTexture _rightRenderTexture;
        private RenderTexture _leftRenderTexture;
        private bool _rearviewMirrorActive;
        private bool _rightMirrorActive;
        private bool _leftMirrorActive;
        public static Cars CurrentCar { get; set; }

        private enum SwitchMirrorsNum
        {
            None,
            Right,
            RightCenter,
            All,
            Center,
            RightLeft
        }

        /// <summary>
        /// Called when mod is loading
        /// </summary>
        public override void OnLoad()
        {
            Keybind.Add(this, virtualMirrorKey);
            Keybind.Add(this, toggleMenuKey);

            _settings = CreateData(Version);
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

        /// <summary>
        ///  OnGui
        /// </summary>
        public override void OnGUI()
        {
            if (_guiActive)
            {
                GUI.backgroundColor = Color.gray;
                GUI.skin.window.fontStyle = FontStyle.Bold;
                rect = GUI.Window(0, rect, DoWindow, "MIRROR MOD OPTION MENU");
            }
            else
            {
                // Reset window position, if GUI is closed.
                rect = new Rect((Screen.width / 2) - (500 / 2), (Screen.height / 2) - (500 / 2), 500, 500);
            }
        }

        // Change virtual mirror camera position
        private static void DoWindow(int id)
        {
            SettingsGUI gui = new SettingsGUI();
            gui.DoWindow();
        }



        /// <summary>
        /// Update is called once per frame
        /// </summary>
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
                        CurrentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(CurrentCar);
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        _rightRenderTexture = GameObject.Find("RightSideMirrorCam").GetComponent<Camera>().targetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam);
                        break;
                    case VAN:
                        CurrentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(CurrentCar);
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        _rightRenderTexture = GameObject.Find("RightSideMirrorCam").GetComponent<Camera>().targetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam);
                        break;
                    case SATSUMA:
                        CurrentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(CurrentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/Body/pivot_door_right/door right(Clone)").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj + "/CarRearMirrorPivot").transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        _rightRenderTexture = _rightMirrorTargetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam);
                        break;
                    case KEKMET:
                        CurrentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(CurrentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors 1/doorr/door(right)").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        SetCameraPositon(
                            camera: LEFTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors 1/doorl/door(leftx)").transform,
                            cam_settings: CurrentCar.LeftCam
                            );
                        _rightRenderTexture = _rightMirrorTargetTexture;
                        _leftRenderTexture = _leftMirrorTargetTexture;
                        SetActive(CurrentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam, LEFTSIDE_Cam);
                        break;
                    case MUSCLE:
                        CurrentCar = _settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrors(CurrentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors/door(right)").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        _rightRenderTexture = _rightMirrorTargetTexture;
                        _leftRenderTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        //RIGHTSIDE_Mirror.GetComponent<Renderer>().material.mainTexture = _rightMirrorTargetTexture;
                        //LEFTSIDE_Mirror.GetComponent<Renderer>().material.mainTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam);
                        break;
                    case BOAT:
                        break;
                    case MOPED:
                        break;
                    case RUSCKO:
                        CurrentCar = _settings.Cars.Find(x => x.Name == cars);
                        //if (_buttonPressed)
                        //{
                        //    car_list.LeftCam.LocalPosition += _newPos;
                        //    ModConsole.Print("in ruscko: " + car_list.LeftCam.LocalPosition);
                        //    _buttonPressed = false;
                        //}
                        SwitchMirrors(CurrentCar);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors/doorr").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        SetCameraPositon(
                            camera: LEFTSIDE_Cam,
                            parent: GameObject.Find(car_obj + "/DriverDoors/doorl").transform,
                            cam_settings: CurrentCar.LeftCam
                            );
                        //LEFTSIDE_Mirror.GetComponent<Renderer>().material.mainTexture = _leftMirrorTargetTexture;
                        //LEFTSIDE_Mirror.SetActive(true);
                        _rightRenderTexture = _rightMirrorTargetTexture; // <- これいる？
                        _leftRenderTexture = _leftMirrorTargetTexture;
                        SetActive(CurrentCar.SwitchMirrorsNum, out _rearviewMirrorActive, out _rightMirrorActive, out _leftMirrorActive, REARVIEW_Cam, RIGHTSIDE_Cam, LEFTSIDE_Cam);
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
            try
            {
                _player = GameObject.Find("PLAYER");
                if (!_player) return false;
                _playerInMenu = FsmVariables.GlobalVariables.FindFsmBool("PlayerInMenu");
                //_fpsCamera = GameObject.Find("FPSCamera");
                //_fpsfpsCamera = GameObject.Find("FPSCamera/FPSCamera");
                //_CAM_VERTICAL = GameObject.Find("PLAYER/Pivot/Camera/FPSCamera");
                //foreach (var r in Resources.FindObjectsOfTypeAll<GameObject>())
                //{
                //    if (r.name == "OptionsMenu")
                //    {
                //        _optionMenu = r;
                //        break;
                //    }
                //}

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

                Vector3 rearview_scale = new Vector3(10, 3, 0);
                Vector3 sidemirror_scale = new Vector3(6, 4, 0);
                Vector3 rearview_mirror_position = new Vector3(0, 8.8f, 0);
                Vector3 right_mirror_position = new Vector3(14.5f, 8.3f, 0);
                Vector3 left_mirror_position = new Vector3(-14.5f, 8.3f, 0);
                // right-side mirror for Truck & Van
                float res = (float)Screen.width / (float)Screen.height;
                if (Math.Abs(res - (16f / 9f)) < 0.1f)
                {
                    //ModConsole.Print("16:9");
                    // default
                }
                else if (Math.Abs(res - (16f / 10f)) < 0.1f)
                {
                    //ModConsole.Print("16:10");
                    right_mirror_position = new Vector3(14.5f, 8.3f, 0);
                    left_mirror_position = new Vector3(-13.8f, 8.3f, 0);
                }
                else if (Math.Abs(res - (4f / 3f)) < 0.1f)
                {
                    //ModConsole.Print("4:3");
                    right_mirror_position = new Vector3(14.5f, 8.3f, 0);
                    left_mirror_position = new Vector3(-12, 8.3f, 0);
                }
                else if (Math.Abs(res - (3f / 2f)) < 0.1f)
                {
                    //ModConsole.Print("3:2");
                    right_mirror_position = new Vector3(14.5f, 8.3f, 0);
                    left_mirror_position = new Vector3(-12, 8.3f, 0);
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
                    right_mirror_position = new Vector3(14.5f, 8.3f, 0);
                    left_mirror_position = new Vector3(12, 8.5f, 0);
                }

                RIGHTSIDE_Mirror = CreateVirtualMirror(
                    "RightVirtualMirror",
                    right_mirror_position,
                    sidemirror_scale,
                    _rightMirrorTargetTexture
                    );
                LEFTSIDE_Mirror = CreateVirtualMirror(
                    "LeftVirtualMirror",
                    left_mirror_position,
                    sidemirror_scale,
                    _leftMirrorTargetTexture
                    );
                REARVIEW_Mirror = CreateVirtualMirror(
                    "RearviewVirtualMirror",
                    rearview_mirror_position,
                    rearview_scale,
                    _rearviewTargetTexture);
#if DEBUG
                test = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                test.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                test.transform.localEulerAngles = Vector3.zero;
                test.GetComponent<Collider>().enabled = false;
#endif
            }
            catch (Exception e)
            {
                ModConsole.Error(e.ToString());
            }
            return true;
        }

        private static Mesh MeshFlip(Mesh mesh)
        {
            var uvs = mesh.uv;
            for (var i = 0; i < uvs.Length; i++)
            {
                if (Mathf.Approximately(uvs[i].x, 1.0f))
                    uvs[i].x = 0.0f;
                else
                    uvs[i].x = 1.0f;
            }
            mesh.uv = uvs;
            return mesh;
        }

        private static GameObject CreateVitualMirrorCam(String name, RenderTexture targetTexture)
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

        private static GameObject CreateVirtualMirror(String name, Vector3 position, Vector3 scale, RenderTexture texture)
        {
            /* hierarchy: GUI/HUD/Day/BarBG
             *                                     |--/Data */
            var hud = GameObject.Instantiate<GameObject>(GameObject.Find("GUI/HUD"));
            hud.transform.localPosition = Vector3.zero;
            hud.transform.localScale = Vector3.zero;
            var day = hud.transform.Find("Day").gameObject;
            day.transform.localPosition = Vector3.zero;
            day.transform.localScale = Vector3.zero;
            GameObject.Destroy(day.transform.Find("Data").gameObject);
            var bg = day.transform.Find("BarBG").gameObject;
            bg.name = name;
            MeshFlip(bg.GetComponent<MeshFilter>().mesh);
            bg.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Texture"));
            bg.GetComponent<MeshRenderer>().material.mainTexture = texture;
            bg.transform.parent = GameObject.Find("GUI").transform;
            bg.transform.localScale = scale;
            bg.transform.localPosition = position;
#if DEBUG
            /* obj.SetActive(false); */
#else
            obj.SetActive(false);
#endif
            return bg;
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
        private static void SetActive(int mirror_num, out bool rearview_mirror_active, out bool right_mirror_active, out bool left_mirror_active, GameObject rearview_cam, GameObject right_cam = null, GameObject left_cam = null)
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
                right_mirror_active = true;
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
        
        /// <summary>
        /// Create default mirror settings
        /// </summary>
        /// <param name="version">mod version</param>
        /// <returns>default settings</returns>
        public static Settings CreateData(string version)
        {
            Settings settings = new Settings { Cars = new List<Cars>() };

            settings.Version = version;
            settings.FarClipPlane = 100;
            settings.RenderTextureDepth = 16;
            settings.SideMirrorsRenderTextureWidth = 256;
            settings.SideMirrorsRenderTextureHeight = 256 + 16;
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
