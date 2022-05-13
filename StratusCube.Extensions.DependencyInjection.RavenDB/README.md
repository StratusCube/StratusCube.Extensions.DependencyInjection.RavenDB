
# StratusCube.Extensions.DependencyInjection.RavenDB

Configure an instance of [RavenDB's `IDocumentStore`](https://ravendb.net/docs/article-page/5.3/Csharp/client-api/creating-document-store) to `IServiceCollection` 

## Getting Started

Add the following package from [nuget]()

```bash
dotnet add package StratusCube.Extensions.DependencyInjection.RavenDB
```

The `ServiceCollectionExtensions` class resides in the `Microsoft.Extensions.DependencyInjection` namespace.
In most new web projects the namespace will not need to be specified in the Program.cs file.

## Usage

The following example shows how to add an instance of `IDocumentStore` to
an `IServiceCollection`. See comments within the code example for the available
options.

```csharp
// Add services to the container. .//

builder.Services.AddDocumentStore(options =>
     //. can configure settings though fluent builder .//
     options.ConfigureSettings(builder =>
        builder.AddUrl("http://localhost:8080")
            .ConfigureDatabaseName("SampleApi")
     )
    //. can configure settings though an IConfiguration .//
    //.ConfigureSettings(builder.Configuration.GetSection("DocumentStoreSettings"))
    //. can configure settings though action .//
    //.ConfigureSettings(settings => {
    //    settings.DatabaseName = "SampleApi";
    //    settings.Urls = new[] { "http://localhost:8080" };
    //})
    .ConfigureConventions(conventions => { 
        /* configure conventions */ 
    }) ,
    preInitialization: store => { 
        /* pre document initialization can go here. */ 
    } ,
    postInitialization: async store => {
        //. post document initialization can go here. .//
        //. perhaps attach subscriptions? .//
        await store.Subscriptions.CreateAsync<object>();
        //. subscribe through the change api
        //. store.Changes().ForAllDocuments();
    }
//. Can add the certificate as follows! (there are many more overloads)! .//
//. AddCertificate("thumprint", StoreLocation.CurrentUser)
).AddAsyncDocumentSession();
//. Can add a regular document session with the below function! .//
//.AddDocumentSession();
```

### Additional Resources

- [RavenDB](https://ravendb.net/)