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
    public partial class objects : System.Web.UI.Page
    {

        public static int personcount = 0;
        public static DateTime previouspersoncounttime = DateTime.UtcNow;
        public static int vestcount = 0;
        public static int hardhatcount = 0;
        public static int safetyglasscount = 0;
        public static DateTime previousvesttime = DateTime.UtcNow;
        public static DateTime previoushardhattime = DateTime.UtcNow;
        public static DateTime previoussafetyglasstime = DateTime.UtcNow;
        public static int alertcount = 0;
        public static int safetycount = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                loaddata();
            }
            
        }

        public void loadobjects()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConfigurationManager.AppSettings["connsr"];


                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {
                  

                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("SELECT TOP 1 label, confidence, ConnectionDeviceGenerationId, EnqueuedTime, inserttime FROM visionkitinputs order by EnqueuedTime desc");
                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
 
                                labels.Text = reader.GetString(0).ToString();
                                if(reader.GetString(0).ToString().ToLower().Contains("person"))
                                {
                                    //if (DateTime.UtcNow.Subtract(reader.GetDateTime(3)).TotalSeconds < 30)
                                    //{
                                    //    personcount++;
                                    //}
                                    if(previouspersoncounttime != reader.GetDateTime(3))
                                    {
                                        personcount++;
                                        pCount.Text = personcount.ToString();
                                        previouspersoncounttime = reader.GetDateTime(3);
                                    }
                                    
                                }
                                if (reader.GetString(0).ToString().ToLower().Contains("vest"))
                                {
                                    if (previousvesttime != reader.GetDateTime(3))
                                    {
                                        vestcount++;
                                        vestcountlbl.Text = vestcount.ToString();
                                        previousvesttime = reader.GetDateTime(3);
                                    }

                                }
                                if (reader.GetString(0).ToString().ToLower().Contains("hardhat"))
                                {
                                    if (previoushardhattime != reader.GetDateTime(3))
                                    {
                                        hardhatcount++;
                                        hardhatcountlbl.Text = hardhatcount.ToString();
                                        previoushardhattime = reader.GetDateTime(3);
                                    }

                                }
                                if (reader.GetString(0).ToString().ToLower().Contains("safety"))
                                {
                                    if (previoussafetyglasstime != reader.GetDateTime(3))
                                    {
                                        safetyglasscount++;
                                        safetyglasscountlbl.Text = safetyglasscount.ToString();
                                        previoussafetyglasstime = reader.GetDateTime(3);
                                    }

                                }
                                timeupd.Text = reader.GetDateTime(3).AddMinutes(-300).ToString();
                                confidence.Text = reader.GetDouble(1).ToString();
                                updtime.Text = DateTime.Now.AddMinutes(-300).ToString();
                                double lagminute = DateTime.UtcNow.Subtract(reader.GetDateTime(3)).TotalSeconds;
                                lagminute = Math.Round(lagminute, 2);
                                String s = String.Format("{0:C2}", lagminute.ToString());
                                lagtime.Text = s;
                                
                                
                            }
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

        protected void timer1_Tick(object sender, EventArgs e)
        {
            loaddata();
        }

        public void loaddata()
        {
            try
            {
                loadobjects();
                loadchart1();
                loadchart2();
                BindListView();
                violationcount();
            }
            catch (Exception ex)
            {
                errortxt.Text = "Error: " + ex.Message.ToString() + System.Environment.NewLine;
                errortxt.Text += "Error: " + ex.StackTrace.ToString() + System.Environment.NewLine;
            }
        }

        public void loadchart1()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConfigurationManager.AppSettings["connsr"];


                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {


                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("SELECT TOP 1 label, confidence, ConnectionDeviceGenerationId, EnqueuedTime, inserttime FROM visionkitinputs order by EnqueuedTime desc");

                    sb.Append("select max(label) as Label, max(EnqueuedTime) as EnqueuedTime, count(*) as Labelcount ");
                    sb.Append("from visionkitinputs ");
                    sb.Append("where EnqueuedTime between dateadd(dd,-1,getUTCdate()) and getUTCdate() ");
                    sb.Append("group by DATEPART(hour, EnqueuedTime) ");
                    sb.Append("order by EnqueuedTime desc");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            

                            Chart1.DataSource = reader;
                            Chart1.Series[0].YValueMembers = "Label";
                            Chart1.Series[1].YValueMembers = "Labelcount";
                            Chart1.Series[0].XValueMember = "EnqueuedTime";
                            Chart1.Series[1].XValueMember = "EnqueuedTime";
                            Chart1.DataBind();
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

        public void loadchart2()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConfigurationManager.AppSettings["connsr"];


                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {


                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("SELECT TOP 1 label, confidence, ConnectionDeviceGenerationId, EnqueuedTime, inserttime FROM visionkitinputs order by EnqueuedTime desc");

                    sb.Append("select max(EnqueuedTime) as EnqueuedTime, Avg(confidence) as confidence ");
                    sb.Append("from visionkitinputs ");
                    sb.Append("where EnqueuedTime between dateadd(dd,-1,getUTCdate()) and getUTCdate() ");
                    sb.Append("group by DATEPART(hour, EnqueuedTime) ");
                    sb.Append("order by EnqueuedTime desc");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {


                            Chart2.DataSource = reader;
                            Chart2.Series[0].YValueMembers = "confidence";
                            Chart2.Series[0].XValueMember = "EnqueuedTime";
                            Chart2.DataBind();
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


        private void BindListView()
        {

            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConfigurationManager.AppSettings["connsr"];


                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {


                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    //sb.Append("SELECT TOP 1 label, confidence, ConnectionDeviceGenerationId, EnqueuedTime, inserttime FROM visionkitinputs order by EnqueuedTime desc");

                    sb.Append("select top 10 * ");
                    sb.Append("from visionkitinputs ");
                    sb.Append("order by EnqueuedTime desc");

                    String sql = sb.ToString();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {


                            lview.DataSource = reader;
                            lview.DataBind();
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


        public void violationcount()
        {
            try
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.ConnectionString = ConfigurationManager.AppSettings["connsr"];


                using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                {


                    connection.Open();
                    StringBuilder sb = new StringBuilder();
                    sb.Append("select ");
                    sb.Append("label,");
                    sb.Append("count(*) as Count");
                    sb.Append(" from visionkitinputs");
                    sb.Append(" where EnqueuedTime between DATEADD(minute, -2, GETUTCDATE()) and GETUTCDATE()");
                    sb.Append(" group by label;");
                    String sql = sb.ToString();

                    alert al = new alert();

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                if (reader.GetString(0).ToString().ToLower().Contains("person"))
                                {
                                    al.person = true;
                                }
                                if (reader.GetString(0).ToString().ToLower().Contains("vest"))
                                {
                                    al.vest = true;
                                }
                                if (reader.GetString(0).ToString().ToLower().Contains("hardhat"))
                                {
                                    al.hardhat = true;
                                }
                                if (reader.GetString(0).ToString().ToLower().Contains("safety"))
                                {
                                    al.safetyglass = true;
                                }
                            }

                            if(al.person && (!al.vest || !al.hardhat || !al.safetyglass))
                            {
                                alertcount++;
                                alertcountlbl.Text = alertcount.ToString();
                            }
                            if (al.person && (al.vest && al.hardhat && al.safetyglass))
                            {
                                safetycount++;
                                safetycountlbl.Text = safetycount.ToString();
                            }

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