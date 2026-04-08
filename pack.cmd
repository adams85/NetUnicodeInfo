@ECHO OFF

dotnet build -c Release

dotnet pack System.Unicode\System.Unicode.csproj -c Release --no-build
