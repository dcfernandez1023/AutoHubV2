CREATE TABLE "User" (
    "Id" UUID PRIMARY KEY,
    "Email" VARCHAR(255) NOT NULL,
    "Password" VARCHAR(255) NOT NULL,
    "DateCreated" bigint NOT NULL,
    "Settings" VARCHAR NOT NULL
);

CREATE TABLE "Vehicle" (
  "Id" UUID PRIMARY KEY,
  "UserId" UUID NOT NULL,
  "Name" VARCHAR(255) NOT NULL,
  "Mileage" INTEGER NOT NULL,
  "Year" INTEGER NOT NULL,
  "Make" VARCHAR(255) NOT NULL,
  "Model" VARCHAR(255) NOT NULL,
  "LicensePlate" VARCHAR(255) NOT NULL,
  "Vin" VARCHAR(255) NOT NULL,
  "Notes" TEXT NOT NULL,
  "DateCreated" bigint NOT NULL,
  "SharedWith" UUID[] NOT NULL,
  "Base64Image" TEXT,
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id")
);

CREATE TABLE "ScheduledServiceType" (
  "Id" UUID PRIMARY KEY,
  "UserId" UUID NOT NULL,
  "Name" VARCHAR(255) NOT NULL,
  "DateCreated" bigint NOT NULL,
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id")
);

CREATE TABLE "VehicleSchedule" (
  "Id" UUID PRIMARY KEY,
  "UserId" UUID NOT NULL,
  "VehicleId" UUID NOT NULL,
  "SstId" UUID NOT NULL,
  "MileInterval" INTEGER NOT NULL,
  "TimeInterval" INTEGER NOT NULL,
  "TimeUnits" VARCHAR(10) NOT NULL CHECK ("TimeUnits" IN ('day', 'week', 'month', 'year')),
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
  FOREIGN KEY ("VehicleId") REFERENCES "Vehicle" ("Id"),
  FOREIGN KEY ("SstId") REFERENCES "ScheduledServiceType" ("Id")
);

CREATE TABLE "ScheduledLog" (
  "Id" UUID PRIMARY KEY,
  "UserId" UUID NOT NULL,
  "VehicleId" UUID NOT NULL,
  "SstId" UUID NOT NULL,
  "DatePerformed" bigint NOT NULL,
  "MileagePerformed" INTEGER NOT NULL,
  "NextServiceDate" bigint,
  "NextServiceMileage" INTEGER,
  "PartsCost" NUMERIC(10, 2) NOT NULL,
  "LaborCost" NUMERIC(10, 2) NOT NULL,
  "TotalCost" NUMERIC(10, 2) NOT NULL,
  "Notes" TEXT NOT NULL,
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
  FOREIGN KEY ("VehicleId") REFERENCES "Vehicle" ("Id"),
  FOREIGN KEY ("SstId") REFERENCES "ScheduledServiceType" ("Id")
);

CREATE TABLE "RepairLog" (
  "Id" UUID PRIMARY KEY,
  "UserId" UUID NOT NULL,
  "VehicleId" UUID NOT NULL,
  "DatePerformed" bigint NOT NULL,
  "Name" VARCHAR(255) NOT NULL,
  "MileagePerformed" INTEGER NOT NULL,
  "PartsCost" NUMERIC(10, 2) NOT NULL,
  "LaborCost" NUMERIC(10, 2) NOT NULL,
  "TotalCost" NUMERIC(10, 2) NOT NULL,
  "Notes" TEXT NOT NULL,
  FOREIGN KEY ("UserId") REFERENCES "User" ("Id"),
  FOREIGN KEY ("VehicleId") REFERENCES "Vehicle" ("Id")
);