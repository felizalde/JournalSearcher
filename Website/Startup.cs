using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Website.Data;
using Website.Models;
using Website.Models.Account;
using Website.Utils.Account;
using Website.VueConnection;
using Scraper;
using Website.Contracts;
using WebAPI.Services;
using WebAPI.Utils.Swagger;
using Microsoft.OpenApi.Models;
using SearchEngine;
using SearchEngine.Interfaces;
using SearchEngine.Repositories;
using Website.Utils;
using Common;
using SearchEngine.Indices;

namespace Website;


public class Startup
{

    readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
        // Adding Identity and EF 
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(Configuration.GetConnectionString("MainDB")));

        services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

        //Loading app settings and injecting
        var appSettingsSection = Configuration.GetSection("AppSettings");
        services.Configure<AppSettings>(appSettingsSection);
        services.AddSingleton(Configuration);

        // Get JWT secret and setup JWT configuration
        var appSettings = appSettingsSection.Get<AppSettings>();
        var tokenConfig = new JWTTokenConfiguration(appSettings.JWTSecret);
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = tokenConfig.GetJWTTokenValidationParameters();
        });

        //Adding support for controllers
        services.AddControllersWithViews();

        //Injecting anglesharp (from Scraper module)
        services.AddAngleSharp();

        services.AddCors(options =>
                options.AddPolicy(MyAllowSpecificOrigins,
                           builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
                           )
        );

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IUserClaimsPrincipalFactory<User>, ApplicationClaimsIndentityFactory>();
        services.AddTransient<JournalsRecommenderData>();
        services.AddTransient<IUserService, UserService>();
        services.AddSingleton<WileyScraper>();
        services.AddSingleton<ElsevierScraper>();
        services.AddSingleton<SpringerScraper>();

        //Inject Search Dependencies
        services.AddElasticsearch(Configuration);
        services.AddTransient<IJournalSearcher<JournalDocument>, JournalSearcher>();
        services.AddTransient<IJournalsRepository<JournalDocument>, JournalsRepository>();

        // In production, the Vue files will be served from this directory
        services.AddSpaStaticFiles(configuration =>
        {
            configuration.RootPath = "ClientApp/dist";
        });

        services.AddRouting(x => x.LowercaseUrls = true);

        //Swagger documentation

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", DocSwaggerHelper.ApiInfo);

            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "WebAPI.xml"));

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        });

    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Journals Recommender API v1");
            });
        }
        else
        {
            app.UseHsts();
        }

        //app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseSpaStaticFiles();

        app.UseCors(MyAllowSpecificOrigins);
        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.UseSpa(spa =>
        {
            spa.Options.SourcePath = "ClientApp";

            if (env.IsDevelopment())
            {
                spa.UseVueDevelopmentServer();
            }
        });
    }
}
