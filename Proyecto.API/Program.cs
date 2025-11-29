using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proyecto.API.BackgroundJobs;
using Proyecto.BW.CU;
using Proyecto.BW.Interfaces.BW;
using Proyecto.BW.Interfaces.DA;
using Proyecto.DA.Acciones;
using Proyecto.DA.Config;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:8100", "https://localhost:8100")
                  .AllowAnyMethod()
                  .AllowAnyHeader()
                  .AllowCredentials();
        });
});

builder.Services.AddDbContext<BancoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
     sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()));

// Add services to the container.
builder.Services.AddTransient<IAuditoriaBW, GestionAuditoriaBW>();
builder.Services.AddTransient<IBeneficiarioBW, GestionBeneficiarioBW>();
builder.Services.AddTransient<IClienteBW, GestionClienteBW>();
builder.Services.AddTransient<IComprobanteBW, GestionComprobanteBW>();
builder.Services.AddTransient<IConsumoLimiteDiarioBW, GestionConsumoLimiteDiarioBW>();
builder.Services.AddTransient<ICuentaBW, GestionCuentaBW>();
builder.Services.AddTransient<IGestorClienteBW, GestionGestorClienteBW>();
builder.Services.AddTransient<IPagoServicioBW, GestionPagoServicioBW>();
builder.Services.AddTransient<IParametroSistemaBW, GestionParametroSistemaBW>();
builder.Services.AddTransient<IProveedorServicioBW, GestionProveedorServicioBW>();
builder.Services.AddTransient<ITransaccionCuentaBW, GestionTransaccionCuentaBW>();
builder.Services.AddTransient<ITransferenciaBW, GestionTransferenciaBW>();
builder.Services.AddTransient<IUsuarioBW, GestionUsuarioBW>();
builder.Services.AddTransient<IAuditoriaDA, GestionAuditoriaDA>();
builder.Services.AddTransient<IBeneficiarioDA, GestionBeneficiarioDA>();
builder.Services.AddTransient<IClienteDA, GestionClienteDA>();
builder.Services.AddTransient<IComprobanteDA, GestionComprobanteDA>();
builder.Services.AddTransient<IConsumoLimiteDiarioDA, GestionConsumoLimiteDiarioDA>();
builder.Services.AddTransient<ICuentaDA, GestionCuentaDA>();
builder.Services.AddTransient<IGestorClienteDA, GestionGestorClienteDA>();
builder.Services.AddTransient<IPagoServicioDA, GestionPagoServicioDA>();
builder.Services.AddTransient<IParametroSistemaDA, GestionParametroSistemaDA>();
builder.Services.AddTransient<IProveedorServicioDA, GestionProveedorServicioDA>();
builder.Services.AddTransient<ITransaccionCuentaDA, GestionTransaccionCuentaDA>();
builder.Services.AddTransient<ITransferenciaDA, GestionTransferenciaDA>();
builder.Services.AddTransient<IUsuarioDA, GestionUsuarioDA>();

builder.Services.AddTransient<IAutenticacionBW, GestionAutenticacionBW>();
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<EjecutarTransferenciasProgramadasJob>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();    // 1. Redirección HTTPS

app.UseCors("AllowFrontend"); // 2. CORS (DESPUÉS de HTTPS Redirection)

app.UseAuthentication();      // 3. Autenticación

app.UseAuthorization();       // 4. Autorización

app.MapControllers();         // 5. Controladores

app.Run();
