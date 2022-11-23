using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;


namespace pc_info
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string key = string.Empty;
            switch (comboBox1.SelectedItem.ToString())
            {
                case "Процесор":
                    key = "Win32_Processor";
                    break;
                case "Відеокарта":
                    key = "Win32_VideoController";
                    break;
                case "Чіпсет":
                    key = "Win32_IDEController";
                    break;
                case "Батарея":
                    key = "Win32_Battery";
                    break;
                case "Біос":
                    key = "Win32_BIOS";
                    break;
                case "Оперативна пам'ять":
                    key = "Win32_PhysicalMemory";
                    break;
                case "Кеш":
                    key = "Win32_CacheMemory";
                    break;
                case "USB":
                    key = "Win32_USBController";
                    break;
                case "Диски":
                    key = "Win32_DiskDrive";
                    break;
                case "Мережевий адаптер":
                    key = "Win32_NetworkAdapter";
                    break;
                default:
                    key = "Win32_Processor";
                    break;
            }
            GetHardWareInfo(key, listView1);
        }


        private void GetHardWareInfo(string key, ListView list)
        {
            list.Items.Clear();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM " + key);
            try
            {
                var items = searcher.Get();
                if (items.Count == 0)
                    list.Items.Add(new ListViewItem("Не вдалося отримати інформацію"));
                else
                    foreach (ManagementObject obj in items)
                    {
                        if (obj.Properties.Count == 0)
                        {
                            MessageBox.Show("Не вдалося отримати інформацію", "Помилка",
                           MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        ListViewGroup listViewGroup;
                        try
                        {
                            listViewGroup = list.Groups.Add(obj["Name"].ToString(),
                           obj["Name"].ToString());
                        }
                        catch
                        {
                            listViewGroup = list.Groups.Add(obj.ToString(), obj.ToString());
                        }
                        foreach (PropertyData data in obj.Properties)
                        {
                            ListViewItem item = new ListViewItem(listViewGroup);
                            if (list.Items.Count % 2 == 0)
                            {
                                item.BackColor = Color.WhiteSmoke;
                            }
                            item.Text = data.Name;

                            if (data.Value != null && !string.IsNullOrEmpty(data.Value.ToString()))
                            {
                                string resStr = string.Empty;
                                switch (data.Value.GetType().ToString())
                                {
                                    case "System.String[]":
                                        string[] stringData = data.Value as string[];
                                        foreach (string s in stringData)
                                        {
                                            resStr += s + " ";
                                        }
                                        item.SubItems.Add(resStr);
                                        break;
                                    case "System.UInt16[]":
                                        ushort[] ushortData = data.Value as ushort[];
                                        foreach (ushort us in ushortData)
                                        {
                                            resStr += us.ToString() + " ";
                                        }
                                        item.SubItems.Add(resStr);
                                        break;
                                    default:
                                        item.SubItems.Add(data.Value.ToString());
                                        break;
                                }
                                list.Items.Add(item);
                            }
                        }
                    }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Помилка", MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
