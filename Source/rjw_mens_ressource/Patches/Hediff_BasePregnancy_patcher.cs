using HarmonyLib;
using RimWorld;
using rjw;
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
    [HarmonyPatch(typeof(Hediff_BasePregnancy))]
    public static class Hediff_BasePregnancy_patcher
    {
        [HarmonyPostfix]
        [HarmonyPatch("Initialize")]
        static void InitializePostfix(ref Hediff_BasePregnancy __instance, Pawn mother, Pawn dad)
        {
            float num = 1f;
            if (__instance.babies[0]!=null && dad != null)
            {
                if (__instance.babies[0].def.defName == "Carrier") {
                    num = dad.RaceProps.gestationPeriodDays * 30000f;
                    num *= RJWPregnancySettings.normal_pregnancy_duration;
                    __instance.p_end_tick = __instance.p_start_tick + num;
                }
            }
        }


    }


}


