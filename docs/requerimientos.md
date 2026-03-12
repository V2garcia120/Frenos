# Requerimientos del Sistema

Sistema de Gestión para Taller de Frenos

---

# 1. Introducción

Este documento describe los **requerimientos funcionales y no funcionales** del sistema diseñado para gestionar las operaciones de un **taller especializado en frenos automotrices**.

El sistema permitirá administrar clientes, vehículos, diagnósticos, órdenes de servicio, inventario y pagos.

---

# 2. Objetivo del Sistema

Desarrollar un sistema que permita mejorar la gestión del taller mediante el registro y control de:

* Clientes
* Vehículos
* Diagnósticos
* Servicios
* Inventario
* Facturación
* Pagos

---

# 3. Actores del Sistema

Los usuarios que interactúan con el sistema son:

### Administrador

Responsable de la gestión completa del sistema.

Funciones:

* Administrar usuarios
* Gestionar inventario
* Consultar reportes

---

### Recepcionista

Responsable de registrar clientes y órdenes de servicio.

Funciones:

* Registrar clientes
* Registrar vehículos
* Crear órdenes de servicio

---

### Técnico

Responsable de realizar diagnósticos y reparaciones.

Funciones:

* Registrar diagnósticos
* Agregar servicios a la orden
* Registrar piezas utilizadas

---

### Cajero

Responsable de procesar pagos.

Funciones:

* Consultar órdenes terminadas
* Registrar pagos
* Imprimir recibos
* Realizar cierre de caja

---

# 4. Requerimientos Funcionales

### Gestión de Clientes

RF01 – El sistema debe permitir registrar nuevos clientes.

RF02 – El sistema debe permitir consultar clientes registrados.

RF03 – El sistema debe permitir actualizar información de clientes.

RF04 – El sistema debe permitir eliminar clientes.

---

### Gestión de Vehículos

RF05 – El sistema debe permitir registrar vehículos asociados a un cliente.

RF06 – El sistema debe permitir consultar los vehículos de un cliente.

RF07 – El sistema debe permitir actualizar información de vehículos.

---

### Diagnóstico

RF08 – El sistema debe permitir registrar diagnósticos de vehículos.

RF09 – El sistema debe permitir agregar observaciones técnicas.

---

### Órdenes de Servicio

RF10 – El sistema debe permitir crear órdenes de servicio.

RF11 – El sistema debe permitir agregar servicios a la orden.

RF12 – El sistema debe permitir agregar piezas o repuestos a la orden.

RF13 – El sistema debe calcular automáticamente el total de la orden.

RF14 – El sistema debe permitir cambiar el estado de la orden.

Estados posibles:

* Pendiente
* En proceso
* Terminado
* Pagado

---

### Inventario

RF15 – El sistema debe permitir registrar productos o repuestos.

RF16 – El sistema debe permitir consultar el inventario.

RF17 – El sistema debe actualizar automáticamente el stock cuando se utilicen piezas.

---

### Facturación

RF18 – El sistema debe generar facturas para órdenes completadas.

RF19 – El sistema debe almacenar el historial de facturas.

---

### Pagos

RF20 – El sistema debe permitir registrar pagos de facturas.

RF21 – El sistema debe permitir seleccionar el método de pago.

Métodos posibles:

* Efectivo
* Tarjeta
* Transferencia

RF22 – El sistema debe permitir imprimir recibos.

---

### Caja

RF23 – El sistema debe permitir apertura de caja.

RF24 – El sistema debe permitir registrar ingresos de efectivo.

RF25 – El sistema debe permitir registrar egresos de efectivo.

RF26 – El sistema debe permitir realizar cierre de caja.

---

# 5. Requerimientos No Funcionales

### Seguridad

RNF01 – El sistema debe requerir autenticación de usuarios.

RNF02 – El sistema debe manejar roles de usuario.

RNF03 – El sistema debe registrar auditoría de operaciones.

---

### Rendimiento

RNF04 – Las consultas principales deben responder en menos de 3 segundos.

---

### Disponibilidad

RNF05 – El sistema debe permitir funcionamiento continuo durante el horario del taller.

---

### Escalabilidad

RNF06 – La arquitectura debe permitir agregar nuevos módulos en el futuro.

---

### Mantenibilidad

RNF07 – El sistema debe seguir una arquitectura modular.

---

# 6. Tecnologías del Sistema

Backend:

* .NET
* ASP.NET Web API

Frontend:

* Blazor (Aplicación Web)

Aplicación de Caja:

* Windows Forms

Base de datos:

* SQL Server

---

# 7. Alcance del Proyecto

El sistema cubrirá las operaciones principales del taller:

* Gestión de clientes
* Gestión de vehículos
* Gestión de servicios
* Control de inventario
* Facturación
* Registro de pagos

No se incluyen funcionalidades avanzadas como:

* Integración con bancos
* Facturación electrónica gubernamental
* Aplicaciones móviles
