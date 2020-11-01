using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Security.Cryptography;
using ExtremeEnviroment.Model;
using System.Collections;
using System.Linq;
using System.Collections.ObjectModel;
using ExtremeEnviroment.Module.ImageList;


namespace ExtremeEnviroment.Module.ImagePropView
{
    /// <summary>
    /// ImagePropViewControl.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ImagePropViewControl : UserControl
    {
        public ObservableCollection<PropsItem> currentPropsItem;

        public ObservableCollection<PropsItem> CurrentPropsItem
        {
            get { return this.currentPropsItem; }
        }

        public PropsItem SelectedItem;

        public ImagePropViewControl()
        {
            InitializeComponent();
            currentPropsItem = new ObservableCollection<PropsItem>();
        }

        private void DgImageProp_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        public void SetImageProps(Dictionary<String, String> metadataMap)
        {

            for (int index = 0; index < metadataMap.Count; index++)
            {
                var item = metadataMap.ElementAt(index);
                this.AddRow(index, new PropsItem
                {
                    PROP = item.Key,
                    VALUE = item.Value
                });
            }
        }

        public void AddRow(int index, Dictionary<string, string> row)
        {
            this.AddRow(index, new PropsItem(row));
        }


        public void AddRow(int index, PropsItem item)
        {
            item.INDEX = index;
            item.NUM = DgImageProp.Items.Count + 1;
            currentPropsItem.Add(item);
            this.RefreshDataGrid();
        }

        public void UpdateRow(int index, PropsItem item)
        {
            if (!currentPropsItem.Any(d => d.PROP == item.PROP))
            {
                this.AddRow(index, item);
            }
            else
            {
                currentPropsItem[index] = item;
            }
            this.RefreshDataGrid();

        }


        private void RefreshDataGrid()
        {
            this.DgImageProp.ItemsSource = null;
            this.DgImageProp.ItemsSource = this.currentPropsItem;
        }



        private void DgImageProp_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            PropsItem editedPropsItem = null;

            int edtiedCellIndex = e.Column.DisplayIndex;

            if (edtiedCellIndex == 0)
            {
                TextBox prop = e.EditingElement as TextBox;

                if (currentPropsItem.Any(d => d.PROP == prop.Text))
                {
                    MessageBox.Show("동일한 속성이 존재합니다.", "Notice", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.RefreshDataGrid();
                    return;
                }

                if (prop != null && DgImageProp.Columns[1].GetCellContent(DgImageProp.Items[e.Row.GetIndex()]) is TextBlock value)
                {
                    editedPropsItem = new PropsItem
                    {
                        PROP = prop.Text,
                        VALUE = value.Text
                    };
                }
                    
            }
            else if (edtiedCellIndex == 1)
            {
                if (DgImageProp.Columns[0].GetCellContent(DgImageProp.Items[e.Row.GetIndex()]) is TextBlock prop && e.EditingElement is TextBox value)
                {
                    editedPropsItem = new PropsItem
                    {
                        PROP = prop.Text,
                        VALUE = value.Text
                    };
                }
            }

            if (!String.IsNullOrEmpty(editedPropsItem.PROP) && !String.IsNullOrEmpty(editedPropsItem.VALUE))
            {
                
                this.UpdateRow(e.Row.GetIndex(), editedPropsItem);
                this.UpdateTreeItem();
            }

        }

        private void BtbClick_Delete(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItems = DgImageProp.SelectedItems.Cast<PropsItem>().ToList();

                foreach (PropsItem item in selectedItems)
                {
                    currentPropsItem.Remove(item);
                }
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("Unable to cast the selecte item.");
            }
            finally
            {
                this.RefreshDataGrid();
                this.UpdateTreeItem();
            }
            
        }

        private void UpdateTreeItem() 
        {
            Dictionary<string, string> currentProps = currentPropsItem.Where(d => !String.IsNullOrEmpty(d.PROP))
                .Select(d => new PropsItem { PROP = d.PROP, VALUE = d.VALUE, INDEX = d.INDEX, NUM = d.NUM })
                .ToDictionary(v => v.PROP, v => v.VALUE);
            
            MainWindow mainWindow = ExtremeEnviroment.MainWindow._mainWindow;
            ImageListControl imageListControl = mainWindow.GetImageListControl();
            imageListControl.SetTreeItem(currentProps);

            if(this.HasMapData(currentProps))
            {
                mainWindow.GetMapViewControl().ZoomMap(currentProps, 15);
            }
        }

        private Boolean HasMapData(Dictionary<string, string> currentProps)
        {
            return currentProps.ContainsKey("Latitude") && currentProps.ContainsKey("Longitude");
        }
    }
    public class PropsItem
    {
        public PropsItem()
        {
        }
        public PropsItem(Dictionary<string, string> keyValuePairs)
        {
            this.FromDictionary(keyValuePairs);
        }
        public int INDEX { get; set; }
        public int NUM { get; set; }
        public string PROP { get; set; }
        public string VALUE { get; set; }

        public void FromDictionary(Dictionary<string, string> keyValuePairs)
        {
            this.PROP = keyValuePairs.GetValueOrDefault("PROP").ToString();
            this.VALUE = keyValuePairs.GetValueOrDefault("VALUE").ToString();
        }

    }
}
