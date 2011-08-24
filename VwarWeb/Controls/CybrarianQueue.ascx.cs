//  Copyright 2011 U.S. Department of Defense

//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at

//      http://www.apache.org/licenses/LICENSE-2.0

//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.



using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
/// <summary>
/// 
/// </summary>
public partial class Administrators_CybrarianQueue : Website.Pages.ControlBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            this.BindNoMetadataGridView();
            this.BindUnrecognizedModelsGridView();


        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void BindNoMetadataGridView()
    {
        this.ModelsWithoutMetadataGridView.DataSource = this.GetModelsWithoutMetadata();
        this.ModelsWithoutMetadataGridView.DataBind();

    }
    /// <summary>
    /// 
    /// </summary>
    private void BindUnrecognizedModelsGridView()
    {

        this.UnrecognizedModelsGridView.DataSource = this.GetUnrecognizedModels();
        this.UnrecognizedModelsGridView.DataBind();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private DataTable GetUnrecognizedModels()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PID");
        dt.Columns.Add("Title");
        dt.Columns.Add("SubmitterEmail");
        dt.Columns.Add("UploadedDate");


        //TODO: Query fedora 

        return dt;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private DataTable GetModelsWithoutMetadata()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("PID");
        dt.Columns.Add("Title");
        dt.Columns.Add("SubmitterEmail");
        dt.Columns.Add("UploadedDate");


        //TODO: Query fedora 

        return dt;
    }
    //private DataTable CreateModelsDataTableTestData()
    //{
    //    DataTable dt = new DataTable();
    //    dt.Columns.Add("PID");
    //    dt.Columns.Add("Title");
    //    dt.Columns.Add("SubmitterEmail");
    //    dt.Columns.Add("UploadedDate");

    //    DataRow row = dt.Rows.Add();
    //    row["PID"] = "adl:10";
    //    row["Title"] = "Fighter Jet";
    //    row["SubmitterEmail"] = "user@problemsolutions.net";
    //    row["UploadedDate"] = "6/15/2010";

    //    return dt;

    //}
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void GridView_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string pid = e.CommandArgument.ToString();

        switch (e.CommandName)
        {
            case "EditModel":

                //redirect to the model details pages
                Response.Redirect(Website.Common.FormatEditUrl(pid));


                break;

            case "Download":


                vwarDAL.IDataRepository vd = DAL;
                var co = vd.GetContentObjectById(pid, false);
                var url = vd.GetContentFile(co.PID, co.Location);
                vd.IncrementDownloads(pid);
                Website.Documents.ServeDocument(url, co.Location);

                break;

            case "Delete":

                //TODO: Implement

                break;
        }
    }
}
