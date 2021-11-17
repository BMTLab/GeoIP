create database geoipdb;

create table if not exists locations
(
    geoname_id	            integer primary key,
    locale_code	            varchar(2),
    continent_code	        varchar(2),
    continent_name	        text,
    country_iso_code	    varchar(2),
    country_name	        text,
    subdivision_1_iso_code	varchar(3),
    subdivision_1_name	    text,
    subdivision_2_iso_code	varchar(3),
    subdivision_2_name	    text,
    city_name	            text,
    metro_code	            smallint,
    time_zone	            text,
    is_in_european_union	boolean
);

create table if not exists blocks
(
    network                         cidr primary key,
    geoname_id                      integer references locations,
    registered_country_geoname_id	integer,
    represented_country_geoname_id	integer,
    is_anonymous_proxy	            boolean,
    is_satellite_provider	        boolean,
    postal_code	                    text,
    latitude	                    decimal(6, 4),
    longitude                       decimal(7, 4),
    accuracy_radius	                smallint
);