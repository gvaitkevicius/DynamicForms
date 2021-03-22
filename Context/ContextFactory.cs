using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DynamicForms.Context
{
    public class ContextFactory : IDesignTimeDbContextFactory<JSgi>
    {
        public JSgi CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JSgi>();
            //optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=APS_V01_PARAIBUNA;User ID=apsadmin;Password=play123PLAY!@#", opt => opt.UseRowNumberForPaging());
            optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=APS_V01_PARAIBUNA;User ID=sa;Password=123456", opt => opt.UseRowNumberForPaging());
            //optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=APS_CARTRON_QA;User ID=sa;Password=123456", opt => opt.UseRowNumberForPaging());
            //optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=APS_CARTRON_QA;User ID=sa;Password=123456", opt => opt.UseRowNumberForPaging());
            //optionsBuilder.UseSqlServer("Data Source=MASTERDAWEB-PC\\SQLEXPRESS;Initial Catalog=APS_V01;User ID=APS_USER;Password=$aPSPLaYSiS#1029384756%", opt => opt.UseRowNumberForPaging());
            //optionsBuilder.UseSqlServer("Data Source=170.81.42.168,1433;Initial Catalog=APS_V01;User ID=APS_USER;Password=$aPSPLaYSiS#1029384756%", opt => opt.UseRowNumberForPaging());
            //optionsBuilder.UseSqlServer("Data Source=192.168.10.251,63918;Initial Catalog=APS_V01;User ID=sa;Password=sacartrom", opt => opt.UseRowNumberForPaging());
            //optionsBuilder.UseSqlServer("Data Source=192.168.10.251,63918;Initial Catalog=APS_V01_PR;User ID=sa;Password=sacartrom", opt => opt.UseRowNumberForPaging());
            //optionsBuilder.UseSqlServer("Data Source=192.168.10.251,63918;Initial Catalog=APS_V01_TESTE;User ID=sa;Password=sacartrom", opt => opt.UseRowNumberForPaging());
            return new JSgi(optionsBuilder.Options);
        }

        public JSgi CreateDbContext(string[] args, string connection)
        {
            var optionsBuilder = new DbContextOptionsBuilder<JSgi>();
            optionsBuilder.UseSqlServer(connection, opt => opt.UseRowNumberForPaging());
            return new JSgi(optionsBuilder.Options);
        }
    }
}
