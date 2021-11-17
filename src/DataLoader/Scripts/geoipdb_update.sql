-- Update script. 
-- Use 'p' param to path to locations csv file, 'v' param to path to blocks csv file

truncate geoipdb.public.locations   cascade;
truncate geoipdb.public.blocks      cascade;
copy geoipdb.public.locations   from :p delimiter E',' csv header;
copy geoipdb.public.blocks      from :v delimiter E',' csv header;