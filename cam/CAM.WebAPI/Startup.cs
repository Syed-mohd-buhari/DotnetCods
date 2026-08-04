using AutoMapper;
using CAM.BusinessManager.AbstractionLayer;
using CAM.BusinessManager.Business.PlannedActivity;
using CAM.BusinessManager.CommonUtilities;
using CAM.BusinessManager.Dapper;
using CAM.BusinessManager.Entity;
using CAM.BusinessManager.Entity.BPT;
using CAM.BusinessManager.Entity.ClusterLevelPA;
using CAM.BusinessManager.Entity.ComponentSoftware;
using CAM.BusinessManager.Entity.ExodusProgram;
using CAM.BusinessManager.Entity.NetworkElementNodeCountManager;
using CAM.BusinessManager.Entity.OMC;
using CAM.BusinessManager.Entity.PassThroughData;
using CAM.BusinessManager.Entity.Pat;
using CAM.BusinessManager.Entity.PlannedActivities;
using CAM.BusinessManager.Entity.Report;
using CAM.BusinessManager.Entity.Report.ExodusAssetLevelReport;
using CAM.BusinessManager.Entity.Report.FNT_Report;
using CAM.BusinessManager.Entity.Report.GraphicalReport.AssetReport;
using CAM.BusinessManager.Entity.Report.GraphicalReport.Exodus_Graphical_Report;
using CAM.BusinessManager.Entity.Report.TSR_Report;
using CAM.BusinessManager.Entity.Report.XBom;
using CAM.BusinessManager.Entity.XBom.CBOM;
using CAM.BusinessManager.Entity.XBom.VBom;
using CAM.BusinessManager.GenericReports;
using CAM.BusinessManager.Grid;
using CAM.BusinessManager.ILookUp;
using CAM.BusinessManager.LookUp;
using CAM.BusinessManager.MapConfiguration;
using CAM.BusinessManager.MapConfiguration.ClusterLevelPA;
using CAM.BusinessManager.MapConfiguration.FNT;
using CAM.BusinessManager.MapConfiguration.OMC;
using CAM.BusinessManager.MapConfiguration.TSRNonTems;
using CAM.BusinessManager.NewPortal;
using CAM.BusinessManager.Settings;
using CAM.Contracts;
using CAM.Contracts.RepositoryContracts.Base;
using CAM.Exports;
using CAM.Identity;
using CAM.Imports;
using CAM.Infrastucture;
using CAM.Mail;
using CAM.Repository;
using CAM.ResourcesKey;
using CAM.WebAPI.Extensions;
using CAM.WebAPI.Helper;
using CAM.WebAPI.Identity;
using CAM.WebAPI.Middelware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using OracleModels.DBContext;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace CAM.WebAPI
{
    public class Startup
    {
        Logger logger;
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            logger = NLogBuilder.ConfigureNLog($"{Directory.GetCurrentDirectory()}/NLog.config").GetCurrentClassLogger();
            Configuration = configuration;
            CurrentEnvironment = environment;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddApplicationInsightsTelemetry();
            services.AddDistributedMemoryCache();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TEMS_API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            if (CurrentEnvironment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }


            #region JWT

            var tokenValidationParameters = JwtTokenParameters.RetrieveTokenParamters();
            services.AddSingleton(tokenValidationParameters);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.SaveToken = true;
                jwt.TokenValidationParameters = tokenValidationParameters;
            });
            #endregion

            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            ); 

            services.AddTransient(typeof(Lazy<>), typeof(LazyService<>));

            services.AddScoped<NetworkElementManager>();
            services.AddScoped<IdentityManager>();
            services.AddScoped<SoftwareComponentManager>();
            services.AddScoped<SoftwareConfigurationManager>();
            services.AddScoped<HardwareConfigurationManager>();
            services.AddScoped<AuditHistoryManager>();
            services.AddScoped<ResourceKeyMasterManager>();
            services.AddScoped<DesignComponentFamilyLifeCycleManager>();
            services.AddScoped<GenericReportManager>();
            services.AddScoped<GenericReportGenration>();
            services.AddScoped<AuditLogManager>();

            services.AddScoped<SWConfigManager>();

            services.AddScoped<CommonManager>();

            services.AddScoped<DropdownDataServiceManager>();

            services.AddScoped<NetworkVisualizerManager>();
           
            services.AddScoped<AspnetuserroleManager>();
            services.AddScoped<AspnetusersManager>();
            services.AddScoped<ReconciliationManager>();

            services.AddScoped<FeedBackLoopLogManager>();

            //services.AddScoped<LcmUpdateTrackerManager>();
            //add Round 9
            services.AddScoped<LcmAncillaryDataManager>();

            services.AddScoped<MajorHardwareBuildManager>();
            services.AddScoped<SystemTypeManager>();
            services.AddScoped<MajorSoftwareBuildManager>();

            //services.AddScoped<SoftwareBuildCompatibilityManager>();
            services.AddScoped<AssetOverviewbyMarketManager>();

            services.AddScoped<DesignComponentFamilyManager>();
            services.AddScoped<DesignComponentManager>();
            services.AddScoped<LcmEngineeringManager>();
            services.AddScoped<ReportSoftwareManager>();
            services.AddScoped<ReportHardwareManager>();
            services.AddScoped<PlannedActivityManager>();
            services.AddScoped<PlannedActivityCommon>();
            services.AddScoped<UpdatePlannedActivityManager>();
            services.AddScoped<PlannedActivityTypesManager>();
            services.AddScoped<AssetPivotReportManager>();

            services.AddScoped<GridCustomColumnManager>();
            services.AddScoped<BundleUpgradeInitiativeManager>();
            services.AddScoped<VNFTransitionManager>();
            services.AddScoped<NFVITransitionManager>();
            services.AddScoped<InizializeNewProductManager>();
            services.AddScoped<ProductLifecycleConstraintsManager>();
            services.AddScoped<SettingsUpdatePlannedActivityManager>();
            services.AddScoped<NetworkElementsAsPlannedManager>();
            services.AddScoped<NetworkElementAsIsManager>();
            services.AddScoped<UserManager>();
            services.AddScoped<TokenManager>();
            services.AddScoped<VolteKPIManager>();
            services.AddScoped<NetworkElementNodeCountManager>();
            services.AddScoped<ViaExportHardwareManager>();
            services.AddScoped<ViaExportSoftwareManager>();
            services.AddScoped<DesignAspectManager>();
            services.AddScoped<IExportService, ExportService>();
            services.AddScoped<IResourceKey, ResourceKey>();
            services.AddScoped<IImportService, ImportService>();
            services.AddScoped<LcmImport>();
            services.AddScoped<NetworkElementAsIsImport>();
            services.AddScoped<DeliveryTrackingsImport>();
            services.AddScoped<IdentityAsIsImport>();
            services.AddScoped<TsrPassThroughImport>();
            services .AddScoped<NfviSoftwareCompatibilityImport>();
            services.AddScoped<VBomImportService>();
            services.AddScoped<CBomImportService>();
            services.AddScoped<BPTImport>();
            services.AddScoped<ProjectPlanImport>();
            services.AddScoped<PassThroughImport>();
            services.AddScoped<TemsFntImport>();
            //Ticket 767 - Bundle - CRUD operation for MajorsoftwareBuildBundle Table and implement Configuration File
            services.AddScoped<SoftwareBuildCompatibilityManager>();

            services.AddScoped<NfviSoftwareCompatibilityManager>();
            services.AddScoped<PATManager>();
            services.AddScoped<CategoryManager>();
            services.AddScoped<ClassManager>();
            services.AddScoped<TypeManager>();
            services.AddScoped<IdentityAsIsManager>();
            services.AddScoped<DeliveryTrackingManager>();
            services.AddScoped<LcmExportSettingManager>();
            services.AddScoped<GlossaryItemsManager>();

            services.AddScoped<AuthorizedRoleManager>();
            services.AddScoped<SystemVerificationProblemManager>();
            services.AddScoped<ProblemCategoryManager>();
            services.AddScoped<OrganisationManager>();
             services.AddScoped<UserDefinedReportLogManager>();

            services.AddScoped<LcmAtGlanceManager>();
            services.AddScoped<LcmAtGlanceForEosManager>();

            #region  BPT
            services.AddScoped<BPTManager>();

            #endregion
            #region //NFVI
            services.AddScoped<NFVICompatibilityAtGlanceManager>();

            #endregion

            //Exodus
            services.AddScoped<ExodusAssetLevelReportManager>();
            #region // FNT
            services.AddScoped<TemsFntReportManager>();
            services.AddScoped<NonTemsFntReportManger>();
            #endregion

            services.AddScoped<NetworkElementLevelTwoManager>();
            services.AddScoped<ReportSubnetWorkManager>();
            services.AddScoped<ReportHardwareConfigurationManager>();
            services.AddScoped<TsrPassThroughManager>();
            services.AddScoped<TsrLogManager>();
            services.AddScoped<AppSettingsConfiguartionManager>();
            services.AddScoped<ProjectsPlanManager>();
            services.AddScoped<DesignAspectPlannedActivityManger>();
            services.AddScoped<FNTReportManager>();
            services.AddScoped<AssetPassThroughManager>();
            services.AddScoped<HwPassThroughLcmManager>();
            services.AddScoped<SwPassThroughLcmManager>();
            services.AddScoped<PassThroughLcmManager>();
            services.AddScoped<TsrNonTemManager>();
            services.AddScoped<ReportSchedulerManager>();
            services.AddScoped<ClusterLevelManager>();

            AddLookUpService(services);

            services.AddScoped<IExportService, ExportService>();
            //////////////////////////////////////////////////////////////////////////////////////////
            //services.Configure<ConnectionStrings>(Configuration.GetSection(nameof(ConnectionStrings)));
            //////////////////////////////////////////////////////////////////////////////////////////
            services.AddControllers();
            services.ConfigureMsSqlContext(Configuration);
            services.AddHttpContextAccessor();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.ConfigureLoggerService();

            services.ConfigureRepositoryWrapper();
            #region systeofsystem
            services.AddScoped<ComponentSoftwareBuildManager>();
            services.AddScoped<BuildBagManager>();
            services.AddScoped<ComponentSoftwareBuildBagMappingManager>();
            services.AddScoped<ComponentManufacturersManager>();
            #endregion

            services.AddScoped<InfraClusterAsPlannedManager>();
            services.AddScoped<ClusterUpgradeManager>();
            services.AddScoped<NetworkElementClusterAsPlannedManager>();
            #region XBom
            services.AddScoped<VBomManager>();
            services.AddScoped<CBomManager>();
            services.AddScoped<VBomReportManager>();
            services.AddScoped<CBomReportManager>();
            #endregion

            #region Exodus

            services.AddScoped<BusinessManager.Entity.ExodusProgram.DaMigrationStatusManager>();
            services.AddScoped<PlatformMigrationManager>();
            services.AddScoped<AssetHardwareAncillaryManager>();
            services.AddScoped<ExodusGraphicalLevel1ReportManager>();
            services.AddScoped<ExodusAssetLGraphicalevel1ReportManager>();
            services.AddScoped<ExodusGraphicalLevel2ReportManager>();
            services.AddScoped<ExodusGraphicalLevel3ReportManager>();
            #endregion
            #region

            #endregion

            #region // Abstraction Layer
            services.AddScoped<AbstractionLayerManager>();
            #endregion

            services.AddScoped<CAM.BusinessManager.LookUp.MajorHardwareBuildAsIsManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.AssetMapInfoManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.AspnetUserRoleModuleMappingManager>();
            #region OMC
            services.AddScoped<AssetAsisSdiInfoManager>();
            services.AddScoped<AssetAsisSdiSwitchInfoManager>();
            services.AddScoped<AssetAsisHwAncillaryDataManager>();
            #endregion
            services.AddScoped<ServicePlanManager>();
            services.AddScoped<TeamsManager>();
            services.AddScoped<TeamMembersManager>();

            services.AddSession();
            services.AddIdentityService(Configuration);
            services.AddTransient<IDateTime, DateTimeService>();


            #region Dapper
            services.AddScoped<DapperContext>(); 
            services.AddScoped<CommonDapperRepository>();
            services.AddScoped<AbstractionLayerDapperManager>();
            services.AddScoped<LCMPADapperQueries>();
            services.AddScoped<HomePageManager>();
            services.AddScoped<DapperCommonManager>();
            services.AddScoped<ProductGraphManager>();
            services.AddScoped<UpcomingLinkManager>();
            services.AddScoped<OpenLinkManager>();
            services.AddScoped<AchivementLinkDapperManager>();
            services.AddScoped<MajorSoftwareDapperQueryManager>();
            #endregion

            services.AddMailContex(o =>
            {
                o.Host = Configuration["Smtp:Host"];
                o.Username = Configuration["Smtp:Username"];
                o.Password = Configuration["Smtp:Password"];
                o.Port = int.Parse(Configuration["Smtp:Port"]);
                o.EnableSsl = bool.Parse(Configuration["Smtp:EnableSsl"]);
                o.EncryptionMethod = Configuration["Smtp:EncryptionMethod"];
            });

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new NetworkElementMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new IdentityMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new SoftwareComponentMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new SoftwareConfigurationMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new HardwareConfigurationMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new AuditHistoryMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new ResourceKeyMasterMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new DesignComponentFamilyLifeCycleMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new GenericReportMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new AuditLogMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new SWConfigMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));


                cfg.AddProfile(new ReconciliationMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new SystemVerificationProblemMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new ProblemCategoryMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new OrganisationMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));

                cfg.AddProfile(new FeedBackLoopLogMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));


                cfg.AddProfile(new AspnetuserroleMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new AspnetuserMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new PlannedActivityTypesMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                //add Lcm_Round 9

                cfg.AddProfile(new LcmAncillaryDataMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new LcmEngineeringMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new IdentityAsIsMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new TypeMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new DesignComponentMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new DesignComponentFamilyMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new NetworkElementsAsPlannedMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new SystemTypeMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(),provider.GetService<CommonManager>()));
                cfg.AddProfile(new NetworkElementAsIsMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new MajorHardwareBuildMapper(provider.GetService<CommonManager>()));
                cfg.AddProfile(new MajorSoftwareBuildMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(),provider.GetService<CommonManager>()));
                cfg.AddProfile(new PlannedActivityMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new VolteKPIMapper());
                cfg.AddProfile(new ForeignIndexMapper());
                cfg.AddProfile(new BusinessManager.MapConfiguration.MappingProfile());
                cfg.AddProfile(new UserDefinedReportLogMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new BusinessManager.MapConfiguration.ComponentSoftware.ComponentSwBuildMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new BusinessManager.MapConfiguration.ComponentSoftware.ComponentMappingSwBuildMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new BusinessManager.MapConfiguration.ComponentSoftware.ComponentManufacturersMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new BusinessManager.MapConfiguration.ComponentSoftware.BuildBagMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new TsrPassThroughMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new TsrLogMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new AppSettingsConfigurationMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));

                cfg.AddProfile(new NfviSoftwareCompatibilitMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new BudgetProjectTrackerMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                
                cfg.AddProfile(new BusinessManager.MapConfiguration.XBom.VBomMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new PassThroughMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new PassThroughLcmHardwareMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new PassThroughLcmSoftwareMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new TemsFntReportMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new NonTemsReportMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new TsrNonTemMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new ReportSchedulerMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new MajorHardwareBuildAsisMapper());
                cfg.AddProfile(new AssetMapInfoMapper());
                cfg.AddProfile(new AspnetUserRoleModuleMappingMapper());
                cfg.AddProfile(new GlossaryItemsMapper ());
                cfg.AddProfile(new InfraClusterAsPlannedMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>()));
                cfg.AddProfile(new ServicePlanMapper(provider.GetService<IHttpContextAccessor>(), provider.GetService<IEnumerable<IRepositoryWrapper>>(), provider.GetService<IRepositoryWrapper>(), provider.GetService<CommonManager>()));
                cfg.AddProfile(new AssetAsisSdiInfoMapper());
                cfg.AddProfile(new AssetAsisSdiSwitchInfoMapper());
                cfg.AddProfile(new AssetAsIsHwAncillaryDataMapper());

            }).CreateMapper());
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MajorHardwareBuildManager).GetTypeInfo().Assembly);
        }

        private static void AddLookUpService(IServiceCollection services)
        {
            services.AddScoped<CAM.BusinessManager.LookUp.SoftwareComponentManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SoftwareConfigurationManager>();


            services.AddScoped<CAM.BusinessManager.LookUp.MainOrganisationManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.PracticeManager>();


            services.AddScoped<CAM.BusinessManager.LookUp.RiskClusterManager>();

            services.AddScoped<CAM.BusinessManager.LookUp.ActivityStatusesManager>();
           

            services.AddScoped<CAM.BusinessManager.LookUp.AssetCategoriesManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.AssetTypeManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.AssetClassManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.DeliveryStatusManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.OpCosManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.OperatingSystemManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.OriginalEquipmentManufacturerManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.PlannedActivityResourceManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.ProductImportanceManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SubDomainResponsibleManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.VulnerabilityStatusManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.VerticalResponsibleManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.ResponsibilityPhaseManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.PlanningActivityStatusManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.HardwareSolutionResourcesManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.NFVIStatusManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.EquipmentStatusManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.VNFDesignComponentManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.NFVIBundleIDManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.BuildConstructionManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.EndOfSupportContractManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.PlatformManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.HardwareTypeManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SubDomainSpocManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.BudgetAvailabilityManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.RiskManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.ReasonCheckboxResourcesManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SupportedResourceManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.FullOrPartialResourceManager>();

            services.AddScoped<CAM.BusinessManager.LookUp.LocationManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.EnvironmentManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.DeploymentStatusManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.DeploymentTypeManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.NetworkConstructManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.DriverManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.BenefitManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.PlanningRiskManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.ActivityDetailsManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SubNetworkBoundaryManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SystemFunctionManager>();
            services.AddScoped<SharingTypesManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SecurityTireZoneManager>();
            services.AddScoped<CAM.BusinessManager.ForeignIndex.ForeignIndexManager>();

            services.AddScoped<NetworkFunctionManager>();
            services.AddScoped<CriticalAssetTypeManager>();
            services.AddScoped<CustomerWheelManager>();
            services.AddScoped<SupportedServiceManager>();

            services.AddScoped<LicenseModelManager>();
            services.AddScoped<BusinessContinuityMethodManager>();
            services.AddScoped<SiteResilienceManager>();
            services.AddScoped<InstanceResilienceManager>();
            services.AddScoped<ThirdPartyAccessTypeManager>();
            services.AddScoped<AuthenicationTypeManager>();
            services.AddScoped<SecurityManagerManager>();
            services.AddScoped<SWDeliveryLifeCycleManager>();
            services.AddScoped<OperationalContractManager>();
            services.AddScoped<IProductNameManager, ProductNameManager>();
            services.AddScoped<IVodafoneNameManager, VodafoneNameManager>();
            services.AddScoped<ILcmDeploymentStatusManager, LcmDeploymentStatusManager>();
            services.AddScoped<IUserLoggingLevelManager, UserLoggingLevelManager>();
            services.AddScoped<CAM.BusinessManager.LookUp.SystemNamesManager>();
            services.AddScoped<PlannedActivityCategoryManager>();
            services.AddScoped<ProgramManager>();
            services.AddScoped<VnfClusterNameManager>();
            services.AddScoped<VnfNameManager>();
            services.AddScoped<VmTypeNameManager>();
            services.AddScoped<IntraVmTypeManager>();
            services.AddScoped<InterVmTypeManager>();
            services.AddScoped<VmWorkloadTypeManager>();
            services.AddScoped<CnfClusterManager>();
            services.AddScoped<PodTypeInfoManager>();
            services.AddScoped<CnfFunctionStandardNameManager>();
            services.AddScoped<CnfHardwareTypeManager>();
            services.AddScoped<CnfNameManager>();
            services.AddScoped<CnfPriorityManager>();
            services.AddScoped<VnfHardwareTypeManager>();
            services.AddScoped<ServiceMasterManager>();









        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var forwardingOptions = new ForwardedHeadersOptions()
            { ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto | ForwardedHeaders.All };
            app.UseForwardedHeaders(forwardingOptions);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var userId = User.Id;

            app.UseAuthentication();

            app.UseSession();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseMiddleware<CAMErrorHandlingMiddleware>();

            app.UseMiddleware<JwtMiddleware>();
            //app.UseMiddleware<UserLoginSessionHandlerMiddleware>();            
            app.UseRequestLocalization();

            app.UseStaticFiles();

            app.UseRequestResponseLogging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "CAM.WebAPI");
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


        }
    }
}
