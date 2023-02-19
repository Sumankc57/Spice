using System.ComponentModel.DataAnnotations;

namespace Spice.Models.TableModel
{
    public class TableType
    {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings =false ,ErrorMessage ="You Must enter the table type name!!!!!")]
        public string Name { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "You Must enter Description that descripe this type")]
        [MinLength(20 ,ErrorMessage ="The Length Must Be more than 20 characters ")]
        [MaxLength(300, ErrorMessage = "The Length Must Be less than 300 characters ")]
        public string Description { get; set; }
        
        public bool IsBusy { get; set; }
        [Required(AllowEmptyStrings =false ,ErrorMessage ="You Must enter the price for this type ")]
        public double Price { get; set; }

        public TableBooking tableBooking { get; set; }
    }
}
