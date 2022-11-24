using AutoMapper;
using BackEndApi_minimal.Data;
using BackEndApi_minimal.Data.Models;
using BackEndApi_minimal.DTOs;
using BackEndApi_minimal.Services.Contrato;
using BackEndApi_minimal.Services.Implementacion;
using BackEndApi_minimal.Utilidades;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// agregamos el contexto
builder.Services.AddDbContext<DBEmpleadoContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("cadenaSQL")));

// minusculas URL
builder.Services.AddRouting(routing => routing.LowercaseUrls = true);

// implementar la inyeccion de los services
builder.Services.AddScoped<IDepartamentoService, DepartamentoService>();
builder.Services.AddScoped<IEmpleadoService, EmpleadoService>();

// inyectamos el automapper creado
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

// Configuracion para los Cors de Angular
builder.Services.AddCors(options => {
    options.AddPolicy("NuevaPolitica", app => {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




#region PETICIONES API REST
app.MapGet("/api/departamento/lista", async (IDepartamentoService _departamentoService, IMapper _mapper) => {
    var listaDepartamento = await _departamentoService.GetList();
    var listaDepartamentoDTO = _mapper.Map<List<DepartamentoDTO>>(listaDepartamento);

    if (listaDepartamentoDTO.Count > 0) return Results.Ok(listaDepartamentoDTO);
    else return Results.NotFound();
});
app.MapGet("/api/empleado/lista", async (IEmpleadoService _empleadoService, IMapper _mapper) => {
    var listaEmpleado = await _empleadoService.GetList();
    var listaEmpleadoDTO = _mapper.Map<List<EmpleadoDTO>>(listaEmpleado);

    if (listaEmpleadoDTO.Count > 0) return Results.Ok(listaEmpleadoDTO);
    else return Results.NotFound();
});
app.MapGet("/api/empleado/{idEmpleado}", async (int idEmpleado, IEmpleadoService _empleadoService, IMapper _mapper) => {
    var _encontrado = await _empleadoService.Get(idEmpleado);
    if (_encontrado is null) return Results.NotFound();
    else return Results.Ok(_mapper.Map<EmpleadoDTO>(_encontrado));
});
app.MapPost("/api/empleado/guardar", async (EmpleadoDTO modelo, IEmpleadoService _empleadoService, IMapper _mapper) => {
    var _empleado = _mapper.Map<Empleado>(modelo);
    var _empleadoCreado = await _empleadoService.Add(_empleado);

    if (_empleadoCreado.IdEmpleado != 0) return Results.Ok(_mapper.Map<EmpleadoDTO>(_empleadoCreado));
    else return Results.StatusCode(StatusCodes.Status500InternalServerError);
});
app.MapPut("/api/empleado/actualizar/{idEmpleado}", async (int idEmpleado, EmpleadoDTO modelo, IEmpleadoService _empleadoService, IMapper _mapper) => {
    var _encontrado = await _empleadoService.Get(idEmpleado);
    if (_encontrado is null) return Results.NotFound();

    var _empleado = _mapper.Map<Empleado>(modelo);

    _encontrado.NombreCompleto = _empleado.NombreCompleto;
    _encontrado.IdDepartamento = _empleado.IdDepartamento;
    _encontrado.Sueldo = _empleado.Sueldo;
    _encontrado.FechaContrato = _empleado.FechaContrato;

    var respuesta = await _empleadoService.Update(_encontrado);
    if (respuesta) return Results.Ok(_mapper.Map<EmpleadoDTO>(_encontrado));
    else return Results.StatusCode(StatusCodes.Status500InternalServerError);

});
app.MapDelete("/api/empleado/eliminar/{idEmpleado}", async (int idEmpleado, IEmpleadoService _empleadoService) => {
    var _encontrado = await _empleadoService.Get(idEmpleado);
    if (_encontrado is null) return Results.NotFound();

    var respuesta = await _empleadoService.Delete(_encontrado);
    if (respuesta) return Results.Ok();
    else return Results.StatusCode(StatusCodes.Status500InternalServerError);
});
#endregion

// injectamos los cors, que configuramos
app.UseCors("NuevaPolitica");

app.Run();
