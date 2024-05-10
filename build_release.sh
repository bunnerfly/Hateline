#!/bin/sh
set -e
dotnet build src/Hateline.csproj -c Debug
rm -rf HatelinePackage Hateline.zip
mkdir HatelinePackage
cp everest.yaml HatelinePackage/everest.yaml
cp -r src/bin/Hateline.* HatelinePackage/bin
cp -r Dialog HatelinePackage
cp -r Graphics HatelinePackage
cp -r Loenn HatelinePackage

if command -v 7z &> /dev/null
then
    7z a -tzip -r ./Hateline.zip ./HatelinePackage/*
else
	( cd HatelinePackage; zip -r ../Hateline.zip * )
fi

