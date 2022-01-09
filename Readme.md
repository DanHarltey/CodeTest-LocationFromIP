# Introduction

Proposed task consists of designing and implementing a RESTful microservice:

Design a micro-service that would use any of the 3rd party location-from-IP address service.

The service must expose a REST interface with one endpoint that accepts an IP address as input parameter and returns location data

It should maintain a cache and persistent store of the looked-up values.

It should have both unit and integration tests.

No UI is needed.

**Optionally** it may use Swagger (or Postman) as a UI

There is nothing special about IP lookup – you may chose some other comparable 3rd party service that would look something up – say, weather from location.

We expect the service to be implemented in C#/Net 5/6.