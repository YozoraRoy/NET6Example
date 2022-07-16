using Dapper;
using Microsoft.AspNetCore.Mvc;
using SqlApplication.Models;
using System.Data.SqlClient;

namespace SqlApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;

        public HomeController(IConfiguration configuration)
        {
            // appsettings.json 建立連線字串
            _connectionString = configuration.GetConnectionString("TodoItemsConnection");
        }

        /// <summary>
        /// ToDo的物件全文檢索
        /// </summary>
        public async Task<IActionResult> Index()
        {
            using var connection = new SqlConnection(_connectionString);

            var sql = "SELECT * FROM TodoItems";

            // 轉換成Model
            var Items = await connection.QueryAsync<TodoItemViewModel>(sql);

            return View(new TodoItemViewModel { Items = Items.ToList() });
        }

        /// <summary>
        /// ToDo新增
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = "INSERT INTO TodoItems Values(@Name, 'false');";

                // 參數名稱設定
                var param = new TodoItemViewModel { Name = todoItem.Name };

                // INSERT文の実行
                await connection.ExecuteAsync(sql, param);
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// 完成後刪除物件
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Delete([Bind("Id")] TodoItemViewModel todoItem)
        {
            if (ModelState.IsValid)
            {
                using var connection = new SqlConnection(_connectionString);

                var sql = "DELETE FROM TodoItems WHERE Id = @Id";
                var param = new TodoItemViewModel { Id = todoItem.Id };

                // Delete
                await connection.ExecuteAsync(sql, param);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}