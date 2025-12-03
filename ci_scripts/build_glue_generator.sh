#!/bin/bash

# compile the glue generator
cd glue_generator
dotnet restore
dotnet publish -c Release
cd ..

