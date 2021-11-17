-- noinspection SqlResolveForFile

create index gist_network
    on public.blocks using gist
        (network inet_ops)
    include (network)
    with (fillfactor = 90)
    tablespace pg_default;