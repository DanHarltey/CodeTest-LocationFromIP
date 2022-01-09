# The task

Proposed task consists of designing and implementing a RESTful microservice:

Design a micro-service that would use any of the 3rd party location-from-IP address service.

The service must expose a REST interface with one endpoint that accepts an IP address as input parameter and returns location data

It should maintain a cache and persistent store of the looked-up values.

It should have both unit and integration tests.

No UI is needed.

**Optionally** it may use Swagger (or Postman) as a UI

There is nothing special about IP lookup – you may chose some other comparable 3rd party service that would look something up – say, weather from location.

We expect the service to be implemented in C#/Net 5/6.

## Completion Notes

* To use this, you need to add your own WeatherApi ApiKey into the config; or add it as WeatherApi__ApiKey environment variable.
* A local Redis is needed, one is setup with docker-compose in scripts\dev-dependencies.
* There is a simple GitHub action that builds the code and runs the tests.
* Solution is set up in a Clean Architecture type style, but I didn’t want to over engineer it.
* Swagger docs are at /swagger/index.html.
* I used System.Text.Json for json as I wanted to play around with the new source code generator as I hadn’t used them yet. It was really easy.

I had limited time so there are quite a few things I would improve given more time: 
* The integration tests use the live WeatherApi service. I would have preferred to mock this out with something like wiremock.
* I added a Polly Policy to the http client as a talking point. The values are hard coded. In a real situation these would be in config and adjusted by viewing metrics.
* In ConfigureHttpClient I alter the default config as a talking point.
* There are no health checks, so I would add these. The addition of a health check url would ensure smoother operating of a production system.
* There are no security headers CORS, CSP, HSTS, so these should be added.
* There is no auth.
* There are warnings I would like to fix and changes I would like to make within the nullability of properties.
* Add docker support.
* the caching was a bit rushed; so basic but it works. It has hard coded connection strings and config values. I would always place this into config/secret store.
* If we are caching, I would also add the caching Http headers to allow clients to cache as well.
* I would normally place a Polly circuit breaker around the cache. This will allow the service to continue if the cache goes down. However, must be careful that this would not cause the downstream service to fail with the extra load and cause cascading failure.
* The key for the cache is based on user input, so 02.02.02.02 will not match 2.2.2.2. I would use the class IpAddress to correct this. 
 * there is a list of exceptions that are not handled in ExecuteRequest.
