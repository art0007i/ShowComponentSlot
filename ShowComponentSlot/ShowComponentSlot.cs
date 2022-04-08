using HarmonyLib;
using NeosModLoader;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Collections.Generic;
using FrooxEngine;
using FrooxEngine.LogiX;
using FrooxEngine.UIX;
using BaseX;

namespace ShowComponentSlot
{
    public class ShowComponentSlot : NeosMod
    {
        public override string Name => "ShowComponentSlot";
        public override string Author => "art0007i";
        public override string Version => "1.0.0";
        public override string Link => "https://github.com/art0007i/ShowComponentSlot/";
        public override void OnEngineInit()
        {
            Harmony harmony = new Harmony("me.art0007i.ShowComponentSlot");
            harmony.PatchAll();

        }
        [HarmonyPatch(typeof(WorkerInspector))]
        [HarmonyPatch("BuildUIForComponent")]
        class WorkerInspector_BuildUIForComponent_Patch
        {
            public static void Postfix(Worker worker, bool allowRemove, bool allowDuplicate, WorkerInspector __instance)
            {
                if (allowRemove && !allowDuplicate)
                {
                    UIBuilder ui = new UIBuilder(__instance.Slot[0][0]);
                    ui.Style.MinHeight = 24f;
                    ui.Style.FlexibleWidth = 0f;
                    ui.Style.MinWidth = 32f;

                    color color = color.Yellow;
                    color white = color.White;
                    color color2 = MathX.Lerp(color, white, 0.7f);
                    var button = ui.Button("⤴", color2);
                    var edit = button.Slot[0].AttachComponent<RefEditor>();
                    (AccessTools.Field(edit.GetType(), "_targetRef").GetValue(edit) as RelayRef<ISyncRef>).Target = (ISyncRef)AccessTools.Field(__instance.GetType(), "_targetWorker").GetValue(__instance);
                    button.Pressed.Target = (ButtonEventHandler)AccessTools.Method(edit.GetType(), "OpenInspectorButton").CreateDelegate(typeof(ButtonEventHandler), edit);
                }
            }
        }
    }
}