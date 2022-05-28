using UnityEngine;
using Verse;

namespace Lilith.RimWorld.Biosculptor {
    public class BiosculptorMod : Mod {
        public BiosculptorMod(ModContentPack content) : base(content) {
            Settings = base.GetSettings<BiosculptorSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect) {
            Settings.DoSettingsWindowContents(inRect);
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() {
            return Settings.SettingsCategory();
        }

        public static BiosculptorSettings Settings;
    }
}