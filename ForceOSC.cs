using FrooxEngine;
using HarmonyLib;
using ResoniteModLoader;
using System;
using UnityFrooxEngineRunner;

namespace ForceOSC
{

    public partial class ForceOSC : ResoniteMod
    {
        public override string Name => "ForceOSC";
        public override String Author => "zahndy";
        public override String Link => "https://github.com/zahndy/ForceOSC";
        public override String Version => "1.0.0";

        [AutoRegisterConfigKey]
        private static readonly ModConfigurationKey<bool> Enabled = new ModConfigurationKey<bool>("Enabled", "Force OSC (restart required)", () => true);

        internal static ModConfiguration Config;

        public override void OnEngineInit()
        {
            Config = GetConfiguration();
            Config.Save(true);

            Harmony harmony = new Harmony("com.zahndy.ForceOSC");
            harmony.PatchAll();
        }

        [HarmonyPatch(typeof(SteamVRDriver), "RegisterInputs")]
        private class RegisterInputsForceOSCPatch
        {
            public static void Postfix(InputInterface inputInterface)
            {
                if (Config.GetValue(Enabled))
                {
                    Msg("Forced Registering Steam Link OSC driver", false);
                    inputInterface.RegisterInputDriver((IInputDriver)new SteamLinkOSC_Driver());
                }
            }
        }
    }
}