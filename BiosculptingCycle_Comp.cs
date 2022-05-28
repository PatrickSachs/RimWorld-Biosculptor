using System.Collections.Generic;
using RimWorld;
using Verse;

namespace Lilith.RimWorld.Biosculptor {
    public class BiosculptingCycle_Comp : CompBiosculpterPod_Cycle {
        public override void CycleCompleted(Pawn occupant) {
            if (!BiosculptorMod.Settings.DidAutoinit) {
                BiosculptorMod.Settings.Autoinit();
            }
            
            var remove = new List<Hediff>();
            foreach (var hediff in occupant.health.hediffSet.hediffs) {
                var def = hediff.def;
                if (BiosculptorMod.Settings.AddictionDefs.Contains(def.defName)) {
                    remove.Add(hediff);
                }
            }

            foreach (var hediff in remove) {
                Log.Message("Removing hediff " + hediff.def.defName + " from " + occupant.Name);
                occupant.health.RemoveHediff(hediff);
            }
            Messages.Message( "Lilith_Biosculptor_Done".Translate(new NamedArgument(remove.Count, "removed")), remove.Count > 0 ? MessageTypeDefOf.PositiveEvent : MessageTypeDefOf.NeutralEvent);
        }
    }
}