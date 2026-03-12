# Sistema de Gestión para Taller de Frenos

## Proyecto Final – Desarrollo de Software III

---

# 1. Descripción del Proyecto

Este proyecto consiste en el desarrollo de un sistema distribuido para gestionar las operaciones de un **taller especializado en frenos automotrices**.

El sistema permitirá administrar:

* Clientes
* Vehículos
* Diagnósticos de frenos
* Órdenes de servicio
* Inventario de repuestos
* Facturación
* Pagos en caja

El sistema se compone de **cuatro aplicaciones independientes** que se comunican mediante **servicios web (REST APIs)**.

Cada aplicación tiene **su propia base de datos**.

---

# 2. Arquitectura del Sistema

La solución se divide en los siguientes módulos:

* **Core** → Sistema central con la lógica del negocio
* **Integración** → API Gateway que conecta los sistemas
* **Web** → Aplicación administrativa
* **Caja** → Sistema POS para cobros

### Arquitectura general

```
                CLIENTES / EMPLEADOS
                       │
                       ▼
                     WEB
                       │
                       │ REST API
                       ▼
                 INTEGRACIÓN
                (API Gateway)
                  │        │
                  │        │
                  ▼        ▼
                 CORE     CAJA
```

Flujo general:

```
Web o Caja
     ↓
Integración
     ↓
Core
```

---

# 3. CORE – Sistema Central

El **Core** contiene toda la lógica del negocio del taller.

Es responsable de manejar los datos oficiales del sistema.

## Funciones principales

* Gestión de clientes
* Gestión de vehículos
* Registro de diagnósticos
* Gestión de órdenes de servicio
* Control de inventario
* Facturación
* Cuentas por cobrar
* Auditoría del sistema

---

# 4. Entidades del Sistema

## Clientes

Representa las personas que llevan sus vehículos al taller.

Campos principales:

* Id
* Nombre
* Teléfono
* Email
* Dirección

---

## Vehículos

Cada cliente puede tener uno o varios vehículos.

Campos principales:

* Id
* ClienteId
* Marca
* Modelo
* Año
* Placa

---

## Servicios

Servicios que ofrece el taller.

Ejemplos:

* Cambio de pastillas de freno
* Rectificación de discos
* Cambio de líquido de frenos
* Reparación de calipers
* Diagnóstico de frenos

Campos principales:

* Id
* Nombre
* Descripción
* PrecioBase

---

## Productos / Repuestos

Piezas utilizadas en las reparaciones.

Ejemplos:

* Pastillas de freno
* Discos de freno
* Líquido de frenos
* Tambores

Campos principales:

* Id
* Nombre
* Precio
* Stock

---

## Diagnóstico

Registro de la revisión inicial del vehículo.

Campos principales:

* Id
* VehiculoId
* Fecha
* Observaciones
* Técnico

---

## Orden de Servicio

Registro del trabajo realizado en el vehículo.

Campos principales:

* Id
* ClienteId
* VehiculoId
* DiagnosticoId
* Fecha
* Estado
* Total

Estados posibles:

* Pendiente
* En proceso
* Terminado
* Pagado

---

## Detalle de Servicios

Servicios realizados dentro de una orden.

Campos principales:

* Id
* OrdenId
* ServicioId
* Precio

---

## Piezas utilizadas

Repuestos usados durante la reparación.

Campos principales:

* Id
* OrdenId
* ProductoId
* Cantidad
* Precio

---

## Facturas

Documento generado cuando se completa una orden.

Campos principales:

* Id
* OrdenId
* Fecha
* Total
* Estado

---

## Pagos

Registro de pagos realizados por el cliente.

Campos principales:

* Id
* FacturaId
* Monto
* Método de pago
* Fecha

---

# 5. INTEGRACIÓN – API Gateway

La capa de **Integración** conecta los canales con el Core.

Responsabilidades:

* Exponer servicios para Web y Caja
* Consumir servicios del Core
* Manejar logs
* Manejar errores
* Cachear información importante

---

## Ejemplo de Endpoints

Clientes

```
GET /api/clientes
POST /api/clientes
```

Vehículos

```
GET /api/vehiculos
POST /api/vehiculos
```

Órdenes

```
GET /api/ordenes
POST /api/ordenes
GET /api/ordenes/{id}
```

Servicios

```
GET /api/servicios
```

Inventario

```
GET /api/productos
```

Pagos

```
POST /api/pagos
```

---

# 6. Aplicación Webb (Portal del Cliente

La aplicación **Web** funciona como un **portal para clientes**, permitiendo que los usuarios interactúen directamente con el taller.

Los clientes pueden acceder desde un navegador.

Responsabilidades:

* Registro de clientes
* Inicio de sesión
* Gestión de vehículos
* Solicitud de servicios
* Consulta del estado de reparación
* Consulta de historial de servicios
* Consulta de facturas

Flujo de interacción:

```
Cliente
   ↓
Portal Web
   ↓
Integración
   ↓
Core
```

### Funcionalidades del Portal

#### Registro de Usuario

Permite a un cliente crear una cuenta en el sistema.

Datos registrados:

* Nombre
* Correo electrónico
* Teléfono
* Contraseña

---

#### Registro de Vehículos

El cliente puede registrar sus vehículos.

Información almacenada:

* Marca
* Modelo
* Año
* Placa

---

#### Solicitud de Servicio

El cliente puede solicitar servicios relacionados con frenos.

Ejemplos de servicios:

* Cambio de pastillas
* Rectificación de discos
* Revisión general de frenos
* Cambio de líquido de frenos

---

#### Seguimiento del Servicio

El cliente puede ver el estado actual de su vehículo.

Estados posibles:

* Pendiente
* En diagnóstico
* En reparación
* Listo
* Entregado

---

#### Historial de Servicios

El cliente puede consultar todos los servicios realizados a sus vehículos.

Ejemplo de información mostrada:

* Fecha del servicio
* Tipo de servicio
* Vehículo
* Costo del servicio

---

#### Facturas

El cliente puede consultar sus facturas y pagos realizados.

Información disponible:

* Número de factura
* Servicio realizado
* Total
* Estado de pago

---


# 7. Aplicación de Caja (POS)

La aplicación de caja será desarrollada utilizando **Windows Forms**.

Esta aplicación será utilizada por los cajeros del taller.

## Funciones principales

* Login de cajero
* Cobrar órdenes
* Registrar pagos
* Imprimir recibos
* Registrar entrada de efectivo
* Registrar salida de efectivo
* Cuadre de caja
* Cierre del día

---

## Flujo de pago

```
Caja consulta orden terminada
        ↓
Cliente realiza pago
        ↓
Caja registra el pago
        ↓
Caja imprime recibo
        ↓
Integración envía el pago al Core
```

---

## Modo Offline

La aplicación de caja debe poder funcionar sin conexión.

Proceso:

1. Guardar transacciones localmente.
2. Cuando vuelva la conexión, sincronizar con Integración.
3. Integración envía las transacciones al Core.

---

# 8. Base de Datos por Sistema

## Core

Tablas principales:

* Clientes
* Vehiculos
* Servicios
* Productos
* Diagnosticos
* OrdenesServicio
* OrdenServicioDetalle
* OrdenPiezas
* Facturas
* Pagos
* Usuarios
* LogsAuditoria

---

## Integración

* ProductosCache
* ServiciosCache
* TransaccionesPendientes
* LogsIntegracion

---

## Web

* UsuariosWeb
* Sesiones
* LogsWeb

---

## Caja

* Cajas
* Transacciones
* TransaccionesOffline
* Recibos
* CuadresCaja

---

# 9. Seguridad

El sistema debe implementar autenticación y autorización.

Requisitos:

* Login con usuario y contraseña
* Contraseñas seguras
* Registro de actividad

---

# 10. Tecnologías Sugeridas

Backend:

* .NET 8
* ASP.NET Web API

Frontend Web:

* Blazor

Aplicación de Caja:

* Windows Forms

Base de datos:

* SQL Server

Logs:

* Serilog

---

# 11. Estructura del Repositorio

```
sistema-taller-frenos
│
├── core
│
├── integracion
│
├── web
│
├── caja
│
└── docs
```

---

# 12. Distribución del Trabajo

| Integrante      | Módulo      |
| --------------- | ----------- |
| Vismil Garcia   | Core        |
| Diana Patricia  | Integración |
| Pamela Aimee    | Web         |
| Diego Alejandro | Caja        |

---

# 13. Objetivo del Proyecto

Desarrollar un sistema distribuido capaz de gestionar de forma eficiente las operaciones de un taller de frenos, permitiendo administrar clientes, vehículos, servicios, inventario, facturación y pagos desde múltiples canales.

El sistema debe garantizar:

* Seguridad
* Auditoría
* Escalabilidad
* Integración entre sistemas
* Manejo de fallos
* Registro de operaciones
