using AopLibrary.Intercepts;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AopLibrary.Extension;

public static class HostBuilderExtensions
{
    public static IHostBuilder RegisterAllTypeAsSingle(this IHostBuilder host, Type assmblyObj)
    {
        // AutofacのProviderFacotryを登録する
        host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        // AutofacのDIですべてControllers/Services/Reposiitoryを登録する
        host.ConfigureContainer<Autofac.ContainerBuilder>((hostBuilderContext, containerBuilder) =>
        {
            // SingleInstance()は性能改善のために、Singleインスタンスに変更する
            // 開始終了ログのAOPの設定
            containerBuilder.RegisterType<LoggerIntercept>().SingleInstance();
            // 開始終了ログのAOPの設定
            containerBuilder.RegisterType<ProxyGenerator>().As<IProxyGenerator>().SingleInstance();
            // SQL文とパラメータログ出力のAOPの設定
            containerBuilder.RegisterType<SqlLoggerIntercept>().SingleInstance();
            // DB接続のAOPの設定
            containerBuilder.RegisterType<DbConnectionIntercept>().SingleInstance();

            var assemblyType = assmblyObj.GetTypeInfo();

            // AssemblyからControllerのインターフェースを一括登録する
            containerBuilder.RegisterAssemblyTypes(assemblyType.Assembly).Where(t => t.FullName!.Contains($"Controllers")).PropertiesAutowired();

            // AssemblyからServicesのインターフェースを一括登録し、LoggerInterceporを自動適用する
            containerBuilder.RegisterAssemblyTypes(assemblyType.Assembly).Where(t => t.FullName!.Contains($"Services")).AsImplementedInterfaces().EnableInterfaceInterceptors().PropertiesAutowired().InterceptedBy(typeof(LoggerIntercept));

            // AssemblyからRepositoryのインターフェースを一括登録し、LoggerInterceporを自動適用する
            containerBuilder.RegisterAssemblyTypes(assemblyType.Assembly).Where(t => t.FullName!.Contains($"Repositories")).AsImplementedInterfaces().EnableInterfaceInterceptors().PropertiesAutowired().InterceptedBy(typeof(LoggerIntercept));


        });

        return host;
    }
}

