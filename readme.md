# MyNoSql.Sdk

**A simple library for DotNet [MyNoSql](https://github.com/MyJetTools/MyNoSqlServer) DB  integration.**

**Client library:**
* ![MyNoSqlServer.DataReader](https://img.shields.io/nuget/v/MyNoSql.Sdk?label=MyNoSql.Sdk)


Setup and run client:
````c#
using MyNoSql.Sdk;

var builder = WebApplication.CreateBuilder(args);

builder.Services.CreateAndRegisterMyNoSqlClient(() => "http://mynosqlserver:5152", "TestApp");
var app = builder.Build();

app.Services.StartMyNoSqlClient();

app.MapGet("/", () => "Hello World!");

app.Run();
````

Register reader:

````c#
using MyNoSql.Sdk;

var builder = WebApplication.CreateBuilder(args);

var client = builder.Services.CreateAndRegisterMyNoSqlClient(() => "http://mynosqlserver:5152", "TestApp");
builder.Services.RegisterMyNoSqlReader<MyNoSqlEntity>(client, "my-table-name");

var app = builder.Build();

app.Services.StartMyNoSqlClient();

app.MapGet("/", () => "Hello World!");

app.Run();
````


Register writer

````c#
using MyNoSql.Sdk;
using MyNoSqlServer.Abstractions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterMyNoSqlWriter<MyNoSqlEntity>(() => "http://mynosqlserver:5155", "my-table-name", true,
    DataSynchronizationPeriod.Immediately);
var app = builder.Build();

app.Services.StartMyNoSqlClient();
app.MapGet("/", () => "Hello World!");
app.Run();
````

