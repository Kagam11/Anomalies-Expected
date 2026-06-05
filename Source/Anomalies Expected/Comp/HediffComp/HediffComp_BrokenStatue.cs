using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace AnomaliesExpected
{
    public class HediffComp_BrokenStatue : HediffComp
    {
        public override void CompPostTick(ref float severityAdjustment)
        {
            base.CompPostTick(ref severityAdjustment);
            if (Pawn.IsHashIntervalTick(20))
            {
                if (Pawn.SpawnedOrAnyParentSpawned)
                {
                    Vector2 pos = (Vector2)Find.Camera.WorldToScreenPoint(Pawn.DrawPos) / Prefs.UIScale;
                    if (pos.x >= 0 && pos.y >= 0 && pos.x <= Screen.width && pos.y <= Screen.height)
                    {
                        parent.Severity = 0.5f;
                        return;
                    }
                }
                parent.Severity = 1;
            }
        }
    }
}
