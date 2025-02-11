using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;
using ModelReplacement;
using BepInEx.Configuration;
using System;
using System.Xml.Linq;

namespace ModelReplacement
{
    [BepInPlugin("com.raiden.lemressuit", "Lemres Suit", "1.0.0")]
    [BepInDependency("meow.ModelReplacementAPI", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static ConfigFile config;

        // Example Config for single model mod
        public static ConfigEntry<bool> enableModelForAllSuits { get; private set; }

        public static ConfigEntry<bool> enableModelAsDefault { get; private set; }

        private static void InitConfig()
        {
            enableModelForAllSuits = config.Bind<bool>("Suits to Replace Settings", "Enable Model for all Suits", false, "Enable to model replace every suit. Set to false to specify suits");
			enableModelAsDefault = config.Bind<bool>("Suits to Replace Settings", "Enable Model as default", false, "Enable to model replace every suit that hasn't been otherwise registered.");
        }

        private void Awake()
        {
            config = base.Config;
            InitConfig();
            Assets.PopulateAssets();

            ModelReplacementAPI.RegisterSuitModelReplacement("Lemres", typeof(MRLEMRES));

            // Plugin startup logic
            if (enableModelForAllSuits.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementOverride(typeof(MRLEMRES));
            }

            if (enableModelAsDefault.Value)
            {
                ModelReplacementAPI.RegisterModelReplacementDefault(typeof(MRLEMRES));
            }

            Harmony harmony = new Harmony("com.raiden.lemressuit");
            harmony.PatchAll();
            Logger.LogInfo($"Plugin {"com.raiden.lemressuit"} is loaded!");
        }
    }

    public static class Assets
    {
        // Replace mbundle with the Asset Bundle Name from your unity project 
        public static string mainAssetBundleName = "lemresbundle";
        public static AssetBundle MainAssetBundle = null;

        private static string GetAssemblyName() => Assembly.GetExecutingAssembly().GetName().Name.Replace(" ","_");
        public static void PopulateAssets()
        {
            if (MainAssetBundle == null)
            {
                Console.WriteLine(GetAssemblyName() + "." + mainAssetBundleName);
                using (var assetStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetAssemblyName() + "." + mainAssetBundleName))
                {
                    MainAssetBundle = AssetBundle.LoadFromStream(assetStream);
                }
            }
        }
    }
}