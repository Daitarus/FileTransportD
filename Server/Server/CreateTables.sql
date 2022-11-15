DROP TABLE IF EXISTS public."Client_File";
DROP TABLE IF EXISTS public."Histories";
DROP TABLE IF EXISTS public."Clients";
DROP TABLE IF EXISTS public."Files";

CREATE TABLE IF NOT EXISTS public."Clients"
(
    "Id" SERIAL,
    "Hash" bytea NOT NULL,
    "Name" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Clients_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Files"
(
    "Id" SERIAL,
    "Name" character varying(50) COLLATE pg_catalog."default" NOT NULL,
    "Path" character varying(100) COLLATE pg_catalog."default" NOT NULL,
    "FullPath" character varying(200) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "Files_pkey" PRIMARY KEY ("Id")
);

CREATE TABLE IF NOT EXISTS public."Client_File"
(
    "Id" SERIAL,
    "Id_Client" integer NOT NULL,
    "Id_File" integer NOT NULL,
    CONSTRAINT "FileForClient_Id_Client_fkey" FOREIGN KEY ("Id_Client")
        REFERENCES public."Clients" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION,
    CONSTRAINT "FileForClient_Id_File_fkey" FOREIGN KEY ("Id_File")
        REFERENCES public."Files" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);

CREATE TABLE IF NOT EXISTS public."Histories"
(
    "Id" SERIAL,
    "Id_Client" integer,
    "Ip" cidr NOT NULL,
    "Port" integer NOT NULL,
    "Time" timestamp with time zone NOT NULL,
    "Action" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "History_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "Histories_Id_Client_fkey" FOREIGN KEY ("Id_Client")
        REFERENCES public."Clients" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
);
