using HarmonyLib;
using RimWorld;
using RJW_Menstruation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace rjw_mens_ressource
{
    [HarmonyPatch(typeof(Dialog_HybridCustom))]
    public static class Dialog_HybridCustom_patcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("DoRow")]
        static bool DoRowPrefix(Rect rect, HybridInformations extension, ref List<HybridInformations> ___removeList)
        {
            Rect buttonRect = new Rect(rect.xMax - 90f, rect.y, 80f, rect.height);
            Widgets.Label(rect, extension.GetDef?.label ?? "Undefined");
            if (Widgets.ButtonText(buttonRect, "Delete"))
            {
                ___removeList.Add(extension);
                //raceList.Add(new FloatMenuOption(extension.GetDef.label, delegate { AddHybridOverride(extension.GetDef); }));
            }
            buttonRect.x -= 80f;
            if (Widgets.ButtonText(buttonRect, "Edit"))
            {
                Dialog_EditHybrid_Extention.OpenWindow(extension);
            }


            Widgets.DrawHighlightIfMouseover(rect);
            return false;
        }


    }


}


