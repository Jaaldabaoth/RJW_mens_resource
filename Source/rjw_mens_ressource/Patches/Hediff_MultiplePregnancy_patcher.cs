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
    [HarmonyPatch(typeof(Hediff_MultiplePregnancy))]
    public static class Hediff_MultiplePregnancy_patcher
    {
        [HarmonyPrefix]
        [HarmonyPatch("GiveBirth")]
        static bool GiveBirthPrefix(ref Hediff_MultiplePregnancy __instance )
        {
            if (!__instance.babies.NullOrEmpty())
                foreach (Pawn baby in __instance.babies)
                {
                    if (baby?.def.defName == "Carrier")
                    {
                        RessourceBirth(ref __instance);
                        return false;
                    }

                }
            return true;
        }

        public static void RessourceBirth(ref Hediff_MultiplePregnancy instance)
        {
            Name s;
            NameSingle s2;
            ThingDef thingdef;
            Thing thing;
            String[] parts;
            string[] separatingStrings = { "-#-" };
            foreach (Pawn baby in instance.babies)
            {
                s = baby.Name;
                if (s is NameSingle)
                {
                    s2 = (NameSingle)s;
                    parts = s2.Name.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);
                    thingdef = DefDatabase<ThingDef>.GetNamedSilentFail(parts[0]);
                    thing = ThingMaker.MakeThing(thingdef);
                    thing.stackCount = (int)(instance.pawn.BodySize * int.Parse(parts[1]));
                    GenSpawn.Spawn(thing, instance.pawn.Position, instance.pawn.Map, WipeMode.Vanish);

                }
            }

            instance.GetMenstruationCompFromPregnancy().Pregnancy = null;
            if (!instance.babies.NullOrEmpty())
            {
                foreach (Pawn baby in instance.babies)
                {
                    baby.Destroy(DestroyMode.Vanish);
                    baby.Discard();
                }
            }
            instance.pawn.health?.RemoveHediff(instance);
            HediffComp_Menstruation comp = instance.GetMenstruationCompFromPregnancy();
            if (comp != null) comp.Pregnancy = null;
            HediffComp_Breast breastcomp = instance.pawn.GetBreastComp();
            if (ModsConfig.BiotechActive && xxx.is_human(instance.pawn) && breastcomp != null)
                instance.pawn.health.AddHediff(HediffDefOf.Lactating);
            breastcomp?.GaveBirth();
            FilthMaker.TryMakeFilth(instance.pawn.Position, instance.pawn.Map, ThingDefOf.Filth_AmnioticFluid, instance.pawn.LabelIndefinite(), 5);
        }

        [HarmonyPostfix]
        [HarmonyPatch("GenerateBaby")]
        static void GenerateBabyPostfix(ref Pawn __result, Pawn mother, Pawn father)
        {
            if (__result.def.defName == "Carrier")
            {
                string name = GetHybrid2(mother, father);
                NameSingle nameSingle = new NameSingle(name);
                __result.Name = nameSingle;
            }
        }

        public static string GetHybrid2(Pawn first, Pawn second)
        {
            string res = null;
            Pawn opposite = second;
            HybridInformations info = null;


            if (!Configurations.HybridOverride.NullOrEmpty())
            {
                info = Configurations.HybridOverride.FirstOrDefault(x => x.DefName == first.def?.defName && (x.hybridExtension?.Exists(y => y.DefName == second.def?.defName) ?? false));
                if (info == null)
                {
                    info = Configurations.HybridOverride.FirstOrDefault(x => x.DefName == second.def?.defName && (x.hybridExtension?.Exists(y => y.DefName == first.def?.defName) ?? false));
                    opposite = first;
                }
            }

            if (info != null)
            {
                res = GetHybridWith2(opposite.def.defName,info);
            }
            if (res != null) return res;


            PawnDNAModExtension dna;
            dna = first.def.GetModExtension<PawnDNAModExtension>();
            if (dna != null)
            {
                res = GetHybridWith2(second.def.defName,dna);
            }
            else
            {
                dna = second.def.GetModExtension<PawnDNAModExtension>();
                if (dna != null)
                {
                    res = GetHybridWith2(first.def.defName,dna);
                }
            }
            return res;
        }
        public static string GetHybridWith2(string race, PawnDNAModExtension dna)
        {
            if (dna == null) return null;
            return ChooseOne2(dna.GetHybridExtension(race));
        }

        public static string GetHybridWith2(string race, HybridInformations info)
        {
            if (info == null) return null;
            return ChooseOne2(info.GetHybridExtension(race));
        }
        public static string ChooseOne2(HybridExtension ext)
        {
            if (ext == null) return null;
            if (ext.hybridInfo == null) return null;
            if (ext.hybridInfo.EnumerableNullOrEmpty()) return null;
            string key = null;
            ThingDef res = null;
            do
            {
                key = ext.hybridInfo.RandomElement().Key;

                foreach (ThingDef td in DefsOf.AllIngestible)
                {
                    if (td.defName == key)
                    {
                        res = td;
                    }
                }
               
            } while (res == null && !ext.hybridInfo.EnumerableNullOrEmpty());
            int a = 80;
            if (key != null)
            {
                a = (int)ext.hybridInfo.TryGetValue(key);
            }
            return res.defName+"-#-"+a;
        }
    }


}


