using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildManager : MonoBehaviour
{
    static string[] GetBuildScenes()
    {
        List<string> buildScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (scene.enabled)
            {
                buildScenes.Add(scene.path);
            }
        }
        return buildScenes.ToArray();
    }

    [MenuItem("Build/Build All Platforms")]
    public static void BuildAllPlatforms()
    {
        BuildForPlatform(BuildTarget.StandaloneWindows);
        BuildForPlatform(BuildTarget.WebGL);
    }

    public static void BuildForPlatform(BuildTarget target)
    {
        BuildPlayerOptions options = new BuildPlayerOptions();
        options.scenes = GetBuildScenes();
        options.target = target;
        options.options = BuildOptions.None;

        // Set the output path based on the platform
        string outputPath = "";
        switch (target)
        {
            case BuildTarget.Android:
                outputPath = "Builds/Android";
                break;
            case BuildTarget.iOS:
                outputPath = "Builds/iOS";
                break;
            case BuildTarget.StandaloneWindows:
                outputPath = "Builds/Windows";
                break;
            case BuildTarget.StandaloneOSX:
                outputPath = "Builds/Mac";
                break;
            // Add more cases for other platforms if needed
            default:
                Debug.LogError("Unsupported build target: " + target);
                return;
        }

        // Create the output directory if it doesn't exist
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }

        // Build the player and save the output to the specified path
        string buildPath = Path.Combine(outputPath, GetBuildName(target));
        BuildPipeline.BuildPlayer(options, buildPath, target);
    }

    static string GetBuildName(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "MyGame.apk";
            case BuildTarget.iOS:
                return "MyGame.ipa";
            case BuildTarget.StandaloneWindows:
                return "MyGame.exe";
            case BuildTarget.StandaloneOSX:
                return "MyGame.app";
            // Add more cases for other platforms if needed
            default:
                Debug.LogError("Unsupported build target: " + target);
                return null;
        }
    }
}