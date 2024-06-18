using CsvHelper.Configuration;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using EFGetStarted.Models;
using F1LapsGame.Models;

var factory = new AppContextFactory();
using var db = factory.CreateDbContext();

ImportCsvFileBasic<Driver, DriverMap>("drivers.csv");
ImportCsvFileBasic<Race, RaceMap>("races.csv");
ImportCsvFileBasic<Constructor, ConstructorMap>("constructors.csv");
ImportCsvFileBasic<Result, ResultMap>("results.csv");
ImportCsvFileBasic<Laptime, LaptimeMap>("lap_times.csv");

var driverCount = db.Drivers.Count();
var raceCount = db.Races.Count();
var constructorCount = db.Constructors.Count();
var laptimeCount = db.Laptimes.Count();
var resultCount = db.Results.Count();

// 859 drivers, 1125 races, 567466 laptimes
Console.WriteLine($"Drivers: {driverCount}");
Console.WriteLine($"{raceCount} races");
Console.WriteLine($"{laptimeCount} laptimes");
Console.WriteLine($"{constructorCount} constructors");
Console.WriteLine($"{resultCount} results");

// Differences in import methods
// xxx.csv file
// RegisterClassMap<Racemap>
// csv.GetRecords<Race>
// DELETE ...name = 'Races'
// db.Races.Add(...)
// entityCounter size
// CW ... Races recorder".
// ...
// (file.csv, class, classmap, tableName, counter)
void ImportCsvFileBasic<T, TMap>(string filename)
    where T : class, new()
    where TMap : ClassMap<T>
{
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        PrepareHeaderForMatch = args => args.Header.ToLower(),
    };

    using (var reader = new StreamReader($"CSV\\{filename}"))
    using (var csv = new CsvReader(reader, config))
    {
        csv.Context.RegisterClassMap<TMap>();
        var records = csv.GetRecords<T>();

        // get the name of the table from the context
        var tableName = db.Set<T>();
        //Console.WriteLine(tableName.GetType()); 
        //Console.WriteLine(typeof(DbSet<Driver>));

        var entityTypes = db.Model.GetEntityTypes();    // count 5
        var entityType = entityTypes.First(t => t.ClrType == typeof(T));   // Driver
        var entityTableName = entityType.GetTableName();    // Drivers
        string qry = $"DELETE FROM {entityTableName}";

        // lets delete to all data from the sqlite database
        db.Database.ExecuteSqlRaw(qry);
        db.SaveChanges();

        int rowCounter = 0;

        foreach (var record in records)
        {
            rowCounter++;

            db.Set<T>().Add(record);

            if (rowCounter % 1000 == 0)
            {
                Console.Write($".");
                db.SaveChanges();
            }
        }
        db.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Total of {rowCounter} added from {entityTableName}.");
        Console.ResetColor();
    }
}

void ImportCsvFileAdvanced<T, TMap>(string filename, int entryCount)
    where T : class, new()
    where TMap : ClassMap<T>
{
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        PrepareHeaderForMatch = args => args.Header.ToLower(),
    };

    using (var reader = new StreamReader($"CSV\\{filename}"))
    using (var csv = new CsvReader(reader, config))
    {
        csv.Context.RegisterClassMap<TMap>();
        var records = csv.GetRecords<T>();

        // lets delete to all data from the sqlite database
        //db.Database.ExecuteSqlRaw("DELETE FROM Drivers");
        //db.SaveChanges();

        int entityCounter = 0;
        int rowCounter = 0;


        foreach (var record in records)
        {
            // lets check the type of generic class T
            if (typeof(T) == typeof(Driver)) { }

            //var existingEntity = db.Set<T>().FirstOrDefault(d => d.DriverRef == record.DriverRef);
            //rowCounter++;


            //if (existingEntity == null)
            //{
            //    entityCounter++;
            //    db.Set<T>().Add(record);
            //    db.SaveChanges();

            //    if (entityCounter % entryCount == 0)
            //    {
            //        Console.WriteLine($"{entryCount} more added.");
            //    }
            //}
        }
        //db.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Total of {entityCounter} drivers added.");
        Console.ResetColor();

    }
}

void ImportDrivers()
{
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        PrepareHeaderForMatch = args => args.Header.ToLower(),
    };

    using (var reader = new StreamReader("CSV\\drivers.csv"))
    using (var csv = new CsvReader(reader, config))
    {
        csv.Context.RegisterClassMap<DriverMap>();
        var records = csv.GetRecords<Driver>();

        // lets delete to all data from the sqlite database
        //db.Database.ExecuteSqlRaw("DELETE FROM Drivers");
        //db.SaveChanges();

        int entityCounter = 0;
        int rowCounter = 0;

        foreach (var record in records)
        {
            var existingDriver = db.Drivers.FirstOrDefault(d => d.DriverRef == record.DriverRef);
            rowCounter++;

            //if (rowCounter > 15)
            //{
            //    db.SaveChanges();
            //    break;
            //}

            if (existingDriver == null)
            {
                entityCounter++;
                db.Drivers.Add(record);
                //db.SaveChanges();

                if (entityCounter % 200 == 0)
                {
                    Console.WriteLine($"{entityCounter} drivers added.");
                }
            }
        }
        db.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Total of {entityCounter} drivers added.");
        Console.ResetColor();

    }
}

void ImportLaptimes()
{
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        PrepareHeaderForMatch = args => args.Header.ToLower(),
    };

    // 567467 rows in csv file
    using (var reader = new StreamReader("CSV\\lap_times.csv"))
    using (var csv = new CsvReader(reader, config))
    {
        csv.Context.RegisterClassMap<LaptimeMap>();
        var records = csv.GetRecords<Laptime>();

        // reset Laptime.RowId sequence
        db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Laptimes'");
        // lets delete to all data from the sqlite database
        db.Database.ExecuteSqlRaw("DELETE FROM Laptimes");
        db.SaveChanges();

        int entityCounter = 0;
        int rowCounter = 0;

        foreach (var record in records)
        {
            //let us this later in production
            //var existingLaptime = db.Laptimes.FirstOrDefault(lp => lp.RaceId == record.RaceId && lp.DriverId == record.DriverId && lp.Lap == record.Lap);
            rowCounter++;


            entityCounter++;
            db.Laptimes.Add(record);

            if (entityCounter % 1000 == 0)
            {
                db.SaveChanges();
                Console.WriteLine($"{entityCounter} laps added.");

                // dispose the context after each SaveChanges() and create a new one
                //db.Dispose();


            }

            //if (entityCounter > 3000)
            //    break;
        }
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Total of {entityCounter} laptimes recorded.");
        Console.ResetColor();
    }
}

void AddLaptimes()
{
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        PrepareHeaderForMatch = args => args.Header.ToLower(),
    };

    // 567467 rows in csv file
    using (var reader = new StreamReader("CSV\\lap_times.csv"))
    using (var csv = new CsvReader(reader, config))
    {
        csv.Context.RegisterClassMap<LaptimeMap>();
        var records = csv.GetRecords<Laptime>();

        int entityCounter = 0;
        int rowCounter = 0;
        Laptime? existingLaptime = null;

        foreach (var record in records)
        {
            rowCounter++;
            existingLaptime = db.Laptimes.FirstOrDefault(lp => lp.RaceId == record.RaceId && lp.DriverId == record.DriverId && lp.Lap == record.Lap);

            if (existingLaptime == null)
            {
                entityCounter++;
                db.Laptimes.Add(record);
            }

            if (rowCounter % 1000 == 0 && rowCounter != 0)
            {
                db.SaveChanges();
                Console.WriteLine($"{rowCounter} lines checked.");
            }

        }
        db.SaveChanges();
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Total of {entityCounter} additional laptimes recorded.");
        Console.ResetColor();
    }
}

void ImportRaces()
{
    CsvConfiguration config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
        PrepareHeaderForMatch = args => args.Header.ToLower(),
    };

    // 567467 rows in csv file
    using (var reader = new StreamReader("CSV\\races.csv"))
    using (var csv = new CsvReader(reader, config))
    {
        csv.Context.RegisterClassMap<RaceMap>();
        var records = csv.GetRecords<Race>();

        // reset Laptime.RowId sequence
        db.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name = 'Races'");
        // lets delete to all data from the sqlite database
        db.Database.ExecuteSqlRaw("DELETE FROM Races");
        db.SaveChanges();

        int entityCounter = 0;
        int rowCounter = 0;

        foreach (var record in records)
        {
            //let us this later in production
            //var existingLaptime = db.Laptimes.FirstOrDefault(lp => lp.RaceId == record.RaceId && lp.DriverId == record.DriverId && lp.Lap == record.Lap);
            rowCounter++;


            entityCounter++;
            db.Races.Add(record);

            if (entityCounter % 500 == 0)
            {
                db.SaveChanges();
                Console.WriteLine($"{entityCounter} laps added.");

                // for speed, dispose the context after each SaveChanges() and create a new one
                //db.Dispose();


            }

            //if (entityCounter > 3000)
            //    break;
        }
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine($"Total of {entityCounter} Races recorded.");
        Console.ResetColor();
    }
}

// write a generic method to export imported csv files to sqlite database. The method should take the class Like Race or Driver.
// The method should check if ID in class table does not exist and if this is new data then write it to the right table. 
// For class Race it  should be written to the table Races and for class Driver it should be written to the table Drivers.



void ImportCsv<T>(string filePath) where T : class
{
    using var reader = new StreamReader(filePath);
    using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));

    foreach (var record in csv.GetRecords<T>())
    {
        // check if this set contains the record with column driverRef
        Console.WriteLine(record);

        // PROBLEM: with record.driverRef
        if (db.Set<T>().Find(record.GetType()) == null)
        {
            db.Set<T>().Add(record);
        }
    }

    db.SaveChanges();
}

