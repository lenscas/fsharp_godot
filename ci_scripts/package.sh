#!/bin/bash

#make the folder that will be zipped
mkdir fsglue
mkdir fsglue/glue_generator

#move the glue generator so it can be zipped
cp glue_generator/bin/Release/net8.0/publish/glue_generator.dll fsglue/glue_generator/generate_glue.dll
cp glue_generator/bin/Release/net8.0/publish/* fsglue/glue_generator/

#copy the helper scripts
cp helper_scripts/*.fs fsglue

zip -r fsglue.zip fsglue