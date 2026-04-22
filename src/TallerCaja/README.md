# TallerCaja — Módulo de Caja | Taller de Frenos
Desarrollo de Software III - 2026

---

## Cómo abrir en Visual Studio

1. Descomprime el ZIP en cualquier carpeta
2. Abre **Visual Studio 2022** (versión 17.8 o superior)
3. `Archivo → Abrir → Proyecto/Solución` → selecciona `TallerCaja.csproj`
4. Espera que Visual Studio cargue el proyecto
5. Click derecho sobre el proyecto → **Restaurar paquetes NuGet**
6. Presiona `F5` para ejecutar

---

## Dependencias NuGet (se restauran automáticamente)

| Paquete | Versión | Para qué |
|---|---|---|
| Microsoft.EntityFrameworkCore | 8.0.0 | ORM base |
| Microsoft.EntityFrameworkCore.Sqlite | 8.0.0 | Base de datos local |
| Microsoft.EntityFrameworkCore.Tools | 8.0.0 | Migrations |
| Microsoft.Extensions.Configuration | 8.0.0 | Lectura de appsettings |
| Microsoft.Extensions.Configuration.Json | 8.0.0 | Lectura de appsettings.json |
| Newtonsoft.Json | 13.0.3 | Serialización JSON |
| System.IdentityModel.Tokens.Jwt | 7.5.1 | Validación de tokens JWT |

---

## Configuración en appsettings.json

```json
{
  "Integracion": {
    "BaseUrl": "http://localhost:5001",   ← Cambia esto a la URL real de Integración
    "TimeoutSeconds": 10
  },
  "App": {
    "NombreTaller": "Taller de Frenos",
    "ITBIS": 0.18                        ← ITBIS República Dominicana = 18%
  },
  "Database": {
    "LocalDbPath": "tallercaja_local.db" ← SQLite local, se crea automáticamente
  }
}
```

---

## Cómo ejecutar

1. Presiona `F5` en Visual Studio
2. Aparece `frmLogin` — credenciales de prueba:
   - **Email:** cualquier correo (ej. `cajero@taller.com`)
   - **Contraseña:** cualquier texto (ej. `demo123`)
3. Aparece `frmInicioDia` — registra el monto inicial de caja
4. Se abre `frmCobro` — pantalla principal de operación

> El proyecto usa **MockService** por defecto, no necesita conexión a Integración para funcionar.

---

## Dónde conectar las APIs reales de Integración

### Paso 1 — Cambiar URL base
En `appsettings.json`:
```json
"BaseUrl": "http://tu-servidor-integracion:puerto"
```

### Paso 2 — Cambiar el servicio en Program.cs
```csharp
// Línea ~35 en Program.cs — REEMPLAZAR:
IIntegracionService integracionService = new IntegracionMockService();

// POR:
IIntegracionService integracionService = new IntegracionService(httpClient);
```

### Paso 3 — Ajustar DTOs si hace falta
Si los campos del JSON de Integración tienen nombres diferentes, edita:
`Models/DTOs/DTOs.cs`

Todos los endpoints están documentados con comentarios `// PUNTO DE CONEXIÓN` en:
- `Services/IntegracionService.cs`
- `Program.cs`

---

## Estructura del proyecto

```
TallerCaja/
├── Forms/
│   ├── frmLogin.cs/.Designer.cs       ← Login de cajero
│   ├── frmInicioDia.cs/.Designer.cs   ← Apertura de turno
│   ├── frmCobro.cs/.Designer.cs       ← PANTALLA PRINCIPAL (cobro)
│   ├── frmPago.cs/.Designer.cs        ← Métodos de pago + confirmación
│   ├── frmRecibo.cs                   ← Recibo/factura de venta
│   ├── frmCierreDia.cs                ← Cierre de turno
│   └── frmAuxiliares.cs               ← Cambiar cantidad, selector cliente, buscar factura
├── Services/
│   ├── IntegracionService.cs          ← IIntegracionService + impl real + MOCK
│   └── CajaLocalService.cs            ← ICajaLocalService, ISyncService, IReciboService
├── Models/
│   ├── DTOs/DTOs.cs                   ← Todos los DTOs del catálogo de servicios
│   └── Entities/EntidadesLocales.cs   ← Entidades SQLite locales
├── Data/
│   └── CajaDbContext.cs               ← EF Core DbContext (SQLite)
├── Helpers/
│   └── Helpers.cs                     ← AppConfig, SessionManager, FacturaCalculator,
│                                         ConexionMonitor, OfflineQueue, MonedaHelper
├── Program.cs                         ← Punto de entrada + composición de dependencias
└── appsettings.json                   ← Configuración
```

---

## Métodos de pago implementados

| Método | Cambio | Monto editable | Comportamiento especial |
|---|---|---|---|
| **Efectivo** | ✅ Se calcula | ✅ Sí | Valida monto ≥ total |
| **Tarjeta** | ❌ No aplica | ❌ Fijo | Terminal POS externo |
| **Transferencia** | ❌ No aplica | ❌ Fijo | Verificar comprobante |
| **Crédito** | ❌ No aplica | ❌ Fijo | Crea CxC pendiente |

---

## Modo offline

- Si Integración no responde, la app continúa trabajando
- Las ventas se guardan en SQLite local con estado `PendienteSync`
- Al reiniciar con conexión, el `SyncService` reenvía automáticamente
- La cola se puede revisar en `TransaccionesPendientes` en la BD local

---

## ITBIS

Definido como constante en `Helpers/Helpers.cs`:
```csharp
public const decimal TASA_ITBIS = 0.18m; // 18% República Dominicana
```
Centralizado en `FacturaCalculator` — un solo lugar para cambiar si la tasa cambia.
