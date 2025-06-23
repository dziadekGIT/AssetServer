DO
$$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_roles WHERE rolname = 'myuser') THEN
        CREATE USER myuser WITH PASSWORD 'mypassword';
END IF;
END
$$;

DO
$$
BEGIN
    IF NOT EXISTS (SELECT 1 FROM pg_database WHERE datname = 'assetdb') THEN
        CREATE DATABASE assetdb;
        GRANT ALL PRIVILEGES ON DATABASE assetdb TO myuser;
END IF;
END
$$;
