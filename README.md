<h1 align="center">URL Shortener</h1>

## Development Setup

### Prerequisites
- Install [Node.js] which includes [Node Package Manager][npm]
- Install [.dotnet] version 7
- VisualStudio or VisualStudio Code or Rider

### Setting Up a Project

It is two projects Angular and .NET7 API and both should be running

For API:
1. open `URLShortener.sln`
2. run `URLShortener.WebApi` project

For Angular:
```
cd URLShortener.Ngx
npm install
ng serve --open
```

## Documentation

### Input
Let’s assume we have 15M new URL shortenings per month, so  in 5 years (60 months), there will be a total of 15 million x 60 = 900 million records.

### Data Capacity Allocation
- Id: 16 bytes (for a Guid)   
- OriginalUrl: 4000 bytes (assuming each character takes 2 byte)   
- ShortUrl: 14 bytes (assuming each character takes 2 byte)   
- CreatedAt: 8 bytes (for a DateTimeOffset)

The total number of bytes for the Url model would be: 16 + 4000 + 14 + 8 = 4038 bytes.   
Cause we use CosmosDB, it has a minimum size of 1KB per document, then we have ~5.038KB per record.   
So overall used space would be approximately 61.57 GB per month, and in let's say 5 years it would be 61.57 * 12 * 5 = 37494.4GB

### URL shortening algorithm
MD5 Encoding and First 7 chars

Based on the calculation and requirements in the document I would suggest using Azure service for this URL shortener
- Azure Cosmos DB: This is a globally distributed, multi-model database service that can be used to store the long and short URLs. It supports horizontal scaling and allows you to scale the throughput and storage of your database as needed.   
	- The CosmosDB limitations:   
      - Document size: The maximum size of a document in a Cosmos DB container is 2 megabytes (MB).
      - Maximum throughput: The maximum provisioned throughput that can be set on a container is 100,000 request units per second (RU/s).
      - Maximum number of documents: The maximum number of documents that can be stored in a container is currently set at 10 billion, but this number is subject to change.
- Azure Static Web Apps is a service that allows you to easily build and deploy static web applications, such as those built with Angular, React, or Vue.js.
  - It can make sense to host the Angular app and API in different services for a few reasons:
      - Scalability: If the Angular app and API are hosted separately, it's easier to scale each service independently to meet the demands of the app.
      - Security: Hosting the Angular app and API separately can provide an additional layer of security by isolating the client-side code from the server-side code.
      - Development: Hosting the Angular app and API separately can make it easier for different teams to work on the front-end and back-end code simultaneously.
      - Deployment: Hosting the Angular app and API separately can make it easier to deploy changes to the Angular app and API independently, without affecting the other service.
- Azure App Service: This is a fully managed platform for building and hosting web apps. You can use this service to host your web application and take advantage of features such as automatic scaling and load balancing.
- Azure Redis Cache: This is a fully managed, in-memory cache service that can be used to store the URLs and fast lookups.
- Azure Log (Application Insights)

### User Flows
- A user inputs a long URL on the Angular web app that is hosted on Azure Static Web Apps.
- The Angular app sends a request to the API, which is hosted on Azure App Service, to shorten the URL.
- The API, built with .NET Core, uses the MD5 Encoding and First 7 chars algorithm to generate a short URL.
- The API then checks Azure Redis Cache to see if the original URL has already been shortened and stored in the cache.
- If the original URL is found in the cache, the API returns the previously generated short URL to the Angular app.
- If the original URL is not found in the cache, the API creates a new document in Azure Cosmos DB that contains the original URL, the short URL, and the creation date.
- The API also stores the original URL and its corresponding short URL in Azure Redis Cache for fast lookups in the future.
- The API returns the newly generated short URL to the Angular app.
- The Angular app displays the short URL to the user.
- The user can then share the short URL with others.
- When a user clicks on the short URL, we sent a request to the API to get the original URL associated with the short URL and redirect to that URL.
- The API looks up the short URL in Azure Redis Cache to find the corresponding original URL.
- If the short URL is found in the cache, the API redirects to the original URL.
- If the short URL is not found in the cache, the API queries Azure Cosmos DB to find the corresponding original URL.
- The API redirects to the original URL.


 

 
