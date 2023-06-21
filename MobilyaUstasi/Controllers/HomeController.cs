using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using MobilyaUstasi.Models;
using Microsoft.Data.SqlClient;

namespace MobilyaUstasi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IConfiguration _configuration;
        private string connectionString;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            connectionString = configuration.GetSection("Connections").GetSection("MSSQL").Value;
        }

        public IActionResult Index()
        {
            List<SliderModel> sliderModels = new List<SliderModel>();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataAdapter mssql = new SqlDataAdapter("Select * From sliders", connection);
            DataTable dataTable = new DataTable();
            mssql.Fill(dataTable);
            foreach (DataRow row in dataTable.Rows)
            {
                sliderModels.Add(
                    new SliderModel()
                    {
                        id = row.Field<int>("id"),
                        title = row.Field<string>("title"),
                        content = row.Field<string>("content"),
                        url = row.Field<string>("url"),
                        img = row.Field<string>("img")
                    });

            }

            string command = "SELECT * FROM services";
            SqlCommand cmd = new SqlCommand(command, connection);
            SqlDataReader rd = cmd.ExecuteReader();
            List<MobilyaModel> modelList = new List<MobilyaModel>();
            while (rd.Read())
            {
                MobilyaModel model = new MobilyaModel();
                model.Id = rd.GetInt32("Id");
                model.ServiceName = rd.GetString("ServiceName");
                model.Description = rd.GetString("Description");
                model.Price = rd.GetDecimal("Price");
                model.IsAvailable = rd.GetBoolean("IsAvailable");
                model.Image = rd.GetString("Image");
                modelList.Add(model);
            }
            ViewBag.mobilyaList = modelList;
            connection.Close();
            return View(sliderModels);
        }

        public IActionResult Privacy() //bu sayfalar oluşturulmadı
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)] 
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}