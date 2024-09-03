#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development

main () {
	set -ex
	update_database
}

update_database () {
	echo "Update database to latest migration..."
	dotnet.exe ef database update \
		--project TodoAPI \
		--startup-project TodoAPI.DbContext \
	    --configuration Development \
		--context TodoContext
}

main
