# Prepara y arranca el modulo 9 (Requests and Administrative Services) en una base de datos nueva.
# Uso, desde cualquier carpeta del proyecto, en la terminal de Visual Studio:
#   .\Demo\preparar-demo.ps1
# Si la base ya existe y falla, usa otro nombre:  .\Demo\preparar-demo.ps1 -Base UnivDemoGrupo9b
# Para solo preparar sin arrancar:                .\Demo\preparar-demo.ps1 -NoArrancar
param(
    [string]$Base = 'UnivDemoGrupo9',
    [switch]$NoArrancar
)

$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot
Set-Location $raiz

function Paso($texto) { Write-Host "`n== $texto" -ForegroundColor Cyan }

Paso '1/7 Comprobando .NET'
# El proyecto es net10: hace falta el runtime de ASP.NET 10 (el SDK puede ser 10 o superior)
$runtimes = dotnet --list-runtimes | Select-String 'Microsoft\.AspNetCore\.App 10\.'
if (-not $runtimes) { throw 'No hay runtime de ASP.NET Core 10 instalado. Avisa al profesor.' }
Write-Host ($runtimes | Select-Object -Last 1)
dotnet --version

Paso '2/7 Comprobando dotnet ef'
$efOk = $true
try { dotnet ef --version | Out-Null; if ($LASTEXITCODE -ne 0) { $efOk = $false } } catch { $efOk = $false }
if (-not $efOk) {
    Write-Host 'dotnet ef no esta instalado: instalando (necesita internet)...'
    dotnet tool install --global dotnet-ef
    $env:PATH += ";$env:USERPROFILE\.dotnet\tools"
}

Paso "3/7 Apuntando esta terminal a la base nueva '$Base'"
$cadena = "Server=(localdb)\mssqllocaldb;Database=$Base;Trusted_Connection=True;MultipleActiveResultSets=true"
$env:ConnectionStrings__UsersConnection = $cadena
$env:ConnectionStrings__UniversityConnection = $cadena
$env:ASPNETCORE_ENVIRONMENT = 'Development'

Paso '4/7 Restaurando paquetes y compilando (la primera vez tarda)'
dotnet build --configuration Release --nologo -v q
if ($LASTEXITCODE -ne 0) { throw 'La compilacion ha fallado. Revisa los errores de arriba.' }

Paso '5/7 Migracion'
$migracion = Get-ChildItem (Join-Path $raiz 'Migrations') -Filter '*_Demo.cs' -ErrorAction SilentlyContinue |
    Where-Object { $_.Name -notlike '*Designer*' } | Select-Object -First 1
if (-not $migracion) {
    dotnet ef migrations add Demo -c UniversityDbContext --configuration Release
    if ($LASTEXITCODE -ne 0) { throw 'Fallo al crear la migracion.' }
    $migracion = Get-ChildItem (Join-Path $raiz 'Migrations') -Filter '*_Demo.cs' |
        Where-Object { $_.Name -notlike '*Designer*' } | Select-Object -First 1
} else { Write-Host "Ya existe $($migracion.Name), se reutiliza." }

# Arreglo del error de cascada que viene de master (CourseStudyPlanYear): Cascade -> Restrict
$texto = [IO.File]::ReadAllText($migracion.FullName)
$i = $texto.IndexOf('FK_CourseStudyPlanYear_Course_CourseId')
if ($i -ge 0) {
    $j = $texto.IndexOf('ReferentialAction.Cascade', $i)
    $fin = $texto.IndexOf(');', $i)
    if ($j -ge 0 -and $j -lt $fin) {
        $texto = $texto.Substring(0, $j) + 'ReferentialAction.Restrict' + $texto.Substring($j + 'ReferentialAction.Cascade'.Length)
        [IO.File]::WriteAllText($migracion.FullName, $texto)
        Write-Host 'Arreglado el borrado en cascada de CourseStudyPlanYear.'
    } else { Write-Host 'La clave de CourseStudyPlanYear ya estaba arreglada.' }
}

Paso '6/7 Creando las tablas'
dotnet ef database update -c UniversityDbContext --configuration Release
if ($LASTEXITCODE -ne 0) { throw 'Fallo al crear las tablas de la universidad.' }
dotnet ef database update -c ApplicationUsersDbContext --configuration Release
if ($LASTEXITCODE -ne 0) { throw 'Fallo al crear las tablas de usuarios.' }

if ($NoArrancar) { Write-Host "`nListo. Base '$Base' preparada. Arranca con: dotnet run --configuration Release --launch-profile http" -ForegroundColor Green; return }

Paso '7/7 Arrancando (abre http://localhost:5244/AdministrativeServices; Ctrl+C para parar)'
dotnet run --configuration Release --launch-profile http
