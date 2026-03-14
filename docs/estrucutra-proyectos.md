# Estructura del Proyecto

Este documento describe la organización del repositorio **Sistema de Gestión para Taller de Frenos**.

Cada aplicación tiene una responsabilidad específica dentro de la arquitectura.

---

# Aplicaciones del Sistema

| Aplicación            | Responsable | Descripción                       |
| --------------------- | ----------- | --------------------------------- |
| **TallerCore**        | Dev 1       | Sistema principal para empleados  |
| **TallerIntegracion** | Dev 2       | API de integración entre sistemas |
| **TallerWeb**         | Dev 3       | Portal web para clientes          |
| **TallerCaja**        | Dev 4       | Sistema POS para facturación      |

---

# TallerCore

Aplicación **ASP.NET Core Web App (Razor Pages + API)** utilizada por los empleados del taller.

```
TallerCore/
│
├── Controllers/
│   └── Api/                    # Endpoints REST para Integración
│       ├── AuthController.cs
│       ├── UsuariosController.cs
│       ├── ClientesController.cs
│       ├── ProductosController.cs
│       ├── ServiciosController.cs
│       ├── CotizacionesController.cs
│       ├── OrdenesController.cs
│       ├── FacturasController.cs
│       └── CuentasPorCobrarController.cs
│
├── Pages/                      # UI Razor Pages para empleados
│   ├── Auth/
│   │   └── Login.cshtml
│   │
│   ├── Dashboard/
│   │   └── Index.cshtml        # Resumen del día
│   │
│   ├── Clientes/
│   │   ├── Index.cshtml
│   │   ├── Crear.cshtml
│   │   └── Editar.cshtml
│   │
│   ├── Productos/
│   │   ├── Index.cshtml
│   │   ├── Crear.cshtml
│   │   └── Editar.cshtml
│   │
│   ├── Servicios/
│   │   ├── Index.cshtml
│   │   └── Crear.cshtml
│   │
│   ├── Ordenes/
│   │   ├── Index.cshtml
│   │   └── Detalle.cshtml
│   │
│   ├── Facturas/
│   │   ├── Index.cshtml
│   │   └── Detalle.cshtml
│   │
│   ├── Usuarios/
│   │   ├── Index.cshtml
│   │   └── Crear.cshtml
│   │
│   └── Shared/
│       ├── _Layout.cshtml
│       └── _NavBar.cshtml
│
├── Models/
│   ├── Entities/               # Tablas de la base de datos
│   │   ├── Usuario.cs
│   │   ├── Cliente.cs
│   │   ├── Producto.cs
│   │   ├── Servicio.cs
│   │   ├── Orden.cs
│   │   ├── OrdenItem.cs
│   │   ├── Factura.cs
│   │   ├── FacturaItem.cs
│   │   ├── Cotizacion.cs
│   │   ├── CuentaPorCobrar.cs
│   │   ├── Sucursal.cs
│   │   └── AuditLog.cs
│   │
│   └── DTOs/                   # Objetos usados por la API
│       ├── ClienteDto.cs
│       ├── ProductoDto.cs
│       ├── OrdenDto.cs
│       └── FacturaDto.cs
│
├── Services/                   # Lógica de negocio
│   ├── ClienteService.cs
│   ├── OrdenService.cs
│   ├── FacturaService.cs
│   └── InventarioService.cs
│
├── Data/
│   ├── AppDbContext.cs
│   └── Migrations/
│
└── Program.cs
```

---

# TallerIntegracion

API encargada de comunicar los diferentes sistemas.

```
TallerIntegracion/
│
├── Controllers/
│   ├── AuthIntController.cs            ← login clientes, cajeros, health
│   ├── CatalogoController.cs           ← productos y servicios (con caché)
│   ├── OrdenesWebController.cs         ← órdenes desde la app web
│   └── CajaController.cs               ← turno, cobro, sync offline
│
├── Services/
│   ├── Core/
│   │   ├── ICoreAuthService.cs        ← clientes HTTP hacia el Core
│   │   ├── CoreAuthService.cs         
│   │   ├── ICoreProductoService.cs
│   │   ├── CoreProductoService.cs
│   │   ├── ICoreOrdenService.cs
│   │   └── CoreOrdenService.cs
│
│   ├── Cache/
│   │   ├── ICacheService.cs
│   │   └── CacheService.cs            ← IMemoryCache + fallback a BD
│
│   └── Sync/
│       ├── IColaSyncService.cs
│       ├── ColaSyncService.cs          ← procesa ColaPendiente
│       └── SyncHostedService.cs        ← IHostedService, corre cada 30s
│
├── Models/
│   ├── Entities/                       ← tablas de la BD de Integración
│   │   ├── ProductoCache.cs
│   │   ├── ServicioCache.cs
│   │   ├── ColaPendiente.cs
│   │   └── LogPeticion.cs
│
│   └── DTOs/                            ← contratos que expone a Web y Caja
│       ├── CatalogoDto.cs
│       ├── OrdenWebDto.cs
│       ├── CobroDto.cs
│       ├── TurnoDto.cs
│       └── SyncDto.cs
│
├── Data/
│   ├── IntegracionDbContext.cs
│   ├── Migrations/
│   └── Seed.cs
│
├── Middleware/
│   ├── RequestLogMiddleware.cs           ← guarda cada req/res en LogPeticion
│   └── ExceptionMiddleware.cs
│
├── Helpers/
│   ├── ApiResponse.cs                    ← mismo wrapper que el Core
│   └── CircuitBreaker.cs                 ← detecta caída del Core
│
├── Program.cs
└── appsettings.json
```

---

# TallerWeb

Portal web para clientes.

Permite a los clientes:

* Ver servicios del taller
* Solicitar cotizaciones
* Ver estado de su vehículo
* Ver historial de servicios

```
TallerWeb/
│
├── Pages/                                  ← componentes de página (.razor)
│   ├── Login.razor
│   ├── Registro.razor
│   ├── Catalogo.razor                      ← lista de productos y servicios
│   ├── DetalleProducto.razor
│   ├── Carrito.razor
│   ├── Orden.razor                          ← confirmar y crear orden
│   ├── Pago.razor                           ← simulación de pago
│   ├── MisOrdenes.razor                     ← historial del cliente
│   └── EstadoOrden.razor                    ← consulta de estado por ID
│
├── Components/                              ← componentes reutilizables
│   ├── NavBar.razor
│   ├── ProductoCard.razor
│   ├── CarritoItem.razor
│   ├── EstadoBadge.razor                    ← chip de estado (Pendiente, Lista…)
│   ├── AlertaMensaje.razor
│   └── CargandoSpinner.razor
│
├── Services/
│   ├── IAuthWebService.cs
│   ├── AuthWebService.cs                    ← login, registro, guardar token
│   ├── ICatalogoService.cs
│   ├── CatalogoService.cs                   ← llama /int/catalogo/*
│   ├── IOrdenWebService.cs
│   ├── OrdenWebService.cs                    ← crea orden, consulta estado
│   ├── IPagoService.cs
│   ├── PagoService.cs                        ← simula pago vía Integración
│   └── CarritoStateService.cs                ← estado del carrito en memoria
│
├── Models/                                  ← DTOs que llegan de Integración
│   ├── ProductoModel.cs
│   ├── OrdenWebModel.cs
│   ├── CarritoItem.cs
│   └── ApiResponse.cs                        ← deserializa { success, data, error }
│
├── Data/
│   ├── WebDbContext.cs                         ← EF Core para BD Web (en servidor)
│   └── Migrations/
│
├── Shared/
│   ├── MainLayout.razor
│   └── AuthorizeRouteView.razor
│
├── wwwroot/
│   └── css/app.css
│
├── App.razor
├── Program.cs
└── appsettings.json
```

---

# TallerCaja

Aplicación **Windows Forms POS** para facturación rápida en el taller.

```
TallerCaja/
│
├── Forms/                            ← pantallas de la aplicación
│   ├── frmLogin.cs
│   ├── frmPrincipal.cs                ← menú principal con estado online/offline
│   ├── frmInicioDia.cs                ← monto inicial del turno
│   ├── frmCobro.cs                    ← búsqueda de items + carrito
│   ├── frmPago.cs                      ← monto, vuelto, método de pago
│   ├── frmRecibo.cs                   ← vista previa del recibo
│   ├── frmEfectivo.cs                 ← entrada / salida de efectivo
│   ├── frmCierreDia.cs                ← cuadre y resumen del turno
│   └── frmTransacciones.cs            ← historial del turno activo
│
├── Services/
│   ├── IIntegracionService.cs
│   ├── IntegracionService.cs           ← HttpClient hacia Integración
│   ├── ICajaLocalService.cs
│   ├── CajaLocalService.cs            ← lee/escribe en SQLite
│   ├── ISyncService.cs
│   ├── SyncService.cs                  ← envía ColaPendiente a Integración
│   ├── IReciboService.cs
│   └── ReciboService.cs                ← genera PDF o imprime con PrintDocument
│
├── Models/
│   ├── Entities/                       ← tablas SQLite (EF Core)
│   │   ├── TurnoCaja.cs        
│   │   ├── MovimientoEfectivo.cs
│   │   ├── TransaccionCaja.cs
│   │   ├── DetalleTransaccion.cs
│   │   ├── ProductoCacheLocal.cs
│   │   └── CierreDia.cs
│
│   └── DTOs/                            ← lo que llega/sale de Integración
│       ├── CobroDto.cs
│       ├── TurnoDto.cs
│       └── ApiResponse.cs
│
├── Data/
│   ├── CajaDbContext.cs                ← EF Core con SQLite
│   └── Migrations/
│
├── Helpers/
│   ├── ConexionMonitor.cs               ← timer que verifica /int/auth/health
│   ├── SessionManager.cs                ← guarda cajero y turno activo en memoria
│   └── OfflineQueue.cs                  ← encola transacciones cuando offline
│
├── Program.cs
└── appsettings.json
```

---

# Estructura del Repositorio

```
Frenos/
│
├── docs/                # Documentación del proyecto
│
├── src/
│   ├── TallerCore/
│   ├── TallerIntegracion/
│   ├── TallerWeb/
│   └── TallerCaja/
│
├── README.md
└── .gitignore
```
