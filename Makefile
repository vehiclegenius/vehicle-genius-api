.PHONY: prepare
prepare:
	cd VehicleGenius.Api && rm -rf bin/ && dotnet restore && dotnet tool restore && dotnet ef database update

.PHONY: start
start:
	make prepare
	./launch-profile.sh http
