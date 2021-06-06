using BepInEx;

using R2API;
using R2API.Utils;
using RoR2;
using System.Runtime.CompilerServices;

namespace DeathsRegardsMod
{
    [BepInDependency(R2API.R2API.PluginGUID)]
    [BepInDependency("dev.ontrigger.itemstats", BepInDependency.DependencyFlags.SoftDependency)]
    [R2APISubmoduleDependency(nameof(ItemAPI), nameof(LanguageAPI), nameof(UnlockableAPI), nameof(CommandHelper))]
    [BepInPlugin(ModGuid, ModName, ModVer)]
    public class DeathsRegardsMod : BaseUnityPlugin
    {
        private const string ModVer = "1.0.0";
        private const string ModName = "DeathsRegardsMod";
        private const string ModGuid = "com.headline.deathsregards";

        private static DeathsRegardsItem item;

        public static DeathsRegardsItem Item { get => item; set => item = value; }

        public void Awake()
        {
            AddLanguageTokens();

            Item = new DeathsRegardsItem();

            var def = UnlockableAPI.AddUnlockable<DeathsRegardsAchievement>(false);
            Item.BuildItemDefinition(def);
            this.AddItem(Item);
            
            RoR2Application.onLoad += () =>
            {
                if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("dev.ontrigger.itemstats"))
                {
                    this.AddToItemStats();
                }                
            };
            
            On.RoR2.Run.Start += Item.OnRunStart;
            On.RoR2.HealthComponent.TakeDamage += Item.OnTakeDamage;

            CommandHelper.AddToConsoleWhenReady();
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void AddToItemStats()
        {
            var idx = ItemCatalog.FindItemIndex("DeathsRegards");
            if (idx.ToString() == "None")
            {
                this.Logger.LogError($"Unable to add DeathsRegards to ItemStatsMod");
                return;
            }
            this.Logger.LogMessage($"Adding DeathsRegard (idx {idx}) to ItemStatsMod");
            ItemStats.ItemStatsMod.AddCustomItemStatDef(idx, Item.CreateItemStatDef());
        }

        private void AddItem(DeathsRegardsItem item)
        {
            var lens_item = new CustomItem(item.Definition, item.BuildDisplayRules());
            if (!ItemAPI.Add(lens_item))
            {
                this.Logger.LogError("Unable to add Death's Regards");
            }
        }

        [RoR2.ConCommand(commandName = "regards_unlock", flags = RoR2.ConVarFlags.None, helpText = "Unlocks Death's Regards.")]
        static void UnlockCommand(RoR2.ConCommandArgs args)
        {
            if (DeathsRegardsAchievement.Instance == null)
            {
                args.Log("Achievement has already been granted.");
            }
            else
            {
                args.Log("Achievement granted.");
                DeathsRegardsAchievement.Instance.Grant();
            }
        }


        private static void AddLanguageTokens()
        {
            LanguageAPI.Add("DEATHSITEM_NAME", "Death's Regards");
            LanguageAPI.Add("DEATHSITEM_PICKUP", "Barrier after taking high damage.");
            LanguageAPI.Add("DEATHSITEM_DESC", "Upon taking <style=cIsDamage>damage</style> to below <style=cIsDamage>30% combined health</style>, gain a <style=cIsUtility>barrier</style> for <style=cIsUtility>200%</style> of the damage dealt. Recharges after <style=cIsUtility>60 seconds</style> <style=cStack>(-33% per stack)</style>.");
            LanguageAPI.Add("DEATHSITEM_LORE", "Death seldom gives his regards, but something strange resides inside you, survivor.");
            
            LanguageAPI.Add("HEADLINE_DEATHSREGARDS_ACHIEVEMENT_NAME", "Lord Death's Regards");
            LanguageAPI.Add("HEADLINE_DEATHSREGARDS_ACHIEVEMENT_DESC", "Cheat death by using Dio's Best Friend.");
            LanguageAPI.Add("HEADLINE_DEATHSREGARDS_UNLOCKABLE_NAME", "Death's Regards");

        }
    }
}