using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace webdisplay
{
    public partial class aggrdisplay : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loaddata();
            }
        }

        protected void timer1_Tick(object sender, EventArgs e)
        {
            loaddata();
        }

        public void loaddata()
        {
            try
            {
                violationcount();
            }
            catch (Exception ex)
            {
                errortxt.Text = "Error: " + ex.Message.ToString() + System.Environment.NewLine;
                errortxt.Text += "Error: " + ex.StackTrace.ToString() + System.Environment.NewLine;
            }
        }

        public void violationcount()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConfigurationManager.AppSettings["connsr"];


                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {

                    //select top 20 label, AvgConfidence, count, inserttime from visionkitcount order by inserttime desc;
                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select top 20 ");
                    sb.Append("label,");
                    sb.Append("AvgConfidence, count, inserttime ");
                    sb.Append(" from visionkitcount");
                    sb.Append(" order by inserttime desc;");
                    String sql = sb.ToString();

                    alert al = new alert();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            GridView1.DataSource = reader;
                            GridView1.DataBind();

                        }
                    }
                }


            }
            catch (Exception ex)
            {
                errortxt.Text = "Error: " + ex.Message.ToString() + System.Environment.NewLine;
                errortxt.Text += "Error: " + ex.StackTrace.ToString() + System.Environment.NewLine;
                //throw ex;
            }
        }

    }
}