using Microsoft.EntityFrameworkCore;
using AssetServerAPI;
using AssetServerAPI.Utilities;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddSingleton<BlobStorageService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var blobStorageService = scope.ServiceProvider.GetRequiredService<BlobStorageService>();
    await blobStorageService.CreateBucketIfNotExistsAsync(); 

}



if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => 
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Password Reset API V1");
    });
}
app.UseRouting();
app.UseAuthorization();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();  
});
#pragma warning restore ASP0014

//app.UseHttpsRedirection();
app.MapControllers();

app.Run();