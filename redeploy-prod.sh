#!/usr/bin/env bash

# Exit on first error
set -e

sudo supervisorctl stop vehicle-genius-api-prod || true

(cd VehicleGenius.Api && ./build.sh)

sudo supervisorctl reread
sudo supervisorctl update vehicle-genius-api-prod
sudo supervisorctl restart vehicle-genius-api-prod
