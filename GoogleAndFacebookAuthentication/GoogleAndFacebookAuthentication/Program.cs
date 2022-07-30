/*Para la autenticacion con google debemos de irnos a la pagina
console.cloud.google.com, iniciar sesion y crear un proyecto nuevo,
primero nos vamos a biblioteca y habilitamos la api de google, la cual
se llama Google+ api, una vez habilitada nos vamos al apartado de 
pantalla de consentimiento, aqui creamos un nuevo proyecto, lo
seleccionamos y posteriormente nos vamos a credenciales, aqui creamos
unas credenciales para nuestro proyecto, una vez hecho esto nos dara
nuestro ClientId y nuestro ClientSecret para la autenticacion*/

/*Para la autenticacion con facebook debemos de irnos a la pagina
https://developers.facebook.com/, iniciar sesion con nuestra cuenta
de facebook, despues creamos una aplicacion, configuramos el servicio,
de inicio de sesion y nos daran nuestra AppId y AppSecret y ya 
podemos llevar a cabo la autenticacion*/

using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options => {
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
    .AddCookie(options => {
        options.LoginPath = "/account/google-login";
        //options.LoginPath = "/account/facebook-login";
    })
    .AddGoogle(options => {
        options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
    })
    .AddFacebook(options => {
        options.AppId = builder.Configuration["Authentication:Facebook:AppId"];
        options.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
    });

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();