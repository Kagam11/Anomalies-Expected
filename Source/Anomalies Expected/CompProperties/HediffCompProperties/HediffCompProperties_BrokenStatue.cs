using Verse;

namespace AnomaliesExpected
{
    public class HediffCompProperties_BrokenStatue : HediffCompProperties
    {
        public ThingDef spawnedThingDef;
        public DamageDef damageDef;
        public float spineDMG = 100;
        public float spinePenetration = 1;
        public SoundDef soundTransform;
        public SoundDef soundSpineBreak;

        public HediffCompProperties_BrokenStatue()
        {
            compClass = typeof(HediffComp_BrokenStatue);
        }
    }
}
