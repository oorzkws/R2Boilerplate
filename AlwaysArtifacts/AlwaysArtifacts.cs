using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace AlwaysArtifacts
{

	//This attribute specifies that we have a dependency on R2API, as we're using it to add our item to the game.
    //You don't need this if you're not using R2API in your plugin, it's just to tell BepInEx to initialize R2API before this plugin so it's safe to use R2API.
    [BepInDependency(R2API.R2API.PluginGUID)]
	
	//This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
	
	//This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class AlwaysArtifacts : BaseUnityPlugin
	{
        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        // ReSharper disable InconsistentNaming
        // ReSharper disable MemberCanBePrivate.Global
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "oorzkws";
        public const string PluginName = "AlwaysArtifacts";
        public const string PluginVersion = "1.0.2";
        // ReSharper restore MemberCanBePrivate.Global
        // ReSharper restore InconsistentNaming
        
        //The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            //Init our logging class so that we can properly log for debugging
            Log.Init(Logger);
            
            //Generate and load our config
            AlwaysArtifactsConfig.Init(Config);
            
            // Add ourselves to the run start event
            On.RoR2.RunArtifactManager.SetArtifactEnabled += (orig, self, def, newEnabled) =>
            {
	            var desiredVal = newEnabled || AlwaysArtifactsConfig.ArtifactToggles[def.artifactIndex].Value;
	            Log.LogDebug($"RunArtifactManager.SetArtifactEnabled: {Language.GetString(def.nameToken)}: {newEnabled} (o:{desiredVal})");
	            // Prevent double-enable
	            if (desiredVal)
		            orig(self, def, false);
	            orig(self, def, desiredVal);
            };
            // ...and the server run start event
			On.RoR2.RunArtifactManager.SetArtifactEnabledServer += (orig, self, def, newEnabled) =>
			{
				var desiredVal = newEnabled || AlwaysArtifactsConfig.ArtifactToggles[def.artifactIndex].Value;
				Log.LogDebug($"RunArtifactManager.SetArtifactEnabledServer: {Language.GetString(def.nameToken)}: {newEnabled} (o:{desiredVal})");
				// Prevent double-enable
				if (desiredVal)
					orig(self, def, false);
				orig(self, def, desiredVal);
			};
			
            // This line of log will appear in the bepinex console when the Awake method is done.
            Log.LogInfo(nameof(Awake) + " done.");
        }
	}
}
