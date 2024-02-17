open System


let getSeededFromConnId = 
    fun id ->
        connId.ToCharArray()
        |> lifo (fun s v -> s + ((int) v)) 0
        |> Random

//Note: (i) get 1.56x to ensure extra for dupes
//      (ii) below proc tested in terms of cli/svr separation issues; works as expected

let getMaskedVals =
    fun id n ->
        let r = getSeededFromConnId id
        let padded = Math.round(n * 1.5)
        [0..(padded)] |> lima(fun v -> counties.[r.Next(0..(len counties))]) |> List.uniq |> List.sort

= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
Probably b8r 2 go w/a .resources <a href='https://learn.microsoft.com/en-us/dotnet/core/extensions/create-resource-files#resources-in-resources-files'>file</a>
= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
See <a href='https://stackoverflow.com/questions/74915414/serve-static-files-from-controller'>this</a> To serve static files from a controller (bypassing aspNet default staticFiles setup)
(1)<a href='https://learn.microsoft.com/en-us/aspnet/core/fundamentals/file-providers?view=aspnetcore-7.0'>msHelp</a>: File Providers
(2)  example code <a href='https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/fundamentals/file-providers/samples/3.x/FileProviderSample/Startup.cs'>here</a>

1 & 2 above are all that's reqd to embed manifest resources into the assembly + 
Specify a custom root path for these embedded res.s +
Specify the root to scope calls to GetDirectoryContents to those resources under the provided path.
= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
Access HttpContext from minAPIs:
  app.MapGet("/", (HttpContext context) => context.Response.WriteAsync("Hello World"));

= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
HttpContext.Session Property
public abstract Microsoft.AspNetCore.Http.ISession Session { get; set; }
  >> ISession.Id Property
  public string Id { get; }
  public bool IsAvailable { get; }  //Indicates whether the current session loaded successfully
= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
Serve static files from different folder than wwwroot
        app.UseStaticFiles();
        app.UseStaticFiles(new StaticFileOptions() {
                FileProvider =  new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Content")),
                RequestPath = new PathString("/Admin")
        });
now, we will be able to execute HTTP request http://localhost:1234/admin/admin.html to display static admin.html page.
= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 
Using Embedded resources (have to be embedded individually)
add to csproj: <useembeddedmanif...>true and add itms indiv in <itemgroup>,
see <a href='https://stackoverflow.com/questions/54769558/how-to-read-an-embedded-resource-using-the-manifestembeddedfileprovider'>this</a>
= = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = 



