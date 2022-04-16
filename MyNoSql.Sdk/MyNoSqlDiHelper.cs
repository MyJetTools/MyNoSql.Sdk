using Microsoft.Extensions.DependencyInjection;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;
using MyNoSqlServer.DataWriter;

namespace MyNoSql.Sdk;

public static class MyNoSqlDiHelper
{
    public static void CreateAndRegisterMyNoSqlClient(this IServiceCollection builder, Func<string> readerHostPort,
        string appName)
    {
        var myNoSqlClient = new MyNoSqlTcpClient(readerHostPort, appName);

        builder.AddSingleton(myNoSqlClient);
    }

    public static void StartMyNoSqlClient(this IServiceProvider serviceProvider)
    {
        var noSqlClient = serviceProvider.GetService<MyNoSqlTcpClient>();
        if (noSqlClient == null)
        {
            throw new NullReferenceException(
                "Cant start nosql client, not found. Use CreateAndRegisterMyNoSqlClient method first");
        }

        noSqlClient.Start();
    }


    public static IMyNoSqlServerDataWriter<T> RegisterMyNoSqlWriter<T>(this IServiceCollection builder,
        Func<string> writerUrl, string tableName, bool persist = true,
        DataSynchronizationPeriod dataSynchronizationPeriod = DataSynchronizationPeriod.Sec5)
        where T : IMyNoSqlDbEntity, new()
    {
        var writer = new MyNoSqlServerDataWriter<T>(writerUrl, tableName, persist, dataSynchronizationPeriod);
        builder.AddSingleton<IMyNoSqlServerDataWriter<T>>(writer);

        return writer;
    }

    public static IMyNoSqlServerDataReader<T> RegisterMyNoSqlReader<T>(this IServiceCollection builder,
        IMyNoSqlSubscriber client, string tableName) where T : IMyNoSqlDbEntity, new()
    {
        var reader = new MyNoSqlReadRepository<T>(client, tableName);
        builder.AddSingleton<IMyNoSqlServerDataReader<T>>(reader);
        return reader;
    }
}