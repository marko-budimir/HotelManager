-- Rename "DatedUpdated" to "DateUpdated" in all tables
ALTER TABLE IF EXISTS "Role" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "User" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "RoomType" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "Room" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "Review" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "Reservation" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "Discount" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "Invoice" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "Service" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";

ALTER TABLE IF EXISTS "InvoiceService" RENAME COLUMN IF EXISTS "DatedUpdated" TO "DateUpdated";
