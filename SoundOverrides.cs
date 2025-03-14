using HarmonyLib;
using XRL.World;

namespace SofterCleaveSound
{
    public static class SoundOverrides
    {
        private static readonly string[][] Overrides =
        {
            new string[] {"breakage", "Clink1"},
        };

        [HarmonyPatch(typeof(IComponent<GameObject>))]
        [HarmonyPatch("PlayUISound")]
        public class IComponentGameObject_PlayUISound_Patch
        {
            /*
             * Fix: Updated parameter name from "clip" to "Clip" (capitalized) to match
             * the actual method signature in the game code.
             * 
             * Reason: Harmony requires an exact match of parameter names when using "ref".
             * If the capitalization doesn't match, Harmony will fail to apply the patch,
             * resulting in a runtime error.
             */
            public static bool Prefix(ref string Clip)
            {
                return CheckSound(ref Clip);
            }
        }

        [HarmonyPatch(typeof(IComponent<GameObject>))]
        [HarmonyPatch("PlayWorldSound")]
        public class IComponentGameObject_PlayWorldSound_Patch
        {
            /*
             * Fix: Same as above—corrected the parameter name to "Clip" to ensure
             * Harmony can properly apply the patch.
             */
            public static bool Prefix(ref string Clip)
            {
                return CheckSound(ref Clip);
            }
        }

        private static bool CheckSound(ref string Clip)
        {
            foreach (string[] soundOverride in Overrides)
            {
                var sound = soundOverride[0];
                var replacement = soundOverride[1];
                if (Clip == sound)
                {
                    if (string.IsNullOrEmpty(replacement))
                    {
                        return false;
                    }
                    else
                    {
                        Clip = replacement;
                        return true;
                    }
                }
            }

            return true;
        }
    }
}
