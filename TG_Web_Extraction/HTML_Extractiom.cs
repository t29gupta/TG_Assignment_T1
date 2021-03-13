﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using HtmlAgilityPack;

namespace TG_Web_Extraction
{
    public class HTML_Extractiom
    {
        public string ReadHtmlFile()
        {
            try
            {
                //bool validFile = false;
                var fileLocation = @"E:\Cloud Drives\OneDrive\Documents\HQ plus - Backend Development Assignment\HQ plus - Backend Development Assignment\Task 1\Kempinski Hotel Bristol Berlin, Germany - Booking.com.html";

                //while (!validFile)
                //{
                //    Console.WriteLine("Enter the location of the html file...");
                //    fileLocation = Console.ReadLine();

                //    if (Path.GetExtension(fileLocation) == ".html" && File.Exists(fileLocation))
                //    {
                //        validFile = true;
                //    }
                //    else
                //    {
                //        Console.WriteLine("File not found or invalid file name entered");
                //    }
                //}

                return File.ReadAllText(fileLocation);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return "";
        }

        const string HoltelName_Id = "hp_hotel_name";
        const string Address_Id = "hp_address_subtitle";
        const string Classification_stars = "";

        const string Review_points_Block_Id = "js--hp-gallery-scorecard";
        const string Review_points_Text_Class = "js--hp-scorecard-scoreword";
        const string Review_points_Score_Class = "average";
        const string Review_points_OutOf_Class = "best";

        const string Number_of_reviews_Parent = "trackit";
        const string Number_of_reviews = "count";

        // Content is in form of paragraphs in <p> tags
        const string Description_Id = "summary";

        // its a table. Content is in tbody in form of rows
        const string Room_Categories_Table_Id = "maxotel_rooms";
        const string Room_Categories_row_class = "ftd";
        private const string Room_Category_ToolTip = "occ_no_dates";

        const string Alternative_hotels_Table_Id = "althotelsRow";
        const string Alt_hotel_row_name_class = "althotels-name";
        const string Alt_hotel_row_Desc_class = "hp_compset_description";

        const string Alt_hotel_row_RatingText_class = "js--hp-scorecard-scoreword";
        const string Alt_hotel_row_RatingPoint_class = "average";

        public void Extractinfo(string htmlContent)
        {
            try
            {
                BookingModel bmResult = new BookingModel();

                var html = new HtmlDocument();
                html.LoadHtml(htmlContent);

                // Getting Hotel Name
                var hotelNameElement = html.GetElementbyId(HoltelName_Id);
                if (hotelNameElement == null)
                {
                    Console.WriteLine("Hotel name not found");
                }
                Console.WriteLine($"element : {hotelNameElement}\nName : {hotelNameElement?.InnerText}");
                bmResult.HotelName = hotelNameElement?.InnerText.Replace("\n", "");

                // Getting Hotel Address
                var hotelAddressElement = html.GetElementbyId(Address_Id);
                if (hotelAddressElement == null)
                {
                    Console.WriteLine("Hotel address not found");
                }
                Console.WriteLine($"element : {hotelAddressElement}\nAddress : {hotelAddressElement?.InnerText}");
                bmResult.Address = hotelAddressElement?.InnerText.Replace("\n", "");

                // Getting Hotel ReviewPoints and no of reviews
                var reviewElement = html.GetElementbyId(Review_points_Block_Id);
                var reviewDecendents = reviewElement?.Descendants("span").ToList();

                var reviewText = reviewDecendents.FirstOrDefault(x => x.HasClass(Review_points_Text_Class))?.InnerText.Replace("\n", "");
                var reviewscore = reviewDecendents.FirstOrDefault(x => x.HasClass(Review_points_Score_Class))?.InnerText.Replace("\n", "");
                var reviewBest = reviewDecendents.FirstOrDefault(x => x.HasClass(Review_points_OutOf_Class))?.InnerText.Replace("\n", "");
                var reviewCount = reviewDecendents.FirstOrDefault(x => x.HasClass(Number_of_reviews_Parent))?.ChildNodes.FirstOrDefault(c => c.HasClass(Number_of_reviews))?.InnerText.Replace("\n", "");

                foreach (var item in reviewDecendents)
                {
                    Console.WriteLine(item.GetClasses().FirstOrDefault());
                }

                bmResult.ReviewPoint = new ReviewPoint
                {
                    Text = reviewText,
                    Score = $"{reviewscore}/{reviewBest}"
                };

                bmResult.NoOfReviews = reviewCount;

                // Getting description
                var descElement = html.GetElementbyId(Description_Id);
                var descDecendents = descElement.ParentNode.Descendants("p").ToList();// descElement?.Descendants("p").ToList();

                StringBuilder sbDesc = new StringBuilder();
                foreach (var para in descDecendents)
                {
                    var tempPara = para.InnerText.Replace("\n", "");

                    if (!string.IsNullOrEmpty(tempPara))
                    {
                        sbDesc.AppendLine(para.InnerText);
                    }
                }

                bmResult.Description = sbDesc.ToString();

                // Getting room categories
                var roomCategoriesElement = html.GetElementbyId(Room_Categories_Table_Id);
                var roomCategoriesRows = roomCategoriesElement
                    .Descendants("tr")
                    .Select(i =>
                        i.Descendants("td")
                        .Where(c => c.HasClass(Room_Category_ToolTip) || c.HasClass(Room_Categories_row_class))
                        .ToList())
                    .Where(d => d.Any())
                    .ToList();
                //roomCategoriesElement.Descendants("td")?.Where(x => x.HasClass(Room_Categories_row_class)).ToList();// descElement?.Descendants("p").ToList();

                bmResult.RoomCategories = new List<RoomCategories>();

                foreach (var roomCat in roomCategoriesRows)
                {
                    var toolTip = roomCat[0].ChildNodes.Select(x => x.GetAttributeValue("title", "")).FirstOrDefault(t => !string.IsNullOrEmpty(t));
                    var roomCategoryName = roomCat[1].InnerText.Replace("\n", "");

                    bmResult.RoomCategories.Add(new RoomCategories
                    {
                        Description = toolTip,
                        TypeName = roomCategoryName
                    });
                }

                // Alternative Hotels
                var altHotelsElement = html.GetElementbyId(Alternative_hotels_Table_Id);
                var altHotelCollection = altHotelsElement.Descendants("td")
                    .Select(p =>
                         p.Descendants()
                        .Where(z =>
                                    z.HasClass(Alt_hotel_row_name_class) ||
                                    z.HasClass(Alt_hotel_row_RatingText_class) ||
                                    z.HasClass(Alt_hotel_row_RatingPoint_class) ||
                                    z.HasClass(Alt_hotel_row_Desc_class))
                        .Select(s => new
                        {
                            className = s.GetClasses().FirstOrDefault(),
                            text = s.InnerText
                        }).ToList()
                    );

                bmResult.AlternativeHotels = new List<AltHotels>();

                foreach (var hotelRow in altHotelCollection)
                {
                    var altHotel = new AltHotels();
                    altHotel.AltHotelRaiting = new ReviewPoint();
                    foreach (var hotelRowElement in hotelRow)
                    {
                        switch (hotelRowElement.className)
                        {
                            case Alt_hotel_row_name_class:
                                {
                                    var hName = hotelRowElement.text.Split("\n").Where(s => !string.IsNullOrEmpty(s)).ToArray();

                                    altHotel.AltHotelName = hName[0];
                                    altHotel.AltHotelToolTip = hName[1];
                                }
                                break;
                            case Alt_hotel_row_Desc_class:
                                altHotel.AltHotelDesc = hotelRowElement.text.Replace("\n", "");
                                break;
                            case Alt_hotel_row_RatingText_class:
                                altHotel.AltHotelRaiting.Text = hotelRowElement.text.Replace("\n", "");
                                break;
                            case Alt_hotel_row_RatingPoint_class:
                                altHotel.AltHotelRaiting.Score = hotelRowElement.text.Replace("\n", "") + "/" + reviewBest;
                                break;
                            default:
                                break;
                        }
                    }

                    bmResult.AlternativeHotels.Add(altHotel);
                }

                string resJson = JsonSerializer.Serialize(bmResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}