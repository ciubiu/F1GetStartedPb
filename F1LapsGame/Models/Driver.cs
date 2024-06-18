using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace F1LapsGame.Models;

public class Driver
{
    // 2,"heidfeld",\N,"HEI","Nick","Heidfeld","1977-05-10","German","http..."
    public int DriverId { get; set; }
    public string DriverRef { get; set; } = string.Empty;
    public int? Number { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Forename { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class LaptimeEntityConfigration : IEntityTypeConfiguration<Driver>
{
    public void Configure(EntityTypeBuilder<Driver> builder)
    {
        builder.HasKey(e => e.DriverId);
        builder.Property(e => e.Code).HasMaxLength(3);
        builder.Property(e => e.Dob).HasColumnType("date");
    }
}