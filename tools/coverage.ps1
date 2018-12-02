cd tools
dotnet restore
dotnet minicover instrument --workdir ../ --assemblies test/**/bin/**/*.dll --sources src/**/*.cs
dotnet minicover reset --workdir ../
cd ..\test
gci -Filter *.csproj -Recurse | %{
    dotnet test $_.FullName
}
cd ..\tools
dotnet minicover uninstrument --workdir ../
dotnet minicover htmlreport --workdir ../ --threshold 90
dotnet minicover report --workdir ../ --threshold 90
cd ..