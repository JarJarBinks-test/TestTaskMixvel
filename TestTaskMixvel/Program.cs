
using System.Reflection;
using TestTaskMixvel.Services;
using TestTaskMixvel.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers((options) => {
    options.Filters.Add<AppExceptionFilter>();
});
builder.Services.AddServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(setupAction =>
{
    var xmlDocumentationFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlDocumentationFullPath = Path.Combine(AppContext.BaseDirectory, xmlDocumentationFile);
    setupAction.IncludeXmlComments(xmlDocumentationFullPath, true);
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
