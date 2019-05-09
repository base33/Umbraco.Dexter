@echo off
set /p id="Enter ID: "

nuget pack Dexter.nuspec -Version %id%
nuget pack Dexter.Core.nuspec -Version %id%
nuget pack Dexter.Indexers.ElasticSearch.nuspec -Version %id%
nuget pack Dexter.IndexStrategies.nuspec -Version %id%