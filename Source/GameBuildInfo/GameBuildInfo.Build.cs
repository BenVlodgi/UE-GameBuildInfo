// Copyright 2025 Dream Seed LLC.

using System.Diagnostics;
using System;
using UnrealBuildTool;
using System.IO;

public class GameBuildInfo : ModuleRules
{
    public GameBuildInfo(ReadOnlyTargetRules Target) : base(Target)
    {
        PCHUsage = ModuleRules.PCHUsageMode.UseExplicitOrSharedPCHs;

        PublicDependencyModuleNames.AddRange(new string[]
            {
                "Core",
            });


        PrivateDependencyModuleNames.AddRange(new string[]
            {
                "CoreUObject",
                "Engine",
                "Slate",
                "SlateCore",
            });


        // Build Constant: UTC DateTime
        {
            string utcBuildTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ");
            PublicDefinitions.Add("BUILD_DATETIME_UTC=\"" + utcBuildTime + "\"");
        }

        // Build Constant: Perforce changelist
        {
            string p4CL = "-1";

            try
            {
                System.Console.WriteLine("GameBuildInfo.Build.cs: Looking for current Perforce changelist.");


                var p4 = new Process();
                p4.StartInfo.FileName = "p4";
                p4.StartInfo.Arguments = "changes -m1 ...#have";
                p4.StartInfo.UseShellExecute = false;
                p4.StartInfo.RedirectStandardOutput = true;
                p4.StartInfo.RedirectStandardError = true;
                p4.StartInfo.CreateNoWindow = true;

                string projectRoot = FindUProjectRoot(ModuleDirectory);
                if (projectRoot != null)
                {
                    p4.StartInfo.WorkingDirectory = projectRoot;
                    System.Console.WriteLine($"GameBuildInfo.Build.cs: Set p4 working dir to project root: {projectRoot}");
                }
                else
                {
                    System.Console.WriteLine("GameBuildInfo.Build.cs: Could not locate .uproject root. p4 may fail.");
                }

                p4.Start();
                string output = p4.StandardOutput.ReadToEnd();
                string errors = p4.StandardError.ReadToEnd();
                p4.WaitForExit();

                if (p4.ExitCode == 0 && !string.IsNullOrWhiteSpace(output))
                {
                    var parts = output.Split(' ');
                    if (parts.Length > 1)
                    {
                        p4CL = parts[1];
                        System.Console.WriteLine($"GameBuildInfo.Build.cs: Found current Perforce changelist: {p4CL}.");
                    }
                    else
                    {
                        System.Console.WriteLine("GameBuildInfo.Build.cs: couldn't find current Perforce changelist.");
                    }
                }
                else
                {
                    System.Console.WriteLine($"GameBuildInfo.Build.cs: `p4 changes` failed. Error: {errors.Trim()}");
                }
            }
            catch (Exception exception)
            {
                System.Console.WriteLine($"GameBuildInfo.Build.cs: Could not retrieve Perforce changelist. p4 may not be installed, accessible, or may not be on PATH. Exception: {exception.GetType().Name}: {exception.Message}");
            }

            PublicDefinitions.Add("BUILD_P4_REVISION=\"" + p4CL + "\"");
        }
    }

    // Walk up from module directory to find .uproject file
    static string FindUProjectRoot(string startDir)
    {
        var directory = new DirectoryInfo(startDir);
        while (directory != null)
        {
            var uprojectFiles = directory.GetFiles("*.uproject");
            if (uprojectFiles.Length > 0)
                return directory.FullName;
            directory = directory.Parent;
        }
        return null;
    }
}
