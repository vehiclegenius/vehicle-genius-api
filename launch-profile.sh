#!/usr/bin/env bash

set -e

cd VehicleGenius.Api

if [ -z "$1" ]; then
    echo 'Please provide a launch profile name:'
    echo '  ./launch-profile.sh <profile-name>'
    echo ''
    if [ "$(which jq)" != 'jq not found' ]; then
        echo 'Available profiles:'
        echo ''
        jq -r '.profiles | keys | .[]' Properties/launchSettings.json
    fi
    exit 1
fi

export PATH="$PATH:$HOME/.dotnet"

dotnet run --launch-profile $1
