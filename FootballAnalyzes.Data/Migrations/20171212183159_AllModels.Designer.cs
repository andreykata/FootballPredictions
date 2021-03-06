﻿namespace FootballAnalyzes.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;

    [DbContext(typeof(FootballAnalyzesDbContext))]
    [Migration("20171212183159_AllModels")]
    partial class AllModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FootballAnalyzes.Data.Models.FootballGame", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayTeamId");

                    b.Property<int?>("FirstHalfResultId");

                    b.Property<int?>("FullTimeResultId");

                    b.Property<int?>("GameStatisticId");

                    b.Property<int>("HomeTeamId");

                    b.Property<int>("LeagueId");

                    b.Property<DateTime>("MatchDate");

                    b.HasKey("Id");

                    b.HasIndex("AwayTeamId");

                    b.HasIndex("FirstHalfResultId");

                    b.HasIndex("FullTimeResultId");

                    b.HasIndex("GameStatisticId");

                    b.HasIndex("HomeTeamId");

                    b.HasIndex("LeagueId");

                    b.ToTable("FootballGames");
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.GameResult", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayTeamGoals");

                    b.Property<int>("HomeTeamGoals");

                    b.Property<int>("Result");

                    b.HasKey("Id");

                    b.ToTable("GameResults");
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.GameStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AwayTeamCorners");

                    b.Property<int>("AwayTeamFouls");

                    b.Property<int>("AwayTeamOffsides");

                    b.Property<int>("AwayTeamShotsOnTarget");

                    b.Property<int>("AwayTeamShotsWide");

                    b.Property<int>("HomeTeamCorners");

                    b.Property<int>("HomeTeamFouls");

                    b.Property<int>("HomeTeamOffsides");

                    b.Property<int>("HomeTeamShotsOnTarget");

                    b.Property<int>("HomeTeamShotsWide");

                    b.HasKey("Id");

                    b.ToTable("GameStatistics");
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.League", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Country");

                    b.Property<string>("Name");

                    b.Property<string>("Stage");

                    b.Property<string>("Type");

                    b.Property<string>("UniqueName");

                    b.Property<string>("Year");

                    b.HasKey("Id");

                    b.ToTable("Leagues");
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.Prediction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("GameId");

                    b.Property<string>("Name");

                    b.Property<double>("Procent");

                    b.Property<string>("Result");

                    b.Property<bool>("Selected");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Predictions");
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<string>("UniqueName");

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime>("Birthdate");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.FootballGame", b =>
                {
                    b.HasOne("FootballAnalyzes.Data.Models.Team", "AwayTeam")
                        .WithMany()
                        .HasForeignKey("AwayTeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballAnalyzes.Data.Models.GameResult", "FirstHalfResult")
                        .WithMany()
                        .HasForeignKey("FirstHalfResultId");

                    b.HasOne("FootballAnalyzes.Data.Models.GameResult", "FullTimeResult")
                        .WithMany()
                        .HasForeignKey("FullTimeResultId");

                    b.HasOne("FootballAnalyzes.Data.Models.GameStatistic", "GameStatistic")
                        .WithMany()
                        .HasForeignKey("GameStatisticId");

                    b.HasOne("FootballAnalyzes.Data.Models.Team", "HomeTeam")
                        .WithMany()
                        .HasForeignKey("HomeTeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballAnalyzes.Data.Models.League", "League")
                        .WithMany("Games")
                        .HasForeignKey("LeagueId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("FootballAnalyzes.Data.Models.Prediction", b =>
                {
                    b.HasOne("FootballAnalyzes.Data.Models.FootballGame", "Game")
                        .WithMany("Predictions")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("FootballAnalyzes.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("FootballAnalyzes.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("FootballAnalyzes.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("FootballAnalyzes.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
