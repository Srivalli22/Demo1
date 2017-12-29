using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldNomadsGroup.Automation.Accelerators
{
    public class Act
    {
        private Boolean _isSuccess = true;
        TimeZone _zone = TimeZone.CurrentTimeZone;

        /// <summary>
        /// Creates Action instance
        /// </summary>
        /// <param name="title"></param>
        public Act(String title)
        {
            this.Title = title;
            Console.WriteLine(title);
            //this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time"));
            //this.TimeStamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            this.TimeStamp = _zone.ToLocalTime(DateTime.Now);
        }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public String Title { get; set; }

        /// <summary>
        /// Gets or sets TimeStamp
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets or sets Extra
        /// </summary>
        public String Extra { get; set; }

        /// <summary>
        /// Gets or sets isSuccess
        /// </summary>
        public Boolean IsSuccess
        {
            get
            {
                return _isSuccess;
            }
            set
            {
                _isSuccess = value;
            }
        }
    }
}