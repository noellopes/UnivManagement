using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnivManagement.Migrations
{
    /// <inheritdoc />
    public partial class InicialUniversidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AcademicYear",
                columns: table => new
                {
                    AcademicYearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartYear = table.Column<int>(type: "int", nullable: false),
                    EndYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicYear", x => x.AcademicYearId);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationApplication",
                columns: table => new
                {
                    AccommodationApplicationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasScholarshipPriority = table.Column<bool>(type: "bit", nullable: false),
                    PriorityCheckedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PriorityOverrideReason = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationApplication", x => x.AccommodationApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationContract",
                columns: table => new
                {
                    AccommodationContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    RoomAllocationId = table.Column<int>(type: "int", nullable: false),
                    SignatureDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationContract", x => x.AccommodationContractId);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationPayment",
                columns: table => new
                {
                    AccommodationPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccommodationContractId = table.Column<int>(type: "int", nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationPayment", x => x.AccommodationPaymentId);
                });

            migrationBuilder.CreateTable(
                name: "AntigosAlunos",
                columns: table => new
                {
                    IdAluno = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Curso = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SituacaoProfissional = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AntigosAlunos", x => x.IdAluno);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationStatusHistory",
                columns: table => new
                {
                    ApplicationStatusHistoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccommodationApplicationId = table.Column<int>(type: "int", nullable: false),
                    ChangedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChangedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationStatusHistory", x => x.ApplicationStatusHistoryId);
                });

            migrationBuilder.CreateTable(
                name: "Docente",
                columns: table => new
                {
                    DocenteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Nif = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FuncaoDocenteFK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RegimeFK = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataAdmissao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Morada = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DepartamentoFk = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Docente", x => x.DocenteId);
                });

            migrationBuilder.CreateTable(
                name: "Empresas",
                columns: table => new
                {
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Setor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contacto = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empresas", x => x.IdEmpresa);
                });

            migrationBuilder.CreateTable(
                name: "GestoresEstagio",
                columns: table => new
                {
                    IdGestor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GestoresEstagio", x => x.IdGestor);
                });

            migrationBuilder.CreateTable(
                name: "InscrCourse",
                columns: table => new
                {
                    InscrCourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoursePlanYearId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    StateInscr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateInscr = table.Column<DateOnly>(type: "date", nullable: false),
                    DateGrad = table.Column<DateOnly>(type: "date", nullable: true),
                    FinalAvgGrade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CourseFeePaid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscrCourse", x => x.InscrCourseId);
                });

            migrationBuilder.CreateTable(
                name: "InscrUC",
                columns: table => new
                {
                    InscrUCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UcId = table.Column<int>(type: "int", nullable: false),
                    InscrCourseId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    StateInscr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateInscr = table.Column<DateOnly>(type: "date", nullable: false),
                    DateEnd = table.Column<DateOnly>(type: "date", nullable: true),
                    Grade = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscrUC", x => x.InscrUCId);
                });

            migrationBuilder.CreateTable(
                name: "OrientadoresEstagio",
                columns: table => new
                {
                    IdOrientador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrientadoresEstagio", x => x.IdOrientador);
                });

            migrationBuilder.CreateTable(
                name: "Refeicoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Preco = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Refeicoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    RoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ResidenceId = table.Column<int>(type: "int", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Floor = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestStatuses",
                columns: table => new
                {
                    ServiceRequestStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsFinal = table.Column<bool>(type: "bit", nullable: false),
                    RequiresNote = table.Column<bool>(type: "bit", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestStatuses", x => x.ServiceRequestStatusId);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequestTypes",
                columns: table => new
                {
                    ServiceRequestTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequestTypes", x => x.ServiceRequestTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ServicoFuncionario",
                columns: table => new
                {
                    ServicoFuncionarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false),
                    DataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicoFuncionario", x => x.ServicoFuncionarioId);
                });

            migrationBuilder.CreateTable(
                name: "StayRecord",
                columns: table => new
                {
                    StayRecordId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoomAllocationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    OccurredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConditionNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    RecordedBy = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StayRecord", x => x.StayRecordId);
                });

            migrationBuilder.CreateTable(
                name: "Student",
                columns: table => new
                {
                    StudentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumStudent = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    NIF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(45)", maxLength: 45, nullable: false),
                    Telephone = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Student", x => x.StudentId);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlan",
                columns: table => new
                {
                    StudyPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EctsTotal = table.Column<int>(type: "int", nullable: false),
                    EctsTotalYear = table.Column<int>(type: "int", nullable: false),
                    MinEctsYear2 = table.Column<int>(type: "int", nullable: false),
                    MinEctsYear3 = table.Column<int>(type: "int", nullable: false),
                    NumYears = table.Column<int>(type: "int", nullable: false),
                    CreationDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlan", x => x.StudyPlanId);
                });

            migrationBuilder.CreateTable(
                name: "StudyPlanUC",
                columns: table => new
                {
                    StudyPlanUCId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudyPlanId = table.Column<int>(type: "int", nullable: false),
                    UcId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Semester = table.Column<int>(type: "int", nullable: false),
                    ValueEcts = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudyPlanUC", x => x.StudyPlanUCId);
                });

            migrationBuilder.CreateTable(
                name: "UC",
                columns: table => new
                {
                    UcId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Acronym = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UC", x => x.UcId);
                });

            migrationBuilder.CreateTable(
                name: "SchoolCalendar",
                columns: table => new
                {
                    SchoolCalendarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartSemesterS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSemesterS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartFreqExamS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndFreqExamS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartResitExamS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndResitExamS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartSpecialExamS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSpecialExamS1 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartSemesterS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSemesterS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartFreqExamS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndFreqExamS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartResitExamS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndResitExamS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartSpecialExamS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSpecialExamS2 = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolCalendar", x => x.SchoolCalendarId);
                    table.ForeignKey(
                        name: "FK_SchoolCalendar_AcademicYear_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "AcademicYear",
                        principalColumn: "AcademicYearId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carreiras",
                columns: table => new
                {
                    IdCarreira = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CargoAtual = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmpresaAtual = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Competencias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Certificacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HistoricoProfissional = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAluno = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carreiras", x => x.IdCarreira);
                    table.ForeignKey(
                        name: "FK_Carreiras_AntigosAlunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "AntigosAlunos",
                        principalColumn: "IdAluno",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Empregadores",
                columns: table => new
                {
                    IdEmpregador = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empregadores", x => x.IdEmpregador);
                    table.ForeignKey(
                        name: "FK_Empregadores_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ofertas",
                columns: table => new
                {
                    IdOferta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Titulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Localizacao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Requisitos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remuneracao = table.Column<double>(type: "float", nullable: false),
                    DataLimite = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEmpresa = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ofertas", x => x.IdOferta);
                    table.ForeignKey(
                        name: "FK_Ofertas_Empresas_IdEmpresa",
                        column: x => x.IdEmpresa,
                        principalTable: "Empresas",
                        principalColumn: "IdEmpresa",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    ServiceRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ServiceRequestTypeId = table.Column<int>(type: "int", nullable: false),
                    ServiceRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    RequesterName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RequesterEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    AssignedTo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.ServiceRequestId);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_ServiceRequestStatuses_ServiceRequestStatusId",
                        column: x => x.ServiceRequestStatusId,
                        principalTable: "ServiceRequestStatuses",
                        principalColumn: "ServiceRequestStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_ServiceRequestTypes_ServiceRequestTypeId",
                        column: x => x.ServiceRequestTypeId,
                        principalTable: "ServiceRequestTypes",
                        principalColumn: "ServiceRequestTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AcademicDegree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Acronym = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CourseState = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SchoolCalendarId = table.Column<int>(type: "int", nullable: false),
                    DocenteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Course_Docente_DocenteId",
                        column: x => x.DocenteId,
                        principalTable: "Docente",
                        principalColumn: "DocenteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Course_SchoolCalendar_SchoolCalendarId",
                        column: x => x.SchoolCalendarId,
                        principalTable: "SchoolCalendar",
                        principalColumn: "SchoolCalendarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estagios",
                columns: table => new
                {
                    IdEstagio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Duracao = table.Column<int>(type: "int", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PlanoTrabalhos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAluno = table.Column<int>(type: "int", nullable: false),
                    IdEmpregador = table.Column<int>(type: "int", nullable: false),
                    IdGestor = table.Column<int>(type: "int", nullable: false),
                    IdOrientador = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estagios", x => x.IdEstagio);
                    table.ForeignKey(
                        name: "FK_Estagios_AntigosAlunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "AntigosAlunos",
                        principalColumn: "IdAluno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estagios_Empregadores_IdEmpregador",
                        column: x => x.IdEmpregador,
                        principalTable: "Empregadores",
                        principalColumn: "IdEmpregador",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estagios_GestoresEstagio_IdGestor",
                        column: x => x.IdGestor,
                        principalTable: "GestoresEstagio",
                        principalColumn: "IdGestor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Estagios_OrientadoresEstagio_IdOrientador",
                        column: x => x.IdOrientador,
                        principalTable: "OrientadoresEstagio",
                        principalColumn: "IdOrientador");
                });

            migrationBuilder.CreateTable(
                name: "Candidaturas",
                columns: table => new
                {
                    IdCandidatura = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataCandidatura = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Curriculo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdAluno = table.Column<int>(type: "int", nullable: false),
                    IdOferta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidaturas", x => x.IdCandidatura);
                    table.ForeignKey(
                        name: "FK_Candidaturas_AntigosAlunos_IdAluno",
                        column: x => x.IdAluno,
                        principalTable: "AntigosAlunos",
                        principalColumn: "IdAluno",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Candidaturas_Ofertas_IdOferta",
                        column: x => x.IdOferta,
                        principalTable: "Ofertas",
                        principalColumn: "IdOferta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Avaliacoes",
                columns: table => new
                {
                    IdAvaliacao = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataAvaliacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Classificacao = table.Column<double>(type: "float", nullable: false),
                    Observacoes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEstagio = table.Column<int>(type: "int", nullable: false),
                    IdOrientador = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avaliacoes", x => x.IdAvaliacao);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_Estagios_IdEstagio",
                        column: x => x.IdEstagio,
                        principalTable: "Estagios",
                        principalColumn: "IdEstagio",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Avaliacoes_OrientadoresEstagio_IdOrientador",
                        column: x => x.IdOrientador,
                        principalTable: "OrientadoresEstagio",
                        principalColumn: "IdOrientador",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RelatoriosEstagio",
                columns: table => new
                {
                    IdRelatorio = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataEntrega = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ficheiro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IdEstagio = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RelatoriosEstagio", x => x.IdRelatorio);
                    table.ForeignKey(
                        name: "FK_RelatoriosEstagio_Estagios_IdEstagio",
                        column: x => x.IdEstagio,
                        principalTable: "Estagios",
                        principalColumn: "IdEstagio",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_IdEstagio",
                table: "Avaliacoes",
                column: "IdEstagio");

            migrationBuilder.CreateIndex(
                name: "IX_Avaliacoes_IdOrientador",
                table: "Avaliacoes",
                column: "IdOrientador");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_IdAluno",
                table: "Candidaturas",
                column: "IdAluno");

            migrationBuilder.CreateIndex(
                name: "IX_Candidaturas_IdOferta",
                table: "Candidaturas",
                column: "IdOferta");

            migrationBuilder.CreateIndex(
                name: "IX_Carreiras_IdAluno",
                table: "Carreiras",
                column: "IdAluno",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_Code",
                table: "Course",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Course_DocenteId",
                table: "Course",
                column: "DocenteId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_SchoolCalendarId",
                table: "Course",
                column: "SchoolCalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Empregadores_IdEmpresa",
                table: "Empregadores",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_Estagios_IdAluno",
                table: "Estagios",
                column: "IdAluno");

            migrationBuilder.CreateIndex(
                name: "IX_Estagios_IdEmpregador",
                table: "Estagios",
                column: "IdEmpregador");

            migrationBuilder.CreateIndex(
                name: "IX_Estagios_IdGestor",
                table: "Estagios",
                column: "IdGestor");

            migrationBuilder.CreateIndex(
                name: "IX_Estagios_IdOrientador",
                table: "Estagios",
                column: "IdOrientador");

            migrationBuilder.CreateIndex(
                name: "IX_Ofertas_IdEmpresa",
                table: "Ofertas",
                column: "IdEmpresa");

            migrationBuilder.CreateIndex(
                name: "IX_RelatoriosEstagio_IdEstagio",
                table: "RelatoriosEstagio",
                column: "IdEstagio");

            migrationBuilder.CreateIndex(
                name: "IX_Room_ResidenceId_Number",
                table: "Room",
                columns: new[] { "ResidenceId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SchoolCalendar_AcademicYearId",
                table: "SchoolCalendar",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ServiceRequestStatusId",
                table: "ServiceRequests",
                column: "ServiceRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ServiceRequestTypeId",
                table: "ServiceRequests",
                column: "ServiceRequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestStatuses_Name",
                table: "ServiceRequestStatuses",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequestTypes_Name",
                table: "ServiceRequestTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_Email",
                table: "Student",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_NIF",
                table: "Student",
                column: "NIF",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_NumStudent",
                table: "Student",
                column: "NumStudent",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Student_Telephone",
                table: "Student",
                column: "Telephone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UC_Code",
                table: "UC",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccommodationApplication");

            migrationBuilder.DropTable(
                name: "AccommodationContract");

            migrationBuilder.DropTable(
                name: "AccommodationPayment");

            migrationBuilder.DropTable(
                name: "ApplicationStatusHistory");

            migrationBuilder.DropTable(
                name: "Avaliacoes");

            migrationBuilder.DropTable(
                name: "Candidaturas");

            migrationBuilder.DropTable(
                name: "Carreiras");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "InscrCourse");

            migrationBuilder.DropTable(
                name: "InscrUC");

            migrationBuilder.DropTable(
                name: "Refeicoes");

            migrationBuilder.DropTable(
                name: "RelatoriosEstagio");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "ServicoFuncionario");

            migrationBuilder.DropTable(
                name: "StayRecord");

            migrationBuilder.DropTable(
                name: "Student");

            migrationBuilder.DropTable(
                name: "StudyPlan");

            migrationBuilder.DropTable(
                name: "StudyPlanUC");

            migrationBuilder.DropTable(
                name: "UC");

            migrationBuilder.DropTable(
                name: "Ofertas");

            migrationBuilder.DropTable(
                name: "Docente");

            migrationBuilder.DropTable(
                name: "SchoolCalendar");

            migrationBuilder.DropTable(
                name: "Estagios");

            migrationBuilder.DropTable(
                name: "ServiceRequestStatuses");

            migrationBuilder.DropTable(
                name: "ServiceRequestTypes");

            migrationBuilder.DropTable(
                name: "AcademicYear");

            migrationBuilder.DropTable(
                name: "AntigosAlunos");

            migrationBuilder.DropTable(
                name: "Empregadores");

            migrationBuilder.DropTable(
                name: "GestoresEstagio");

            migrationBuilder.DropTable(
                name: "OrientadoresEstagio");

            migrationBuilder.DropTable(
                name: "Empresas");
        }
    }
}
