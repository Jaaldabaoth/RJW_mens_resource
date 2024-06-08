using HarmonyLib;
using rjw;
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
                List<string>  categories = new List<string>()
                    { "Foods", "Medicine", "Drugs", "PlantMatter",
                "FoodMeals","FoodRaw","MeatRaw","PlantFoodRaw","AnimalProductRaw","ResourcesRaw"};
                allingestible = DefDatabase<ThingDef>.AllDefsListForReading.Where(thingdef => (IsIncategories(thingdef,categories) && !thingdef.IsCorpse)).ToList();
                return allingestible;
            }
        }

        public static bool IsIncategories(ThingDef thing, List<string>  categories)
        {
            if(thing.category== ThingCategory.Item && thing.thingCategories!=null)
            foreach (ThingCategoryDef catdef in thing.thingCategories) {
                if(catdef!=null)
                if(categories.Contains(catdef.defName)) return true;
            }
            return false;
        }
    }
}
