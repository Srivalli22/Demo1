using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace WorldNomadsGroup.Automation.Accelerators
{
	public class Engine
	{
		private String _reportsPath = String.Empty;
		private String _serverName = String.Empty;
		private String _timestamp = String.Empty;
		private Object _provisionalSummaryLocker = new Object();
        TimeZone _zone = TimeZone.CurrentTimeZone;

        public  Summary Summary = new Summary();

		/// <summary>
		/// Gets Report Path
		/// </summary>
		public String ReportPath
		{
			get
			{
				return _reportsPath;
			}
		}

		/// <summary>
		/// Gets Reports TimeStamp
		/// </summary>
		public String Timestamp
		{
			get
			{
				return _timestamp;
			}
		}

		/// <summary>
		/// Gets Server name
		/// </summary>
		public String ServerName
		{
			get
			{
				return _serverName;
			}
		}

		/// <summary>
		/// Gets or sets Reporter
		/// </summary>
		public Summary Reporter
		{
			get
			{
				return Summary;
			}
		}

		/// <summary>
		/// Creates Engine instance
		/// </summary>
		/// <param name="resultPath">Path to Report Results</param>
		public Engine(String resultPath, String serverName)
		{
			this._serverName = serverName;
			//this.timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).ToString("MM-dd-yyyy HH-mm-ss");
			//this.timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("MM-dd-yyyy HH-mm-ss");
            this._timestamp = _zone.ToLocalTime(DateTime.Now).ToString("MM-dd-yyyy HH-mm-ss");
            this._reportsPath = Path.Combine(resultPath, this._timestamp);
			System.IO.Directory.CreateDirectory(this._reportsPath);
			System.IO.Directory.CreateDirectory(Path.Combine(this._reportsPath, "Screenshots"));

		}

	    public Engine(String resultPath, String serverName,String rerun)
	    {
	        this._serverName = serverName;
	        //this.timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time")).ToString("MM-dd-yyyy HH-mm-ss");
	        //this.timestamp = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time")).ToString("MM-dd-yyyy HH-mm-ss");
	        this._timestamp = _zone.ToLocalTime(DateTime.Now).ToString("MM-dd-yyyy HH-mm-ss");
	        if (rerun.Equals("YES"))
	        {
	            this._reportsPath = Path.Combine(resultPath+ "\\Rerun", this._timestamp);
	            System.IO.Directory.CreateDirectory(this._reportsPath);
	            System.IO.Directory.CreateDirectory(Path.Combine(this._reportsPath, "Screenshots"));
            }
	        else
	        {
	            this._reportsPath = Path.Combine(resultPath, this._timestamp);
	            System.IO.Directory.CreateDirectory(this._reportsPath);
	            System.IO.Directory.CreateDirectory(Path.Combine(this._reportsPath, "Screenshots"));

            }
	  

	    }
        

        /// <summary>
        /// Publishes Summary Report of an iteration
        /// </summary>
        public void PublishIteration(Iteration iteration)
		{
			// If current iteration is a failure, get screenshot
			if (!iteration.IsSuccess)
			{
				try
				{
					File.WriteAllBytes(Path.Combine(this._reportsPath, "Screenshots", String.Format("{0} {1} {2} Error.png", iteration.Browser.TestCase.Title, iteration.Browser.Title, iteration.Title)),
					                   Convert.FromBase64String(iteration.Screenshot));
				}
				catch (Exception)
				{
				}
			}

			#region Write HTML Content

			String template = @"
            <html>
            <head>
	        <link href='http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css' rel='stylesheet'>
	        <script src='http://code.jquery.com/jquery-1.11.0.min.js' type='text/javascript'></script>
	        <script src='http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js'></script>
	        <style>
            html {
	        overflow: -moz-scrollbars-vertical; /* Vertical Scroll bar always visible, to avoid flicker while collapse/expand */
	        overflow-y: scroll;
	        }
            .bigger-icon {
		        transform:scale(3.0,3.0);
		        -ms-transform:scale(3.0,3.0); /* IE 9 */
		        -moz-transform:scale(3.0,3.0); /* Firefox */
		        -webkit-transform:scale(3.0,3.0); /* Safari and Chrome */
		        -o-transform:scale(3.0,3.0); /* Opera */}
             .default {
		
		            font-family: Courier New;
					font-size: 15px;
	            }
	        .Report-Chapter {
                padding:12px; margin-bottom: 5px;
		        background-color: #26466D; color: #fff;
		        font-size: 90%; font-weight:bold;
		        border: 1px solid #03242C; border-radius: 4px;
		        font-family: Menlo,Monaco,Consolas,'Courier New',monospace; cursor: pointer; }
	        .Report-Step {
		        padding:12px; margin-bottom: 5px;
		        background-color: #ddd; color: #000;
		        font-size: 90%; font-weight:bold;
		        border: 1px solid #bebebe; border-radius: 4px;
		        font-family: Menlo,Monaco,Consolas,'Courier New',monospace; cursor: pointer;}
	        .Report-Action {
		        padding:12px; margin-bottom: 5px;
		        background-color: #f7f7f9; color: #000; font-size: 90%;
		        border: 1px solid #e1e1e8; border-radius: 4px;
		        font-family: Menlo,Monaco,Consolas,'Courier New',monospace;}
	        .green { color:green; }
	        .red {color: red; }
	        .normal {color: black; }
            .brightgreen {color:lime;}
	        .brightred {color: orangered;}
            .darkbg {background-image:url('https://passportplus2btraining.freemanco.com/Content/img/freeman_header_bkg.jpg');}
            .timestamp {color:#555;}
	        </style>
            <script language='javascript'>
            	$(function() {
		            $('.Report-Chapter').click(function(){
			            $(this).parent().children('.wrapper').slideToggle();
		            });
		
		            $('.Report-Step').click(function(){
			            $(this).parent().children('.Report-Action').slideToggle();
		            });
	            });
            </script>
            </head>
            <body>
	            <div class='container'>
		            <div style='padding-top: 5px; padding-bottom:5px;'>
			            <img src='https://media.licdn.com/media/AAEAAQAAAAAAAAISAAAAJDlmOTU0MWQ5LWY1MWUtNGY5Ni1iZmJhLTU4MTM2NGQ1ZWMyMw.png' style='padding-top:20px; width:200px; height:auto;' />
			            <div class='pull-right'><img src='http://www.cigniti.com/wp-content/uploads/logo.png'/></div>
			
		            </div>
	            </div>
	            <div class='container default'>
                    <div class='darkbg' style='background-color:#26466D; color:#fff; min-height:100px; padding:20px; margin-bottom:5px; margin-top:5px; top:-40px;'>
		                <div class='row'>
		                  <div class='col-md-6' > <b> Server: </b>{{SERVER}}<br/> <b> Browser: </b>{{BROWSER}}<br/> <b> Environment: </b>{{ENVIRONMENT}}</div>
		                  <div class='col-md-6' > <b> Start: </b>{{EXECUTION_BEGIN}}<br/> <b> End: </b>{{EXECUTION_END}}<br/><b> Duration: </b>{{EXECUTION_DURATION}}</div>
		                </div>
	                </div>
                </div>
                <div class='container default'>
                    <div class='darkbg' style='background-color:#26466D; color:#fff; min-height:60px; padding:20px; margin-bottom:5px; margin-top:5px; top:-20px;'>
		                <div class='row'>
                          <div class='col-md-3' > <b> {{TCID}} </b> </div>
                          <div class='col-md-8' > <b> {{TC_NAME}} </b> </div>
                          <div class='col-md-1' > <span class='glyphicon glyphicon-{{STATUS_ICON}} bigger-icon' style='padding-left:10px;'></span>  </div>
                        </div>
	                </div>
                </div>
                <div class='container'>
                    {{CONTENT}}
                </div>
            </body>
            </html>";
			#endregion

			StringBuilder builder = new StringBuilder();

			foreach (Chapter chapter in iteration.Chapters)
			{
				builder.AppendFormat("<div><p class='Report-Chapter'>Chapter: {0}<span class='pull-right'><span class='glyphicon glyphicon-{1}'></span></span></p>", chapter.Title, chapter.IsSuccess ? "ok brightgreen" : "remove brightred");

				foreach (Step step in chapter.Steps)
				{
					builder.AppendFormat("<div class='wrapper'><p class='Report-Step'>Step: {0}<span class='pull-right'><span class='glyphicon glyphicon-{1}'></span></span></p>", step.Title, step.IsSuccess ? "ok green" : "remove red");

					foreach (Act action in step.Actions)
					{
						builder.AppendFormat("<p class='Report-Action' style='display:none;'>{0}<span class='pull-right'><span class='timestamp'>{1}</span>&nbsp;&nbsp; ", action.Title, action.TimeStamp.ToString("H:mm:ss"));
						if (action.IsSuccess)
						{
							builder.Append("<span class='glyphicon glyphicon-ok green'></span>");
						}
						else
						{
							builder.Append("<a href='" + Path.Combine("Screenshots", String.Format("{0} {1} {2} Error.png", iteration.Browser.TestCase.Title, iteration.Browser.Title, iteration.Title)) + "'><span class='glyphicon glyphicon-paperclip normal'></span></a>&nbsp;");
							builder.Append("<span class='glyphicon glyphicon-remove red'></span>");
						}

						builder.Append("</span></p>");
					}

					builder.Append("</div>");
				}

				builder.Append("</div>");
			}

			if (!iteration.IsSuccess)
			{
				//builder.AppendFormat("<div class='default'><p>URL: {0}</p></div>", driver.Url);
				builder.AppendFormat("<div class='default'><p>{0}</p></div>", iteration.Chapter.Step.Action.Extra);
			}

            this._timestamp = _zone.ToLocalTime(DateTime.Now).ToString("MM-dd-yyyy HH-mm-ss");

            template = template.Replace("{{STATUS_ICON}}", iteration.IsSuccess ? "ok brightgreen" : "remove brightred");
			template = template.Replace("{{TCID}}", iteration.Browser.TestCase.Title);
			template = template.Replace("{{TC_NAME}}", iteration.Browser.TestCase.Name);
			template = template.Replace("{{SERVER}}", this.ServerName);
			template = template.Replace("{{BROWSER}}", String.Format("{0} {1}", iteration.Browser.BrowserName, iteration.Browser.BrowserVersion));
			template = template.Replace("{{ENVIRONMENT}}", String.Format("{0} {1}", iteration.Browser.PlatformName, iteration.Browser.PlatformVersion));
            //template = template.Replace("{{EXECUTION_BEGIN}}", String.Format("{0} {1}", iteration.StartTime.ToString("MM-dd-yyyy HH:mm:ss"), "Central Time (US & Canada)"));
            //template = template.Replace("{{EXECUTION_END}}", String.Format("{0} {1}", iteration.EndTime.ToString("MM-dd-yyyy HH:mm:ss"), "Central Time (US & Canada)"));
            template = template.Replace("{{EXECUTION_BEGIN}}", iteration.StartTime.ToString("MM-dd-yyyy HH:mm:ss"));            
            template = template.Replace("{{EXECUTION_END}}",  iteration.EndTime.ToString("MM-dd-yyyy HH:mm:ss"));
			template = template.Replace("{{EXECUTION_DURATION}}", iteration.EndTime.Subtract(iteration.StartTime).ToString());

			String fileName = Path.Combine(this._reportsPath, String.Format("{0} {1} {2}.html", iteration.Browser.TestCase.Title, iteration.Browser.Title, iteration.Title));

			using (StreamWriter output = new StreamWriter(fileName))
			{
				output.Write(template.Replace("{{CONTENT}}", builder.ToString()));
			}
		}

		/// <summary>
		/// Publishes Summary Report
		/// </summary>
		public void Summarize(bool isFinal = true)
		{
			#region HTML Template

			String template = @"
            <!DOCTYPE html>
            <html>
            <head>
	            <link href='http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css' rel='stylesheet'>
	            <script src='http://code.jquery.com/jquery-1.11.0.min.js' type='text/javascript'></script>
	            <script src='http://netdna.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js'></script>
	        <style>
	            html {
	            overflow: -moz-scrollbars-vertical;
	            overflow-y: scroll;
	            }
	            .bigger-icon {
		            transform:scale(2.0,2.0);
		            -ms-transform:scale(2.0,2.0); /* IE 9 */
		            -moz-transform:scale(2.0,2.0); /* Firefox */
		            -webkit-transform:scale(2.0,2.0); /* Safari and Chrome */
		            -o-transform:scale(2.0,2.0); /* Opera */
	            }
	            .default {
		
		            font-family: Courier New;
					font-size: 15px;
	            }
	            .Report-Chapter {
		            padding:12px;
		            margin-bottom: 5px;
		            background-color: #26466D;
		            color: #fff;
		            font-size: 90%; font-weight:bold;
		            border: 1px solid #03242C;
		            border-radius: 4px;
		            font-family: Menlo,Monaco,Consolas,'Courier New',monospace;
		            cursor: pointer;
	            }
	            .Report-Step {
		            padding:12px;
		            margin-bottom: 5px;
		            background-color: #ddd;
		            color: #000;
		            font-size: 90%; font-weight:bold;
		            border: 1px solid #bebebe;
		            border-radius: 4px;
		            font-family: Menlo,Monaco,Consolas,'Courier New',monospace;
		            cursor: pointer;
	            }
	            .Report-Action {
		            padding:12px;
		            margin-bottom: 5px;
		            background-color: #f7f7f9;
		            color: #000;
		            font-size: 90%;
		            border: 1px solid #e1e1e8;
		            border-radius: 4px;
		            font-family: Menlo,Monaco,Consolas,'Courier New',monospace;
	            }
	            .green { color:green; }
	            .red {color: red; }
	            .normal {color: black; }
	            .darkbg {background-image:url('https://passportplus2btraining.freemanco.com/Content/img/freeman_header_bkg.jpg')};
	        </style>


            <style>
			   #example thead th {
				  background-color: #1B3F73;
				  color: white;
                  text-align:center;
				}
				
			  #example tbody td:hover {
				    cursor: pointer;
                  	
                }

            #example tbody td {
				     text-align:center;                  	
                }
			
			  #example tbody td a{
			        text-decoration: none;
                    color: black;
                   
			    }
	        </style>

            <script type='text/javascript'>
			    $(document).ready(function() {

				    $('#example tr').click(function() {
					    var href = $(this).find('a').attr('href');
					    if(href) {
						    window.open(href);
					    }
				    });
			    });
			</script>

            </head>

            <body>
	        <div class='container'>
		        <div style='padding-top: 5px; padding-bottom:5px;'>
			        <img src='https://media.licdn.com/media/AAEAAQAAAAAAAAISAAAAJDlmOTU0MWQ5LWY1MWUtNGY5Ni1iZmJhLTU4MTM2NGQ1ZWMyMw.png' style='padding-top:20px; width:200px; height:auto;' />
			        <div class='pull-right'><img src='http://www.cigniti.com/wp-content/uploads/logo.png'/></div>
			
		        </div>
	        </div>

            <div class='container default'>
                <div class='darkbg' style='background-color:#26466D; color:#fff; min-height:100px; padding:20px; margin-bottom:20px; margin-top:10px; top:-20px;'>
		            <div class='row'>
		              <div class='col-md-6' >
                            <br/>
                            <b> Server: </b> {{SERVER}}<br/>
                            <b> Parallel Cases (Max): {{MAX_PARALLEL}}
                      </div>
		              <div class='col-md-6' > <b> Start: </b> {{EXECUTION_BEGIN}}<br/> <b> End: </b> {{EXECUTION_END}}<br/> <b> Duration: </b> {{EXECUTION_DURATION}}<br/> <b> Duration (Cumulative): {{EXECUTION_DURATION_CUM}}</b> </div>
		            </div>
	            </div>
            </div>

            <div class='container'>
		         <div class='col-md-6' style='padding-left:0px;'> {{BARCHART_TABLE}} </div>
		         <div class='col-md-6' > <div id='barChart' style='height:200px; width:550px;'></div> </div>
            </div>
            </br>
            <div class='container'>
	        <table id='example' class='table table-striped table-bordered table-condensed default table-hover'>
                 <thead>
		            <tr>
			            <th>SNO.</th>
			            <th>Test Case ID</th>
			            <th>Browser</th>
			            <th>Iteration</th>
			            <th>Duration</th>                        
			            <th>Result</th>
		            </tr>
                </thead>
			    <tbody>
                    {{CONTENT}}
                </tbody>
             </table>
            </div>
            <script type='text/javascript' src='https://www.google.com/jsapi'></script>
            <script  type='text/javascript'>

                google.load('visualization', '1', {packages:['corechart']});
                var BarChartData = {{BARCHARTDATA}};
                google.setOnLoadCallback(drawVisualization);
                function drawVisualization() {
                    var data = google.visualization.arrayToDataTable(BarChartData);

		            var options = {
                      title: 'Browser Wise Status',
                      legend: {position: 'top', alignment:'center'},
                      vAxis: {title: 'Count'},
		              hAxis: {title: 'Browser'},
                      seriesType: 'bars',
                      colors: ['green', 'red']
                    };
       
                    var chart = new google.visualization.ComboChart(document.getElementById('barChart'));
                    chart.draw(data, options);
                  }
                
             </script>
            </body>
            </html>";

            #endregion

            this._timestamp = _zone.ToLocalTime(DateTime.Now).ToString("MM-dd-yyyy HH:mm:ss");

            Int16 caseCounter = 1;
			StringBuilder builder = new StringBuilder();
			//DateTime FirstCaseBeginTime = DateTime.Now;
			//DateTime LastCaseEndTime = DateTime.Now;
            DateTime firstCaseBeginTime = _zone.ToLocalTime(DateTime.Now);
            DateTime lastCaseEndTime = _zone.ToLocalTime(DateTime.Now);
            TimeSpan executionTimeCumulative = TimeSpan.Zero;

			foreach (TestCase testCase in Reporter.TestCases)
			{
				foreach (Browser browser in testCase.Browsers)
				{
					foreach (Iteration iteration in browser.Iterations.FindAll(itr => itr.IsCompleted == true))
					{
						builder.Append("<tr>");
						builder.AppendFormat("<td>{0}</td>", caseCounter.ToString());
						builder.AppendFormat("<td><a href='{0}' target='_blank'>{1}</a></td>", String.Format("{0} {1} {2}.html", testCase.Title, browser.Title, iteration.Title), testCase.Title);
						builder.AppendFormat("<td>{0} {1}</td>", browser.BrowserName, browser.BrowserVersion);
						builder.AppendFormat("<td>{0}</td>", iteration.Title);
						builder.AppendFormat("<td>{0}</td>", iteration.EndTime.Subtract(iteration.StartTime).ToString());
						//builder.AppendFormat("<td>{0}</td>", iteration.BugInfo);
						builder.AppendFormat("<td><span class='glyphicon glyphicon-{0}'></span></td>", iteration.IsSuccess == true ? "ok green" : "remove red");
						builder.Append("</tr>");
						caseCounter++;

						if (iteration.StartTime < firstCaseBeginTime) firstCaseBeginTime = iteration.StartTime;
						if (iteration.EndTime > lastCaseEndTime) lastCaseEndTime = iteration.EndTime;
						executionTimeCumulative = executionTimeCumulative.Add(iteration.EndTime.Subtract(iteration.StartTime));
					}
                    
                }
                
            }

			Dictionary<String, Dictionary<String, long>> getStatusByBrowser = Summary.GetStatusByBrowser();

			template = template.Replace("{{SERVER}}", this.ServerName);
			template = template.Replace("{{MAX_PARALLEL}}", ConfigurationManager.AppSettings.Get("MaxDegreeOfParallelism"));
			// template = template.Replace("{{EXECUTION_BEGIN}}", String.Format("{0} {1}", FirstCaseBeginTime.ToString("MM-dd-yyyy HH:mm:ss"), "Central Time (US & Canada)"));
			// template = template.Replace("{{EXECUTION_END}}", String.Format("{0} {1}", LastCaseEndTime.ToString("MM-dd-yyyy HH:mm:ss"), "Central Time (US & Canada)"));
			template = template.Replace("{{EXECUTION_BEGIN}}",  firstCaseBeginTime.ToString("MM-dd-yyyy HH:mm:ss"));
			template = template.Replace("{{EXECUTION_END}}", lastCaseEndTime.ToString("MM-dd-yyyy HH:mm:ss"));
			template = template.Replace("{{EXECUTION_DURATION}}", lastCaseEndTime.Subtract(firstCaseBeginTime).ToString());
			template = template.Replace("{{EXECUTION_DURATION_CUM}}", executionTimeCumulative.ToString());
			template = template.Replace("{{BARCHARTDATA}}", BuildBarChartData(getStatusByBrowser));
			template = template.Replace("{{BARCHART_TABLE}}", BuildBarChartTable(getStatusByBrowser));

			String fileName = Path.Combine(this._reportsPath, isFinal ? "Summary.html" : "Summary_Provisional.html");
			lock (_provisionalSummaryLocker)
			{
				using (StreamWriter output = new StreamWriter(fileName))
				{
					output.Write(template.Replace("{{CONTENT}}", builder.ToString()));
				}
			}
		}

		/// <summary>
		/// Build Bar Chart Data
		/// </summary>
		public string BuildBarChartData(Dictionary<String, Dictionary<String, long>> browserStatus)
		{
			String strReturn = String.Empty;
			int temp;

			strReturn = strReturn + "[ ['Browser', 'Passed',  { role: 'style' }, 'Failed',  { role: 'style' } ],";

			foreach (String browserName in browserStatus.Keys)
			{
				strReturn = strReturn + "['" + browserName + "',";

				temp = 1;
				Dictionary<String, long> status = browserStatus[browserName];
				foreach (long statusCount in status.Values)
				{
					if (temp == 1)
						strReturn = strReturn + statusCount + ", 'green',";
					else
						strReturn = strReturn + statusCount + ", 'red',";

					temp++;
				}

				strReturn = strReturn.TrimEnd(',') + " ],";
			}

			strReturn = strReturn.TrimEnd(',');
			strReturn = strReturn + " ]";
			return strReturn;
		}

		/// <summary>
		/// Build Bar Chart Table
		/// </summary>
		public string BuildBarChartTable(Dictionary<String, Dictionary<String, long>> browserStatus)
		{
			String strReturn = String.Empty;
			int total = 0;
			int passedTotal = 0;
			int failedTotal = 0;
			int temp;

			strReturn = strReturn + "<table class='table table-striped table-bordered table-condensed default'> <tr> <th colspan='4' style='background-color: #1B3F73; color: white'> <center> Test Result Status </center> </th> </tr>";
			strReturn = strReturn + "<tr> <th style='background-color: #1B3F73; color: white'> Test Results </th> <th style='background-color: #1B3F73; color: #1BDE38'> <center> Passed </center> </th> <th style='background-color: #1B3F73; color: red'> <center> Failed </center> </th> <th style='background-color: #1B3F73; color: white; font-weight: bold'> <center> Total </center> </th> </tr>";

			foreach (String browserName in browserStatus.Keys)
			{
				strReturn = strReturn + "<tr> <td> " + browserName + "</td>";

				Dictionary<String, long> status = browserStatus[browserName];

				total = 0;
				temp = 1;

				foreach (long statusCount in status.Values)
				{
					strReturn = strReturn + "<td> <center> " + statusCount + " </center> </td>";
					total = total + Convert.ToInt32(statusCount);

					if (temp == 1)
					{
						passedTotal = passedTotal + Convert.ToInt32(statusCount);
					}
					else
					{
						failedTotal = failedTotal + Convert.ToInt32(statusCount);
					}

					temp++;
				}

				strReturn = strReturn + "<td style='font-weight: bold'> <center> " + total + " </center> </td> </tr>";
			}

			strReturn = strReturn + "<tr style='font-weight: bold'> <td> Total </td> <td> <center> " + passedTotal + " </center> </td> <td> <center> " + failedTotal + " </center> </td> <td> <center> " + (passedTotal + failedTotal) + " </center> </td> </tr> </table>";

			return strReturn;
		}

	}
}