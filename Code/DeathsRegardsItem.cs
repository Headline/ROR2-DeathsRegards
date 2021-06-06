using System;
using System.Collections.Generic;
using System.Reflection;
using ItemStats;
using ItemStats.Stat;
using R2API;
using RoR2;
using UnityEngine;

using static On.RoR2.HealthComponent;
using static On.RoR2.Run;

namespace DeathsRegardsMod
{
    public class DeathsRegardsItem
    {
        public DeathsRegardsAchievement Achievement { get; private set; }
        public ItemDef Definition { get; private set; }

        public GameObject ItemModel;
        public Sprite ItemSprite;

        public Dictionary<int, float> Cooldowns;

        // amount of time in seconds until the item can be used again
        private const float CooldownTime = 60.0f;
        // amount that the damage is multiplied and applied to the character
        // as a shield.
        private const float ShieldItemFactor = 2.0f;

        public DeathsRegardsItem()
        {
            this.LoadAssets();

            this.Cooldowns = new Dictionary<int, float>();
        }

        public ItemStatDef CreateItemStatDef()
        {
            return new ItemStatDef
            {
                Stats = new List<ItemStat>
                {
                    new ItemStat(
                        (itemCount, ctx) => CalculateCooldown((int)itemCount),
                        (value, ctx) => $"Cooldown: <style=cIsUtility>{value:0.0} seconds</style>"
                    )
                }
            };
        }
        private void LoadAssets()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("DeathsRegards.deathsregardsmod"))
            {
                var bundle = AssetBundle.LoadFromStream(stream);

                this.ItemModel = bundle.LoadAsset<GameObject>("Assets/craneo.obj");
                this.ItemSprite = bundle.LoadAsset<Sprite>("Assets/icon.png");
            }
        }

        public void BuildItemDefinition(UnlockableDef def)
        {
            var itemdef = ScriptableObject.CreateInstance<ItemDef>();
            itemdef.name = "DeathsRegards";
            itemdef.tier = ItemTier.Tier3;
            itemdef.tags = new[]{
                ItemTag.Utility
            };
            itemdef.unlockableDef = def;
            itemdef.pickupModelPrefab = this.ItemModel;
            itemdef.pickupIconSprite = this.ItemSprite;
            itemdef.nameToken = "DEATHSITEM_NAME";
            itemdef.pickupToken = "DEATHSITEM_PICKUP";
            itemdef.descriptionToken = "DEATHSITEM_DESC";
            itemdef.loreToken = "DEATHSITEM_LORE";
            this.Definition = itemdef;
        }

        public ItemDisplayRuleDict BuildDisplayRules()
        {
            ItemDisplayRuleDict dict = new ItemDisplayRuleDict();
            dict.Add("mdlCommandoDualies", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(0.01159F, 0.14157F, -0.06636F),
                localAngles = new Vector3(300F, 180F, 180F),
                localScale = new Vector3(0.1F, 0.1F, 0.1F)
            });
            dict.Add("mdlHuntress", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(0.01371F, 0.05648F, -0.09268F),
                localAngles = new Vector3(300F, 180F, 180F),
                localScale = new Vector3(0.075F, 0.075F, 0.075F)
            });
            dict.Add("mdlToolbot", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "MainWheelL",
                localPos = new Vector3(-0.01748F, 1.08975F, -1.52357F),
                localAngles = new Vector3(300F, 0F, 0F),
                localScale = new Vector3(1F, 1F, 1F)
            });
            dict.Add("mdlEngi", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(-0.00026F, 0.20756F, -0.12849F),
                localAngles = new Vector3(310F, 180F, 180F),
                localScale = new Vector3(0.1F, 0.1F, 0.1F)
            });
            dict.Add("mdlMage", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(-0.0118F, 0.09237F, -0.05244F),
                localAngles = new Vector3(15F, 180F, 180F),
                localScale = new Vector3(0.075F, 0.075F, 0.075F)
            });
            dict.Add("mdlMerc", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(-0.00364F, 0.07695F, -0.06853F),
                localAngles = new Vector3(340F, 180F, 180F),
                localScale = new Vector3(0.075F, 0.075F, 0.075F)
            });
            dict.Add("mdlTreebot", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootFrontL",
                localPos = new Vector3(0.00159F, 0.024F, -0.00071F),
                localAngles = new Vector3(0F, 120F, 180F),
                localScale = new Vector3(0.3F, 0.3F, 0.3F)
            });
            dict.Add("mdlLoader", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(0.0136F, 0.0441F, -0.08881F),
                localAngles = new Vector3(0.00001F, 180F, 180F),
                localScale = new Vector3(0.1F, 0.1F, 0.1F)
            });
            dict.Add("mdlCroco", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(-0.1655F, 0.32189F, -1.2976F),
                localAngles = new Vector3(10F, 180F, 180F),
                localScale = new Vector3(0.5F, 0.5F, 0.5F)
            });
            dict.Add("mdlCaptain", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(0.05678F, 0.12228F, -0.13881F),
                localAngles = new Vector3(345F, 150F, 180F),
                localScale = new Vector3(0.08F, 0.08F, 0.08F)
            });
            dict.Add("mdlBandit2", new ItemDisplayRule
            {
                ruleType = ItemDisplayRuleType.ParentedPrefab,
                followerPrefab = this.ItemModel,
                childName = "FootL",
                localPos = new Vector3(0.02064F, 0.09363F, -0.11238F),
                localAngles = new Vector3(330F, 170F, 180F),
                localScale = new Vector3(0.1F, 0.1F, 0.1F)
            });

            return dict;
        }

        internal void OnRunStart(orig_Start orig, Run self)
        {
            this.Cooldowns.Clear();
            orig(self);
        }

        internal void OnTakeDamage(orig_TakeDamage orig, HealthComponent self, DamageInfo damageInfo)
        {
            orig(self, damageInfo);

            if (self.body.inventory == null) {
                return;
            }

            var itemCount = self.body.inventory.GetItemCount(this.Definition);
            if (itemCount > 0)
            {
                // Damaged below 30% HP
                if (self.combinedHealthFraction < .30f)
                {
                    int playerId = self.body.inventory.GetInstanceID();
                    if (!this.Cooldowns.TryGetValue(playerId, out float timeLastUsed))
                    {
                        timeLastUsed = 0.0f; // never used, we can just go with zero here
                    }

                    var timeNow = Time.time;
                    if (timeLastUsed + DeathsRegardsItem.CalculateCooldown(itemCount) < timeNow)
                    {
                        self.AddBarrier(damageInfo.damage * DeathsRegardsItem.ShieldItemFactor);
                        this.Cooldowns[playerId] = timeNow;
                    }
                }
            }
        }

        // starting from 1 item the cooldown sequence is 60, 40 20, 13, 10, ...
        private static float CalculateCooldown(int itemCount)
        {
            if (itemCount == 1)
            {
                return 60.0f;
            }
            else
            {
                return CooldownTime / (1.5f * (itemCount - 1));
            }    
        }
    }
}
