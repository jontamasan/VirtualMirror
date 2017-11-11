using System;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualMirror
{
    public class Settings
    {
        public string Version { get; set; }
        [System.Xml.Serialization.XmlElement("MaxRenderingDistance")]
        public int FarClipPlane { get; set; }
        public int RenderTextureDepth { get; set; }
        public int SideMirrorsRenderTextureWidth { get; set; }
        public int SideMirrorsRenderTextureHeight { get; set; }
        public int RearviewMirrorsRenderTextureWidth { get; set; }
        public int RearviewMirrorsRenderTextureHeight { get; set; }

        [System.Xml.Serialization.XmlElement("Car")]
        public List<Cars> Cars { get; set; }
    }

    public class Cars
    {
        [System.Xml.Serialization.XmlAttribute("name")]
        public String Name { get; set; }

        public int SwitchMirrorsNum { get; set; }
        public Cam RightCam { get; set; }
        public Cam LeftCam { get; set; }
        public Cam RearviewCam { get; set; }
    }
    public class Cam
    {
        [System.Xml.Serialization.XmlElement("MinRenderingDistance")]
        public float NearClipPlane { get; set; }
        public float FieldOfView { get; set; }
        public Vector3 LocalPosition { get; set; }
        public Vector3 LocalEulerAngles { get; set; }
    }
}
