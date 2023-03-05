using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;
using Structure.ViewModel;
using Structure.Model;
using Structure.OOP;
using dl = DSCore.List;
using ds = DSCore.String;
using RevitServices.Transactions;
using Autodesk.Revit.DB.Structure;
using wf = System.Windows.Forms;

namespace Structure
{
    /// <summary>
    /// Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        #region Properties

        public  List<RebarBarType> _rebarBarTypes;
        public List<RebarHookType> _rebarHookTypes;
        public int index = 0;

        #endregion
        public MainForm()
        {
            InitializeComponent();

            #region Filter object

            var rebarTyles = new FilteredElementCollector(VarGlobal.doc)
                               .OfClass(typeof(RebarBarType))
                               .WhereElementIsElementType()
                               .Cast<RebarBarType>()
                               .ToList();
            var rebarHookTyles = new FilteredElementCollector(VarGlobal.doc)
                               .OfClass(typeof(RebarHookType))
                               .Cast<RebarHookType>()
                               .ToList();
            var nameRebarsTyle = (from x in rebarTyles select x.Name).ToList();
            var nameRebarsHookTyle = (from x in rebarHookTyles select x.Name).ToList();
            #endregion

            #region Set source for combobox

            cb_RebarMainType.ItemsSource = nameRebarsTyle;
            cb_RebarBranchStyle.ItemsSource = new List<String> { "StirrupTie", "Standard" };
            cb_RebarBranchTyle.ItemsSource = nameRebarsTyle;
            cb_RebarBranchStartHook.ItemsSource = nameRebarsHookTyle;
            cb_RebarBranchEndHook.ItemsSource = nameRebarsHookTyle;
            cb_RebarBranchStartOri.ItemsSource = new List<string> { "Left", "Right" };
            cb_RebarBranchEndOri.ItemsSource = new List<string> { "Left", "Right" };

            #endregion

            #region Set var 
            _rebarBarTypes = rebarTyles;
            _rebarHookTypes = rebarHookTyles;
            VarGlobal.DistanceConcreteProtect = Convert.ToDouble(txt_DistanceConcreteProtect.Text);
            VarGlobal.Spacing = Convert.ToDouble(txt_Spacing.Text);
            index = 1;

            #endregion
        }

        private void txt_DistanceConcreteProtect_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                double DistanceConcreteProtect = Convert.ToDouble(txt_DistanceConcreteProtect.Text);
                VarGlobal.DistanceConcreteProtect = DistanceConcreteProtect;
            }
            catch
            {

                wf.MessageBox.Show("Please enter number is double or integer !!!");
                txt_DistanceConcreteProtect.Text = "20";
            }
        }

        private void txt_Spacing_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                double Spacing = Convert.ToDouble(txt_Spacing.Text);
                VarGlobal.Spacing = Spacing;
            }
            catch
            {

                wf.MessageBox.Show("Please enter number is double or integer !!!");
                txt_Spacing.Text = "150";
            }
        }

        private void cb_RebarMainType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarGlobal.RebarMainTyle = _rebarBarTypes[cb_RebarMainType.SelectedIndex];
        }

        private void cb_RebarBranchStyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarGlobal.RebarBranchStyle = cb_RebarBranchStyle.SelectedItem.ToString();
        }

        private void cb_RebarBranchTyle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarGlobal.RebarBranchTyle = _rebarBarTypes[cb_RebarBranchTyle.SelectedIndex];
        }

        private void cb_RebarBranchStartHook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarGlobal.RebarBranchStartHookTyle = _rebarHookTypes[cb_RebarBranchStartHook.SelectedIndex];
        }

        private void cb_RebarBranchEndHook_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarGlobal.RebarBranchEndHookTyle = _rebarHookTypes[cb_RebarBranchEndHook.SelectedIndex];
        }

        private void cb_RebarBranchStartOri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarGlobal.RebarBranchStartOri = cb_RebarBranchStartOri.SelectedItem.ToString();
        }

        private void cb_RebarBranchEndOri_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            VarGlobal.RebarBranchEndOri = cb_RebarBranchEndOri.SelectedItem.ToString();
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            VarGlobal.EventPutRebarOnColumn.Raise();
        }
    }
}
