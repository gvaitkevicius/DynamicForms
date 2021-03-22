using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DynamicForms.Areas.PlugAndPlay.Models
{
    public class TipoMovimentoEstoqueMap : IEntityTypeConfiguration<TipoMovimentoEstoque>
    {
        public void Configure(EntityTypeBuilder<TipoMovimentoEstoque> builder)
        {
            builder.ToTable("T_TIPO_MOV_ESTOQUE");
            builder.HasKey(x => x.TIP_ID);
            builder.Property(x => x.TIP_ID).HasColumnName("TIP_ID").HasMaxLength(3);
            builder.Property(x => x.TIP_DESCRICAO).HasColumnName("TIP_DESCRICAO").HasMaxLength(100).IsRequired();
            builder.Property(x => x.SPR).HasColumnName("SPR").IsRequired();
            builder.HasDiscriminator<int>("TIP_TYPE").

                HasValue<TipoMovEntradaProducao>(1).
                HasValue<TipoMovEntradaInventario>(50).
                HasValue<TipoMovEntradaCompras>(100).
                HasValue<TipoMovEntradaDevolucoes>(150).
                HasValue<TipoMovEntredaTransferenciaInterna>(200).
                HasValue<TipoMovTransferenciaSimples>(500).
                HasValue<TipoMovSaidaInventario>(501).
                HasValue<TipoMovSaidaTransferenciaInterna>(550).
                HasValue<TipoMovSaidaVendas>(600).
                HasValue<TipoMovSaidaPerdas>(650).
                HasValue<TipoMovSaidaConsumo>(700).
                HasValue<TipoMovRetencao>(101).
                HasValue<TipoMovEstorno>(1001);

            builder.HasKey(x => x.TIP_ID);
        }
    }



    public class TipoMovSaidaConsumoMap : IEntityTypeConfiguration<TipoMovSaidaConsumo>
    {
        public void Configure(EntityTypeBuilder<TipoMovSaidaConsumo> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }
    public class TipoMovTransferenciaSimplesMap : IEntityTypeConfiguration<TipoMovTransferenciaSimples>
    {
        public void Configure(EntityTypeBuilder<TipoMovTransferenciaSimples> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }
    public class TipoMovReservaMap : IEntityTypeConfiguration<TipoMovRetencao>
    {
        public void Configure(EntityTypeBuilder<TipoMovRetencao> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }
    public class TipoMovSaidaPerdasMap : IEntityTypeConfiguration<TipoMovSaidaPerdas>
    {
        public void Configure(EntityTypeBuilder<TipoMovSaidaPerdas> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }

    public class TipoMovSaidaVendasMap : IEntityTypeConfiguration<TipoMovSaidaVendas>
    {
        public void Configure(EntityTypeBuilder<TipoMovSaidaVendas> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }
    public class TipoMovSaidaTransferenciaInternaMap : IEntityTypeConfiguration<TipoMovSaidaTransferenciaInterna>
    {
        public void Configure(EntityTypeBuilder<TipoMovSaidaTransferenciaInterna> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }
    public class TipoMovSaidaInventarioMap : IEntityTypeConfiguration<TipoMovSaidaInventario>
    {
        public void Configure(EntityTypeBuilder<TipoMovSaidaInventario> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }
    public class TipoMovEntredaTransferenciaInternaMap : IEntityTypeConfiguration<TipoMovEntredaTransferenciaInterna>
    {
        public void Configure(EntityTypeBuilder<TipoMovEntredaTransferenciaInterna> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }
    public class TipoMovEntradaDevolucoesMap : IEntityTypeConfiguration<TipoMovEntradaDevolucoes>
    {
        public void Configure(EntityTypeBuilder<TipoMovEntradaDevolucoes> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }

    public class TipoMovEntradaProducaoMap : IEntityTypeConfiguration<TipoMovEntradaProducao>
    {
        public void Configure(EntityTypeBuilder<TipoMovEntradaProducao> builder)
        {
            builder.ToTable("T_TIPO_MOV_ESTOQUE");
            builder.HasKey(x => x.TIP_ID);
        }
    }


    public class TipoMovEntradaInventarioMap : IEntityTypeConfiguration<TipoMovEntradaInventario>
    {
        public void Configure(EntityTypeBuilder<TipoMovEntradaInventario> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }

    public class TipoMovEntradaComprasMap : IEntityTypeConfiguration<TipoMovEntradaCompras>
    {
        public void Configure(EntityTypeBuilder<TipoMovEntradaCompras> builder)
        {
            builder.HasKey(x => x.TIP_ID);
        }
    }





}