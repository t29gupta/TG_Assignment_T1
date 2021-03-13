using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TG_Web_Extraction
{
    public class BookingModel
    {
        //        Hotel name(red)
        //Address(red)
        //Classification / stars(red)
        //Review points(pink)
        //Number of reviews(pink)
        //Description(blue)
        //Room categories(green)
        //Alternative hotels(yellow)

        public string HotelName { get; set; }
        public string Address { get; set; }
        public ReviewPoint ReviewPoint { get; set; }
        public string Classification_Stars { get; set; }
        public string NoOfReviews { get; set; }
        public string Description { get; set; }
        public List<RoomCategories> RoomCategories { get; set; }
        public List<AltHotels> AlternativeHotels { get; set; }
    }

    public class RoomCategories
    {
        public string Description { get; set; }
        public string TypeName { get; set; }
    }

    public class AltHotels
    {
        public string AltHotelName { get; set; }
        public string AltHotelDesc { get; set; }
        public string AltHotelToolTip { get; set; }
        public ReviewPoint AltHotelRaiting { get; set; }
    }

    public class ReviewPoint
    {
        public string Text { get; set; }
        public string Score { get; set; }
    }
}
