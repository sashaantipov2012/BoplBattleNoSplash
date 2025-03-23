using BepInEx;
using HarmonyLib;
using UnityEngine;
using System;

namespace NoSplash
{
    [BepInPlugin("com.SashaAnt.NoBoplSplash", "NoBoplSplash", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        private void Awake()
        {
            Logger.LogInfo("NoBoplSplash loaded!");
            new Harmony("NoBoplSplash").PatchAll(typeof(Patches));
            CharacterSelectHandler_online.clientSideMods_you_can_increment_this_to_enable_matchmaking_for_your_mods__please_dont_use_it_to_cheat_thats_really_cringe_especially_if_its_desyncing_others___you_didnt_even_win_on_your_opponents_screen___I_cannot_imagine_a_sadder_existence += 1;
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
