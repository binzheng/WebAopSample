using AopLibrary.Extension;
using AopLibrary.Filter;
using AopLibrary.Intercepts;
using Autofac;
using Castle.DynamicProxy;
using Dapper;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using System.Data;
using System.Data.Common;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Controller��Filter���g�p����(Controller��AOP)
builder.Services.AddControllers(options =>
{
    // �J�n�I�����O��Filter
    options.Filters.Add(typeof(LoggerActionFilter));
    // �ُ폈����Filter
    options.Filters.Add(typeof(ExceptionHandlingFilter));
}

    );
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DB�̎����}�b�s���O�ݒ�
DefaultTypeMap.MatchNamesWithUnderscores= true;

// DbConnection�̓o�^
builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

// HTTPS�Ń��X�|���X���k��ݒ肷��
builder.Services.AddResponseCompression(options => options.EnableForHttps = true);

// DI�̃C���X�^���X�̓o�^��AOP�̐ݒ�
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => {
    // DbConnection�̓o�^
    containerBuilder.Register<IDbConnection>((ctx) => {
        var target = new NpgsqlConnection(Environment.GetEnvironmentVariable("Connection_PGSQL"));
        // var target = new NpgsqlConnection("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=postgres;");
        var proxyGenerator = ctx.Resolve<IProxyGenerator>();
        var proxy = ctx.Resolve<DbConnectionIntercept>();
        return proxyGenerator.CreateClassProxyWithTarget<DbConnection>(target, proxy);
    });

    // XXX�̓o�^
    // containerBuilder.RegisterType<ProxyGenerator>.As<IProxyGenerator>().EnableInterfaceInterceptors();
});

// Controller/Service/Repository��DI�����o�^
// ����AOP��Interceptor�̐ݒ�
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
