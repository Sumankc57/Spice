using FluentValidation;
using Spice.Models.TableModel;
using System;

namespace Spice.Data.Validations
{
    public class TableBookingValidator : AbstractValidator<TableBooking>
    {
        public TableBookingValidator()
        {
            RuleFor(x => x.ExitingDate).Must(
                (t, d) => d > t.BookingDate
                ).WithMessage("The Exiting Date Must be Greater than Booking Date");
        }

    }
}
