using AopLibrary.Common;
using AopLibrary.Extension;
using AopLibrary.Filter;
using AopLibrary.Intercepts;
using Autofac;
using Castle.DynamicProxy;
using Dapper;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog.Extensions.Logging;
using Npgsql;
using System.Data;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);


// NLogを使用する
NLogConfig.Initialize();
builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddNLog();
});

// Add services to the container.
// ControllerにFilterを使用する(ControllerのAOP)
builder.Services.AddControllers(options =>
{
    // 開始終了ログのFilter
    options.Filters.Add(typeof(LoggerActionFilter));
    // 異常処理のFilter
    options.Filters.Add(typeof(ExceptionHandlingFilter));
}

    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DBの自動マッピング設定
DefaultTypeMap.MatchNamesWithUnderscores = true;

// DbConnectionの登録
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

// HTTPSでレスポンス圧縮を設定する
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

// DIのインスタンスの登録とAOPの設定
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    // DbConnectionの登録
    containerBuilder.Register<IDbConnection>((ctx) =>
    {
        var target = new NpgsqlConnection(Environment.GetEnvironmentVariable("Connection_PGSQL"));
        // var target = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres;");
        var proxyGenerator = ctx.Resolve<IProxyGenerator>();
        var proxy = ctx.Resolve<DbConnectionIntercept>();
        return proxyGenerator.CreateClassProxyWithTarget<DbConnection>(target, proxy);
    });

    // XXXの登録
    // containerBuilder.RegisterType<ProxyGenerator>.As<IProxyGenerator>().EnableInterfaceInterceptors();
});

// Controller/Service/RepositoryのDI自動登録
// 共通AOPのInterceptorの設定
builder.Host.RegisterAllTypeAsSingle(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
