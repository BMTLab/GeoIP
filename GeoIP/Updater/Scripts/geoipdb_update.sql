truncate geoipdb.public.blocks      cascade;
truncate geoipdb.public.locations   cascade;
copy geoipdb.public.locations   from :p delimiter ',' csv header;
copy geoipdb.public.blocks      from :v delimiter ',' csv header;
