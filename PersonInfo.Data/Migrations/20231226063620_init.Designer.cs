﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonInfo.Data;

#nullable disable

namespace PersonInfo.Data.Migrations
{
    [DbContext(typeof(PersonInfoContext))]
    [Migration("20231226063620_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PersonInfo.Model.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime?>("DoB")
                        .IsRequired()
                        .HasColumnType("date");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PIN")
                        .IsRequired()
                        .HasMaxLength(11)
                        .IsUnicode(false)
                        .HasColumnType("char(11)")
                        .IsFixedLength();

                    b.HasKey("Id");

                    b.ToTable("Persons", (string)null);
                });

            modelBuilder.Entity("PersonInfo.Model.Models.PersonRelatedPeople", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ParentId")
                        .HasColumnType("int")
                        .HasColumnName("ParentId");

                    b.Property<int>("RelatedPersonId")
                        .HasColumnType("int")
                        .HasColumnName("RelatedPersonId");

                    b.Property<int>("RelationTypeId")
                        .HasColumnType("int")
                        .HasColumnName("RelationTypeId");

                    b.HasKey("Id");

                    b.HasAlternateKey("ParentId", "RelationTypeId", "RelatedPersonId");

                    b.HasIndex("RelatedPersonId");

                    b.HasIndex("RelationTypeId");

                    b.ToTable("PersonRelatedPeople", (string)null);
                });

            modelBuilder.Entity("PersonInfo.Model.Models.PhoneNumber", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("PersonId")
                        .HasColumnType("int");

                    b.Property<string>("PhoneNum")
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<int?>("PhoneTypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId");

                    b.HasIndex("PhoneTypeId");

                    b.ToTable("PhoneNumbers", (string)null);
                });

            modelBuilder.Entity("PersonInfo.Model.Models.PhoneType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("PhoneNumberType")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("PhoneTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PhoneNumberType = "Office"
                        },
                        new
                        {
                            Id = 2,
                            PhoneNumberType = "Home"
                        },
                        new
                        {
                            Id = 3,
                            PhoneNumberType = "Mobile"
                        });
                });

            modelBuilder.Entity("PersonInfo.Model.Models.RelationType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Relation")
                        .IsRequired()
                        .IsUnicode(true)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.ToTable("RelationTypes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Relation = "Relative"
                        },
                        new
                        {
                            Id = 2,
                            Relation = "Colleague"
                        },
                        new
                        {
                            Id = 3,
                            Relation = "Acquaintance"
                        },
                        new
                        {
                            Id = 4,
                            Relation = "Other"
                        });
                });

            modelBuilder.Entity("PersonInfo.Model.Models.PersonRelatedPeople", b =>
                {
                    b.HasOne("PersonInfo.Model.Models.Person", "Person")
                        .WithMany("PersonRelatedPeople")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PersonInfo.Model.Models.Person", "RelatedPerson")
                        .WithMany()
                        .HasForeignKey("RelatedPersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("PersonInfo.Model.Models.RelationType", "RelationType")
                        .WithMany("PersonRelatedPeoples")
                        .HasForeignKey("RelationTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");

                    b.Navigation("RelatedPerson");

                    b.Navigation("RelationType");
                });

            modelBuilder.Entity("PersonInfo.Model.Models.PhoneNumber", b =>
                {
                    b.HasOne("PersonInfo.Model.Models.Person", "Person")
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("PersonId");

                    b.HasOne("PersonInfo.Model.Models.PhoneType", "PhoneTypes")
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("PhoneTypeId");

                    b.Navigation("Person");

                    b.Navigation("PhoneTypes");
                });

            modelBuilder.Entity("PersonInfo.Model.Models.Person", b =>
                {
                    b.Navigation("PersonRelatedPeople");

                    b.Navigation("PhoneNumbers");
                });

            modelBuilder.Entity("PersonInfo.Model.Models.PhoneType", b =>
                {
                    b.Navigation("PhoneNumbers");
                });

            modelBuilder.Entity("PersonInfo.Model.Models.RelationType", b =>
                {
                    b.Navigation("PersonRelatedPeoples");
                });
#pragma warning restore 612, 618
        }
    }
}
