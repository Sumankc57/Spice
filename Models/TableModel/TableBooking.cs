using Spice.Models.MyEnum;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spice.Models.TableModel
{
    public class TableBooking
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        [Required(AllowEmptyStrings =false,ErrorMessage ="You Must enter your email")]
        [Display(Name = "User Email")]

        public string UserEmail { get; set; }
        [Required]
        [Display(Name = "Booking Date")]

        public DateTime BookingDate { get; set; }
        [Required]
        [Display(Name = "Exiting Date")]

        public DateTime ExitingDate { get; set; }
        public string Status { get; set; }
        [Required]
        [Display(Name = "Reservation For")]

        public string BookingFor { get; set;}

        [Display(Name = "Type OF Table")]
        [Required(ErrorMessage ="Please You Must Choose One Type Of Table ,, If There is no Type avaliable Try again Later")]
        public int TypeID { get; set; }
        public TableType  tableType { get; set; }

    }
}
