#!/usr/bin/env bash

./_tests-consumer.sh

./_upload_to_broker.sh

./_tests-service.sh

