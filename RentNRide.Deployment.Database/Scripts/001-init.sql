CREATE TABLE IF NOT EXISTS "Driver" (
    DriverId VARCHAR(6) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    Cnpj VARCHAR(14) NOT NULL UNIQUE,
    BirthDate DATE NOT NULL,
    LicenseNumber VARCHAR(11) NOT NULL UNIQUE,
    LicenseType INT NOT NULL,
    LicenseImageUrl TEXT
);
-- Idx
CREATE INDEX IF NOT EXISTS IX_Driver_Cnpj ON "Driver" (Cnpj);

CREATE TABLE IF NOT EXISTS "Plan" (
    PlanId SERIAL PRIMARY KEY,
    Description VARCHAR(50) NOT NULL,
    DurationInDays INT NOT NULL,
    DailyValue NUMERIC(5,2) NOT NULL,
    FinePercentage NUMERIC(5,2),
    AditionalDailyValue NUMERIC(5,2)
);

CREATE TABLE IF NOT EXISTS "Motorcycle" (
    MotorcycleId VARCHAR(6) PRIMARY KEY,
    Year INT NOT NULL,
    Model VARCHAR(50) NOT NULL,
    Plate VARCHAR(7) NOT NULL UNIQUE
);
-- Idx
CREATE INDEX IF NOT EXISTS IX_Motorcycle_Plate ON "Motorcycle" (Plate);

CREATE TABLE IF NOT EXISTS "Rental" (
    RentalId VARCHAR(6) PRIMARY KEY,
    StartDate TIMESTAMP NOT NULL,
    ExpectedEndDate TIMESTAMP NOT NULL,
    ActualEndDate TIMESTAMP,
    BaseValue NUMERIC(7,2) NOT NULL,
    Penalty NUMERIC(5,2),
    Additional NUMERIC(5,2),
    TotalCost NUMERIC(7,2) NOT NULL,

    MotorcycleId VARCHAR(6) NOT NULL,
    DriverId VARCHAR(6) NOT NULL,
    PlanId INT NOT NULL,

    CONSTRAINT FK_Rental_Motorcycle FOREIGN KEY (MotorcycleId) REFERENCES "Motorcycle"(MotorcycleId),
    CONSTRAINT FK_Rental_Driver FOREIGN KEY (DriverId) REFERENCES "Driver"(DriverId),
    CONSTRAINT FK_Rental_Plan FOREIGN KEY (PlanId) REFERENCES "Plan"(PlanId)
);

CREATE TABLE IF NOT EXISTS "MotorcycleEvent" (
    MotorcycleEventId VARCHAR(6) PRIMARY KEY,
    CreatedDatetime TIMESTAMP NOT NULL,

    MotorcycleId VARCHAR(6) NOT NULL,
    CONSTRAINT FK_MotorcycleEvent_Motorcycle FOREIGN KEY (MotorcycleId) REFERENCES "Motorcycle"(MotorcycleId)
);
