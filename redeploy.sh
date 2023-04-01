#!/usr/bin/env bash

# Exit on first error
set -e

sudo supervisorctl stop vehicle-genius-api

(cd VehicleGenius.Api && ./build.sh)

sudo supervisorctl reread
sudo supervisorctl update
sudo supervisorctl start vehicle-genius-api
