using Velopack;

namespace Data_Logger_1._3;

public static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
#if RELEASE
        VelopackApp.Build().Run();
#endif

        var app = new App();
        app.InitializeComponent();
        app.Run();
    }
}