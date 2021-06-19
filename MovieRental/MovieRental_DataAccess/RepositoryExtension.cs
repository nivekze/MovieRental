using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieRental_AWSSecretManager;
using MovieRental_DataAccess.Context;

namespace MovieRental_DataAccess
{
    public static class RepositoryExtension
    {
        public static void SetDefaultModelProperty<T>(string field, string defaultValue, ref ModelBuilder model) where T : class
        {
            model.Entity<T>()
                .Property(field)
                .HasDefaultValueSql(defaultValue);
        }

        public static void SetIdentityModelProperty<T>(string field, ref ModelBuilder model) where T : class
        {
            model.Entity<T>()
               .Property(field)
               //.UseSqlServerIdentityColumn()
               .Metadata.SetBeforeSaveBehavior(PropertySaveBehavior.Ignore);
        }

        //public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        //{
        //    var connectionString = configuration.GetConnectionString("DefaultConnection");
        //    var migrationsAssembly = typeof(MovieRentalContext).GetTypeInfo().Assembly.GetName().Name;
        //    services.AddDbContext<MovieRentalContext>(options =>
        //    {
        //        options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
        //        {
        //            sqlOptions.MigrationsAssembly(migrationsAssembly);
        //            sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
        //        });
        //        //options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
        //    });
        //}

        public static void AddMySqlDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("MySqlConnection2");
            var migrationsAssembly = typeof(MovieRentalContext).GetTypeInfo().Assembly.GetName().Name;
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 20));
            services.AddDbContext<MovieRentalContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion, mySqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                });
                //options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });
        }

        public static void AddMySqlDbContext(this IServiceCollection services, IConfiguration configuration, DbSecrectConnetion dbSecret)
        {
            var connectionString = BuildConnectionString(configuration.GetConnectionString("MySqlConnection"), dbSecret);
            var migrationsAssembly = typeof(MovieRentalContext).GetTypeInfo().Assembly.GetName().Name;
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 20));
            services.AddDbContext<MovieRentalContext>(options =>
            {
                options.UseMySql(connectionString, serverVersion, mySqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(migrationsAssembly);
                });
                //options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
            });
        }

        private static string BuildConnectionString(string baseConnection, DbSecrectConnetion dbSecrect) {
            return baseConnection.Replace("{HOST}", dbSecrect.host)
                .Replace("{PORT}", dbSecrect.port.ToString())
                .Replace("{USERNAME}", dbSecrect.username)
                .Replace("{PASSWORD}", dbSecrect.password);
        }

    }
}
