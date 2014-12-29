using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversityManagementSystem;

namespace ProductManagementDB
{
    public partial class productManagementUI : Form
    {
        DatabaseConnection newConnection=new DatabaseConnection();
        SqlCommand command1;
        SqlConnection conn1;
        SqlDataReader selectDataReader;
        Dictionary<string, Product> productDictionary = new Dictionary<string, Product>();
        private string pID = string.Empty;
        private string pName = string.Empty;
        private string pPrice = string.Empty;
        private string pCategory = string.Empty;
        public productManagementUI()
        {
            InitializeComponent();
            PopulateListView();
        }

        private void PopulateListView()
        {


            productDictionary.Clear();
            sqlShowListView.Items.Clear();
            newConnection.sqlQuery = "Select * from tProduct";

            newConnection.connectDB(out command1, out conn1);
            selectDataReader = newConnection.sqlSelect(command1);





           

            while (selectDataReader.Read())
            {
                
                Product product= new Product();
                product.name = selectDataReader["productName"].ToString();
                product.category = selectDataReader["productCategory"].ToString();
                product.price = Convert.ToDouble(selectDataReader["productPrice"]);


                productDictionary.Add(selectDataReader["productID"].ToString(),product);
            }
            newConnection.connectionClose(conn1);
            selectDataReader.Close();
            foreach (KeyValuePair<string,Product> eachProductPair in productDictionary)
            {
                ListViewItem item = new ListViewItem();
                item.Text = eachProductPair.Key;
                item.SubItems.Add(eachProductPair.Value.name);
                item.SubItems.Add(eachProductPair.Value.price.ToString());
                item.SubItems.Add(eachProductPair.Value.category);
                item.Tag = eachProductPair;
                sqlShowListView.Items.Add(item);
            }
        }

        private void sqlShowListView_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem item = sqlShowListView.SelectedItems[0];
            //Dictionary<string, Product> selectedProduct = (Dictionary<string, Product>)item.Tag;
            KeyValuePair<string, Product> selectedProduct = (KeyValuePair<string, Product>)item.Tag;

            nameTextBox.Text = selectedProduct.Value.name.ToString();
            idTextBox.Text = selectedProduct.Key;
            priceTextBox.Text = selectedProduct.Value.price.ToString();
            categoryTextBox.Text = selectedProduct.Value.category.ToString();
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            pID = idTextBox.Text;
            pName = nameTextBox.Text;
            pPrice = priceTextBox.Text;
            pCategory = categoryTextBox.Text;

            newConnection.sqlQuery = string.Format("Select * from tProduct where productID='{0}'", idTextBox.Text);

            newConnection.connectDB(out command1, out conn1);
            selectDataReader = newConnection.sqlSelect(command1);

           // if (productDictionary.ContainsKey(pID))//  check using Dictionary Key value
            
            if(selectDataReader.Read())
            {
                MessageBox.Show("Product ID cannot be duplicate");
                idTextBox.Text = "";
                nameTextBox.Text = "";
                priceTextBox.Text = "";
                categoryTextBox.Text = "";

            }
            else
            {
                newConnection.sqlQuery = string.Format("INSERT INTO tProduct values('{0}','{1}','{2}','{3}')", pID,
                    pName, pPrice, pCategory);

                newConnection.connectDB(out command1, out conn1);
                int affectedROW = newConnection.sqlDML(command1);

                if (affectedROW > 0)
                {
                    MessageBox.Show("Data Inserted");
                    idTextBox.Text = "";
                    nameTextBox.Text = "";
                    priceTextBox.Text = "";
                    categoryTextBox.Text = "";


                }
                else
                {
                    MessageBox.Show("Insertion Failed");

                    idTextBox.Text = "";
                    nameTextBox.Text = "";
                    priceTextBox.Text = "";
                    categoryTextBox.Text = "";
                }
            }
            newConnection.connectionClose(conn1);


                PopulateListView();
            }

        private void searchButton_Click(object sender, EventArgs e)
        {
            //sqlShowListView.Items.Clear();
            //PopulateListView();
            newConnection.sqlQuery = string.Format("Select * from tProduct where productID='{0}'",idTextBox.Text);

            newConnection.connectDB(out command1, out conn1);
            selectDataReader = newConnection.sqlSelect(command1);

            if (selectDataReader.HasRows)
            {
                while (selectDataReader.Read())
            {
                
                nameTextBox.Text = selectDataReader["productName"].ToString();
                categoryTextBox.Text = selectDataReader["productCategory"].ToString();
                priceTextBox.Text = Convert.ToString(selectDataReader["productPrice"]);


                
            }
                MessageBox.Show("Product Found");
            }
            else
            {
                MessageBox.Show("Product Not Found");
            }
        }

        private void srchButton_Click(object sender, EventArgs e)
        {
            newConnection.sqlQuery = string.Format("Select * from tProduct where productID='{0}'",idTextBox.Text);

            newConnection.connectDB(out command1, out conn1);
            selectDataReader = newConnection.sqlSelect(command1);



            if (selectDataReader.HasRows)
            {
                while (selectDataReader.Read())
                {

                    nameTextBox.Text = selectDataReader["productName"].ToString();
                    categoryTextBox.Text = selectDataReader["productCategory"].ToString();
                    priceTextBox.Text = Convert.ToString(selectDataReader["productPrice"]);



                }
                MessageBox.Show("Product Found");
            }
            else
            {
                MessageBox.Show("Product Not Found");

                idTextBox.Text = "";
                nameTextBox.Text = "";
                priceTextBox.Text = "";
                categoryTextBox.Text = "";
            }
           
           
            
        }

        private void sqlShowListView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        }
    }
