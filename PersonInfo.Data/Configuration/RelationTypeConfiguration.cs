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
    public class RelationTypeConfiguration : IEntityTypeConfiguration<RelationType>
    {
        public void Configure(EntityTypeBuilder<RelationType> builder)
        {

            builder.ToTable("RelationTypes");

            builder.HasKey(rt => rt.Id);

            builder.Property(rt => rt.Relation)
                .HasColumnType("nvarchar(30)")
                .IsUnicode(true);


            // relations

            builder.HasMany(prp => prp.PersonRelatedPeoples)
                .WithOne(rt => rt.RelationType)
                .HasForeignKey(prp => prp.RelationTypeId);
        }
    }
}
