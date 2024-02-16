-- Rename "DatedUpdated" to "DateUpdated" in all tables
ALTER TABLE "Role" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "User" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "RoomType" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "Room" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "Review" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "Reservation" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "Discount" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "Invoice" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "Service" RENAME COLUMN "DatedUpdated" TO "DateUpdated";

ALTER TABLE "InvoiceService" RENAME COLUMN "DatedUpdated" TO "DateUpdated";
