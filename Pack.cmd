@echo off

if not exist "%~dp0build" md "%~dp0build"

if "%1"=="" goto rebuildCurrent

nuget pack "%~dp0src\Serilog.Sinks.LINQPad\Serilog.Sinks.LINQPad.csproj" -Symbols -Build -OutputDirectory "%~dp0build" -Properties Configuration=%1
goto end

:rebuildCurrent
nuget pack "%~dp0src\Serilog.Sinks.LINQPad\Serilog.Sinks.LINQPad.csproj" -Symbols -Build -OutputDirectory "%~dp0build"
goto end

:end
