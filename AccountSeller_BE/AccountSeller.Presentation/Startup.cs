using AccountSeller.Application;
using AccountSeller.Presentation.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.OpenApi.Models;
using System.Globalization;
using Serilog;
using AccountSeller.Infrastructure;
using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Newtonsoft.Json;

namespace AccountSeller.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options => options.Filters.Add<ApiExceptionFilterAttribute>())
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddEndpointsApiExplorer();
            services.AddResponseCaching();

            // Application Insights
            services.AddApplicationInsightsTelemetry();
            var aiOptions = new ApplicationInsightsServiceOptions()
            {
                EnableDependencyTrackingTelemetryModule = true,
                InstrumentationKey = !string.IsNullOrEmpty(Configuration["ApplicationInsight:InstrumentationKey"])
                                    ? Configuration["ApplicationInsight:InstrumentationKey"]
                                    : string.Empty,
            };
            services.AddApplicationInsightsTelemetry(aiOptions);

            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddApplicationInsights();
            });

            services.AddHttpContextAccessor();
            services.AddHealthChecks();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAnyCorsPolicy",
                    builder => builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(_ => true)
                    .AllowCredentials()
                    .WithExposedHeaders("Content-Disposition"));
            });

            services.AddRazorPages();

            services.AddSwaggerGen(options =>
            {
                //var commitHash = Configuration["LastedCommitHash"];
                //var commitMsg = Configuration["LastedCommitMsg"];
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "AccountSeller 2023 API",
                    Version = "v1",
                    // Description = $"Commit hash: {commitHash} <br /> Commit message: {commitMsg}"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
                options.CustomSchemaIds(type => type.ToString());
            });

            services.AddMvc().AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix).AddDataAnnotationsLocalization();
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("vn"),
                    new CultureInfo("en"),
                };
                options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            services.Configure<IdentityOptions>(options =>
            {
                options.User.AllowedUserNameCharacters = null;
            });

            // Add Application project's services.
            services.AddApplicationServices();

            // Add Infrastructure project's services.
            services.AddInfrastructureServices(Configuration);
        }

        public void Configure(IServiceProvider provider, IApplicationBuilder application, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
            else
            {
                application.UseExceptionHandler("/Error");
                application.UseHsts();
            }

            application.UseCors("AllowAnyCorsPolicy");
            application.UseAuthentication();

            application.UseResponseCaching();
            application.UseResponseCompression();
            application.UseHealthChecks("/health");
            application.UseHttpsRedirection();
            application.UseStaticFiles();

            application.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "AccountSeller API v1");
            });

            application.UseRouting();
            application.UseAuthorization();
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
