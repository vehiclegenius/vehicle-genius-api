#!/usr/bin/env bash

export DOTNET_CLI_TELEMETRY_OPTOUT=1
export PATH="$PATH:$HOME/.dotnet:$HOME/.dotnet/tools"

rm -rf bin/
dotnet restore
dotnet build
