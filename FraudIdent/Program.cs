using FraudIdent.Backbone;
using FraudIdent.Backbone.BusinessRules;
using FraudIdent.Backbone.Providers;
using FraudIdent.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

//var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
//var dbName = Environment.GetEnvironmentVariable("DB_NAME");
//var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSORD");
//var connectionString = $"Server={dbHost}; Database={dbName}; user id=sa; pwd={dbPassword};";

//Environment.SetEnvironmentVariable("ConnectionStrings.FraudIdentDB", connectionString);

builder.Services.AddSingleton(configuration);

builder.Services.AddControllersWithViews();

CoreBusinessRules _coreBusinessRules = null;
CoreSQLProvider _coreSQLProvider = null;
MqttClientRequest _MqttClient = null;
String clientId = "";

builder.Services.AddScoped<MqttClientRequest>(provider =>
{
    clientId = Guid.NewGuid().ToString() + "-pub";

    _MqttClient = new MqttClientRequest(
        configuration.GetSection("MQTTConfig")["BrokenMqttUrl"].ToString(),
        int.Parse(configuration.GetSection("MQTTConfig")["BrokenMqttPort"].ToString()),
        clientId);

    _MqttClient.Connect();

    return _MqttClient;
});

builder.Services.AddScoped<CoreSQLProvider>(provider =>
{
    _coreSQLProvider = new CoreSQLProvider(configuration);

    return _coreSQLProvider;
});

builder.Services.AddScoped<CoreBusinessRules>(businessRules =>
{
    _coreBusinessRules = new CoreBusinessRules(configuration, _coreSQLProvider, _MqttClient);
    return _coreBusinessRules;
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddScoped<FraudIdentController>(controller =>
{
    return new FraudIdentController(_coreBusinessRules, _coreSQLProvider);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseStaticFiles();
//app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");



using (var serviceScope = app.Services.CreateScope())
{
    // Get the CoreSQLProvider service from the dependency injection container.
    serviceScope.ServiceProvider.GetRequiredService<MqttClientRequest>();
    serviceScope.ServiceProvider.GetRequiredService<CoreSQLProvider>();
    serviceScope.ServiceProvider.GetRequiredService<CoreBusinessRules>();
    //serviceScope.ServiceProvider.GetRequiredService<CoreController>();
}

app.UseCors(builder => {
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

app.Run();
