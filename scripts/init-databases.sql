-- Create databases for each microservice
CREATE DATABASE "IdentityServiceDb";
CREATE DATABASE "ProductServiceDb";
CREATE DATABASE "MediaServiceDb";
CREATE DATABASE "CartServiceDb";
CREATE DATABASE "OrderServiceDb";
CREATE DATABASE "PaymentServiceDb";
CREATE DATABASE "NotificationServiceDb";

-- Grant privileges (optional, as postgres user already has full access)
GRANT ALL PRIVILEGES ON DATABASE "IdentityServiceDb" TO postgres;
GRANT ALL PRIVILEGES ON DATABASE "ProductServiceDb" TO postgres;
GRANT ALL PRIVILEGES ON DATABASE "MediaServiceDb" TO postgres;
GRANT ALL PRIVILEGES ON DATABASE "CartServiceDb" TO postgres;
GRANT ALL PRIVILEGES ON DATABASE "OrderServiceDb" TO postgres;
GRANT ALL PRIVILEGES ON DATABASE "PaymentServiceDb" TO postgres;
GRANT ALL PRIVILEGES ON DATABASE "NotificationServiceDb" TO postgres;
