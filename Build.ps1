Write-Host "Building:"
Write-Host $Env:APPVEYOR_BUILD_VERSION

dotnet build .\src\Plugin.Plumber.Component\Plugin.Plumber.Component.csproj
dotnet pack .\src\Plugin.Plumber.Component\Plugin.Plumber.Component.csproj -c Release -o ..\..\artifacts /p:Version=$Env:APPVEYOR_BUILD_VERSION