// Copyright 2025 Dream Seed LLC.

using System.Diagnostics;
using System;
using UnrealBuildTool;

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
                var p4 = new Process();
                p4.StartInfo.FileName = "p4";
                p4.StartInfo.Arguments = "changes -m1 //...";
                p4.StartInfo.UseShellExecute = false;
                p4.StartInfo.RedirectStandardOutput = true;
                p4.StartInfo.CreateNoWindow = true;
                p4.Start();

                string output = p4.StandardOutput.ReadToEnd();
                p4.WaitForExit();

                var parts = output.Split(' ');
                if (parts.Length > 1)
                {
                    p4CL = parts[1];
                }
            }
            catch
            {
                System.Console.WriteLine("GameBuildInfo.Build.cs: Could not retrieve Perforce changelist. P4 may not be installed, accessible, or may not be on PATH.");
            }

            PublicDefinitions.Add("BUILD_P4_REVISION=\"" + p4CL + "\"");
        }
    }
}
