using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

 namespace Laba5
{
    public partial class fMain : Form
    {
        public fMain()
        {
            InitializeComponent();
        }
        private void fMain_Load(object sender, EventArgs e)
        {
            gvComputers.AutoGenerateColumns = false;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "modelName";
            column.Name = "Бренд";
            gvComputers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Processor";
            column.Name = "Процесор";
            gvComputers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "RAM";
            column.Name = "Оперативна Пам'ять";
            gvComputers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Storage";
            column.Name = "Пам'ять";
            gvComputers.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Price";
            column.Name = "Ціна";
            gvComputers.Columns.Add(column);

            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "HasGraphicsCard";
            column.Name = "Відеокарта";
            column.Width = 60;
            gvComputers.Columns.Add(column);

            column = new DataGridViewCheckBoxColumn();
            column.DataPropertyName = "HasWiFi";
            column.Name = "Вайфай";
            column.Width = 60;
            gvComputers.Columns.Add(column);


            bindSrcComputers.Add(new Computer("Acer", "i7", 16, 1000, 999, true, true));
            EventArgs args = new EventArgs(); OnResize(args);
        }
        private void fMain_Resize(object sender, EventArgs e)
        {
            int buttonsSize = 5 * btnAdd.Width + 2 * tsSeparator1.Width + 30;
            btnExit.Margin = new Padding(Width - buttonsSize, 0, 0, 0);
        }

        private void gvComputers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            IComputer computer = new Computer(); 

            fComputer ft = new fComputer(computer as Computer); 
            if (ft.ShowDialog() == DialogResult.OK)
            {
                bindSrcComputers.Add(computer);
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            {
                Computer computer = (Computer)bindSrcComputers.List[bindSrcComputers.Position];

                fComputer ft = new fComputer(computer);
                if (ft.ShowDialog() == DialogResult.OK)
                {
                    bindSrcComputers.List[bindSrcComputers.Position] = computer;
                }
            
        }
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            {
                if (MessageBox.Show("Видалити поточний запис?", "Видалення запису",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    bindSrcComputers.RemoveCurrent();
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            {
                if (MessageBox.Show("Очистити таблицю?\n\nВсі дані будуть втрачені", "Очищення даних",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    bindSrcComputers.Clear();
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            {
                if (MessageBox.Show("Закрити застосунок?", "Вихід з програми", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

        private void btnSaveAsText_Click(object sender, EventArgs e)
        {
            saveFileDialog.Filter = "Текстові файли (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog.Title = "Зберегти дані у текстовому форматі";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            StreamWriter sw;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8);
                try
                {
                   
                    foreach (Computer computer in bindSrcComputers.List)
                    {
                        
                        sw.Write(computer.ModelName + "\t" + computer.Processor + "\t" +
                                 computer.RAM + "\t" + computer.Storage + "\t" +
                                 computer.Price + "\t" + computer.HasGraphicsCard + "\t" +
                                 computer.HasWiFi + "\n");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталася помилка: \n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    sw.Close();
                }
            }

        }

        private void btnSaveAsBinary_Click(object sender, EventArgs e)
        {
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Файли комп'ютерів (*.comp)|*.comp|Всі файли (*.*)|*.*";
                saveFileDialog.Title = "Зберегти дані у бінарному форматі";
                saveFileDialog.InitialDirectory = Application.StartupPath;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (BinaryWriter bw = new BinaryWriter(saveFileDialog.OpenFile()))
                    {
                        try
                        {
                            foreach (Computer computer in bindSrcComputers.List)
                            {
                                bw.Write(computer.ModelName);
                                bw.Write(computer.Processor);
                                bw.Write(computer.RAM);
                                bw.Write(computer.Storage);
                                bw.Write(computer.Price);
                                bw.Write(computer.HasGraphicsCard);
                                bw.Write(computer.HasWiFi);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Сталася помилка: \n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnOpenFromText_Click(object sender, EventArgs e)
        {
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Текстові файли (*.txt)|*.txt|Всі файли (*.*)|*.*";
                openFileDialog.Title = "Прочитати дані з текстового формату";
                openFileDialog.InitialDirectory = Application.StartupPath;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    bindSrcComputers.Clear();  
                    StreamReader sr = null;

                    try
                    {
                        sr = new StreamReader(openFileDialog.FileName, Encoding.UTF8);
                        string s;

                        while ((s = sr.ReadLine()) != null)
                        {
                            string[] split = s.Split('\t');

                            Computer computer = new Computer
                            (
                                split[0],  
                                split[1],  
                                int.Parse(split[2]),  
                                int.Parse(split[3]),  
                                double.Parse(split[4]),  
                                bool.Parse(split[5]),  
                                bool.Parse(split[6]) 
                            );

                            bindSrcComputers.Add(computer);  
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Сталася помилка: \n{ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        if (sr != null)
                            sr.Close();  
                    }
                }
            }
        }

        private void btnOpenFromBinary_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Файли даних (*.computers)|*.computers|All files (*.*) | *.*";
            openFileDialog.Title = "Прочитати дані з бінарного файлу";
            openFileDialog.InitialDirectory = Application.StartupPath;
            BinaryReader br;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                bindSrcComputers.Clear();
                br = new BinaryReader(openFileDialog.OpenFile());
                try
                {
                    Computer computer;
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        computer = new Computer();
                        for (int i = 1; i <= 7; i++)
                        {
                            switch (i)
                            {
                                case 1:
                                    computer.ModelName = br.ReadString();
                                    break;
                                case 2:
                                    computer.Processor = br.ReadString();
                                    break;
                                case 3:
                                    computer.RAM = br.ReadInt32();
                                    break;
                                case 4:
                                    computer.Storage = br.ReadInt32();
                                    break;
                                case 5:
                                    computer.Price = br.ReadDouble();
                                    break;
                                case 6:
                                    computer.HasGraphicsCard = br.ReadBoolean();
                                    break;
                                case 7:
                                    computer.HasWiFi = br.ReadBoolean();
                                    break;
                            }
                        }
                        bindSrcComputers.Add(computer);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Сталася помилка: \n{ex.Message}", ex.Message,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    br.Close();
                }
            }

        }
    }
}










