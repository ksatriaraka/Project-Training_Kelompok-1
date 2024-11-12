using Matematika_UNDIP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace Matematika_UNDIP.Controllers
{
    public class MatkulController : Controller
    {
        private readonly string _connString;

        public MatkulController(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            List<Matkul> matkuls = new List<Matkul>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("SELECT * FROM Matkul", conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                matkuls.Add(new Matkul
                                {
                                    Kode = reader["Kode"].ToString(),
                                    NamaMataKuliah = reader["NamaMataKuliah"].ToString()
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log the error and handle it appropriately
                    return View("Error"); // Make sure you have an Error view
                }
            }
            return View(matkuls);
        }

        public IActionResult ListDosen(string kode)
        {
            if (string.IsNullOrEmpty(kode))
            {
                return NotFound();
            }

            Matkul matkul = null;
            List<Dosen> dosenList = new List<Dosen>();

            using (SqlConnection conn = new SqlConnection(_connString))
            {
                try
                {
                    conn.Open();

                    // Get Matkul details
                    string matkulQuery = "SELECT * FROM Matkul WHERE Kode = @Kode";
                    using (SqlCommand cmd = new SqlCommand(matkulQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@Kode", kode);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                matkul = new Matkul
                                {
                                    Kode = reader["Kode"].ToString(),
                                    NamaMataKuliah = reader["NamaMataKuliah"].ToString()
                                };
                            }
                        }
                    }

                    // Only proceed to get dosen if matkul exists
                    if (matkul != null)
                    {
                        // Get assigned lecturers
                        string dosenQuery = @"
                        SELECT d.* 
                        FROM Dosen d
                        INNER JOIN DosenMatkul dm ON d.DosenID = dm.DosenID
                        WHERE dm.KodeMatkul = @Kode";

                        using (SqlCommand cmd = new SqlCommand(dosenQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@Kode", kode);
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    dosenList.Add(new Dosen
                                    {
                                        DosenID = reader["DosenID"].ToString(),
                                        Name = reader["Name"].ToString()
                                    });
                                }
                            }
                        }

                        matkul.DosenPengampu = dosenList;
                    }
                }
                catch (Exception ex)
                {
                    // Log the error and handle it appropriately
                    return View("Error"); // Make sure you have an Error view
                }
            }

            if (matkul == null)
            {
                return NotFound();
            }

            return View(matkul);
        }
    }
}
