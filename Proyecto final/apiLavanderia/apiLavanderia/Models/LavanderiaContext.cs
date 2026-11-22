using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apiLavanderia.Models;

public partial class LavanderiaContext : DbContext
{
    public LavanderiaContext(DbContextOptions<LavanderiaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblProfesion> TblProfesiones { get; set; }

    public virtual DbSet<TblTipoDocumento> TblTipoDocumentos { get; set; }

    public virtual DbSet<TblDepartamento> TblDepartamentos { get; set; }

    public virtual DbSet<TblCiudad> TblCiudades { get; set; }

    public virtual DbSet<TblTipoTelefono> TblTipoTelefonos { get; set; }

    public virtual DbSet<TblEstadoPrenda> TblEstadoPrendas { get; set; }

    public virtual DbSet<TblTipoServicio> TblTipoServicios { get; set; }

    public virtual DbSet<TblTipoPrenda> TblTipoPrendas { get; set; }

    public virtual DbSet<TblTipoPrendaServicio> TblTipoPrendaServicios { get; set; }

    public virtual DbSet<TblDescuento> TblDescuentos { get; set; }

    public virtual DbSet<TblEmpleado> TblEmpleados { get; set; }

    public virtual DbSet<TblEmpleadoHasProfesion> TblEmpleadoHasProfesiones { get; set; }

    public virtual DbSet<TblUsuario> TblUsuarios { get; set; }

    public virtual DbSet<TblDireccionEmpleado> TblDireccionEmpleados { get; set; }

    public virtual DbSet<TblTelefonoEmpleado> TblTelefonoEmpleados { get; set; }

    public virtual DbSet<TblCliente> TblClientes { get; set; }

    public virtual DbSet<TblDireccionCliente> TblDireccionClientes { get; set; }

    public virtual DbSet<TblTelefonoCliente> TblTelefonoClientes { get; set; }

    public virtual DbSet<TblIngreso> TblIngresos { get; set; }

    public virtual DbSet<TblDetalleIngreso> TblDetalleIngresos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblProfesion>(entity =>
        {
            entity.HasKey(e => e.IdProfesion);

            entity.ToTable("profesion");

            entity.Property(e => e.IdProfesion)
                .ValueGeneratedNever()
                .HasColumnName("idprofesion");
            entity.Property(e => e.NombreProfesion)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_profesion");
        });

        modelBuilder.Entity<TblTipoDocumento>(entity =>
        {
            entity.HasKey(e => e.IdTipoDocumento);

            entity.ToTable("tipo_documento");

            entity.Property(e => e.IdTipoDocumento)
                .ValueGeneratedNever()
                .HasColumnName("idtipo_documento");
            entity.Property(e => e.NombreDocumento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_documento");
        });

        modelBuilder.Entity<TblDepartamento>(entity =>
        {
            entity.HasKey(e => e.IdDepartamento);

            entity.ToTable("departamento");

            entity.Property(e => e.IdDepartamento)
                .ValueGeneratedNever()
                .HasColumnName("iddepartamento");
            entity.Property(e => e.NombreDepartamento)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_departamento");
        });

        modelBuilder.Entity<TblCiudad>(entity =>
        {
            entity.HasKey(e => e.IdCiudad);

            entity.ToTable("ciudad");

            entity.Property(e => e.IdCiudad)
                .ValueGeneratedNever()
                .HasColumnName("idciudad");
            entity.Property(e => e.IdDepartamento).HasColumnName("iddepartamento");
            entity.Property(e => e.NombreCiudad)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_ciudad");
        });

        modelBuilder.Entity<TblTipoTelefono>(entity =>
        {
            entity.HasKey(e => e.IdTipoTelefono);

            entity.ToTable("tipo_telefono");

            entity.Property(e => e.IdTipoTelefono)
                .ValueGeneratedNever()
                .HasColumnName("idtipo_telefono");
            entity.Property(e => e.TipoTelefono)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipo_telefono");
        });

        modelBuilder.Entity<TblEstadoPrenda>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPrenda);

            entity.ToTable("estado_prenda");

            entity.Property(e => e.IdEstadoPrenda)
                .ValueGeneratedNever()
                .HasColumnName("idestado_prenda");
            entity.Property(e => e.EstadoPrenda)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("estado_prenda");
        });

        modelBuilder.Entity<TblTipoServicio>(entity =>
        {
            entity.HasKey(e => e.IdTipoServicio);

            entity.ToTable("tipo_servicio");

            entity.Property(e => e.IdTipoServicio)
                .ValueGeneratedNever()
                .HasColumnName("idtipo_servicio");
            entity.Property(e => e.NombreServicio)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_servicio");
            entity.Property(e => e.Valor).HasColumnName("valor");
            entity.Property(e => e.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<TblTipoPrenda>(entity =>
        {
            entity.HasKey(e => e.IdTipoPrenda);

            entity.ToTable("tipo_prenda");

            entity.Property(e => e.IdTipoPrenda)
                .ValueGeneratedNever()
                .HasColumnName("idtipo_prenda");
            entity.Property(e => e.NombrePrenda)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nombre_prenda");
        });

        modelBuilder.Entity<TblTipoPrendaServicio>(entity =>
        {
            entity.HasKey(e => e.IdTipoPrendaServicio);

            entity.ToTable("tipo_prenda_servicio");

            entity.Property(e => e.IdTipoPrendaServicio)
                .ValueGeneratedNever()
                .HasColumnName("idtipo_prenda_servicio");
            entity.Property(e => e.IdTipoServicio).HasColumnName("idtipo_servicio");
            entity.Property(e => e.IdTipoPrenda).HasColumnName("idtipo_prenda");
            entity.Property(e => e.ValorUnitario)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor_unitario");
            entity.Property(e => e.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<TblDescuento>(entity =>
        {
            entity.HasKey(e => e.IdDescuento);

            entity.ToTable("descuento");

            entity.Property(e => e.IdDescuento)
                .ValueGeneratedNever()
                .HasColumnName("iddescuento");
            entity.Property(e => e.IdTipoPrenda).HasColumnName("idtipo_prenda");
            entity.Property(e => e.CantidadMin).HasColumnName("cantidad_min");
            entity.Property(e => e.CantidadMax).HasColumnName("cantidad_max");
            entity.Property(e => e.Porcentaje)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("porcentaje");
            entity.Property(e => e.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<TblEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdEmpleado);

            entity.ToTable("empleado");

            entity.Property(e => e.IdEmpleado)
                .ValueGeneratedNever()
                .HasColumnName("idempleado");
            entity.Property(e => e.EmpleadoIdEmpleado).HasColumnName("empleado_idempleado");
            entity.Property(e => e.IdTipoDocumento).HasColumnName("idtipo_documento");
            entity.Property(e => e.NombreApellidoEmp)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre_apellido_emp");
            entity.Property(e => e.DocumentoEmpleado)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("documento_empleado");
            entity.Property(e => e.Activo).HasColumnName("activo");
            entity.Property(e => e.FechaIngreso)
                .HasColumnType("date")
                .HasColumnName("fecha_ingreso");
            entity.Property(e => e.Salario)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("salario");
        });

        modelBuilder.Entity<TblEmpleadoHasProfesion>(entity =>
        {
            entity.HasKey(e => new { e.EmpleadoIdEmpleado, e.ProfesionIdProfesion });

            entity.ToTable("empleado_has_profesion");

            entity.Property(e => e.EmpleadoIdEmpleado).HasColumnName("empleado_idempleado");
            entity.Property(e => e.ProfesionIdProfesion).HasColumnName("profesion_idprofesion");
        });

        modelBuilder.Entity<TblUsuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios);

            entity.ToTable("usuario");

            entity.Property(e => e.IdUsuarios)
                .ValueGeneratedNever()
                .HasColumnName("idusuarios");
            entity.Property(e => e.EmpleadoIdEmpleado).HasColumnName("empleado_idempleado");
            entity.Property(e => e.Usuario)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("usuario");
            entity.Property(e => e.Clave)
                .HasMaxLength(120)
                .IsUnicode(false)
                .HasColumnName("clave");
            entity.Property(e => e.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<TblDireccionEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdDireccionEmpleado);

            entity.ToTable("direccion_empleado");

            entity.Property(e => e.IdDireccionEmpleado)
                .ValueGeneratedNever()
                .HasColumnName("iddireccion_empleado");
            entity.Property(e => e.IdCiudad).HasColumnName("idciudad");
            entity.Property(e => e.EmpleadoIdEmpleado).HasColumnName("empleado_idempleado");
            entity.Property(e => e.DireccionEmpleado)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("direccion_empleado");
        });

        modelBuilder.Entity<TblTelefonoEmpleado>(entity =>
        {
            entity.HasKey(e => e.IdTelefonoEmpleado);

            entity.ToTable("telefono_empleado_2");

            entity.Property(e => e.IdTelefonoEmpleado)
                .ValueGeneratedNever()
                .HasColumnName("idtelefono_empleado_2");
            entity.Property(e => e.IdTipoTelefono).HasColumnName("idtipo_telefono");
            entity.Property(e => e.EmpleadoIdEmpleado).HasColumnName("empleado_idempleado");
            entity.Property(e => e.Numero)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("numero");
        });

        modelBuilder.Entity<TblCliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente);

            entity.ToTable("cliente");

            entity.Property(e => e.IdCliente)
                .ValueGeneratedNever()
                .HasColumnName("idcliente");
            entity.Property(e => e.EmpleadoIdEmpleado).HasColumnName("empleado_idempleado");
            entity.Property(e => e.IdTipoDocumento).HasColumnName("idtipo_documento");
            entity.Property(e => e.NombreApellidoCliente)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("nombre_apellido_cliente");
            entity.Property(e => e.Documento)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("documento");
            entity.Property(e => e.NumeroDocumento).HasColumnName("numero_documento");
        });

        modelBuilder.Entity<TblDireccionCliente>(entity =>
        {
            entity.HasKey(e => e.IdDireccion);

            entity.ToTable("direccion_cliente");

            entity.Property(e => e.IdDireccion)
                .ValueGeneratedNever()
                .HasColumnName("iddireccion");
            entity.Property(e => e.IdCiudad).HasColumnName("idciudad");
            entity.Property(e => e.IdCliente).HasColumnName("idcliente");
            entity.Property(e => e.DireccionCliente)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("direccion_cliente");
        });

        modelBuilder.Entity<TblTelefonoCliente>(entity =>
        {
            entity.HasKey(e => e.IdTelefono);

            entity.ToTable("telefono_cliente");

            entity.Property(e => e.IdTelefono)
                .ValueGeneratedNever()
                .HasColumnName("idtelefono");
            entity.Property(e => e.IdTipoTelefono).HasColumnName("idtipo_telefono");
            entity.Property(e => e.IdCliente).HasColumnName("idcliente");
            entity.Property(e => e.Numero)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("numero");
        });

        modelBuilder.Entity<TblIngreso>(entity =>
        {
            entity.HasKey(e => e.IdIngreso);

            entity.ToTable("ingreso");

            entity.Property(e => e.IdIngreso)
                .ValueGeneratedNever()
                .HasColumnName("idingreso");
            entity.Property(e => e.IdCliente).HasColumnName("idcliente");
            entity.Property(e => e.IdDireccion).HasColumnName("iddireccion");
            entity.Property(e => e.EmpleadoIdEmpleado).HasColumnName("empleado_idempleado");
            entity.Property(e => e.Fecha)
                .HasColumnType("date")
                .HasColumnName("fecha");
            entity.Property(e => e.Domicilio).HasColumnName("domicilio");
            entity.Property(e => e.Activo).HasColumnName("activo");
        });

        modelBuilder.Entity<TblDetalleIngreso>(entity =>
        {
            entity.HasKey(e => e.IdDetalleIngreso);

            entity.ToTable("detalle_ingreso");

            entity.Property(e => e.IdDetalleIngreso)
                .ValueGeneratedNever()
                .HasColumnName("iddetalle_ingreso");
            entity.Property(e => e.IdDescuento).HasColumnName("iddescuento");
            entity.Property(e => e.IdTipoPrendaServicio).HasColumnName("idtipo_prenda_servicio");
            entity.Property(e => e.IdCliente).HasColumnName("idcliente");
            entity.Property(e => e.IdIngreso).HasColumnName("idingreso");
            entity.Property(e => e.IdEstadoPrenda).HasColumnName("idestado_prenda");
            entity.Property(e => e.Cantidad).HasColumnName("cantidad");
            entity.Property(e => e.Valor)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("valor");
            entity.Property(e => e.Observacion)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("observacion");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
