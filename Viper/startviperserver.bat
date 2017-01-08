@echo off
SetLocal EnableDelayedExpansion

REM ======== Basic configuration ========

set BASE_DIR=%~dp0
set MAIN=com.martiansoftware.nailgun.NGServer

REM ======== Arguments ========

set SILICON_CP=silicon.jar
set CARBON_CP=carbon.jar
set FWD_ARGS=

:parse_args
if not %1.==. (
  if /I %1.==silicon. (
		set SILICON_CP=%2
		shift
		shift
  ) else if /I %1.==carbo. (
		set CARBON_CP=%2
		shift
		shift
	) else (
		set FWD_ARGS=%FWD_ARGS% %1
		shift
	)

	goto :parse_args
)

REM ======== Validation ========

if not exist %SILICON_CP% (
  echo %CD%
  echo Error: Cannot find %SILICON_CP%
  goto exit
)
if not exist %CARBON_CP% (
  echo %CD%
  echo Error: Cannot find %CARBON_CP%
  goto exit
)

REM ======== Path-dependent configuration ========

set JAVA_EXE=java
set CP=%BASE_DIR%\nailgun-server-0.9.1.jar
set CP=%CP%;%SILICON_CP%;%CARBON_CP%

REM ======== Java ========

set JVM_OPTS=-Xss64m
set MAIN_OPTS=
set CMD=%JAVA_EXE% %JVM_OPTS% -cp "%CP%" %MAIN% %MAIN_OPTS% %FWD_ARGS%

REM ======== Executing  ========

REM echo.
REM echo %CMD%
REM echo.

call %CMD%

:exit
exit /B 0
