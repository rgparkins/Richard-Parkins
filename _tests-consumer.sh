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
    
    docker-compose -f docker-compose-consumer-tests.yml down
}

trap cleanup EXIT
trap failed ERR

echo
echo =============================================================================
echo  Testing Consumer
echo =============================================================================
echo

rm -f ./pacts/consumer-consumer_api.json

docker-compose -f docker-compose-consumer-tests.yml up
