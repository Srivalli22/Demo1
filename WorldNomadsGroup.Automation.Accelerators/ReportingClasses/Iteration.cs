using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldNomadsGroup.Automation.Accelerators
{
	public class Iteration
	{
		private List<Chapter> _chapters = new List<Chapter>();

        /// <summary>
        /// Creates a new Iteration
        /// </summary>
        /// <param name="title">Title</param>
        /// 

        //public Iteration(String title, String defectID)
        //removed defect id column from test data

        public Iteration(String title)//, String defectID)
		{
            TimeZone zone = TimeZone.CurrentTimeZone;
            this.Title = title;
            //this.BugInfo = defectID;
            //this.StartTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            this.StartTime = zone.ToLocalTime(DateTime.Now);
        }

		/// <summary>
		/// Gets or sets Title
		/// </summary>
		public String Title { get; set; }

		/// <summary>
		/// Gets or sets BugInfo
		/// </summary>
		public String BugInfo { get; set; }

		/// <summary>
		/// Gets or sets Start Time
		/// </summary>
		public DateTime StartTime { get; set; }

		/// <summary>
		/// Gets or sets End Time
		/// </summary>
		public DateTime EndTime { get; set; }

		/// <summary>
		/// Gets Chapters
		/// </summary>
		public List<Chapter> Chapters
		{
			get
			{
				return _chapters;
			}
		}

		/// <summary>
		/// Gets Current Chapter
		/// </summary>
		public Chapter Chapter
		{
			get
			{
				if (Chapters.Count() == 0)
					Chapters.Add(new Chapter("UNKNOWN CHAPTER"));
				return Chapters.Last();
			}
		}
        
        /// <summary>
        /// Adds new Chapter
        /// </summary>
        /// <param name="chapter">Chapter to add</param>
        public void Add(Chapter chapter)
		{
			Chapters.Add(chapter);
		}

		/// <summary>
		/// Adds new Step to current Chapter
		/// </summary>
		/// <param name="step"></param>
		public void Add(Step step)
		{
			this.Chapter.Steps.Add(step);
		}

		/// <summary>
		/// Adds new action to current Step
		/// </summary>
		/// <param name="action"></param>
		public void Add(Act action)
		{
			this.Chapter.Step.Actions.Add(action);
		}

		/// <summary>
		/// Gets or sets IsSuccess
		/// </summary>
		public Boolean IsSuccess
		{
			get
			{
				if (Chapters.Count() > 0)
				{
					return Chapter.IsSuccess;
				}
				return true;
			}
		}

		/// <summary>
		/// Gets or sets Screenshot
		/// </summary>
		public String Screenshot { get; set; }

		public Browser Browser { get; set; }

		public Boolean IsCompleted { get; set; }

        public void Add(string message)
        {
            throw new NotImplementedException();
        }
    }
}