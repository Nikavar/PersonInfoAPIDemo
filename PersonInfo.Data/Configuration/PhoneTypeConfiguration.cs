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
    public class PhoneTypeConfiguration : IEntityTypeConfiguration<PhoneType>
    {
        public void Configure(EntityTypeBuilder<PhoneType> builder)
        {

            builder.ToTable("PhoneTypes");

            builder.HasKey(pt => pt.Id);

            builder.Property(pt => pt.PhoneNumberType)
                .HasColumnType("nvarchar(30)")
                .IsUnicode(true);


            // relations

            builder.HasMany(pn => pn.PhoneNumbers)
                .WithOne(pt => pt.PhoneTypes);
        }
    }
}
