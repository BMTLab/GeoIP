-- Update script. 
-- Use 'p' param to path to locations csv file, 'v' param to path to blocks csv file

truncate geoipdb.public.blocks      cascade;
truncate geoipdb.public.locations   cascade;
copy geoipdb.public.locations   from :p delimiter ',' csv header;
copy geoipdb.public.blocks      from :v delimiter ',' csv header;
