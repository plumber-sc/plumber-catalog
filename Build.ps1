Write-Host "Building:"
Write-Host $Env:APPVEYOR_BUILD_VERSION

dotnet build .\src\Plugin.Plumber.Catalog\Plugin.Plumber.Catalog.csproj
dotnet pack .\src\Plugin.Plumber.Catalog\Plugin.Plumber.Catalog.csproj -c Release -o ..\..\artifacts /p:Version=$Env:APPVEYOR_BUILD_VERSION