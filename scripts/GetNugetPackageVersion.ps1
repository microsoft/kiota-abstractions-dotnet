# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

<#
.Synopsis
    Get the NuGet package version.
.Description
    Get the NuGet package version and write the package version to an environment
    variable named VERSION_STRING in the Azure DevOps release environment.
    VERSION_STRING is used to name a tag for setting a GitHub release. This
    script assumes that the NuGet package has been named with correct version number.

    Also sets a variable named IS_PRE_RELEASE based on whether a preview package is being published.
    
.Parameter packageDirPath
    Specifies the fully qualified path to the NuGet package directory.
#>

Param(
    [string]$packageDirPath
)

Write-Host "Get the NuGet package version and set it in the global variable: VERSION_STRING" -ForegroundColor Magenta

$nugetPackageName = (Get-ChildItem (Join-Path $packageDirPath "Microsoft.Kiota.Abstractions*.nupkg")).Name

Write-Host "Found NuGet package: $nugetPackageName" -ForegroundColor Magenta

## Extracts the package version from nupkg file name.
$packageVersion = $nugetPackageName.Replace("Microsoft.Kiota.Abstractions.", "").Replace(".nupkg", "")

Write-Host "##vso[task.setvariable variable=VERSION_STRING]$($packageVersion)";
Write-Host "Updated the VERSION_STRING environment variable with the package version value '$packageVersion'." -ForegroundColor Green

$isPrerelease = $packageVersion.Contains("preview")
Write-Host "##vso[task.setvariable variable=IS_PRE_RELEASE]$($isPrerelease)";
Write-Host "Updated the IS_PRE_RELEASE environment variable with the pre-release value '$isPrerelease'." -ForegroundColor Green