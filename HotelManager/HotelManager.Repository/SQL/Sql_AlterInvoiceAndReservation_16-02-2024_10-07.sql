-- Add InvoiceNumber column to Invoice table
ALTER TABLE "Invoice"
ADD COLUMN "InvoiceNumber" CHAR(20) DEFAULT substring(gen_random_uuid()::text from 1 for 20) NOT NULL UNIQUE;

-- Add ReservationNumber column to Reservation table
ALTER TABLE "Reservation"
ADD COLUMN "ReservationNumber" CHAR(20) DEFAULT substring(gen_random_uuid()::text from 1 for 20) NOT NULL UNIQUE;