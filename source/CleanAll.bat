@ECHO OFF
pushd "%~dp0"
ECHO.
ECHO.
ECHO This script deletes all temporary build files in their
ECHO corresponding BIN and OBJ Folder contained in the following projects
ECHO.
ECHO InPlaceEditBoxDemo
ECHO InplaceEditBoxLib
ECHO Solution\SolutionLib
ECHO Solution\SolutionLibModels
ECHO Services\ExplorerLib
ECHO Services\ServiceLocator
ECHO.
REM Ask the user if hes really sure to continue beyond this point XXXXXXXX
set /p choice=Are you sure to continue (Y/N)?
if not '%choice%'=='Y' Goto EndOfBatch
REM Script does not continue unless user types 'Y' in upper case letter
ECHO.
ECHO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
ECHO.
ECHO XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

ECHO.
ECHO Deleting .vs and BIN, OBJ Folders in project folder
ECHO.
RMDIR .vs /S /Q

RMDIR /S /Q InPlaceEditBoxDemo\bin
RMDIR /S /Q InPlaceEditBoxDemo\obj

RMDIR /S /Q InplaceEditBoxLib\bin
RMDIR /S /Q InplaceEditBoxLib\obj

RMDIR /S /Q Solution\SolutionLib\bin
RMDIR /S /Q Solution\SolutionLib\obj

RMDIR /S /Q Solution\SolutionLibModels\bin
RMDIR /S /Q Solution\SolutionLibModels\obj

RMDIR /S /Q Services\ExplorerLib\bin
RMDIR /S /Q Services\ExplorerLib\obj

RMDIR /S /Q Services\ServiceLocator\bin
RMDIR /S /Q Services\ServiceLocator\obj

PAUSE

:EndOfBatch
