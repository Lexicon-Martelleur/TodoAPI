#!/bin/bash

export ASPNETCORE_ENVIRONMENT=Development
MIGRATION=$1

main () {
	set -ex
	check_input
	add_migration $MIGRATION
}

check_input () {
	if [ -z "$MIGRATION" ]; then
  		echo "No argument for migration name provided"
		echo "Usage $0 <MIGRATION_NAME>"
  		exit 1
	fi
}

add_migration () {
	echo "Building migration '$1'..."
	dotnet.exe ef migrations add $1 \
		--project TodoAPI/ \
		--startup-project TodoAPI/ \
	    --configuration Development \
		--context TodoContext
}

main
