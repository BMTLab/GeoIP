![.NET](https://github.com/BMTLab/GeoIp/workflows/.NET/badge.svg) 
 
![Recordit GIF](http://g.recordit.co/XEEVodvOsT.gif)

# Geo IP
**Web service for providing geolocation by IPv4**




This is WebApi server


### Requirements

- .NET 6
-  PostgreSQL >= 12

### Usage

- After starting the server, you can get information about the requested IP by using. Server will return JSON with information or error.
       
   
       {domain}/api/geoip/get?ip={value}
 
  
- Data provided thanks to


       Data source: MaxMind GeoIp2 Lite