using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualMirror
{
    public class Settings
    {
        public string Version { get; set; }
        public float ScreenResolution { get; set; }
        public Vector2 LeftVirtualMirrorScale { get; set; }
        public Vector2 RearVirtualMirrorScale { get; set; }
        public Vector2 RightVirtualMirrorScale { get; set; }
        public Vector2 LeftVirtualMirrorPosition { get; set; }
        public Vector2 RearVirtualMirrorPosition { get; set; }
        public Vector2 RightVirtualMirrorPosition { get; set; }
        [System.Xml.Serialization.XmlElement("MaxDrawDistance")]
        public int FarClipPlane { get; set; }
        public int RenderTextureDepth { get; set; }
        public int SideMirrorsSelectionGrid { get; set; }
        public int RearviewMirrorSelectionGrid { get; set; }
        public int SideMirrorsRenderTextureWidth { get; set; }
        public int SideMirrorsRenderTextureHeight { get; set; }
        public int RearviewMirrorRenderTextureWidth { get; set; }
        public int RearviewMirrorRenderTextureHeight { get; set; }

        [System.Xml.Serialization.XmlElement("Car")]
        public List<Cars> Cars { get; set; }
    }

    public class Cars
    {
        [System.Xml.Serialization.XmlAttribute("name")]
        public String Name { get; set; }

        public Cam LeftCam { get; set; }
        public Cam RightCam { get; set; }
        public Cam RearviewCam { get; set; }
        public int SwitchMirrorsNum { get; set; }
    }
    public class Cam
    {
        [System.Xml.Serialization.XmlElement("FOV")]
        public float FieldOfView { get; set; }
        [System.Xml.Serialization.XmlElement("MinDrawDistance")]
        public float NearClipPlane { get; set; }
        [System.Xml.Serialization.XmlElement("Position")]
        public Vector3 LocalPosition { get; set; }
        [System.Xml.Serialization.XmlElement("Rotation")]
        public Vector3 LocalEulerAngles { get; set; }
    }
}
