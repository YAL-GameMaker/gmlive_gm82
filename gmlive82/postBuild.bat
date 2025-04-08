@echo off
set dllPath=%~1
set solutionDir=%~2
set projectDir=%~3
set arch=%~4
set config=%~5

echo Running post-build for %config%

set gmlDir82=%solutionDir%gmlive_test
copy /Y "%dllPath%" "%gmlDir82%\gmlive.dll"
