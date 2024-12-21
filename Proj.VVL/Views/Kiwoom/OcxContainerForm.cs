using Proj.VVL.Interfaces.KiwoomOcx;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proj.VVL.View.Kiwoom
{
    public partial class OcxContainerForm : Form
    {
        internal ConditionFuncDef condition;
        internal LoginFuncDef login;
        internal QueryFuncDef query;
        internal OrderFuncDef order;
        internal TickerInfoFuncDef ticker;
        internal ReceiveDataHandlerDef recvDataHandler;
        public OcxContainerForm()
        {
            InitializeComponent();
            condition = new ConditionFuncDef(axkhOpenapi);
            login = new LoginFuncDef(axkhOpenapi);
            query = new QueryFuncDef(axkhOpenapi);
            order = new OrderFuncDef(axkhOpenapi);
            ticker = new TickerInfoFuncDef(axkhOpenapi);
            recvDataHandler = new ReceiveDataHandlerDef(axkhOpenapi);
        }
    }
}
