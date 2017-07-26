using StructureMap;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Pictures.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var container = new Container();
            {
                container.Configure(r =>
                {
                    r.For(typeof(DAL.Memory.DataAccess.IGenericRepository<>)).Use(typeof(DAL.Memory.DataAccess.GenericRepository<>));

                    r.For(typeof(Domain.DataAccess.IGenericRepository<Dto.Picture>)).Use(typeof(DAL.Memory.GenericRepository<Dto.Picture, DAL.Memory.Domain.Picture>))
                       .Named("PictureRepository");

                    r.For(typeof(Domain.DataAccess.GenericService<Dto.Picture>))
                        .Use(typeof(Domain.DataAccess.GenericService<Dto.Picture>))
                            .Named("PictureService")
                            .Ctor<Domain.DataAccess.IGenericRepository<Dto.Picture>>();

                    r.For<MainWindow>();
                });

                var wnd = container.GetInstance<MainWindow>();
                wnd.initRepo();
                wnd.Show();
            }
        }
    }
}
