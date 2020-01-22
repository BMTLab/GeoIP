 [![Build Status](https://travis-ci.com/BMTLab/GeoIP.svg?branch=master)](https://travis-ci.com/BMTLab/GeoIP)
 
 ![Recordit GIF](http://g.recordit.co/XEEVodvOsT.gif)

# Geo IP
**Web service for providing geolocation by IPv4**




The application consists of a WebApi server, Blazor client and a console application for updating the database

### Setup

- Configure the connection string to appSetting.json located GeoIp/Server/Properties
- Create a PostgreSQL database by running:
```shell
    $ dotnet ef database update
```
- Build or Publish a Server project.

### Requirements

- .NET Core 3.1
-  PostgreSQL >= 9.6

### Usage

- After starting the server, you can get information about the requested IP by using. Server will return JSON with information or error.
       
   
       {domain}/api/geolocationbyip/{ip}
 
  
- To update, you must run GeoIp.Updater


       Data source: MaxMind GeoIp2 Lite