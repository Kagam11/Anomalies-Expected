using RimWorld;
using Verse;

namespace AnomaliesExpected
{
    public class CompProperties_BrokenStatue : CompProperties_Interactable
    {
        public PawnKindDef kindDef;

        public CompProperties_BrokenStatue()
        {
            compClass = typeof(Comp_BrokenStatue);
        }
    }
}
