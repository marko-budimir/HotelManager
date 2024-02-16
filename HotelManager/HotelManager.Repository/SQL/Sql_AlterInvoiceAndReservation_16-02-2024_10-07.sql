-- Add InvoiceNumber column to Invoice table if it does not exist
ALTER TABLE IF EXISTS "Invoice"
ADD COLUMN IF NOT EXISTS "InvoiceNumber" CHAR(20) DEFAULT substring(gen_random_uuid()::text from 1 for 20) NOT NULL UNIQUE;

-- Add ReservationNumber column to Reservation table if it does not exist
ALTER TABLE IF EXISTS "Reservation"
ADD COLUMN IF NOT EXISTS "ReservationNumber" CHAR(20) DEFAULT substring(gen_random_uuid()::text from 1 for 20) NOT NULL UNIQUE;