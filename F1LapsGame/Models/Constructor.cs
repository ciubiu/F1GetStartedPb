using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace F1LapsGame.Models;

public class Constructor
{
    //  constructorId,constructorRef,name,nationality,url
    //  1,"mclaren","McLaren","British","http..."

    public int ConstructorId { get; set; }
    public string ConstructorRef { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Nationality { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
}

public class ConstructorEntityConfigration : IEntityTypeConfiguration<Constructor>
{
    public void Configure(EntityTypeBuilder<Constructor> builder)
    {
        builder.HasKey(e => e.ConstructorId);
    }
}