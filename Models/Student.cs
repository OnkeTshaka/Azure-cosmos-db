using DataAnnotationsExtensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using Xunit.Sdk;

namespace FirewallsAzureProject.Models
{
    public class Student
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }




        [JsonProperty(PropertyName = "studentNo")]
        [ScaffoldColumn(false)]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }




        [JsonProperty(PropertyName = "First Name")]
        [StringLength(20, ErrorMessage = "First name cannot be more than 20 characters.")]
        [Required(ErrorMessage = "Your first name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [JsonProperty(PropertyName ="Last Name")]
        [StringLength(20, ErrorMessage = "Last name cannot be more than 20 characters.")]
        [Required(ErrorMessage = "Your last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }




        [JsonProperty(PropertyName = "email")]
        [Required(ErrorMessage = "The email address is required")]
        [Email(ErrorMessage = "The email address is not valid")]
        [DataType(DataType.EmailAddress, ErrorMessage = "The email address is not valid")]
        public string Email { get; set; }





        [JsonProperty(PropertyName = "homeAddress")]
        [Display(Name = "Home Address")]
        [StringLength(100)]
        [MinLength(10,ErrorMessage = "Home address cannot be less than 10 characters.")]
        [DataType(DataType.MultilineText, ErrorMessage ="Please fill in student's Addresss")]
        public string HomeAddress { get; set; }




        
        [JsonProperty(PropertyName = "mobile")]
        [StringLength(10, ErrorMessage = "Mobile number should only start with Zero(0) and contain 10 numbers")]
        [DataType(DataType.PhoneNumber)]
        [MinLength(10, ErrorMessage = "Mobile number cannot be less than 10 characters.")]
        [Phone]
        public string Mobile { get; set; }







        [JsonProperty(PropertyName = "isActive")]
        [Display(Name = "Is not Active?")]
        public bool isActive { get; set; }



        [JsonProperty(PropertyName = "academicYear")]
        [Range(2020, 2050, ErrorMessage = "Please enter a valid year, range(2020 - 2050)")]
        [StringLength(4, ErrorMessage = "The current century consist of 4 digits, Format(yyyy)")]
        [ScaffoldColumn(false)]
        [Display(Name = "Academic Year")]
        public string acadademicYear { get; set; }


        public int GetRandomNumber { get; set; }


        public string StoreAcad1st { get; set; }


        public string StoreAcad2nd { get; set; }

      

        [JsonProperty(PropertyName = "URL")]
        //[Required(ErrorMessage = "Please upload an image")]
        [DataType(DataType.ImageUrl)]
        public string ProfilePath { get; set; }



        public string GetAcad1st()
        {
            StoreAcad1st = acadademicYear.Substring(0, 1);
            return StoreAcad1st;
        }


        public string GetAcad2nd()
        {
            StoreAcad2nd = acadademicYear.Substring(2, 2);
            return StoreAcad2nd;
        }



        public int GetNumber()
        {

            Random random = new Random();
            int GetRandomNumber = random.Next(10000, 99999);
            return GetRandomNumber;

        }



        public string GetStudentNumber()
        {
            StudentNumber = StoreAcad1st + StoreAcad2nd + GetRandomNumber.ToString();
            return StudentNumber;
        }
    }
}