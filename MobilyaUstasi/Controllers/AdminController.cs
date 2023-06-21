using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlClient;
using MobilyaUstasi.Models;
using Microsoft.Data.SqlClient;

namespace MobilyaUstasi.Controllers
{
    public class AdminController : Controller
    {
        private IConfiguration _configuration;
        private string connectionString;

        public AdminController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = configuration.GetSection("Connections").GetSection("MSSQL").Value;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            List<SliderModel> sliderModels = new List<SliderModel>();
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlDataAdapter mySql = new SqlDataAdapter("Select * From sliders", connection);
            DataTable dataTable = new DataTable();
            mySql.Fill(dataTable);
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

            return View(sliderModels);
        }

        public IActionResult Mobilya()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
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
            connection.Close();
            return View(modelList);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel loginModel) //kullanıcı username password kontrol
        {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("select * from users Where username=@username and password=@password", connection);
            adapter.SelectCommand.Parameters.Clear();
            adapter.SelectCommand.Parameters.AddWithValue("@username", loginModel.username);
            adapter.SelectCommand.Parameters.AddWithValue("@password", loginModel.password);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            if (dataTable.Rows.Count > 0)
            {
                HttpContext.Session.SetString("username", loginModel.username);
                return Redirect("/Admin/Index");
            }
            else
            {
                return Redirect("/Admin/Login");
            }
        }

        [HttpGet]
        public IActionResult SaveSlider()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveSlider(SliderModel sliderModel)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            IFormFile formFile = sliderModel.file; //image seçimi
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads\", formFile.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            sliderModel.img = Path.Combine(@"Uploads\\", formFile.FileName);
            sliderModel.img = sliderModel.img;
            SqlConnection mssql = new SqlConnection(connectionString);
            SqlCommand mssqlcommand = new SqlCommand("INSERT INTO sliders(title,content,url,img) VALUES(@title,@content,@url,@img)", mssql);
            mssqlcommand.Parameters.AddWithValue("@title", sliderModel.title);
            mssqlcommand.Parameters.AddWithValue("@content", sliderModel.content);
            mssqlcommand.Parameters.AddWithValue("@url", sliderModel.url);
            mssqlcommand.Parameters.AddWithValue("@img", sliderModel.img);
            mssql.Open();
            mssqlcommand.ExecuteNonQuery();
            mssql.Close();
            return Redirect("/Admin/Index");
        }

        [HttpGet]
        public IActionResult SaveMobilya()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveMobilya(MobilyaModel model)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            IFormFile formFile = model.file;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads\", formFile.FileName);
            using (Stream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            model.Image = Path.Combine(@"Uploads\\", formFile.FileName);
            model.Image = model.Image;
            SqlConnection mssql = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("INSERT INTO services(ServiceName,Description,Price,IsAvailable,Image) VALUES(@servicename,@description,@price,@isavailable,@image)", mssql);
            cmd.Parameters.AddWithValue("@servicename", model.ServiceName);
            cmd.Parameters.AddWithValue("@description", model.Description);
            cmd.Parameters.AddWithValue("@price", model.Price);
            cmd.Parameters.AddWithValue("@isavailable", model.IsAvailable);
            cmd.Parameters.AddWithValue("@image", model.Image);

            mssql.Open();
            cmd.ExecuteNonQuery();
            mssql.Close();
            return Redirect("/Admin/Index");
        }
        [HttpGet]
        public IActionResult UpdateMobilya(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("select * from services Where Id=@id", connection);
            adapter.SelectCommand.Parameters.Clear();
            adapter.SelectCommand.Parameters.AddWithValue("@id", id);
            DataTable table = new DataTable();
            adapter.Fill(table);
            DataRow row = table.Rows[0];
            MobilyaModel sliderModel = new MobilyaModel()
            {
                Id = row.Field<int>("Id"),
                ServiceName = row.Field<string>("ServiceName"),
                Description = row.Field<string>("Description"),
                Image = row.Field<string>("Image"),
                Price = row.Field<decimal>("Price"),
                IsAvailable = row.Field<bool>("IsAvailable")
            };
            return View(sliderModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateMobilya(MobilyaModel model)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            if (model.file != null)
            {
                IFormFile formFile = model.file;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads\", formFile.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                model.Image = Path.Combine(@"Uploads\\", formFile.FileName);
                model.Image = model.Image;
            }
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("Update services set ServiceName=@servicename,Description=@description,Image=@img,IsAvailable=@isavailable,Price=@price where Id=@id", connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", model.Id);
            command.Parameters.AddWithValue("@servicename", model.ServiceName);
            command.Parameters.AddWithValue("@description", model.Description);
            command.Parameters.AddWithValue("@img", model.Image);
            command.Parameters.AddWithValue("@isavailable", model.IsAvailable);
            command.Parameters.AddWithValue("@price", model.Price);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return Redirect("/Admin/Index");
        }

        [HttpGet]
        public IActionResult UpdateSlider(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            SqlConnection connection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter("select * from sliders Where id=@id", connection);
            adapter.SelectCommand.Parameters.Clear();
            adapter.SelectCommand.Parameters.AddWithValue("@id", id);
            DataTable table = new DataTable();
            adapter.Fill(table);
            DataRow row = table.Rows[0];
            SliderModel sliderModel = new SliderModel()
            {
                id = row.Field<int>("id"),
                title = row.Field<string>("title"),
                content = row.Field<string>("content"),
                url = row.Field<string>("url"),
                img = row.Field<string>("img")
            };
            return View(sliderModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSlider(SliderModel sliderModel)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            if (sliderModel.file != null)
            {
                IFormFile formFile = sliderModel.file;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\Uploads\", formFile.FileName);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await formFile.CopyToAsync(fileStream);
                }
                sliderModel.img = Path.Combine(@"Uploads\\", formFile.FileName);
                sliderModel.img = sliderModel.img;
            }
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("Update sliders set title=@title,content=@content,img=@img,url=@url where id=@id", connection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", sliderModel.id);
            command.Parameters.AddWithValue("@title", sliderModel.title);
            command.Parameters.AddWithValue("@content", sliderModel.content);
            command.Parameters.AddWithValue("@url", sliderModel.url);
            command.Parameters.AddWithValue("@img", sliderModel.img);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return Redirect("/Admin/Index");
        }

        public IActionResult DeleteSlider(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            SqlConnection MSSQLConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("Delete from sliders where id=@id", MSSQLConnection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", id);
            MSSQLConnection.Open();
            command.ExecuteNonQuery();
            MSSQLConnection.Close();
            return Redirect("/Admin/Index");
        }

        public IActionResult DeleteMobilya(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return Redirect("/Admin/Login");
            }
            SqlConnection MSSQLConnection = new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("Delete from services where Id=@id", MSSQLConnection);
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@id", id);
            MSSQLConnection.Open();
            command.ExecuteNonQuery();
            MSSQLConnection.Close();
            return Redirect("/Admin/Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/Admin/Login");
        }
    }
}