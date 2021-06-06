using On.RoR2;
using R2API;
using System;
using UnityEngine;

namespace DeathsRegardsMod
{
    public class DeathsRegardsAchievement : ModdedUnlockable
    {
        // ModdedUnlockable
        public override String AchievementIdentifier { get; } = "HEADLINE_DEATHSREGARDS_ACHIEVEMENT_UNLOCK";
        public override String UnlockableIdentifier { get; } = "HEADLINE_DEATHSREGARDS_UNLOCKED";
        public override String PrerequisiteUnlockableIdentifier { get; } = "";
        public override String AchievementNameToken { get; } = "HEADLINE_DEATHSREGARDS_ACHIEVEMENT_NAME";
        public override String AchievementDescToken { get; } = "HEADLINE_DEATHSREGARDS_ACHIEVEMENT_DESC";
        public override String UnlockableNameToken { get; } = "HEADLINE_DEATHSREGARDS_UNLOCKABLE_NAME";
        public override Func<String> GetHowToUnlock { get; } = new Func<String>(() => "Cheat death by using Dio's Best Friend.");
        public override Func<String> GetUnlocked { get; } = new Func<String>(() => "You've cheated death the Lord Death gives you his regards.");

        public override Sprite Sprite { get { return DeathsRegardsMod.Item.ItemSprite; } }


        public static DeathsRegardsAchievement Instance; // bad

        public DeathsRegardsAchievement()
        {
            Instance = this;
        }

        private void RespawnExtraLife(On.RoR2.CharacterMaster.orig_RespawnExtraLife orig, RoR2.CharacterMaster self)
        {
            base.Grant();
            orig(self);
        }

        public override void OnUninstall()
        {
            base.OnUninstall();
            On.RoR2.CharacterMaster.RespawnExtraLife -= this.RespawnExtraLife;
        }

        public override void OnInstall()
        {
            base.OnInstall();
            On.RoR2.CharacterMaster.RespawnExtraLife += this.RespawnExtraLife;
        }
    }
}