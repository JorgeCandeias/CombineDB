# install minicover
dotnet restore

# instrument assemblies inside 'test' folder to detect hits for source files inside 'src' folder
dotnet minicover instrument --workdir ../ --assemblies test/**/bin/**/*.dll --sources src/**/*.cs

# rem Reset hits count in case minicover was run for this project
dotnet minicover reset --workdir ../

$Location = Get-Location
cd ..

Get-ChildItem -Filter "*tests*.csproj" -Recurse | %{
    dotnet publish $_.FullName
    dotnet test $_.FullName
}

cd $Location

# uninstrument assemblies, it's important if you're going to publish or deploy build outputs
dotnet minicover uninstrument --workdir ../

# create html reports inside folder coverage-html
dotnet minicover htmlreport --workdir ../ --threshold 90

# print console report
# this command returns failure if the coverage is lower than the threshold
dotnet minicover report --workdir ../ --threshold 90
