using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace apiFestRock.Models;

public partial class BdEventosContext : DbContext
{
    /*public BdEventosContext()
    {
    }*/

    public BdEventosContext(DbContextOptions<BdEventosContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblArtBand> TblArtBands { get; set; }

    public virtual DbSet<TblArtistum> TblArtista { get; set; }

    public virtual DbSet<TblBandum> TblBanda { get; set; }

    public virtual DbSet<TblCiudad> TblCiudads { get; set; }

    public virtual DbSet<TblDetInsc> TblDetInscs { get; set; }

    public virtual DbSet<TblDpto> TblDptos { get; set; }

    public virtual DbSet<TblEvento> TblEventos { get; set; }

    public virtual DbSet<TblGenero> TblGeneros { get; set; }

    public virtual DbSet<TblInscrip> TblInscrips { get; set; }

    public virtual DbSet<TblInstrum> TblInstrums { get; set; }

    public virtual DbSet<TblTipoDoc> TblTipoDocs { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-VV1NA63H; Database=bdEventos; Trusted_Connection=true; MultipleActiveResultSets=true; TrustServerCertificate=true;");*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblArtBand>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblArtBa__40F9A207AF80AD2F");

            entity.ToTable("tblArtBand");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.IdArtista).HasColumnName("idArtista");
            entity.Property(e => e.IdBanda).HasColumnName("idBanda");
            entity.Property(e => e.IdInstrum).HasColumnName("idInstrum");

            /*entity.HasOne(d => d.IdArtistaNavigation).WithMany(p => p.TblArtBands)
                .HasForeignKey(d => d.IdArtista)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblArtBan__idArt__6E01572D");

            entity.HasOne(d => d.IdBandaNavigation).WithMany(p => p.TblArtBands)
                .HasForeignKey(d => d.IdBanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblArtBan__idBan__6D0D32F4");

            entity.HasOne(d => d.IdInstrumNavigation).WithMany(p => p.TblArtBands)
                .HasForeignKey(d => d.IdInstrum)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblArtBan__idIns__6EF57B66");*/
        });

        modelBuilder.Entity<TblArtistum>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblArtis__40F9A20715E2B94D");

            entity.ToTable("tblArtista");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");
            entity.Property(e => e.IdGenero).HasColumnName("idGenero");
            entity.Property(e => e.IdtipoDoc).HasColumnName("idtipoDoc");
            entity.Property(e => e.Nombre)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.NroDoc).HasColumnName("nroDoc");

            /*entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.TblArtista)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblArtist__idCiu__6C190EBB");

            entity.HasOne(d => d.IdGeneroNavigation).WithMany(p => p.TblArtista)
                .HasForeignKey(d => d.IdGenero)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblArtist__idGen__6B24EA82");

            entity.HasOne(d => d.IdtipoDocNavigation).WithMany(p => p.TblArtista)
                .HasForeignKey(d => d.IdtipoDoc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblArtist__idtip__693CA210");*/
        });

        modelBuilder.Entity<TblBandum>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblBanda__40F9A2071EF1499B");

            entity.ToTable("tblBanda");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.AnioFund).HasColumnName("anioFund");
            entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.Represent)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("represent");
            entity.Property(e => e.Telefono)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("telefono");

            /*entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.TblBanda)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblBanda__idCiud__6A30C649");*/
        });

        modelBuilder.Entity<TblCiudad>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblCiuda__40F9A2076AADFA88");

            entity.ToTable("tblCiudad");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.IdDpto).HasColumnName("idDpto");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");

            /*entity.HasOne(d => d.IdDptoNavigation).WithMany(p => p.TblCiudads)
                .HasForeignKey(d => d.IdDpto)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblCiudad__idDpt__68487DD7");*/
        });

        modelBuilder.Entity<TblDetInsc>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblDetIn__40F9A2073AA6EC86");

            entity.ToTable("tblDetInsc");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Edad).HasColumnName("edad");
            entity.Property(e => e.IdArtBand).HasColumnName("idArtBand");
            entity.Property(e => e.IdNroInsc).HasColumnName("idNroInsc");

            /*entity.HasOne(d => d.IdArtBandNavigation).WithMany(p => p.TblDetInscs)
                .HasForeignKey(d => d.IdArtBand)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblDetIns__idArt__72C60C4A");

            entity.HasOne(d => d.IdNroInscNavigation).WithMany(p => p.TblDetInscs)
                .HasForeignKey(d => d.IdNroInsc)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblDetIns__idNro__71D1E811");*/
        });

        modelBuilder.Entity<TblDpto>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblDpto__40F9A20739144B46");

            entity.ToTable("tblDpto");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TblEvento>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblEvent__40F9A207BD8E9089");

            entity.ToTable("tblEvento");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Anio).HasColumnName("anio");
            entity.Property(e => e.Fecha)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("fecha");
            entity.Property(e => e.IdCiudad).HasColumnName("idCiudad");
            entity.Property(e => e.Lugar)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("lugar");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
            entity.Property(e => e.VrInscEvent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(9, 2)")
                .HasColumnName("vrInscEvent");

           /* entity.HasOne(d => d.IdCiudadNavigation).WithMany(p => p.TblEventos)
                .HasForeignKey(d => d.IdCiudad)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblEvento__idCiu__6754599E");*/
        });

        modelBuilder.Entity<TblGenero>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblGener__40F9A207A3C21C55");

            entity.ToTable("tblGenero");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TblInscrip>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblInscr__40F9A20746E9ECAB");

            entity.ToTable("tblInscrip");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Fecha)
                .HasMaxLength(12)
                .IsUnicode(false)
                .HasDefaultValueSql("(format(getdate(),'d','en-gb'))")
                .HasColumnName("fecha");
            entity.Property(e => e.IdBanda).HasColumnName("idBanda");
            entity.Property(e => e.IdEvento).HasColumnName("idEvento");
            entity.Property(e => e.Represent)
                .HasMaxLength(80)
                .IsUnicode(false)
                .HasColumnName("represent");
            entity.Property(e => e.TelRepresent)
                .HasMaxLength(25)
                .IsUnicode(false)
                .HasColumnName("telRepresent");
            entity.Property(e => e.ValorInscrip)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(12, 2)")
                .HasColumnName("valorInscrip");

            /*entity.HasOne(d => d.IdBandaNavigation).WithMany(p => p.TblInscrips)
                .HasForeignKey(d => d.IdBanda)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblInscri__idBan__70DDC3D8");

            entity.HasOne(d => d.IdEventoNavigation).WithMany(p => p.TblInscrips)
                .HasForeignKey(d => d.IdEvento)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__tblInscri__idEve__6FE99F9F");*/
        });

        modelBuilder.Entity<TblInstrum>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblInstr__40F9A207C4800E01");

            entity.ToTable("tblInstrum");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<TblTipoDoc>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PK__tblTipoD__40F9A20712CAF378");

            entity.ToTable("tblTipoDoc");

            entity.Property(e => e.Codigo)
                .ValueGeneratedNever()
                .HasColumnName("codigo");
            entity.Property(e => e.Activo)
                .HasDefaultValue(true)
                .HasColumnName("activo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nombre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
