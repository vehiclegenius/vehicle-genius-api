#!/usr/bin/env bash

# Exit on first error
set -e

export DOTNET_CLI_TELEMETRY_OPTOUT=1
export ASPNETCORE_ENVIRONMENT=Staging
export PATH="$PATH:$HOME/.dotnet:$HOME/.dotnet/tools"

rm -rf bin/
dotnet restore
dotnet tool restore
dotnet ef database update
dotnet build
