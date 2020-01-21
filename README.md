 <a href="https://github.com/BMTLab/GeoIP/blob/develop/GeoIP.Docs/Sample%20Images/client_main_view.png"><img src="https://github.com/BMTLab/GeoIP/blob/develop/GeoIP.Docs/Sample%20Images/client_main_view.png?raw=true" title="FVCproductions" alt="FVCproductions"></a>

# Geo IP
**Web service for providing geolocation by IPv4**


The application consists of a WebApi server, Blazor client and an application for updating the database

### Setup

- Configure the connection string to appSetting.json located GeoIp/Server/Properties
- Create a PostgreSQL database by running:
```shell
$ dotnet ef database update
```
- Build or Publish a Server project.

### Requirements

- .NET Core 3.1

### Usage

- After starting the server, you can get information about the requested IP by using
>  {domain}/api/geolocationbyip/{ip} 
 
  Server will return JSON with information or error.

- To update, you must run GeoIp.Updater