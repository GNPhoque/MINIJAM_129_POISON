using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class BuildManager : MonoBehaviour
{
    static string VERSION_NO = "BUILD_12_CONTROLS";

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
        // Set the output path based on the platform
        string outputPath = "";
        switch (target)
        {
            case BuildTarget.Android:
                outputPath = "Builds/Android/" + VERSION_NO;
                break;
            case BuildTarget.iOS:
                outputPath = "Builds/iOS/" + VERSION_NO;
                break;
            case BuildTarget.StandaloneWindows:
                outputPath = "Builds/WINDOWS/" + VERSION_NO;
                break;
            case BuildTarget.StandaloneOSX:
                outputPath = "Builds/Mac/" + VERSION_NO;
                break;
            case BuildTarget.WebGL:
                outputPath = "Builds/WEBGL/" + VERSION_NO;
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

        // Set the build options
        BuildOptions buildOptions = BuildOptions.None;

        // Build the player and save the output to the specified path
        string buildPath = Path.Combine(outputPath, GetBuildName(target));
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = GetBuildScenes();
        buildPlayerOptions.target = target;
        buildPlayerOptions.options = buildOptions;
        buildPlayerOptions.locationPathName = buildPath;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
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
            case BuildTarget.WebGL:
                return "MyGame.html";
            // Add more cases for other platforms if needed
            default:
                Debug.LogError("Unsupported build target: " + target);
                return null;
        }
    }
}