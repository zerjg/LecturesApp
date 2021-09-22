using LecturesApp.ValidationAttributes;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace LecturesApp.Models
{
    public class Lecture
    {
        public int ID { get; set; }

        [Display(Name = "Тема")]
        [Required(ErrorMessage = "Поле \"Тема\" должно быть заполнено")]
        public string Title { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Поле \"Описание\" должно быть заполнено")]
        public string Description { get; set; }

        [Display(Name = "Возраст")]
        [Required(ErrorMessage = "Поле \"Возраст\" должно быть заполнено")]
        [Range(0, 21, ErrorMessage = "Значение должно быть от 0 до 21")]
        public int AgeLimit { get; set; }

        [Display(Name = "Кол-во участников")]
        [Required(ErrorMessage = "Поле \"Кол-во участников\" должно быть заполнено")]
        [Range(1, 100, ErrorMessage = "Значение должно быть от 1 до 100")]
        public int UsersNumberLimit { get; set; }

        [Display(Name = "Дата начала")]
        [Required(ErrorMessage = "Поле \"Дата начала\" должно быть заполнено")]
        [DateGreaterThanToday(ErrorMessage = "Дата начала должна быть позднее текущей")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Дата окончания")]
        [Required(ErrorMessage = "Поле \"Дата окончания\" должно быть заполнено")]
        [DateGreaterThan("StartDate", ErrorMessage = "Дата окончания должна быть позднее даты начала")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy H:mm}")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Состояние")]
        [NotMapped]
        public State State
        {
            get
            {
                if (StartDate > DateTime.Now)
                    return State.Scheduled;
                if (EndDate < DateTime.Now)
                    return State.Past;
                return State.Active;
            }
        }

        // Navigation
        public string HostUserID { get; set; }
        [Display(Name = "Автор")]
        public User HostUser { get; set; }
        public ICollection<UserLecture> RegisteredMembersLink { get; set; }
    }

    public enum State
    {
        [Display(Name = "Завершено")]
        Past,
        [Display(Name = "Идёт")]
        Active,
        [Display(Name = "Запланировано")]
        Scheduled
    }
}
