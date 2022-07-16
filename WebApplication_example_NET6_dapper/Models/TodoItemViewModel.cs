using System.ComponentModel.DataAnnotations;

namespace SqlApplication.Models
{
    public class TodoItemViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Task")]
        public string? Name { get; set; } = string.Empty;

        [Display(Name = "完成確認")]
        public bool IsComplete { get; set; }

        public List<TodoItemViewModel>? Items { get; set; }
    }
}