dotnet build src/Hateline.csproj -c Debug
Remove-Item -Recurse -Path HatelinePackage
Remove-Item -Path Hateline.zip
New-Item -ItemType directory -Path HatelinePackage
Copy-Item -Path everest.yaml -Destination HatelinePackage/everest.yaml
New-Item -ItemType directory -Path HatelinePackage/bin
Copy-Item -Recurse -Path bin/Hateline* -Destination HatelinePackage/bin/
Copy-Item -Recurse -Path Dialog -Destination HatelinePackage
Copy-Item -Recurse -Path Graphics -Destination HatelinePackage
Copy-Item -Recurse -Path Loenn -Destination HatelinePackage
# this does NOT build correct zips in PowerShell 5.1
# Everest will yell at you and there's nothing you can do besides update to PowerShell 7
# or simply refuse to use PowerShell whenever possible and run build_release.sh in bash (Git for Windows Bash)
Compress-Archive -Path HatelinePackage/* -DestinationPath Hateline.zip