using Verse;

namespace AnomaliesExpected
{
    public class HediffCompProperties_BrokenStatue : HediffCompProperties
    {
        public ThingDef spawnedThingDef;

        public HediffCompProperties_BrokenStatue()
        {
            compClass = typeof(HediffComp_BrokenStatue);
        }
    }
}
