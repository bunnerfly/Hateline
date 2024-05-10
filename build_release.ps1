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
Compress-Archive -Path HatelinePackage/* -DestinationPath Hateline.zip
