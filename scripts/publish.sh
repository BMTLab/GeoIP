#!/bin/bash

while getopts c:d:e: option
do
	case "${option}" in
		c) CONF=${OPTARG};;
		d) DEST=${OPTARG};;
		e) CLEAN=${OPTARG};;
    *) exit;;
	esac
done

if [[ ! -z $CLEAN ]]; then
	. clean.sh
fi

cd ..
CURD=$(pwd)
echo "Current directory: $CURD"

if [[ ! -z $DEST ]]; then
    if [[ $DEST == /* ]]; then
		DEST=$DEST 
	else
		DEST="$CURD/$DEST"
	fi
else
    DEST="$CURD/build"
fi
echo "Destination: $DEST"


if [[ -z $CONF ]]; then
    CONF="Production"
fi

echo "Publishing projects" || echo "Configuration: $CONF"
cd src/Server || exit;

if [[ $CONF == "Production" ]]; then	
	dotnet publish \
    -c Release \
    -o $DEST
fi
