using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.Geo;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.Logging;
using Proj.VVL.Data;
using Proj.VVL.Interfaces.Chart;
using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.KiwoomOcx;
using Proj.VVL.Interfaces.PubSub;
using Proj.VVL.Service.Common.Manager;
using Proj.VVL.Services.Abstractions;
using Proj.VVL.Services.Kiwoom.Managers;
using Proj.VVL.View.Kiwoom;
using Proj.VVL.ViewModels;
using Proj.VVL.Views.Kiwoom.View;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace Proj.VVL
{
    public partial class MainForm : Form
    {
        public static OcxContainerForm KiwoomOcxObj = new OcxContainerForm();
        public KiwoomServiceManager KiwoomServices;
        RecommandTickerForm recommandTickerForm;
        static int[] lineInts = new int[10];
        static int[] columnInts = new int[10];
        LineSeries<int> lineS = new LineSeries<int> { Values = lineInts };
        ColumnSeries<int> columnS = new ColumnSeries<int> { Values = columnInts };
        Views.Common.DebugConsolePanel MainDebugConsole = new Views.Common.DebugConsolePanel();
        public static MainForm Instance;
        BindingSource loginViewModelBindingSource;
        public MainFormViewModel viewModel;
        public LiveChartProperties liveChartProperties;
        public CommonServiceManager commonServices;

        public IServiceProvider Handlers { get; }

        public MainForm()
        {
            InitializeComponent();
            Handlers = ConfigurationHandler();
            ServicesStart();
            viewModel = new MainFormViewModel(Handlers.GetService<LoginHandler>());
            loginViewModelBindingSource = new BindingSource();
            loginViewModelBindingSource.DataSource = viewModel;
            Instance = this;
            this.Load += Form_Load;
            this.FormClosing += Form_Closing;

            if (!Data.Define.CreateAllDirectory())
            {
                MessageBox.Show("Create Directory Failed");
            }
        }

        private void ServicesStart()
        {
            KiwoomServices = new KiwoomServiceManager(Handlers.GetService<RecommandTickerHandler>(), Handlers.GetService<ScreenNumberHandler>());
            KiwoomServices.ServiceStart();
            commonServices = new CommonServiceManager();
            commonServices.Start();
        }

        private void ServicesStop()
        {
            if (KiwoomServices != null)
            {
                KiwoomServices.ServiceStop();
            }
            if (commonServices != null)
            {
                commonServices.Stop();
            }
            KiwoomServices = null;
            commonServices = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*
            KiwoomOcxObj.query.SetInputValue("辆格内靛", "000660");
            ERROR_CODE_DEF result = KiwoomOcxObj.query.CommRqData("RQName", "OPT10001", 0, KiwoomServiceManager.Screen.GetScreenNumber().ToString());
            Debug.WriteLine(result.ToString());
            */
            //KiwoomServices.RealTimeQuery?.RegistRealData("000660");
        }

        private void Form_Load(object sender, EventArgs e)
        {
            panel_Debug.Controls.Add(MainDebugConsole);
            Label_LoginStatus.DataBindings.Add(new Binding(nameof(Label_LoginStatus.Text), loginViewModelBindingSource, nameof(viewModel.Text), true, DataSourceUpdateMode.OnPropertyChanged));
            Label_LoginStatus.DataBindings.Add(new Binding(nameof(Label_LoginStatus.BackColor), loginViewModelBindingSource, nameof(viewModel.BackColor), true, DataSourceUpdateMode.OnPropertyChanged));
            Label_LoginStatus.DataBindings.Add(new Binding(nameof(Label_LoginStatus.ForeColor), loginViewModelBindingSource, nameof(viewModel.ForeColor), true, DataSourceUpdateMode.OnPropertyChanged));
            loginToolStripMenuItem.DataBindings.Add(new Binding(nameof(loginToolStripMenuItem.Command), loginViewModelBindingSource, nameof(viewModel.LoginCommand), true, DataSourceUpdateMode.OnPropertyChanged));
            dataGridViewPublishing_Init();

        }

        private void Form_Closing(object sender, EventArgs e)
        {
            ServicesStop();
        }

        private void 包缴辆格ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            recommandTickerForm = new RecommandTickerForm(Handlers.GetService<RecommandTickerHandler>());
            recommandTickerForm.ShowDialog();
        }

        private void dataGridViewPublishing_Init()
        {
            dataGridView_Publishing.DataSource = KiwoomServices.recommandTickerManager.RecommandKoreaTickers;
            KiwoomServices.recommandTickerManager.RecommandKoreaTickers.CollectionChanged += dataGridViewPublishing_Refrash;
        }

        private void dataGridViewPublishing_Refrash(object sender, NotifyCollectionChangedEventArgs e)
        {
            dataGridView_Publishing.DataSource = null;
            dataGridView_Publishing.DataSource = KiwoomServices.recommandTickerManager.RecommandKoreaTickers;
        }


        private static IServiceProvider ConfigurationHandler()
        {
            ServiceCollection handlers = new ServiceCollection();
            handlers.AddTransient(typeof(LoginHandler));
            handlers.AddTransient(typeof(RecommandTickerHandler));
            handlers.AddTransient(typeof(ScreenNumberHandler));
            return handlers.BuildServiceProvider();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Subscriber subTest = new Subscriber();
            //subTest.GetKiwoomCandleData("000660", DateTime.Now);
            subTest.GetRealTimeData("000660");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ChartHandler hChart = new ChartHandler("000660", Interfaces.DataInventoryHandlers.CANDLE_TIME_FRAME_DEF.DAY);
            liveChartProperties = hChart.GetCandleData();
            ChartViewer.Series = liveChartProperties.Series;
            ChartViewer.XAxes = liveChartProperties.xAxes;
            ChartViewer.YAxes = liveChartProperties.yAxes;
            ChartViewer.ZoomMode = LiveChartsCore.Measure.ZoomAndPanMode.X;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            RealTimeQueryHandler handle = new RealTimeQueryHandler();
        }
    }
}
