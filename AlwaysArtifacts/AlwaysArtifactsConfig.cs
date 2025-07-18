using System.Collections.Generic;
using BepInEx.Configuration;
using RoR2;

namespace AlwaysArtifacts;

public static class AlwaysArtifactsConfig
{
    public static readonly Dictionary<ArtifactIndex, ConfigEntry<bool>> ArtifactToggles =
        new Dictionary<ArtifactIndex, ConfigEntry<bool>>();
    private static ConfigFile _config;

    private static void EnumerateArtifacts(On.RoR2.ArtifactCatalog.orig_Init orig)
    {
        orig();
        foreach (var artifact in ArtifactCatalog.artifactDefs)
        {
            
            var artifactName = Language.GetString(artifact.nameToken);
            Log.LogInfo($"Found artifact: {artifactName}");
            ArtifactToggles.Add(
                artifact.artifactIndex,
                _config.Bind("Artifact Toggles", $"Force-enable {artifactName}", false)
            );
        }
    }
    
    public static void Init(ConfigFile config)
    {
        _config = config;
        On.RoR2.ArtifactCatalog.Init += EnumerateArtifacts;
    }
}