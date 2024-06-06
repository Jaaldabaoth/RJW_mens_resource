using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace rjw_mens_ressource
{

    public static class DefsOf
    {
        public static readonly PawnKindDef CarrierDef = DefDatabase<PawnKindDef>.GetNamed("CarrierDef");
        private static List<ThingDef> allingestible = null;
        public static List<ThingDef> AllIngestible
        {
            get
            {
                if (allingestible != null) return allingestible;
                allingestible = DefDatabase<ThingDef>.AllDefsListForReading.Where(thingdef => (thingdef.IsIngestible && thingdef.category == ThingCategory.Item && !thingdef.IsCorpse)).ToList();

                return allingestible;
            }
        }
    }
}
