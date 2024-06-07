using HarmonyLib;
using RimWorld;
using rjw;
using RJW_Menstruation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace rjw_mens_ressource
{
    [HarmonyPatch(typeof(MenstruationUtility))]
    public static class MenstruationUtility_patcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("GetPregnancyIcon")]
        static bool GetPregnancyIconPrefix(ref Texture2D __result , ref HediffComp_Menstruation comp, Hediff hediff)
        {
            float gestationProgress = comp.StageProgress;

            Pawn baby = null;
            int babycount = 0;
            HediffComp_PregeneratedBabies babiescomp = hediff?.TryGetComp<HediffComp_PregeneratedBabies>();
            if (hediff is Hediff_MultiplePregnancy)
            {
                baby = ((Hediff_MultiplePregnancy)hediff).babies[0];
                babycount = ((Hediff_MultiplePregnancy)hediff).babies.Count;
            }
            ThingDef thing = null; 
            if (baby == null) return true;
            Name n = baby.Name;
            if (n == null) return true;
            if(n is NameSingle)
                {
                string s = ((NameSingle)n).Name;
                foreach (ThingDef def in DefsOf.AllIngestible)
                {
                    if(def.defName == s)
                    {
                        thing = def;
                    }

                }
                }
            else { return true; }
            if (thing == null) return true;
            else
            {
                string fetustex = thing.GetModExtension<PawnDNAModExtension>()?.fetusTexPath ?? "Fetus/Fetus_Default";
                if (fetustex == null) return true;
                string icon;
                if (gestationProgress < 0.2f) icon = comp.WombTex + "_Implanted";
                else if (gestationProgress < 0.4f) icon = fetustex + "00";
                else if (gestationProgress < 0.5f) icon = fetustex + "01";
                else if (gestationProgress < 0.6f) icon = fetustex + "02";
                else if (gestationProgress < 0.7f) icon = fetustex + "03";
                else if (gestationProgress < 0.8f) icon = fetustex + "04";
                else icon = fetustex + "05";

                __result = MenstruationUtility.TryGetTwinsIcon(icon, babycount) ?? ContentFinder<Texture2D>.Get(icon, true);
                return false;
            }
        }


    }


}


