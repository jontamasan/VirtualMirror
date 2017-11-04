using MSCLoader;
using UnityEngine;

namespace VirtualMirror
{
    public class VirtualMirror : Mod
    {
        public override string ID => "VirtualMirror";
        public override string Name => "VirtualMirror";
        public override string Author => "Your Username";
        public override string Version => "1.0";

        //Set this to true if you will be load custom assets from Assets folder.
        //This will create subfolder in Assets folder for your mod.
        public override bool UseAssetsFolder => false;

        //Called when mod is loading
        public override void OnLoad()
        {

        }

        // Update is called once per frame
        public override void Update()
        {

        }
    }
}
