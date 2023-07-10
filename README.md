# basket_api
ASP.NET Core 7 Basket API challenge.

Basic ASP.NET Core 7 API, that consumes a BasketAPI to retrieve products, creating orders and getting authorization tokens.
The API follows a standar Layered architecture, as I have used it in the pass to deploy scalable web application. 

Controllers represent our Presentation La.yer
Services represent our Application Layer.
Models represent our data entities used accross the API.

Product controller requirements are implemented.
Basket controller requirements are partially implemented as I did not want to surpass the 2 hours limit. 

Improvements & Next Steps
- Add proper logging through a commonly used Log tool
- Enforce proper response and request validation
- Remove HTTP Client hard dependencies on the ProductService implementation
- Integration test coverage for all endpoints
- Unit tests
