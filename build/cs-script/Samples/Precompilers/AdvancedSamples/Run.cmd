echo off

echo -----------------------------------------
echo Executing classless script...
cscs /nl classless argument1

echo -----------------------------------------
echo Executing script with C++ style #define...
cscs /nl CPPHashDef

echo -----------------------------------------
echo Executing DSL script...
cscs /nl freestyle_dsl argument1
pause

