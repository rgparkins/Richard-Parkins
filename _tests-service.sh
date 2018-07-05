#!/usr/bin/env bash

failed() {
  echo "============== Tests have failed. =================="
  exit 1
}

cleanup() {
    echo
    echo =============================================================================
    echo Testing Complete
    echo =============================================================================
    echo Shutting down containers...
    
    docker-compose -f docker-compose-service-tests.yml down
}

trap cleanup EXIT
trap failed ERR

echo
echo =============================================================================
echo  Testing Service
echo =============================================================================
echo

docker-compose -f docker-compose-service-tests.yml up
