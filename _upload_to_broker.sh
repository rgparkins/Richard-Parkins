#!/usr/bin/env bash

curl -v -XPUT -H "Content-Type: application/json" \
-d@./pacts/consumer-consumer_api.json \
http://Richards-MacBook-Pro-98861.local:32792/pacts/provider/Consumer%20API/consumer/Consumer/version/1.0.2