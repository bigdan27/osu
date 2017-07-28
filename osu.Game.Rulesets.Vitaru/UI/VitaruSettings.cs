using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruSettings : SettingsSubsection
    {
        protected override string Header => "vitaru!";

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Touhosu Mode",
                    Bindable = VitaruRuleset.TouhosuMode,
                }
            };
        }
    }
}