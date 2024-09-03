#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

main () {
	set -ex
	remove_latest_migration
}

remove_latest_migration () {
	echo "Removing latest migration..."
	dotnet.exe ef migrations remove \
		--project TodoAPI.DbContext \
		--startup-project TodoAPI \
	    --configuration Development \
		--context TodoContext
}

main
