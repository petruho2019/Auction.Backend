using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Application.Common.Models.Dto.Users
{
    public abstract class UserRequest
    {
        [Required(ErrorMessage = "Поле 'Имя пользователя' обязательно для заполнения")]
        [MaxLength(20, ErrorMessage = "Имя пользователя должено содержать не больше 20 символов")]
        [MinLength(3, ErrorMessage = "Имя пользователя должно содержать не менее 3 символов")]
        public string Username { get; set; }

        [MinLength(6, ErrorMessage = "Пароль должен содержать не менее 6 символов")]
        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        public string Password { get; set; }
    }
}
