param(
	[bool] $build = $true,
	[string[]] $configurations = @('Debug', 'Release'),
	[bool] $publish = $false,
	[string] $msBuildVerbosity = 'minimal'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$scriptPath = [IO.Path]::GetDirectoryName($MyInvocation.MyCommand.Definition)
$repoPath = Resolve-Path (Join-Path $scriptPath '..')
$slnPath = Get-ChildItem -Path $repoPath -Filter *.sln
$productName = [IO.Path]::GetFileNameWithoutExtension($slnPath)

function GetXmlPropertyValue($fileName, $propertyName)
{
	$result = Get-Content $fileName |`
		Where-Object {$_ -like "*<$propertyName>*</$propertyName>*"} |`
		ForEach-Object {$_.Replace("<$propertyName>", '').Replace("</$propertyName>", '').Trim()}
	return $result
}

if ($build)
{
	foreach ($configuration in $configurations)
	{
		# Restore NuGet packages first
		msbuild $slnPath /p:Configuration=$configuration /t:Restore /v:$msBuildVerbosity /nologo
		msbuild $slnPath /p:Configuration=$configuration /v:$msBuildVerbosity /nologo
	}
}

if ($publish)
{
	$version = GetXmlPropertyValue "$repoPath\src\Directory.Build.props" 'Version'
	$published = $false
	if ($version)
	{
		$artifactsPath = "$repoPath\artifacts"
		if (Test-Path $artifactsPath)
		{
			Remove-Item -Recurse -Force $artifactsPath
		}

		$ignore = mkdir $artifactsPath
		if ($ignore) { } # For PSUseDeclaredVarsMoreThanAssignments

		foreach ($configuration in $configurations)
		{
			if ($configuration -like '*Release*')
			{
				Write-Host "Publishing version $version $configuration profiles to $artifactsPath"
				$profiles = @(Get-ChildItem -r "$repoPath\src\**\Properties\PublishProfiles\*.pubxml")
				foreach ($profile in $profiles)
				{
					$profileName = [IO.Path]::GetFileNameWithoutExtension($profile)
					Write-Host "Publishing $profileName"

					# The Publish target in "C:\Program Files\dotnet\sdk\3.1.101\Sdks\Microsoft.NET.Sdk\targets\Microsoft.NET.Sdk.CrossTargeting.targets"
					# throws an exception if the .csproj uses <TargetFrameworks>. We have to override that and force a specific <TargetFramework> instead.
					$targetFramework = GetXmlPropertyValue $profile 'TargetFramework'
					msbuild $slnPath /t:Publish /p:PublishProfile=$profileName /p:TargetFramework=$targetFramework /v:$msBuildVerbosity /nologo /p:Configuration=$configuration

					Remove-Item "$artifactsPath\$profileName\*.pdb"

					Compress-Archive -Path "$artifactsPath\$profileName\*" -DestinationPath "$artifactsPath\$productName-Portable-$version-$profileName.zip"
					$published = $true
				}
			}
		}
	}

	if ($published)
	{
		Write-Host "`n`n****** REMEMBER TO ADD A GITHUB RELEASE! ******"
	}
}
