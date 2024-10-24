using Proj.VVL.Interfaces.KiwoomHandlers;
using Proj.VVL.Interfaces.KiwoomHandlers.Abstractions;
using Proj.VVL.Service.Common.Manager;
using Proj.VVL.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace Proj.VVL.Views.Kiwoom.View
{
    public partial class RecommandTickerForm : Form
    {
        BindingSource recommandTickerViewModelBindingSource;
        RecommandTickerViewModel viewModel;

        public RecommandTickerForm(IRecommandTickerHandler recommandTickerHandler)
        {
            InitializeComponent();
            viewModel = new RecommandTickerViewModel(recommandTickerHandler);
            recommandTickerViewModelBindingSource = new BindingSource();
            recommandTickerViewModelBindingSource.DataSource = viewModel;
            textBox_Name.DataBindings.Add(new Binding(nameof(textBox_Name.Text), recommandTickerViewModelBindingSource, nameof(viewModel.Name), true, DataSourceUpdateMode.OnPropertyChanged));
            textBox_Code.DataBindings.Add(new Binding(nameof(textBox_Code.Text), recommandTickerViewModelBindingSource, nameof(viewModel.Code), true, DataSourceUpdateMode.OnPropertyChanged));
            button_Save.DataBindings.Add(new Binding(nameof(button_Save.Command), recommandTickerViewModelBindingSource, nameof(viewModel.SaveBtnClick), true, DataSourceUpdateMode.OnPropertyChanged));
            button_Delete.DataBindings.Add(new Binding(nameof(button_Delete.Command), recommandTickerViewModelBindingSource, nameof(viewModel.DeleteBtnClick), true, DataSourceUpdateMode.OnPropertyChanged));
            dataGridView1.DataSource = MainForm.Instance.KiwoomServices.recommandTickerManager.RecommandKoreaTickers;
            this.Load += RecommandTickerForm_Load;
            this.FormClosed += RecommandTickerForm_FormClosed;


        }

        private void RecommandTickerForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            MainForm.Instance.KiwoomServices.recommandTickerManager.RecommandKoreaTickers.CollectionChanged -= RefrashDataGridView; 
        }

        private void RecommandTickerForm_Load(object? sender, EventArgs e)
        {
            MainForm.Instance.KiwoomServices.recommandTickerManager.RecommandKoreaTickers.CollectionChanged += RefrashDataGridView;
        }
        
        private void button_Delete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count <= 0)
            {
                viewModel.SelectRow = -1;
                MessageBox.Show("선택한 데이터가 없습니다.");
                return;
            }
            viewModel.SelectRow = dataGridView1.SelectedCells[0].RowIndex;
            viewModel.SelectName = (string)dataGridView1.Rows[viewModel.SelectRow].Cells["Name"].Value;
            viewModel.SelectCode = (string)dataGridView1.Rows[viewModel.SelectRow].Cells["Code"].Value;
        }
        
        
        private void RefrashDataGridView(object sender, NotifyCollectionChangedEventArgs e)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = MainForm.Instance.KiwoomServices.recommandTickerManager.RecommandKoreaTickers;
        }
        
    }
}
