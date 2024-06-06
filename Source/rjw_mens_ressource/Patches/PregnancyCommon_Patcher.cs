using HarmonyLib;
using RimWorld;
using RJW_Menstruation;
using System.Collections.Generic;
using System.Linq;
using rjw;
using Verse;
using RimWorld.Planet;
using System.Threading;


namespace rjw_mens_ressource
{
    [HarmonyPatch(typeof(HybridExtension))]
    public static class PregnancyCommon_Patcher
    {

        [HarmonyPrefix]
        [HarmonyPatch("ChooseOne")]
        public static bool ChooseOnePrefix(ref PawnKindDef __result, ref HybridExtension __instance)
        {

            if (__instance.hybridInfo.EnumerableNullOrEmpty()) return true;
            ThingDef res = null;
            do
            {
                string key = __instance.hybridInfo.RandomElementByWeight(x => x.Value).Key;
                res = DefDatabase<ThingDef>.GetNamedSilentFail(key);

                if (res == null)
                {
                    Log.Warning($"Could not find pawnKind or race {key}, removing hybrid definition");
                    __instance.hybridInfo.Remove(key);
                }
            } while (res == null && !__instance.hybridInfo.EnumerableNullOrEmpty());

            if (res != null)
            {
                if (DefsOf.AllIngestible.Contains(res))
                {
                    __result = DefsOf.CarrierDef;
                    return false;
                }
            }

            return true;
        }
    }


}
