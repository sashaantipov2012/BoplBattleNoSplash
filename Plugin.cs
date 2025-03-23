using BepInEx;
using HarmonyLib;
using UnityEngine;
using System;

namespace FallDamage
{
    [BepInPlugin("com.SashaAnt.NoBoplSplash", "NoBoplSplash", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("NoSplash loaded!");
            new Harmony("NoSplash").PatchAll(typeof(Patches));
        }
    }

    public class Patches
    {
        [HarmonyPatch(typeof(DeathEffect2), "Init")]
        [HarmonyPrefix]
        public static bool Init(DeathEffect2 __instance)
        {
            var sr = __instance.GetComponent<SpriteRenderer>();
            if (sr) sr.enabled = false;
            Updater.HitStop(__instance.hitStopFrames);
            return false;
        }

        [HarmonyPatch(typeof(DeathEffect2), "SpawnRandomSplash")]
        [HarmonyPrefix]
        public static bool SpawnRandomSplash() => false;

        [HarmonyPatch(typeof(DeathEffect2), "startExplode")]
        [HarmonyPrefix]
        public static bool StartExplode(DeathEffect2 __instance)
        {
            __instance.spriteAnim.beginAnimThenDoAction(__instance.animData.GetAnimation("end"), new Action(__instance.SelfDestruct));
            __instance.vibrate = false;
            return false;
        }

        [HarmonyPatch(typeof(DeathEffect2), "LateUpdate")]
        [HarmonyPrefix]
        public static bool LateUpdate() => false;
    }
}
