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
        public const string VERSION = "0.2";

        public override string ID => "VirtualMirror";
        public override string Name => "VirtualMirror";
        public override string Author => "PigeonBB";
        public override string Version => VERSION;

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        public Keybind virtualMirrorKey = new Keybind("virtualMirror", "Virtual Mirror", KeyCode.F10);
        public Keybind toggleMenuKey = new Keybind("toggleMenu", "Toggle Menu", KeyCode.F10, KeyCode.LeftControl);

        // public valiables
        public static GameObject LEFTSIDE_Cam;
        public static GameObject REARVIEW_Cam;
        public static GameObject RIGHTSIDE_Cam;
        public static GameObject LEFTSIDE_Mirror;
        public static GameObject REARVIEW_Mirror;
        public static GameObject RIGHTSIDE_Mirror;
        public static RenderTexture LeftRenderTexture;
        public static RenderTexture RightRenderTexture;
        public static RenderTexture RearviewRenderTexture;
        public static Vector2 DefaultRearVirtualMirrorScale;
        public static Vector2 DefaultSideVirtualMirrorsScale;
        public static Vector2 DefaultLeftVirtualMirrorPosition;
        public static Vector2 DefaultRearVirtualMirrorPosition;
        public static Vector2 DefaultRightVirtualMirrorPosition;
        public static FsmBool IsPlayerInMenu;
        public static bool IsGuiActive;
        public static bool IsLeftCamUsed;
        public static bool IsRightCamUsed;
        public static bool IsGuiLeftMirrorEnabled;
        public static bool IsGuiRightMirrorEnabled;
        public static bool IsGuiRearviewMirrorEnabled;

        public static Cars CurrentCar;
        public static Settings Settings;

        // private valiables
        private const String SETTINGS_FILE_NAME = "settings.xml";
        private Rect _defaultGUIWindowRect = new Rect((Screen.width / 2) - (500 / 2), (Screen.height / 2) - (500 / 2) + 50, 500, 450);
        private Rect _GUIWindowRect;
        private GameObject _player;
        private bool _isInit = true;
        private bool _isPlayerBoarded = true;
        
        // car names using with GlobalVariables.FindFsmString("PlayerCurrentVehicle") returned value
        private const String BUS = "Bus";
        private const String VAN = "Hayosiko";
        private const String GIFU = "Gifu";
        private const String BOAT = "Boat";
        private const String MOPED = "Jonnez";
        private const String KEKMET = "Kekmet";
        private const String MUSCLE = "Ferndale";
        private const String RUSCKO = "Ruscko";
        private const String SATSUMA = "Satsuma";
        private const String GREENCAR = "Fittan";

        // car object names using with GameObject.Find
        //private const String OBJ_GIFU = "GIFU(750/450psi)";
        //private const String OBJ_VAN = "HAYOSIKO(1500kg, 250)";
        //private const String OBJ_SATSUMA = "SATSUMA(557kg, 248)";
        //private const String OBJ_MUSCLE = "FERNDALE(1630kg)";
        //private const String OBJ_KEKMET = "KEKMET(350-400psi)";
        //private const String OBJ_BOAT = "BOAT";
        //private const String OBJ_MOPED = "JONNEZ ES(Clone)";
        //private const String OBJ_RUSCKO = "RCO_RUSCKO12(270)";

        public enum MirrorsNum
        {
            None = 0,
            Right = 1,
            Center = 2,
            Left = 4
        }

#if DEBUG
        private GameObject test;
#endif

        /// <summary>
        /// Called when mod is loading
        /// </summary>
        public override void OnLoad()
        {
            Keybind.Add(this, virtualMirrorKey);
            Keybind.Add(this, toggleMenuKey);

            Settings = CreateData(this.Version);
            _GUIWindowRect = _defaultGUIWindowRect;
            // load settings from file.
            if (File.Exists(Path.Combine(ModLoader.GetModConfigFolder(this), SETTINGS_FILE_NAME)))
            {
                using (FileStream fs = new FileStream(Path.Combine(ModLoader.GetModConfigFolder(this), SETTINGS_FILE_NAME), FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Settings));
                    Settings = (Settings)serializer.Deserialize(fs);
                }
            }
        }

        /// <summary>
        ///  OnGui
        /// </summary>
        public override void OnGUI()
        {
            if (IsGuiActive)
            {
                GUI.backgroundColor = Color.gray;
                GUI.skin.window.fontStyle = FontStyle.Bold;
                _GUIWindowRect = GUI.Window(0, _GUIWindowRect, DoWindow, "VIRTUAL MIRROR OPTION MENU");
            }
            else
            {
                _GUIWindowRect = _defaultGUIWindowRect;  // Reset window position, if GUI is closed.
            }
        }

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

            var cars = FsmVariables.GlobalVariables.FindFsmString("PlayerCurrentVehicle").Value;
            if (toggleMenuKey.IsDown() && cars.Length != 0)
            {
                if (toggleMenuKey.Key == virtualMirrorKey.Key)
                {
                    if (virtualMirrorKey.Modifier != KeyCode.None &&
                    Input.GetKey(virtualMirrorKey.Modifier))
                    {
                        goto RESUEM;
                    }
                }

                if (IsGuiActive)
                {
                    IsGuiActive = false;
                    IsPlayerInMenu.Value = false;
                }
                else
                {
                    IsGuiActive = true;
                    IsPlayerInMenu.Value = true;
                }
            }
            RESUEM:

            // activate once if player is boarded a car
            if (cars.Length != 0 && _isPlayerBoarded ||
                cars.Length != 0 && virtualMirrorKey.IsDown() )
            {
                string car_obj_name = GameObject.Find("PLAYER").transform.root.name; // If Player is driving mode, we can take car object via "PLAYER" root object.
                switch (cars)
                {
                    case GIFU:
                        CurrentCar = Settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrorsNum(CurrentCar, virtualMirrorKey, toggleMenuKey);
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj_name).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("RightSideMirrorCam").GetComponent<Camera>().targetTexture;
                        LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar, REARVIEW_Mirror, RIGHTSIDE_Mirror, LEFTSIDE_Mirror, REARVIEW_Cam);
                        break;
                    case VAN:
                        CurrentCar = Settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrorsNum(CurrentCar, virtualMirrorKey, toggleMenuKey);
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj_name).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("RightSideMirrorCam").GetComponent<Camera>().targetTexture;
                        LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar, REARVIEW_Mirror, RIGHTSIDE_Mirror, LEFTSIDE_Mirror, REARVIEW_Cam);
                        break;
                    case SATSUMA:
                        CurrentCar = Settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrorsNum(CurrentCar, virtualMirrorKey, toggleMenuKey);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/Body/pivot_door_right/door right(Clone)").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj_name + "/CarRearMirrorPivot").transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = RightRenderTexture;
                        LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar, REARVIEW_Mirror, RIGHTSIDE_Mirror, LEFTSIDE_Mirror, REARVIEW_Cam, RIGHTSIDE_Cam);
                        break;
                    case MUSCLE:
                        CurrentCar = Settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrorsNum(CurrentCar, virtualMirrorKey, toggleMenuKey);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/DriverDoors/door(right)").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj_name).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = RightRenderTexture;
                        LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = GameObject.Find("LeftSideMirrorCam").GetComponent<Camera>().targetTexture;
                        SetActive(CurrentCar, REARVIEW_Mirror, RIGHTSIDE_Mirror, LEFTSIDE_Mirror, REARVIEW_Cam, RIGHTSIDE_Cam);
                        break;
                    case KEKMET:
                        CurrentCar = Settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrorsNum(CurrentCar, virtualMirrorKey, toggleMenuKey);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/DriverDoors 1/doorr/door(right)").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj_name).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        SetCameraPositon(
                            camera: LEFTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/DriverDoors 1/doorl/door(leftx)").transform,
                            cam_settings: CurrentCar.LeftCam
                            );
                        RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = RightRenderTexture;
                        LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = LeftRenderTexture;
                        SetActive(CurrentCar, REARVIEW_Mirror, RIGHTSIDE_Mirror, LEFTSIDE_Mirror, REARVIEW_Cam, RIGHTSIDE_Cam, LEFTSIDE_Cam);
                        break;
                    case RUSCKO:
                        CurrentCar = Settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrorsNum(CurrentCar, virtualMirrorKey, toggleMenuKey);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/DriverDoors/doorr").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj_name).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        SetCameraPositon(
                            camera: LEFTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/DriverDoors/doorl").transform,
                            cam_settings: CurrentCar.LeftCam
                            );
                        RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = RightRenderTexture;
                        LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = LeftRenderTexture;
                        SetActive(CurrentCar, REARVIEW_Mirror, RIGHTSIDE_Mirror, LEFTSIDE_Mirror, REARVIEW_Cam, RIGHTSIDE_Cam, LEFTSIDE_Cam);
                        break;
                    case MOPED:
                        CurrentCar = Settings.Cars.Find(x => x.Name == cars);
                        SwitchMirrorsNum(CurrentCar, virtualMirrorKey, toggleMenuKey);
                        SetCameraPositon(
                            camera: RIGHTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/LOD/Suspension").transform,
                            cam_settings: CurrentCar.RightCam
                            );
                        SetCameraPositon(
                            camera: REARVIEW_Cam,
                            parent: GameObject.Find(car_obj_name).transform,
                            cam_settings: CurrentCar.RearviewCam
                            );
                        SetCameraPositon(
                            camera: LEFTSIDE_Cam,
                            parent: GameObject.Find(car_obj_name + "/LOD/Suspension").transform,
                            cam_settings: CurrentCar.LeftCam
                            );
                        RIGHTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = RightRenderTexture;
                        LEFTSIDE_Mirror.GetComponent<MeshRenderer>().material.mainTexture = LeftRenderTexture;
                        SetActive(CurrentCar, REARVIEW_Mirror, RIGHTSIDE_Mirror, LEFTSIDE_Mirror, REARVIEW_Cam, RIGHTSIDE_Cam, LEFTSIDE_Cam);
                        break;
                    case BOAT:
                    case BUS:
                    case GREENCAR:
                        break;
                    default:
                        ModConsole.Error(string.Format("\n{0}: An unknown vehicle was detected. Please let me know (Author: {1}) :o\nVehicle name: {2}", this.Name, this.Author, cars ));
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
                        serializer.Serialize(fs, Settings, ns);
                }

                IsGuiActive = false;
                IsPlayerInMenu.Value = false;
                _isPlayerBoarded = true;
            }
        }

        private static void SwitchMirrorsNum(Cars car_list, Keybind virtualMirrorKey, Keybind toggleMenuKey)
        {
            if (virtualMirrorKey.IsDown())
            {
                if (toggleMenuKey.Key == virtualMirrorKey.Key)
                {
                    if (toggleMenuKey.Modifier != KeyCode.None &&
                    Input.GetKey(toggleMenuKey.Modifier))
                    {
                        return;
                    }
                }

                car_list.SwitchMirrorsNum++;
                if (car_list.SwitchMirrorsNum > ((int)MirrorsNum.Right + (int)MirrorsNum.Center + (int)MirrorsNum.Left))
                {
                    car_list.SwitchMirrorsNum = (int)MirrorsNum.None;
                }
            }
        }

        private static void SetCameraPositon(GameObject camera, Transform parent, Cam cam_settings/*, Vector3 local_position, Vector3 local_euler_angle, float near_clip_plane, float field_of_view = 20*/)
        {
#if DEBUG
            test.transform.parent = parent;
            test.transform.localPosition = cam_settings.LocalPosition;
            test.transform.localEulerAngles = cam_settings.LocalEulerAngles;
#endif
            camera.transform.parent = parent;
            camera.transform.localPosition = cam_settings.LocalPosition;
            camera.transform.localEulerAngles = cam_settings.LocalEulerAngles;
            camera.GetComponent<Camera>().nearClipPlane = cam_settings.NearClipPlane;
            camera.GetComponent<Camera>().fieldOfView = cam_settings.FieldOfView;
        }

        /**********************
            SwitchMirrors.None;
            SwitchMirrors.Right;
            SwitchMirrors.RightCenter;
            SwitchMirrors.All;
            SwitchMirrors.LeftCenter;
            SwitchMirrors.Left;
            SwitchMirrors.Center;
            SwitchMirrors.RightLeft;
        **********************/
        public static void SetActive(Cars currentCar, GameObject rearview_mirror, GameObject right_mirror, GameObject left_mirror, GameObject rearview_cam, GameObject right_cam = null, GameObject left_cam = null)
        {
            // rearview
            if (currentCar.SwitchMirrorsNum == (int)MirrorsNum.Center ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Center + (int)MirrorsNum.Left||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Center + (int)MirrorsNum.Right ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Center + (int)MirrorsNum.Right + (int)MirrorsNum.Left)
            {
                rearview_cam.SetActive(true);
                rearview_mirror.SetActive(true);
                IsGuiRearviewMirrorEnabled = true;
            }
            else
            {
                rearview_cam.SetActive(false);
                rearview_mirror.SetActive(false);
                IsGuiRearviewMirrorEnabled = false;
            }
            // rightside
            if (currentCar.SwitchMirrorsNum == (int)MirrorsNum.Right ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Right + (int)MirrorsNum.Left ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Right + (int)MirrorsNum.Center ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Right + (int)MirrorsNum.Center + (int)MirrorsNum.Left)
            {
                switch (currentCar.Name)
                {
                    case GIFU:
                    case VAN:
                        right_mirror.SetActive(true);
                        IsRightCamUsed = false;
                        break;
                    case SATSUMA:
                    case MUSCLE:
                    case KEKMET:
                    case RUSCKO:
                    case MOPED:
                        right_cam.SetActive(true);
                        right_mirror.SetActive(true);
                        IsRightCamUsed = true;
                        break;
                }
                IsGuiRightMirrorEnabled = true;
            }
            else
            {
                switch (currentCar.Name)
                {
                    case VAN:
                    case GIFU:
                        right_mirror.SetActive(false);
                        break;
                    case MUSCLE:
                    case SATSUMA:
                    case KEKMET:
                    case RUSCKO:
                    case MOPED:
                        right_cam.SetActive(false);
                        right_mirror.SetActive(false);
                        break;
                }
                IsGuiRightMirrorEnabled = false;
            }
            // leftside
            if (currentCar.SwitchMirrorsNum == (int)MirrorsNum.Left ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Left + (int)MirrorsNum.Right ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Left + (int)MirrorsNum.Center ||
                currentCar.SwitchMirrorsNum == (int)MirrorsNum.Left + (int)MirrorsNum.Center + (int)MirrorsNum.Right)
            {
                switch (currentCar.Name)
                {
                    case VAN:
                    case GIFU:
                    case MUSCLE:
                    case SATSUMA:
                        left_mirror.SetActive(true);
                        IsLeftCamUsed = false;
                        break;
                    case KEKMET:
                    case RUSCKO:
                    case MOPED:
                        left_cam.SetActive(true);
                        left_mirror.SetActive(true);
                        IsLeftCamUsed = true;
                        break;
                }
                IsGuiLeftMirrorEnabled = true;
            }
            else
            {
                switch (currentCar.Name)
                {
                    case VAN:
                    case GIFU:
                    case MUSCLE:
                    case SATSUMA:
                        left_mirror.SetActive(false);
                        break;
                    case KEKMET:
                    case RUSCKO:
                    case MOPED:
                        left_cam.SetActive(false);
                        left_mirror.SetActive(false);
                        break;
                }
                IsGuiLeftMirrorEnabled = false;
            }
        }

        private void SetDeactiveAll()
        {
            LEFTSIDE_Cam.SetActive(false);
            REARVIEW_Cam.SetActive(false);
            RIGHTSIDE_Cam.SetActive(false);
            LEFTSIDE_Mirror.SetActive(false);
            RIGHTSIDE_Mirror.SetActive(false);
            REARVIEW_Mirror.SetActive(false);
            IsGuiLeftMirrorEnabled = false;
            IsGuiRightMirrorEnabled = false;
            IsGuiRearviewMirrorEnabled = false;
            IsRightCamUsed = false;
            IsLeftCamUsed = false;
        }

        private bool Initialize()
        {
            try
            {
                _player = GameObject.Find("PLAYER");
                if (!_player) return false;
                IsPlayerInMenu = FsmVariables.GlobalVariables.FindFsmBool("PlayerInMenu");

                // camera setting
                RightRenderTexture = new RenderTexture(
                    Settings.SideMirrorsRenderTextureWidth,
                    Settings.SideMirrorsRenderTextureHeight,
                    Settings.RenderTextureDepth
                    );
                RIGHTSIDE_Cam = CreateVitualMirrorCam("RIGHTSIDE_Cam", RightRenderTexture, Settings.FarClipPlane);

                LeftRenderTexture = new RenderTexture(
                    Settings.SideMirrorsRenderTextureWidth,
                    Settings.SideMirrorsRenderTextureHeight,
                    Settings.RenderTextureDepth
                    );
                LEFTSIDE_Cam = CreateVitualMirrorCam("LEFTSIDE_Cam", LeftRenderTexture, Settings.FarClipPlane);

                RearviewRenderTexture = new RenderTexture(
                    Settings.RearviewMirrorRenderTextureWidth,
                    Settings.RearviewMirrorRenderTextureHeight,
                    Settings.RenderTextureDepth
                    );
                REARVIEW_Cam = CreateVitualMirrorCam("REARVIEW_Cam", RearviewRenderTexture, Settings.FarClipPlane);

                // virtual mirror setting
                DefaultSideVirtualMirrorsScale = new Vector2(5, 3);
                DefaultRearVirtualMirrorScale = new Vector2(10, 3);
                Settings.LeftVirtualMirrorScale = DefaultSideVirtualMirrorsScale;
                Settings.RearVirtualMirrorScale = DefaultRearVirtualMirrorScale;
                Settings.RightVirtualMirrorScale = DefaultSideVirtualMirrorsScale;
                DefaultRearVirtualMirrorPosition = new Vector2(0, 9); // Rearview mirror is not dependence by resolution
                Settings.RearVirtualMirrorPosition = DefaultRearVirtualMirrorPosition;
                float res = (float)Screen.width / (float)Screen.height;
                if (Math.Abs(res - (16f / 9f)) < 0.1f)
                {
                    DefaultLeftVirtualMirrorPosition = new Vector2(-15.2f, 9);
                    DefaultRightVirtualMirrorPosition = new Vector2(15.2f, 9);
                    if (Settings.ScreenResolution != res)
                    {
                        Settings.ScreenResolution = res;
                        Settings.LeftVirtualMirrorPosition = DefaultLeftVirtualMirrorPosition;
                        Settings.RightVirtualMirrorPosition = DefaultRightVirtualMirrorPosition;
                    }

                }
                else if (Math.Abs(res - (16f / 10f)) < 0.1f)
                {
                    DefaultLeftVirtualMirrorPosition = new Vector2(-13.5f, 9);
                    DefaultRightVirtualMirrorPosition = new Vector2(13.5f, 9);
                    if (Settings.ScreenResolution != res)
                    {
                        Settings.ScreenResolution = res;
                        Settings.LeftVirtualMirrorPosition = DefaultLeftVirtualMirrorPosition;
                        Settings.RightVirtualMirrorPosition = DefaultRightVirtualMirrorPosition;
                    }
                }
                else if (Math.Abs(res - (4f / 3f)) < 0.1f)
                {
                    DefaultLeftVirtualMirrorPosition = new Vector2(-10.8f, 9);
                    DefaultRightVirtualMirrorPosition = new Vector2(10.8f, 9);
                    if (Settings.ScreenResolution != res)
                    {
                        Settings.ScreenResolution = res;
                        Settings.LeftVirtualMirrorPosition = DefaultLeftVirtualMirrorPosition;
                        Settings.RightVirtualMirrorPosition = DefaultRightVirtualMirrorPosition;
                    }
                }
                else if (Math.Abs(res - (3f / 2f)) < 0.1f)
                {
                    // eg: 720*480
                    DefaultLeftVirtualMirrorPosition = new Vector2(-12.4f, 9);
                    DefaultRightVirtualMirrorPosition = new Vector2(12.4f, 9);
                    if (Settings.ScreenResolution != res)
                    {
                        Settings.ScreenResolution = res;
                        Settings.LeftVirtualMirrorPosition = DefaultLeftVirtualMirrorPosition;
                        Settings.RightVirtualMirrorPosition = DefaultRightVirtualMirrorPosition;
                    }
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
                    DefaultLeftVirtualMirrorPosition = new Vector2(10, 9);
                    DefaultRightVirtualMirrorPosition = new Vector2(-10, 9);
                    if (Settings.ScreenResolution != res)
                    {
                        Settings.ScreenResolution = res;
                        Settings.RightVirtualMirrorPosition = DefaultLeftVirtualMirrorPosition;
                        Settings.LeftVirtualMirrorPosition = DefaultRightVirtualMirrorPosition;
                    }
                }

                RIGHTSIDE_Mirror = CreateVirtualMirror(
                    "RightVirtualMirror",
                    Settings.RightVirtualMirrorScale,
                    Settings.RightVirtualMirrorPosition,
                    RightRenderTexture
                    );
                LEFTSIDE_Mirror = CreateVirtualMirror(
                    "LeftVirtualMirror",
                    Settings.LeftVirtualMirrorScale,
                    Settings.LeftVirtualMirrorPosition,
                    LeftRenderTexture
                    );
                REARVIEW_Mirror = CreateVirtualMirror(
                    "RearviewVirtualMirror",
                    Settings.RearVirtualMirrorScale,
                    Settings.RearVirtualMirrorPosition,
                    RearviewRenderTexture);
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

        private static GameObject CreateVitualMirrorCam(String name, RenderTexture renderTexture, float farClipPlane)
        {
            GameObject obj = new GameObject(name);
            obj.transform.localEulerAngles = Vector3.zero;
            obj.AddComponent<Camera>();
            obj.GetComponent<Camera>().targetTexture = renderTexture;
            obj.GetComponent<Camera>().farClipPlane = farClipPlane;
            obj.GetComponent<Camera>().fieldOfView = 20; // default fov
            obj.GetComponent<Camera>().cullingMask = -181598775; // magick number for don't draw player-layer object.
            obj.SetActive(false);
            return obj;
        }

        private static GameObject CreateVirtualMirror(String name, Vector3 scale, Vector3 position, RenderTexture renderTexture)
        {
            /* hierarchy: GUI/HUD/Day/BarBG
             *                                     |--/Data 
             */
            var hud = GameObject.Instantiate<GameObject>(GameObject.Find("GUI/HUD"));
            hud.transform.localPosition = Vector3.zero;
            hud.transform.localScale = Vector3.zero;
            var day = hud.transform.Find("Day").gameObject;
            day.transform.localPosition = Vector3.zero;
            day.transform.localScale = Vector3.zero;
            GameObject.Destroy(day.transform.Find("Data").gameObject);
            var bg = day.transform.Find("BarBG").gameObject;
            bg.name = name;
            MeshFlip(bg.GetComponent<MeshFilter>().mesh); // MeshFlip is GOD!!!1
            bg.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Unlit/Texture"));
            bg.GetComponent<MeshRenderer>().material.mainTexture = renderTexture;
            bg.transform.parent = GameObject.Find("GUI").transform;
            bg.transform.localScale = scale;
            bg.transform.localPosition = position;
#if DEBUG
            /* bg.SetActive(false); */
#else
            bg.SetActive(false);
#endif
            return bg;
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
            settings.FarClipPlane = 200;
            settings.RenderTextureDepth = 24;
            settings.SideMirrorsSelectionGrid = 2;
            settings.RearviewMirrorSelectionGrid = 2;
            settings.SideMirrorsRenderTextureWidth = 256;
            settings.SideMirrorsRenderTextureHeight = 256;
            settings.RearviewMirrorRenderTextureWidth = 1024;
            settings.RearviewMirrorRenderTextureHeight = 256;

            Cars this_car = new Cars
            {
                Name = GIFU,
                SwitchMirrorsNum = (int)MirrorsNum.None,
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
                SwitchMirrorsNum = (int)MirrorsNum.None,
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
                SwitchMirrorsNum = (int)MirrorsNum.None,
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
                Name = MUSCLE,
                SwitchMirrorsNum = (int)MirrorsNum.None,
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
                Name = KEKMET,
                SwitchMirrorsNum = (int)MirrorsNum.None,
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
                Name = RUSCKO,
                SwitchMirrorsNum = (int)MirrorsNum.None,
                RightCam = new Cam
                {
                    LocalPosition = new Vector3(0.5f, 0, 0.5f),
                    LocalEulerAngles = new Vector3(-5, 258, 270),
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                },
                RearviewCam = new Cam
                {
                    LocalPosition = new Vector3(0, 0.8f, 0),
                    LocalEulerAngles = new Vector3(0, 180, 0),
                    NearClipPlane = 2.3f,
                    FieldOfView = 20
                },
                LeftCam = new Cam
                {
                    LocalPosition = new Vector3(0.5f, 0, 0.5f), // z,x,y
                    LocalEulerAngles = new Vector3(5, 258, 270), // ,,y
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                }
            };
            settings.Cars.Add(this_car);

            this_car = new Cars
            {
                Name = MOPED,
                SwitchMirrorsNum = (int)MirrorsNum.None,
                RightCam = new Cam
                {
                    LocalPosition = new Vector3(0.8f, 0.15f, 0.65f),
                    LocalEulerAngles = new Vector3(-10, 260, 265),
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                },
                RearviewCam = new Cam
                {
                    LocalPosition = new Vector3(0, 0.8f, 0),
                    LocalEulerAngles = new Vector3(0, 180, 0),
                    NearClipPlane = 2.3f,
                    FieldOfView = 20
                },
                LeftCam = new Cam
                {
                    LocalPosition = new Vector3(0.8f, -0.15f, 0.65f), // z,x,y
                    LocalEulerAngles = new Vector3(10, 260, 265), // ,,y
                    NearClipPlane = 0.2f,
                    FieldOfView = 30
                }
            };
            settings.Cars.Add(this_car);

            return settings;
        }
    }

}
