#!/usr/bin/env bash

# Exit on first error
set -e

sudo supervisorctl stop vehicle-genius-api || true

(cd VehicleGenius.Api && ./build.sh)

sudo supervisorctl reread
sudo supervisorctl update vehicle-genius-api
sudo supervisorctl restart vehicle-genius-api
