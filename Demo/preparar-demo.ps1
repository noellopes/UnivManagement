# Prepara y arranca el modulo 9 (Requests and Administrative Services) en una base de datos NUEVA.
#
# Uso (terminal de Visual Studio, en cualquier carpeta del proyecto):
#   .\Demo\preparar-demo.ps1                       prepara la base y arranca la app
#   .\Demo\preparar-demo.ps1 -NoArrancar           solo prepara (compila, migra, crea tablas)
#   .\Demo\preparar-demo.ps1 -Verificar            arranca, comprueba las 9 pantallas y los datos, y deja la app en marcha
#   .\Demo\preparar-demo.ps1 -Verificar -ProbarYSalir   igual, pero cierra la app al terminar (para ensayar)
#   .\Demo\preparar-demo.ps1 -AbrirNavegador       arranca y abre el navegador en el menu del modulo
#   .\Demo\preparar-demo.ps1 -Base OtroNombre      usa otra base de datos
#   .\Demo\preparar-demo.ps1 -Servidor '(localdb)\otra'  usa otra instancia de SQL Server (por defecto LocalDB mssqllocaldb)
#   .\Demo\preparar-demo.ps1 -Puerto 5300          usa otro puerto (por defecto 5244; si esta ocupado busca uno libre)
#   .\Demo\preparar-demo.ps1 -Reiniciar            borra la base (solo si se llama UnivDemo*/UnivEnsayo*) y empieza de cero
#
# Si Windows bloquea el script:
#   powershell -ExecutionPolicy Bypass -File .\Demo\preparar-demo.ps1
#
# IMPORTANTE: arranca la app desde ESTA terminal (la que ejecuta el script). Las variables que apuntan a la base
# solo existen aqui. Con F5 o desde otra terminal la app usaria otra base y fallaria.
param(
    [string]$Base = 'UnivDemoGrupo9',
    [string]$Servidor = '(localdb)\mssqllocaldb',
    [int]$Puerto = 5244,
    [switch]$NoArrancar,
    [switch]$Verificar,
    [switch]$AbrirNavegador,
    [switch]$Reiniciar,
    [switch]$ProbarYSalir
)

$ErrorActionPreference = 'Stop'
$raiz = Split-Path -Parent $PSScriptRoot
Set-Location $raiz
$servidor = $Servidor
$registro = Join-Path $env:TEMP 'preparar-demo.log'
try { Start-Transcript -Path $registro -Force | Out-Null } catch { }

function Paso($texto) { Write-Host "`n== $texto" -ForegroundColor Cyan }
function Aviso($texto) { Write-Host "AVISO: $texto" -ForegroundColor Yellow }
function Ok($texto) { Write-Host "OK: $texto" -ForegroundColor Green }

# Ejecuta una consulta que devuelve un valor. Devuelve $null si no se puede consultar.
function Invoke-Sql([string]$Db, [string]$Consulta) {
    try {
        $cn = New-Object System.Data.SqlClient.SqlConnection("Server=$servidor;Database=$Db;Trusted_Connection=True;Connect Timeout=15")
        $cn.Open()
        $cmd = $cn.CreateCommand(); $cmd.CommandText = $Consulta
        $valor = $cmd.ExecuteScalar(); $cn.Close()
        if ($valor -is [DBNull]) { return $null }
        return $valor
    } catch { }
    if (Get-Command sqlcmd -ErrorAction SilentlyContinue) {
        try {
            $salida = sqlcmd -S $servidor -d $Db -Q "SET NOCOUNT ON; $Consulta" -h -1 -W 2>$null
            if ($LASTEXITCODE -eq 0) { return ($salida | Where-Object { $_ } | Select-Object -First 1) }
        } catch { }
    }
    return $null
}

function Puerto-Libre([int]$desde) {
    for ($p = $desde; $p -lt $desde + 50; $p++) {
        try {
            $l = New-Object System.Net.Sockets.TcpListener([System.Net.IPAddress]::Loopback, $p)
            $l.Start(); $l.Stop(); return $p
        } catch { }
    }
    throw "No hay ningun puerto libre entre $desde y $($desde + 49)."
}

function Buscar-Migracion {
    Get-ChildItem (Join-Path $raiz 'Migrations') -Filter '*_Demo.cs' -ErrorAction SilentlyContinue |
        Where-Object { $_.Name -notlike '*Designer*' } | Select-Object -First 1
}

try {
    Paso '1/9 Comprobando el entorno'
    $rama = (git rev-parse --abbrev-ref HEAD 2>$null)
    if ($rama) { Write-Host "Rama actual: $rama" }
    if ($rama -and $rama -ne 'feature/requests-grupo9-demo') { Aviso "No estas en feature/requests-grupo9-demo (estas en '$rama')." }
    $runtimes = dotnet --list-runtimes | Select-String 'Microsoft\.AspNetCore\.App 10\.'
    if (-not $runtimes) { throw 'No hay runtime de ASP.NET Core 10 instalado (dotnet --list-runtimes). Avisa al profesor.' }
    Write-Host ($runtimes | Select-Object -Last 1)
    Write-Host "dotnet $(dotnet --version)"
    if ($env:DOTNET_ROOT) { Aviso "DOTNET_ROOT esta definida ($env:DOTNET_ROOT). Si falla por el runtime, es por esta variable." }
    if (Get-Command sqllocaldb -ErrorAction SilentlyContinue) {
        # LocalDB arranca sola al conectar; este intento solo adelanta el trabajo y se ignora si falla
        try { $null = & sqllocaldb start mssqllocaldb 2>&1 } catch { }
        Ok 'LocalDB instalado'
    } else { Aviso 'No se encuentra sqllocaldb: no puedo comprobar que LocalDB este instalado.' }

    Paso '2/9 Comprobando dotnet ef'
    $efOk = $true
    try { dotnet ef --version | Out-Null; if ($LASTEXITCODE -ne 0) { $efOk = $false } } catch { $efOk = $false }
    if (-not $efOk) {
        Write-Host 'dotnet ef no esta instalado: instalando (necesita internet)...'
        dotnet tool install --global dotnet-ef
        if ($LASTEXITCODE -ne 0) { throw 'No se pudo instalar dotnet-ef. Comprueba la conexion a internet.' }
        $env:PATH += ";$env:USERPROFILE\.dotnet\tools"
    }

    Paso "3/9 Apuntando esta terminal a la base '$Base'"
    $cadena = "Server=$servidor;Database=$Base;Trusted_Connection=True;MultipleActiveResultSets=true"
    $env:ConnectionStrings__UsersConnection = $cadena
    $env:ConnectionStrings__UniversityConnection = $cadena
    $env:ASPNETCORE_ENVIRONMENT = 'Development'
    # Menos ruido en la consola (no afecta a los datos): solo avisos de EF Core
    ${env:Logging__LogLevel__Microsoft.EntityFrameworkCore.Database.Command} = 'Warning'
    ${env:Logging__LogLevel__Microsoft.EntityFrameworkCore.Migrations} = 'Warning'
    Write-Host "Base en uso: $Base  (variables de entorno de ESTA terminal)"

    $dirMig = Join-Path $raiz 'Migrations'
    function Apartar-Migraciones([string]$motivo) {
        $copia = Join-Path $env:TEMP ("Migrations_respaldo_" + (Get-Date -Format 'yyyyMMdd_HHmmss'))
        Move-Item $dirMig $copia
        Aviso "$motivo Migrations/ se ha guardado en $copia"
    }

    Paso '4/9 Comprobando la base de datos'
    if ($Reiniciar -and $Base -notlike 'UnivDemo*' -and $Base -notlike 'UnivEnsayo*') {
        throw "-Reiniciar solo borra bases que empiecen por UnivDemo o UnivEnsayo (la tuya es '$Base')."
    }
    $tablas = $null; $historial = $null
    $existe = Invoke-Sql 'master' "SELECT COUNT(*) FROM sys.databases WHERE name = '$Base'"
    if ($null -ne $existe -and [int]$existe -gt 0) {
        $tablas = Invoke-Sql $Base 'SELECT COUNT(*) FROM sys.tables'
        $historial = Invoke-Sql $Base "SELECT COUNT(*) FROM sys.tables WHERE name = '__EFMigrationsHistory'"
        if (-not $Reiniciar -and $tablas -and [int]$tablas -gt 0 -and [int]$historial -eq 0) {
            throw "La base '$Base' ya existe con tablas pero sin historial de migraciones. Usa otro nombre: -Base ${Base}2  (o -Reiniciar)."
        }
        $accion = if ($Reiniciar) { 'Se borrara (-Reiniciar).' } else { 'Se reutiliza.' }
        Write-Host "La base '$Base' ya existe ($tablas tablas). $accion"
    } else { Write-Host "La base '$Base' no existe todavia: se creara." }

    Paso '5/9 Revisando la carpeta Migrations'
    # Las migraciones antiguas (de otras ramas) o la Demo con -Reiniciar se apartan ANTES de compilar,
    # porque Visual Studio las compila y darian una migracion calculada sobre un modelo viejo.
    if (Test-Path $dirMig) {
        $otras = Get-ChildItem $dirMig -Filter '*.cs' | Where-Object { $_.Name -notlike '*Designer*' -and $_.Name -notlike '*ModelSnapshot*' -and $_.Name -notlike '*_Demo.cs' }
        if ($Reiniciar) { Apartar-Migraciones 'Reinicio pedido (-Reiniciar).' }
        elseif ($otras) {
            if ($tablas -and $historial -and [int]$historial -gt 0) {
                throw "La base '$Base' se creo con otra migracion y hay migraciones antiguas ($($otras.Name -join ', ')). Usa otra base: -Base ${Base}2  (o -Reiniciar para empezar de cero)."
            }
            Apartar-Migraciones "Habia migraciones antiguas ($($otras.Name -join ', '))."
        }
        else { Write-Host 'Migrations/ solo contiene la migracion Demo (o esta vacia): se deja.' }
    } else { Write-Host 'No hay carpeta Migrations/: se creara.' }

    Paso '6/9 Restaurando paquetes y compilando (la primera vez tarda; necesita internet)'
    dotnet build --configuration Release --nologo -v q
    if ($LASTEXITCODE -ne 0) { throw 'La compilacion ha fallado. Revisa los errores de arriba.' }

    Paso '7/9 Migracion'
    if ($Reiniciar -and $null -ne $existe -and [int]$existe -gt 0) {
        dotnet ef database drop -c UniversityDbContext --force --configuration Release --no-build
        if ($LASTEXITCODE -ne 0) { throw "No se pudo borrar la base '$Base'." }
    }
    $migracion = Buscar-Migracion
    if (-not $migracion) {
        dotnet ef migrations add Demo -c UniversityDbContext --configuration Release --no-build
        if ($LASTEXITCODE -ne 0) { throw 'Fallo al crear la migracion.' }
        $migracion = Buscar-Migracion
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
            Ok 'Arreglado el borrado en cascada de CourseStudyPlanYear.'
        } else { Write-Host 'La clave de CourseStudyPlanYear ya estaba arreglada.' }
    }

    Paso '8/9 Creando las tablas'
    # (sin --no-build: hay que recompilar para incluir la migracion Demo recien creada y arreglada)
    dotnet ef database update -c UniversityDbContext --configuration Release
    if ($LASTEXITCODE -ne 0) { throw 'Fallo al crear las tablas de la universidad. Prueba con otro nombre: -Base ' + $Base + '2' }
    dotnet ef database update -c ApplicationUsersDbContext --configuration Release
    if ($LASTEXITCODE -ne 0) { throw 'Fallo al crear las tablas de usuarios.' }

    $esperadas = 'Funcionario', 'ServicoFuncionario', 'ServiceRequests', 'ServiceRequestTypes', 'ServiceRequestStatuses',
                 'ServiceRequestPriorities', 'ServiceRequestCatalogItems', 'ServiceRequestAttachments',
                 'ServiceRequestComments', 'ServiceRequestAssignments', 'ServiceRequestHistories'
    $faltan = @()
    $verificable = $false
    foreach ($t in $esperadas) {
        $r = Invoke-Sql $Base "SELECT COUNT(*) FROM sys.tables WHERE name = '$t'"
        if ($null -ne $r) { $verificable = $true; if ([int]$r -eq 0) { $faltan += $t } }
    }
    if ($faltan.Count -gt 0) { throw "A la base '$Base' le faltan tablas: $($faltan -join ', '). Usa otro nombre de base: -Base ${Base}2" }
    if ($verificable) { Ok "Las $($esperadas.Count) tablas esperadas existen en '$Base'." } else { Aviso 'No pude comprobar las tablas (sin acceso SQL desde el script).' }

    if ($NoArrancar) {
        Write-Host "`nListo. Base '$Base' preparada. Para arrancar, en ESTA terminal:" -ForegroundColor Green
        Write-Host "  `$env:ASPNETCORE_URLS='http://localhost:$Puerto'; dotnet run --configuration Release --no-build --no-launch-profile"
        return
    }

    $puertoLibre = Puerto-Libre $Puerto
    if ($puertoLibre -ne $Puerto) { Aviso "El puerto $Puerto esta ocupado: se usa el $puertoLibre." }
    $url = "http://localhost:$puertoLibre"
    $env:ASPNETCORE_URLS = $url

    Paso "9/9 Arrancando en $url/AdministrativeServices (Ctrl+C para parar)"
    $argumentos = 'run', '--configuration', 'Release', '--no-build', '--no-launch-profile'

    if ($ProbarYSalir) { $Verificar = [switch]$true }
    if (-not $Verificar -and -not $AbrirNavegador) {
        dotnet @argumentos
        return
    }

    # Con -Verificar o -AbrirNavegador la app se lanza como proceso hijo para poder comprobarla
    $app = Start-Process dotnet -ArgumentList $argumentos -NoNewWindow -PassThru
    try {
        $listo = $false
        for ($n = 0; $n -lt 90 -and -not $app.HasExited; $n++) {
            try { if ((Invoke-WebRequest "$url/AdministrativeServices" -UseBasicParsing -TimeoutSec 5).StatusCode -eq 200) { $listo = $true; break } } catch { Start-Sleep 1 }
        }
        if (-not $listo) { throw 'La app no ha arrancado en 90 segundos. Mira los mensajes de arriba (normalmente es la base de datos equivocada).' }

        if ($Verificar) {
            Write-Host "`n-- Comprobando las pantallas y los datos --" -ForegroundColor Cyan
            $controladores = @(
                @{ C = 'ServiceRequests'; T = 'ServiceRequests' }, @{ C = 'ServiceRequestTypes'; T = 'ServiceRequestTypes' },
                @{ C = 'ServiceRequestStatuses'; T = 'ServiceRequestStatuses' }, @{ C = 'ServiceRequestPriorities'; T = 'ServiceRequestPriorities' },
                @{ C = 'ServiceRequestCatalogItems'; T = 'ServiceRequestCatalogItems' }, @{ C = 'ServiceRequestAttachments'; T = 'ServiceRequestAttachments' },
                @{ C = 'ServiceRequestComments'; T = 'ServiceRequestComments' }, @{ C = 'ServiceRequestAssignments'; T = 'ServiceRequestAssignments' },
                @{ C = 'ServiceRequestHistories'; T = 'ServiceRequestHistories' })
            $problemas = 0
            foreach ($x in $controladores) {
                $estado = 'ERROR'
                foreach ($accion in '', '/Create') {
                    try { $codigo = (Invoke-WebRequest "$url/$($x.C)$accion" -UseBasicParsing -TimeoutSec 30).StatusCode } catch { $codigo = 0 }
                    if ($codigo -ne 200) { $estado = "ERROR en $($x.C)$accion"; break } else { $estado = 'pantallas OK' }
                }
                $filas = Invoke-Sql $Base "SELECT COUNT(*) FROM [$($x.T)]"
                if ($estado -ne 'pantallas OK' -or $null -eq $filas -or [int]$filas -eq 0) { $problemas++ }
                Write-Host ("{0,-28} {1,-14} filas: {2}" -f $x.C, $estado, $filas)
            }
            if ($problemas -eq 0) { Ok "Todo correcto: 9 CRUDs con pantallas y datos de ejemplo." } else { Aviso "$problemas CRUD(s) con problemas. Mira la tabla de arriba." }
        }
        if ($AbrirNavegador) { Start-Process "$url/AdministrativeServices" }
        if ($ProbarYSalir) { Write-Host "`nComprobado. Se cierra la app (-ProbarYSalir)." -ForegroundColor Green; return }
        Write-Host "`nLa app sigue en marcha en $url/AdministrativeServices. Ctrl+C para parar." -ForegroundColor Green
        $app.WaitForExit()
    } finally {
        if ($app -and -not $app.HasExited) { taskkill /PID $app.Id /T /F | Out-Null }
    }
}
catch {
    Write-Host "`nERROR: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "Registro completo en: $registro" -ForegroundColor Red
    throw
}
finally {
    try { Stop-Transcript | Out-Null } catch { }
}
