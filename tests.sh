#!/usr/bin/env bash

echo Starting to test the consumer

./_tests-consumer.sh

echo Finishing testing the consumer

echo Uploading to broker

./_upload_to_broker.sh

echo Finished uploading to broker

echo Starting service tests against PACT

./_tests-service.sh

echo Complete
