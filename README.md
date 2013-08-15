# Cognition

An ASP.NET MVC Web app to manage structured data such as profiles or knowledge base articles. A cross between a wiki, CMS and database app.

This is built as an experiment with modern .NET tools and frameworks as of August 2013:

* .NET 4.5
* async/await as much as possible
* MVC5
* SignalR 2.0 beta
* Entity Framework 6 beta
* CouchDB via the awesome MyCouch library: https://github.com/danielwertheim/mycouch
* Bootstrap 3 RC2

## Features

* A CouchDB-based document store
* Supports a type-based system of dynamic document types. Currently these are statically compiled into the app but could be extended to support run-time design.
* Markdown support for string properties with the ```[DataType(DataType.MultilineText)]``` attribute
* View previous versions and supports reverting
* A simple token-based search across document titles
* Permissions system to restrict the following to anonymous users, registered users, email address or email domain:
  * Public documents
  * Internal documents (default)
  * Registration
* Google single sign on support
* Live updating of document pages and update feed

## Planned features
* Comments on documents
* Change Subscription to documents for notifications
* Full text searching
* Advanced search on document properties
* Document collections
* Parent/Child relationships
* Document reference fields
* File attachments
* Image drag and drop

... and lots more!

Developed by Ed Andersen (eandersen at mdsol.com). Only made possible by Innovation Time programme at Medidata Solutions. We are hiring!

Licensed under MIT.
