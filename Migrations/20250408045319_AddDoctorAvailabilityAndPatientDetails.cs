using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HealthcareBookingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorAvailabilityAndPatientDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "AvailableEndTime",
                table: "Doctors",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "AvailableStartTime",
                table: "Doctors",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ContactNumber",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PatientName",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sex",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableEndTime",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "AvailableStartTime",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Age",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ContactNumber",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientName",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "Sex",
                table: "Appointments");
        }
    }
}
