using System.Linq;
using Verse;
using Verse.AI;

namespace AnomaliesExpected
{
    public class ThinkNode_BrokenStatueIdle : ThinkNode
    {
        public override ThinkResult TryIssueJobPackage(Pawn pawn, JobIssueParams jobParams)
        {
            pawn.health.hediffSet.GetHediffComps<HediffComp_BrokenStatue>().FirstOrDefault()?.Wander();
            return ThinkResult.NoJob;
        }
    }
}
