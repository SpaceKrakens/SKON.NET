coco SKON.ATG -namespace SKON.Internal

@echo off

move /Y Parser.cs ../SKON.NET/Parser
move /Y Scanner.cs ../SKON.NET/Parser

@echo on

coco SKEMA.ATG -namespace SKON.SKEMA.Internal

@echo off

move /Y Parser.cs ../SKON.NET/SKEMA/Parser
move /Y Scanner.cs ../SKON.NET/SKEMA/Parser

exit /b