using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonInfo.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PersonInfo.Data.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {

            builder.ToTable("Persons");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.FirstName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(p => p.LastName)
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(true);

            builder.Property(p => p.PIN)
                .HasMaxLength(11)
                .IsFixedLength(true)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(p => p.DoB)
                .HasColumnType("date")
                .IsRequired();

        }
    }
}
