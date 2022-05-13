var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDocumentStore(options =>
     //. can configure settings though fluent builder
     options.ConfigureSettings(builder =>
        builder.AddUrl("http://localhost:8080")
            .ConfigureDatabaseName("SampleApi")
     )
    //. can add certificate via thumbprint or file .//
    //.AddCertificate("435143a1b5fc8bb70a3aa9b10f6673a8")
    //.AddCertificate("./path/to/my-cert.pfx", "password")
    //. can configure settings though an IConfiguration
    //.ConfigureSettings(builder.Configuration.GetSection("DocumentStoreSettings"))
    //. can configure settings though action
    //.ConfigureSettings(settings => {
    //    settings.DatabaseName = "SampleApi";
    //    settings.Urls = new[] { "http://localhost:8080" };
    //})
    .ConfigureConventions(conventions => { /* configure conventions */ }) ,
    preInitialization: store => { /* pre document initialization can go here. */ } ,
    postInitialization: async store => {
        //. post document initialization can go here.
        //. perhaps attach subscriptions?
        await store.Subscriptions.CreateAsync<object>();
        //. subscribe through the change api
        //. store.Changes().ForAllDocuments();
    }
//. Can add the certificate as follows! (there are many more overloads)!
//. AddCertificate("thumprint", StoreLocation.CurrentUser)
).AddAsyncDocumentSession();
//. Can add a regular document session with the below function!
//.AddDocumentSession();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
